using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.rfilkov.kinect;

namespace com.rfilkov.components
{
    /// <summary>
    /// SkeletonOverlayer overlays the the user's body joints and bones with spheres and lines.
    /// </summary>
    public class SkeletonOverlayer : MonoBehaviour
    {
        //�÷��̾� �ε���(0 = 1��°, 1 = 2��° . . .)
        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        public int playerIndex = 0;

        //������ ���������� ���� ������Ʈ
        [Tooltip("Game object used to overlay the joints.")]
        public GameObject jointPrefab;

        //���� ���������� �� ��ü 
        [Tooltip("Line object used to overlay the bones.")]
        public LineRenderer linePrefab;
        //public float smoothFactor = 10f;

        //���� ���� �ε���(0 = 1��°, 1 = 2��° . . .)
        [Tooltip("Depth sensor index used for color frame overlay - 0 is the 1st one, 1 - the 2nd one, etc.")]
        public int sensorIndex = 0; //�츮�� ����� GMD

        //����Ƽ �� ����ī�޶�. ��ġ, ������
        //��濡 3D ������Ʈ�� ���������ϱ� ���� ���� ī�޶�
        [Tooltip("Camera that will be used to overlay the 3D-objects over the background.")]
        public Camera foregroundCamera;

        //������ ������ ��ġ, ȸ������ ��Ÿ�� transform
        [Tooltip("Scene object that will be used to represent the sensor's position and rotation in the scene.")]
        public Transform sensorTransform;

        //�������� - ī�޶� 
        //�������� - ���̷��� ��...
        //public UnityEngine.UI.Text debugText;

        // list of filtered-out joints
        //private - �����ϰ� ������
        //protected - ���

        // list of filtered-out joints
        //����Ʈ ���·� ������ ����. ��ô�� ���� ���� �ε��� ����!
        protected List<int> filteredOutJoints = new List<int>();

        // joints & lines
        //������ ���ٸ��� �ð�ȭ�ϱ� ���� ��ü���� �迭
        protected GameObject[] joints = null;
        protected LineRenderer[] lines = null;

        // initial body rotation
        //���̷��� ���� �ʱ� ȸ������ ����. ��� �ν��� ó�� �� �� ������.
        protected Quaternion initialRotation = Quaternion.identity;

        // reference to KM
        //Kinect ������ �����ϰ� �����͸� ������.
        protected KinectManager kinectManager = null;

        // background rectangle
        //Rect - ����, ��濵���� ����
        protected Rect backgroundRect = Rect.zero;


        protected virtual void Start()
        {
            //KinectManager�� kinectManager�� ����
            //KinectManager= Kinect ���� ����, �����͸� �о���� Ŭ����
            kinectManager = KinectManager.Instance;

            //KinectManager �ν��Ͻ��� ����, �ʱ�ȭ�Ǿ����� Ȯ��
            if (kinectManager && kinectManager.IsInitialized())
            {
                //������ ������ �� �ִ� ���� ������ ����Ͷ�.
                int jointsCount = kinectManager.GetJointCount();

                //���� �������� �����ϸ�, ������ ������Ʈ�� joints �迭 ����
                if (jointPrefab)
                {
                    // array holding the skeleton joints
                    //���� ��Ű�� ����.
                    joints = new GameObject[jointsCount];

                    for (int i = 0; i < joints.Length; i++)
                    {
                        joints[i] = Instantiate(jointPrefab) as GameObject;
                        joints[i].transform.parent = transform;
                        joints[i].name = ((KinectInterop.JointType)i).ToString();
                        joints[i].SetActive(false);
                    }
                }
                //(���� ���ἱ)LineRenderer ��ü�迭 ����
                // array holding the skeleton lines
                lines = new LineRenderer[jointsCount];
            }

            // y������ 180�� ȸ����ŵ�ϴ� - ������ �ſ� ȿ���� ������. 
            // always mirrored
            initialRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

            //if (!foregroundCamera)
            //{
            //    // by default - the main camera
            //    foregroundCamera = Camera.main;
            //}
        }


        protected virtual void Update()
        {
            //KinectManager �ν��Ͻ��� ����, �ʱ�ȭ�Ǿ����� Ȯ��
            if (kinectManager && kinectManager.IsInitialized())
            {
                // ���� ī�޶� �����Ǿ� �ִٸ�,
                if (foregroundCamera)
                {
                    // ����� �ȼ� ������ �����´�.
                    // get the background rectangle (use the portrait background, if available)
                    backgroundRect = foregroundCamera.pixelRect;
                    PortraitBackground portraitBack = PortraitBackground.Instance;

                    //��Ʈ����Ʈ ����� ����/Ȱ��ȭ�Ǿ� �ִٸ�,
                    if (portraitBack && portraitBack.enabled)
                    {
                        // ��Ʈ����Ʈ ������ ������.
                        backgroundRect = portraitBack.GetBackgroundRect();
                    }

                }

                // overlay all joints in the skeleton
                //����̰����� �Ǿ��ٸ�,
                if (kinectManager.IsUserDetected(playerIndex))
                {
                    //0�� �÷��̾�� �ε��� ���� �ҷ��Ͷ�.
                    ulong userId = kinectManager.GetUserIdByIndex(playerIndex);
                    //�������� ���� ������ ���� ���� �����´�.
                    int jointsCount = kinectManager.GetJointCount();

                    //Debug.Log("Displaying user " + playerIndex + ", ID: " + userId + 
                    //    ", body: " + kinectManager.GetBodyIndexByUserId(userId) + ", pos: " + kinectManager.GetJointKinectPosition(userId, 0));

                    for (int i = 0; i < jointsCount; i++)
                    {
                        int joint = i;
                        // ���͸��� ���� ����Ʈ�� ���� Ȯ��.
                        if (filteredOutJoints.Contains(joint))
                        {
                            // ���� �ݺ������� �Ѿ. 
                            continue;
                        }

                        //�߿��� �Լ�. GetJointTrackcingState.
                        //���� ����� �νĵǸ鼭 ID, ������ �ν��Ѵ�. Tracked �̻��� ��쿡�� �ð�ȭ �۾��� ����.
                        if (kinectManager.GetJointTrackingState(userId, joint) >= KinectInterop.TrackingState.Tracked)
                        {
                            //�� ���� posJoint�� �����.
                            Vector3 posJoint = GetJointPosition(userId, joint);
                            //Debug.Log("U " + userId + " " + (KinectInterop.JointType)joint + " - pos: " + posJoint);

                            if (sensorTransform)
                            { 
                                posJoint = sensorTransform.TransformPoint(posJoint);
                            }

                            if (joints != null)
                            {
                                //�ش� ���� ���� ������Ʈ�� Ȱ��ȭ�ϰ� ��ġ �� ȸ���� ����
                                // overlay the joint
                                if (posJoint != Vector3.zero)
                                {
                                    joints[i].SetActive(true);
                                    
                                    joints[i].transform.position = posJoint;

                                    Quaternion rotJoint = kinectManager.GetJointOrientation(userId, joint, false);
                                    rotJoint = initialRotation * rotJoint;
                                    joints[i].transform.rotation = rotJoint;

                                    //if(i == (int)KinectInterop.JointType.WristLeft)
                                    //{
                                    //    Debug.Log(string.Format("USO {0:F3} {1} user: {2}, state: {3}\npos: {4}, rot: {5}", Time.time, (KinectInterop.JointType)i,
                                    //        userId, kinectManager.GetJointTrackingState(userId, joint),
                                    //        kinectManager.GetJointPosition(userId, joint), kinectManager.GetJointOrientation(userId, joint, false).eulerAngles));
                                    //}
                                }
                                else
                                {
                                    //�ν��� �ȵǾ����� �Ⱥ��̵��� ��.
                                    joints[i].SetActive(false);
                                }
                            }
                            //���� �׷����� �۾�.
                            
                            //LineRenderer�� �ʱ�ȭ�ϴ� �۾�.
                            //LineRenderer�� ���� �������� �ʰ� linePrefab�� �����Ǿ��ִٸ�,
                            if (lines[i] == null && linePrefab != null)
                            {
                                //�������� �����Ͽ� ���ο� ������Ʈ ����
                                lines[i] = Instantiate(linePrefab) as LineRenderer;
                                //������ ������Ʈ�� �θ� ��ũ��Ʈ transform���� ����
                                lines[i].transform.parent = transform;
                                //������ ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
                                lines[i].gameObject.SetActive(false);
                            }

                            //LineRenderer�� �����Ǿ��ִٸ�,
                            if (lines[i] != null)
                            {
                                // overlay the line to the parent joint
                                //�θ� ������ ����� ���̷����� �����ش�.
                                //�θ� ���� �ε��� �ҷ��� �� ���� ��ġ�� �ҷ��´�.
                                int jointParent = (int)kinectManager.GetParentJoint((KinectInterop.JointType)joint);
                                Vector3 posParent = GetJointPosition(userId, jointParent);

                                //posParent ���͸� sensorTransform�� ��ǥ��� ��ȯ�� �� �� ����� ��ȯ�մϴ�. 
                                if (sensorTransform)
                                {
                                    //�θ� ���� ��ǥ�踦 ���ӿ��� ������ �� �ִ� ��ǥ��� ��ȯ.
                                    posParent = sensorTransform.TransformPoint(posParent);
                                }

                                //�θ� �� �۵��ϰ� �ִ���(��� ���� ���°� Tracked �̻��� ���) üũ�Ѵ�
                                if (posJoint != Vector3.zero && posParent != Vector3.zero &&
                                    kinectManager.GetJointTrackingState(userId, jointParent) >= KinectInterop.TrackingState.Tracked)
                                {
                                    lines[i].gameObject.SetActive(true);
                                    //lines[i].SetVertexCount(2);
                                    lines[i].SetPosition(0, posParent);
                                    lines[i].SetPosition(1, posJoint);
                                }
                                else
                                {
                                    lines[i].gameObject.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            //�۵��� �׷��� �ȵȴٸ�, ���̳� ������ �Ⱥ��̰� �Ѵ�.
                            if (joints[i] != null)
                            {
                                joints[i].SetActive(false);
                            }

                            if (lines[i] != null)
                            {
                                lines[i].gameObject.SetActive(false);
                            }
                        }
                    }

                }
                else
                {
                    //���� ������Ų��.
                    // disable the skeleton
                    int jointsCount = kinectManager.GetJointCount();

                    for (int i = 0; i < jointsCount; i++)
                    {
                        if (joints[i] != null)
                        {
                            joints[i].SetActive(false);
                        }

                        if (lines[i] != null)
                        {
                            lines[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        // returns body joint position
        //Ű��Ʈ�κ��� ��ġ���� �ҷ����� �Լ�. 
        private Vector3 GetJointPosition(ulong userId, int joint)
        {
            Vector3 posJoint = Vector3.zero;

            //���� ī�޶� �����Ǿ� �ִٸ�,
            if (foregroundCamera)
            {
                //ī�޶� �°� ������ ��ġ�� ã�� �Լ�. 
                posJoint = kinectManager.GetJointPosColorOverlay(userId, joint, sensorIndex, foregroundCamera, backgroundRect);
            }
            else if (sensorTransform)
            {
                //������ ��ȯ�� Kinect ��ġ���� �ҷ�������.
                posJoint = kinectManager.GetJointKinectPosition(userId, joint, true);
            }
            else
            {
                //�⺻�� Kinect ��ǥ���� �ҷ���.
                posJoint = kinectManager.GetJointPosition(userId, joint);
            }

            return posJoint;
        }

    }
}

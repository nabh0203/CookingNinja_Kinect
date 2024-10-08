using UnityEngine;
using System.Collections;
using System;
using com.rfilkov.kinect;


namespace com.rfilkov.components
{
    /// <summary>
    /// JointOverlayer overlays the given body joint with the given virtual object.
    /// </summary>
    public class ThrowBall3 : MonoBehaviour
    {
        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        public int playerIndex = 0;

        [Tooltip("������ ����")]
        public KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandRight;

        [Tooltip("������ ��� ����")]
        public KinectInterop.JointType trackedJoint2 = KinectInterop.JointType.ShoulderRight;

        [Tooltip("Game object used to overlay the joint.")]
        public Transform overlayObject;

        [Tooltip("Whether to rotate the overlay object, according to the joint rotation.")]
        public bool rotateObject = true;

        [Tooltip("Smooth factor used for joint rotation.")]
        [Range(0f, 10f)]
        public float rotationSmoothFactor = 10f;

        [Tooltip("Depth sensor index used for color camera overlay - 0 is the 1st one, 1 - the 2nd one, etc.")]
        public int sensorIndex = 0;

        [Tooltip("Camera that will be used to overlay the 3D-objects over the background.")]
        public Camera foregroundCamera;

        [Tooltip("Scene object that will be used to represent the sensor's position and rotation in the scene.")]
        public Transform sensorTransform;

        [Tooltip("Horizontal offset in the object's position with respect to the object's x-axis.")]
        [Range(-0.5f, 0.5f)]
        public float horizontalOffset = 0f;

        [Tooltip("Vertical offset in the object's position with respect to the object's y-axis.")]
        [Range(-0.5f, 0.5f)]
        public float verticalOffset = 0f;

        [Tooltip("Forward offset in the object's position with respect to the object's z-axis.")]
        [Range(-0.5f, 0.5f)]
        public float forwardOffset = 0f;

        //public UnityEngine.UI.Text debugText;

        [NonSerialized]
        public Quaternion initialRotation = Quaternion.identity;
        private bool objMirrored = false;

        // reference to KM
        private KinectManager kinectManager = null;

        // background rectangle
        private Rect backgroundRect = Rect.zero;

        //������ ������ ����� ������ ������ ��
        float handShoulderGap;
        //������ ������ ����� ������ ������ ��
        float prevHandShoulderGap;
        //���� ���������� Ȯ���ϴ� ����
        bool isThrown = false;
        //������ �� ��ġ
        Vector3 handPos;
        //������ �� ��ġ
        Vector3 prevHandPos;
        public void Start()
        {
            // get reference to KM
            //kinectManager ȣ��� KinectManager�� ���� ���� �޾ƿ´�.
            // Start �� KinectManager�� Ȱ��ȭ�� �ǰ� �ν��Ͻ��� �����Ѵ�.
            //�غ�ܰ�
            kinectManager = KinectManager.Instance;

            //if (!foregroundCamera)
            //{
            //    // by default - the main camera
            //    foregroundCamera = Camera.main;
            //}

            //���� overlayObject�� ���� ���ٸ�
            if (overlayObject == null)
            {
                //overlayObject �� transform�̴�.
                // by default - the current object
                overlayObject = transform;
            }
            else
            {
                //transform ������ ���� 0�̴�.
                transform.position = Vector3.zero;
                //transform ȸ������ �⺻���̴�.
                transform.rotation = Quaternion.identity;
            }
            //����rotateObject �� overlayObject �� �ִٸ�
            if (rotateObject && overlayObject)
            {
                // always mirrored
                //�ſ�� ���� ������
                initialRotation = overlayObject.rotation; // Quaternion.Euler(new Vector3(0f, 180f, 0f));
                //vForward�� ����Ƽ ����ī�޶� ������ ����Ƽ ����ī�޶��� �����̰� �ƴϸ� 3������ �����̴�.
                Vector3 vForward = foregroundCamera ? foregroundCamera.transform.forward : Vector3.forward;
                // overlayObject�� vForward�� �ݴ� �������� �ƴ��� �Ǵ��Ͽ� objMirrored�� true�� �˴ϴ�.
                objMirrored = (Vector3.Dot(overlayObject.forward, vForward) < 0);

                //overlayObject�� ȸ������ �⺻���̴�.
                overlayObject.rotation = Quaternion.identity;
            }
        }

        void Update()
        {
            //Ű��Ʈ�� �غ� �ư� �ʱ�ȭ�� �� �Ǿ��ٸ�?
            if (kinectManager && kinectManager.IsInitialized())
            {
                //����Ƽ�� ����ī�޶� ����Ǿ� �ִ���?
                if (foregroundCamera)
                {
                    // get the background rectangle (use the portrait background, if available)
                    backgroundRect = foregroundCamera.pixelRect;
                    PortraitBackground portraitBack = PortraitBackground.Instance;

                    if (portraitBack && portraitBack.enabled)
                    {
                        backgroundRect = portraitBack.GetBackgroundRect();
                    }
                }

                //userId = ���� �ѹ� ���̵� ���� ����
                // overlay the joint
                ulong userId = kinectManager.GetUserIdByIndex(playerIndex);

                //������
                int iJointIndex = (int)trackedJoint;
                //������ �����
                int iJointIndex2 = (int)trackedJoint2;
                //userId �� ���� ���� iJointIndex �� ���� ������ �޾ƿ´�.
                //���� �հ� ����� �����ϱ� ���� �ڵ� �ۼ�
                if (kinectManager.IsJointTracked(userId, iJointIndex) && kinectManager.IsJointTracked(userId, iJointIndex2))
                {
                    //Vector3 posJoint�� ���������� ��ġ������ ����Ƽ�� ī�޶� ���� ���´�
                    //�Ʒ��� �ڵ忡�� ? �� if else ���� ���ٷ� ���� ����� �ϳ� �̴�.
                    // ���� ���� �ؼ� ���ǹ� �������̶�� �����ϸ�ȴ�.
                    // 1�� 0���� ũ�� 2 ������ 1�� �־�� ��� �ؼ��� ���´�.
                    //int a = 1 > 0 ? 2 : 1;
                    //���� ���ǹ��� ����Ѵٰ� ���� �ȴ�.
                    Vector3 posJoint = foregroundCamera ?
                        //GetJointPosColorOverlay�� ������ ���̵� �� ���� ���� Ű��Ʈī�޶�,Ư�� Ű��Ʈ�� �ε��� ��,����Ƽ ���ӽ��� ���� ������ ã��
                        //posJoint �ȿ� �־��ش�.
                        //foregroundCamera: foregroundCamera�� true�� ���
                        //kinectManager.GetJointPosColorOverlay() �Լ��� ȣ���Ͽ� ������� ���� ��ġ�� �����´�.
                        //Ű��ƽ ī�޶󿡼� ��Ÿ ������(M) ã�Ƴ���.
                        //GetJointPosColorOverlay ���� ���� ���迡���� ��ġ
                        kinectManager.GetJointPosColorOverlay(userId, iJointIndex, sensorIndex, foregroundCamera, backgroundRect) :

                        //foregroundCamera: foregroundCamera�� false�̰� sensorTransform�� true�� ���
                        //kinectManager.GetJointKinectPosition() �Լ��� ȣ���Ͽ� ������� ���� ��ġ�� Kinect ��ǥ�迡�� ��������
                        //�� �Լ��� ����� ID(userId), ���� �ε���(iJointIndex), Kinect ��ǥ��� ��ȯ���� ����(true) ���� ���ڷ� �޴´�.
                        sensorTransform ? kinectManager.GetJointKinectPosition(userId, iJointIndex, true) :

                        //�� ���� ��� (foregroundCamera�� false�̰� sensorTransform�� false�� ���)
                        //kinectManager.GetJointPosition() �Լ��� ȣ���Ͽ� ������� ���� ��ġ�� Unity ���� ��ǥ�迡�� ������
                        //�� �Լ��� ����� ID(userId)�� ���� �ε���(iJointIndex)���� ���ڷ� �޴´�.
                        //GetJointPosition �������迡���� ��ġ
                        kinectManager.GetJointPosition(userId, iJointIndex);

                    //���� Ű��Ʈ ī�޶� ����Ǿ� �ִٸ�
                    if (sensorTransform)
                    {
                        //posJoint�� ���� sensorTransform �� TransformPoint ���� ������ �޾ƿ´�.
                        posJoint = sensorTransform.TransformPoint(posJoint);
                    }
                    //���� posJoint �� ��ġ ������ 0,0,0 �� �ƴϰ� overlayObject �� �׸����� �����ϰ�
                    //posJoint �� ���� ���� �ʾҴٸ�
                    if (posJoint != Vector3.zero && overlayObject)
                    {
                        //horizontalOffset(X��) �� ���� �Ǽ��� 0f �� �ƴϸ�
                        //Offset�� ������ ��ġ���� ��� ���� ���̸� ����
                        //�ַ� ��� ��ġ ���� �޾ƿ� �ű⿡ ������Ʈ�� ���� ���� ������ ���ߴ� ��ġ���� ��� ���̰��� ���� �׸����� ���󰡱� ���� ����(����)
                        //horizontal ���� ���� vertical �� ���� ���� forward�� ������ ���Ѵ�.
                        //horizontal  = X��  vertical = Y�� forward = Z�� �� ���Ѵ�.
                        if (horizontalOffset != 0f)
                        {
                            // add the horizontal offset
                            //��� ���� �� ��ġ ���� GetJointPosition �� ���� ������ ������ ����� ������ ���� ����� ���� �� ���� ���̴�.
                            //Ű��ƽ ī�޶� ���� ����� ������⿡ ���� ���� ����(X��)�� �����Ѵ�. 
                            Vector3 dirShoulders = kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight) -
                            kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderLeft);
                            //Vector3 dirHorizOfs = overlayObject.InverseTransformDirection(new Vector3(horizontalOffset, 0, 0));
                            //posJoint ���� ��� ���� ���� �Ǽ����� ���� ���� ���� ���̴�.(���Ͱ� ����)
                            //dirShoulders�� ���̸� 1�� �����. normalized(���̸� 1�� ���鶧 ���)
                            posJoint += dirShoulders.normalized * horizontalOffset;  // dirHorizOfs;
                        }
                        // verticalOffset(Y��)
                        if (verticalOffset != 0f)
                        {
                            // add the vertical offset
                            //����� �������� ô�� ���� ������� ������ �޾� ���� (Y��)�� ����
                            Vector3 dirSpine = kinectManager.GetJointPosition(userId, KinectInterop.JointType.Neck) -
                                kinectManager.GetJointPosition(userId, KinectInterop.JointType.Pelvis);
                            //Vector3 dirVertOfs = overlayObject.InverseTransformDirection(new Vector3(0, verticalOffset, 0));
                            posJoint += dirSpine.normalized * verticalOffset;  // dirVertOfs;
                        }
                        //forward(Z��)
                        if (forwardOffset != 0f)
                        {
                            // add the forward offset
                            //��� ����� ô�� ������ ���Ͱ��� ���� ���̸� 1�� ������ �������(Z��) ���� ������ �ִ�.
                            Vector3 dirShoulders = (kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight) -
                                kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderLeft)).normalized;
                            Vector3 dirSpine = (kinectManager.GetJointPosition(userId, KinectInterop.JointType.Neck) -
                                kinectManager.GetJointPosition(userId, KinectInterop.JointType.Pelvis)).normalized;
                            //Vector3.Cross�� ������ �����̴�.
                            //�ռ� X��� Y���� ���Ͽ� Z���� ���� �޾ƿ´�.
                            Vector3 dirForward = Vector3.Cross(dirShoulders, dirSpine).normalized;
                            //Vector3 dirFwdOfs = overlayObject.InverseTransformDirection(new Vector3(0, 0, forwardOffset));
                            posJoint += dirForward * forwardOffset;  // dirFwdOfs;
                        }
                        //���� ���� �ȴ������ٸ�
                        if (!isThrown)
                            //�׸��� �����ǿ� ���� ������ �Է��ض�.
                            overlayObject.position = posJoint;


                        //ȸ������ �����ϴ� �ڵ�
                        if (rotateObject)
                        {
                            //GetJointOrientation Ű��Ʈ ������ ȸ������ ������ �Լ�
                            Quaternion rotJoint = kinectManager.GetJointOrientation(userId, iJointIndex, !objMirrored);
                            rotJoint = initialRotation * rotJoint;

                            overlayObject.rotation = rotationSmoothFactor > 0f ?
                                Quaternion.Slerp(overlayObject.rotation, rotJoint, rotationSmoothFactor * Time.deltaTime) : rotJoint;
                        }
                        //������ ����� ��ġ�� �޾ƿ´�
                        Vector3 ShoulderPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight);
                        //������ �� ��ġ�� �޾ƿ´�.
                        handPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.HandRight);
                        //handShoulderGap�� ���� ShoulderPos.z ���� handPos.z�� �� ���̴�.
                        //��ġ�� �ٲ� ������ z���� ���Ҷ� X Y �� ���ؼ� ������ ����� �����̴�.
                        handShoulderGap = ShoulderPos.z - handPos.z;
                        //���� ���� ��� ������ ���ٸ�
                        if (handShoulderGap > 0)
                        {
                            //�׸����� ���������� �ٲٰ�
                            overlayObject.GetComponent<Renderer>().material.color = Color.red;
                            //���� prevHandShoulderGap�� 0 ���� ������
                            if (prevHandShoulderGap < 0)
                            {
                                //�׸����� �ӵ����� 0���� ������
                                overlayObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                //force�� ���� ������ �� ��ġ���� ������ �� ��ġ ���� �� ���� 600f�� ���Ѱ��̴�.
                                Vector3 force = (handPos - prevHandPos) * 1000f;
                                //force��ŭ �о��
                                overlayObject.GetComponent<Rigidbody>().AddForce(force);
                                //isThrown�� ������ ��ȯ
                                isThrown = true;
                            }

                        }
                        else
                        {
                            //�ƴ϶�� �ʷϻ����� �ٲ��
                            overlayObject.GetComponent<Renderer>().material.color = Color.green;
                            //���� �׸����� z�� ����  -10 ���� ������
                            if (overlayObject.position.z < -10f)
                            {
                                //isThrown �������� ��ȯ
                                isThrown = false;
                            }

                        }
                        //����� ������ prevHandShoulderGap �� handShoulderGap �̴�.
                        prevHandShoulderGap = handShoulderGap;
                        //����� ������ prevHandPos�� ���� handPos �̴�.
                        prevHandPos = handPos;
                        /*
                        overlayObject.GetComponent<Renderer>().material.color = handShoulderGap > 0 ? Color.red : Color.green;
                        */
                    }

                }
                else
                {
                    //overlayObject�� ������Ʈ ���� GreenBall�� ����
                    // make the overlay object invisible
                    if (overlayObject && overlayObject.position.z > 0f)
                    {
                        Vector3 posJoint = overlayObject.position;
                        posJoint.z = -10f;
                        overlayObject.position = posJoint;
                    }
                }

            }
        }
    }
}

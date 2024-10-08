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
        //플레이어 인덱스(0 = 1번째, 1 = 2번째 . . .)
        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        public int playerIndex = 0;

        //관절을 오버레이할 게임 오브젝트
        [Tooltip("Game object used to overlay the joints.")]
        public GameObject jointPrefab;

        //뼈를 오버레이할 선 객체 
        [Tooltip("Line object used to overlay the bones.")]
        public LineRenderer linePrefab;
        //public float smoothFactor = 10f;

        //깊이 센서 인덱스(0 = 1번째, 1 = 2번째 . . .)
        [Tooltip("Depth sensor index used for color frame overlay - 0 is the 1st one, 1 - the 2nd one, etc.")]
        public int sensorIndex = 0; //우리가 사용할 GMD

        //유니티 상 가상카메라. 위치, 정보들
        //배경에 3D 오브젝트를 오버레이하기 위해 사용될 카메라
        [Tooltip("Camera that will be used to overlay the 3D-objects over the background.")]
        public Camera foregroundCamera;

        //씬에서 센서의 위치, 회전값을 나타낼 transform
        [Tooltip("Scene object that will be used to represent the sensor's position and rotation in the scene.")]
        public Transform sensorTransform;

        //실제정보 - 카메라 
        //가상정보 - 스켈레톤 등...
        //public UnityEngine.UI.Text debugText;

        // list of filtered-out joints
        //private - 순수하게 내부적
        //protected - 상속

        // list of filtered-out joints
        //리스트 형태로 정수를 저장. 추척을 안할 관절 인덱스 지정!
        protected List<int> filteredOutJoints = new List<int>();

        // joints & lines
        //관절과 뼈다리를 시각화하기 위한 객체들의 배열
        protected GameObject[] joints = null;
        protected LineRenderer[] lines = null;

        // initial body rotation
        //스켈레톤 모델의 초기 회전값을 저장. 사람 인식이 처음 될 때 설정됨.
        protected Quaternion initialRotation = Quaternion.identity;

        // reference to KM
        //Kinect 센서를 제어하고 데이터를 가져옴.
        protected KinectManager kinectManager = null;

        // background rectangle
        //Rect - 영역, 배경영역을 설정
        protected Rect backgroundRect = Rect.zero;


        protected virtual void Start()
        {
            //KinectManager을 kinectManager에 저장
            //KinectManager= Kinect 센서 제어, 데이터를 읽어오는 클래스
            kinectManager = KinectManager.Instance;

            //KinectManager 인스턴스가 존재, 초기화되었는지 확인
            if (kinectManager && kinectManager.IsInitialized())
            {
                //센서가 추적할 수 있는 관절 개수를 갖고와라.
                int jointsCount = kinectManager.GetJointCount();

                //관절 프리팹이 존재하면, 생성돈 오브젝트에 joints 배열 저장
                if (jointPrefab)
                {
                    // array holding the skeleton joints
                    //일을 시키는 구간.
                    joints = new GameObject[jointsCount];

                    for (int i = 0; i < joints.Length; i++)
                    {
                        joints[i] = Instantiate(jointPrefab) as GameObject;
                        joints[i].transform.parent = transform;
                        joints[i].name = ((KinectInterop.JointType)i).ToString();
                        joints[i].SetActive(false);
                    }
                }
                //(관절 연결선)LineRenderer 객체배열 생성
                // array holding the skeleton lines
                lines = new LineRenderer[jointsCount];
            }

            // y축으로 180도 회전시킵니다 - 씬에서 거울 효과를 보여줌. 
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
            //KinectManager 인스턴스가 존재, 초기화되었는지 확인
            if (kinectManager && kinectManager.IsInitialized())
            {
                // 전경 카메라가 설정되어 있다면,
                if (foregroundCamera)
                {
                    // 배경의 픽셀 영역을 가져온다.
                    // get the background rectangle (use the portrait background, if available)
                    backgroundRect = foregroundCamera.pixelRect;
                    PortraitBackground portraitBack = PortraitBackground.Instance;

                    //포트레이트 배경이 설정/활성화되어 있다면,
                    if (portraitBack && portraitBack.enabled)
                    {
                        // 포트레이트 영역을 가져옴.
                        backgroundRect = portraitBack.GetBackgroundRect();
                    }

                }

                // overlay all joints in the skeleton
                //사람이감지가 되었다면,
                if (kinectManager.IsUserDetected(playerIndex))
                {
                    //0번 플레이어에게 인덱스 값을 불러와라.
                    ulong userId = kinectManager.GetUserIdByIndex(playerIndex);
                    //센서에서 추적 가능한 관절 개수 가져온다.
                    int jointsCount = kinectManager.GetJointCount();

                    //Debug.Log("Displaying user " + playerIndex + ", ID: " + userId + 
                    //    ", body: " + kinectManager.GetBodyIndexByUserId(userId) + ", pos: " + kinectManager.GetJointKinectPosition(userId, 0));

                    for (int i = 0; i < jointsCount; i++)
                    {
                        int joint = i;
                        // 필터링된 관절 리스트에 관절 확인.
                        if (filteredOutJoints.Contains(joint))
                        {
                            // 다음 반복문으로 넘어감. 
                            continue;
                        }

                        //중요한 함수. GetJointTrackcingState.
                        //현재 사람이 인식되면서 ID, 관절을 인식한다. Tracked 이상인 경우에만 시각화 작업을 수행.
                        if (kinectManager.GetJointTrackingState(userId, joint) >= KinectInterop.TrackingState.Tracked)
                        {
                            //그 값이 posJoint로 저장됨.
                            Vector3 posJoint = GetJointPosition(userId, joint);
                            //Debug.Log("U " + userId + " " + (KinectInterop.JointType)joint + " - pos: " + posJoint);

                            if (sensorTransform)
                            { 
                                posJoint = sensorTransform.TransformPoint(posJoint);
                            }

                            if (joints != null)
                            {
                                //해당 관절 게임 오브젝트를 활성화하고 위치 및 회전을 설정
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
                                    //인식이 안되었으면 안보이도록 함.
                                    joints[i].SetActive(false);
                                }
                            }
                            //선이 그려지는 작업.
                            
                            //LineRenderer을 초기화하는 작업.
                            //LineRenderer이 아직 생성되지 않고 linePrefab이 설정되어있다면,
                            if (lines[i] == null && linePrefab != null)
                            {
                                //프리팹을 복제하여 새로운 오브젝트 생성
                                lines[i] = Instantiate(linePrefab) as LineRenderer;
                                //생성된 오브젝트의 부모를 스크립트 transform으로 설정
                                lines[i].transform.parent = transform;
                                //생성된 오브젝트를 비활성화한다.
                                lines[i].gameObject.SetActive(false);
                            }

                            //LineRenderer이 생성되어있다면,
                            if (lines[i] != null)
                            {
                                // overlay the line to the parent joint
                                //부모 관절과 연결된 스켈레톤을 보여준다.
                                //부모 관절 인덱스 불러온 후 관절 위치를 불러온다.
                                int jointParent = (int)kinectManager.GetParentJoint((KinectInterop.JointType)joint);
                                Vector3 posParent = GetJointPosition(userId, jointParent);

                                //posParent 벡터를 sensorTransform의 좌표계로 변환한 후 그 결과를 반환합니다. 
                                if (sensorTransform)
                                {
                                    //부모 관절 좌표계를 게임에서 적용할 수 있는 좌표계로 변환.
                                    posParent = sensorTransform.TransformPoint(posParent);
                                }

                                //부모가 잘 작동하고 있는지(모두 추적 상태가 Tracked 이상인 경우) 체크한다
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
                            //작동이 그래도 안된다면, 선이나 관절을 안보이게 한다.
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
                    //모든걸 해제시킨다.
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
        //키넥트로부터 위치값을 불러오는 함수. 
        private Vector3 GetJointPosition(ulong userId, int joint)
        {
            Vector3 posJoint = Vector3.zero;

            //전경 카메라가 설정되어 있다면,
            if (foregroundCamera)
            {
                //카메라에 맞게 관절의 위치를 찾는 함수. 
                posJoint = kinectManager.GetJointPosColorOverlay(userId, joint, sensorIndex, foregroundCamera, backgroundRect);
            }
            else if (sensorTransform)
            {
                //센서에 변환된 Kinect 위치값이 불러와진다.
                posJoint = kinectManager.GetJointKinectPosition(userId, joint, true);
            }
            else
            {
                //기본값 Kinect 좌표값을 불러옴.
                posJoint = kinectManager.GetJointPosition(userId, joint);
            }

            return posJoint;
        }

    }
}

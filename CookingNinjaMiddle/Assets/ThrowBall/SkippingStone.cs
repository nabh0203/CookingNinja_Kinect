using UnityEngine;
using System.Collections;
using System;
using com.rfilkov.kinect;


namespace com.rfilkov.components
{
    /// <summary>
    /// JointOverlayer overlays the given body joint with the given virtual object.
    /// </summary>
    public class SkippingStone : MonoBehaviour
    {
        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        public int playerIndex = 0;

        [Tooltip("오른손 관절")]
        public KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandRight;
        [Tooltip("오른쪽 어깨 관절")]
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

        [Tooltip("The strength float amout to force the ball. 공에게 가하는 힘을 입력.")]      
        public float strength = 200f;

        //현재 손과어깨 갭차이 위치값
        float handShoulderGap;
        //과거 손과어깨 갭차이 위치값
        float prevHandShoulderGap;
        /*float handShoulderGap2;
        float prevHandShoulderGap2;*/
        //던져졌는지 안던져졌는지 bool 값
        bool isThrown;
        //현재 손 위치값
        Vector3 handPos;
        //과거 손 위치값
        Vector3 prevHandPos;

        public GameObject GreenBall;
        

        //public UnityEngine.UI.Text debugText;

        [NonSerialized]
        public Quaternion initialRotation = Quaternion.identity;
        private bool objMirrored = false;

        // reference to KM
        private KinectManager kinectManager = null;

        // background rectangle
        private Rect backgroundRect = Rect.zero;


        public void Start()
        {
            // get reference to KM 키네틱매니저 시작
            kinectManager = KinectManager.Instance;

            //if (!foregroundCamera)
            //{
            //    // by default - the main camera
            //    foregroundCamera = Camera.main;
            //}

            if(overlayObject == null)
            {
                // by default - the current object
                overlayObject = transform;
            }
            else
            {
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }

            if (rotateObject && overlayObject)
            {
                // always mirrored
                initialRotation = overlayObject.rotation; // Quaternion.Euler(new Vector3(0f, 180f, 0f));

                Vector3 vForward = foregroundCamera ? foregroundCamera.transform.forward : Vector3.forward;
                objMirrored = (Vector3.Dot(overlayObject.forward, vForward) < 0);

                overlayObject.rotation = Quaternion.identity;
            }
        }

        void Update()
        {
            if (kinectManager && kinectManager.IsInitialized())
            {
                //메인 카메라(유니티 내 결과/게임씬 카메라)가 지정되어 있다면,
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

                // overlay the joint
                //플레이어가 존재하는지 인덱스에서 값을 불러온다.
                ulong userId = kinectManager.GetUserIdByIndex(playerIndex);

                //Joint는 0~?번까지 존재. (오른손은 ?번째 등)
                int iJointIndex = (int)trackedJoint;
                //어깨 오른쪽
                int iJointIndex2 = (int)trackedJoint2;
                //손(과 어깨도)joint가 감지가 되면. 
                if (kinectManager.IsJointTracked(userId, iJointIndex) && kinectManager.IsJointTracked(userId, iJointIndex2))
                {
                    //posJoint = 선택한 Joint 좌표값

                    //? = 특별한 조건문 - if, else를 한 줄에 작성한 것/true면 1번째값, false면 두번째값을 변수에 저장하는 방법.
                    //int a = 1 > 0 ? 2: -1;
                    //int a가 1보다 작다. true이면 2 , false이면 -1

                    //if (foregroundCamera){
                    Vector3 posJoint = foregroundCamera ?
                        //GetJointPosColorOverlay - 위치정보를 얻어내는 함수. 증강현실 위치를 알아내는 함수.
                        //손을 감지하고 3차원적 위치에서 보여줌.(유저 아이디, 사람의 어떤 관절, 특정 키넥트의 인덱스값(하나를 쓸 시 0), 게임카메라, 게임화면의 정보)
                        kinectManager.GetJointPosColorOverlay(userId, iJointIndex, sensorIndex, foregroundCamera, backgroundRect) :
                        //if (sensortransform){
                        //키넥트 좌표계를 게임으로 변환할지(유저 ID, Joint 인덱스, true(여부받아오기)
                        sensorTransform ? kinectManager.GetJointKinectPosition(userId, iJointIndex, true) : 
                        kinectManager.GetJointPosition(userId, iJointIndex);
                        //}
                    //}
                    if (sensorTransform)
                    {
                        posJoint = sensorTransform.TransformPoint(posJoint);
                    }
                    //만약 값을 가져왔고 greenball을 가져온상태면,
                    if (posJoint != Vector3.zero && overlayObject)
                    {
                        //정의해준 위치보다 조금 더 벗어난 곳으로 지정
                        //가로방향으로 조금 벗어나서 차이를 두고 조정을 원하다. (x)
                        if (horizontalOffset != 0f)
                        {
                            // add the horizontal offset
                            //어깨방향
                            //관절의 위치를 카메라 기준으로 오른쪽 어깨 조인트를 찾음. 사람 기준으로 인식이 되는 것이다.
                            //방향을 어디를 바라보던 어깨를 기준으로 x축.
                            Vector3 dirShoulders = kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight) -
                                kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderLeft);
                            //Vector3 dirHorizOfs = overlayObject.InverseTransformDirection(new Vector3(horizontalOffset, 0, 0));
                            posJoint += dirShoulders.normalized * horizontalOffset;  // dirHorizOfs;
                        }

                        //세로방향으로 조금 벗어나서 차이를 두고 조정을 원하다.(y)
                        if (verticalOffset != 0f)
                        {
                            // add the vertical offset
                            //척추방향
                            //관절의 위치를 카메라 기준으로 목의 조인트를 찾음. 사람 기준으로 인식이 되는 것이다.
                            //방향을 어디를 바라보던 척추를 기준으로 y축.
                            Vector3 dirSpine = kinectManager.GetJointPosition(userId, KinectInterop.JointType.Neck) -
                                kinectManager.GetJointPosition(userId, KinectInterop.JointType.Pelvis);
                            //Vector3 dirVertOfs = overlayObject.InverseTransformDirection(new Vector3(0, verticalOffset, 0));
                            posJoint += dirSpine.normalized * verticalOffset;  // dirVertOfs;
                        }

                        //앞 뒤 방향으로 조금 벗어나서 차이를 두고 조정을 원하다.(z)
                        if (forwardOffset != 0f)
                        {
                            // add the forward offset
                            // 어깨방향과 척추방향을 얻어내고, 이를 베타크로스(*곱셈)
                            //Vector3.Cross - a * b = z축
                            //어깨와 척추방향은 같지만, 길이를 1로 normalize하여(일반화한다) 내가 원하는 z 길이를 만들 수 있는것이다.

                            Vector3 dirShoulders = (kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight) -
                                kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderLeft)).normalized;
                            Vector3 dirSpine = (kinectManager.GetJointPosition(userId, KinectInterop.JointType.Neck) -
                                kinectManager.GetJointPosition(userId, KinectInterop.JointType.Pelvis)).normalized;
                            Vector3 dirForward = Vector3.Cross(dirShoulders, dirSpine).normalized;
                            //Vector3 dirFwdOfs = overlayObject.InverseTransformDirection(new Vector3(0, 0, forwardOffset));
                            posJoint += dirForward * forwardOffset;  // dirFwdOfs;
                        }
                        if (!isThrown)
                        {
                            //overlayPosition에 저장.
                            overlayObject.position = posJoint;
                        }

                        //회전값을 적용할 여부
                        if(rotateObject)
                        {
                            //회전 구하는 함수. 키네틱에서 제공해준다.
                            Quaternion rotJoint = kinectManager.GetJointOrientation(userId, iJointIndex, !objMirrored);
                            rotJoint = initialRotation * rotJoint;

                            overlayObject.rotation = rotationSmoothFactor > 0f ?
                                Quaternion.Slerp(overlayObject.rotation, rotJoint, rotationSmoothFactor * Time.deltaTime) : rotJoint;
                        }

                        /*//카메라 기준으로 오른쪽 어깨와 오른쪽  손의 위치를 인식하고 입력한다.
                        Vector3 shoulderPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight);
                        handPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.HandRight);
                        //갭 차이(손까지의 y좌표값과 어깨까지의 y좌표값을 뻴셈한다)를 지정한다.
                        handShoulderGap = handPos.y - shoulderPos.y;*/
                        //카메라 기준으로 오른쪽 어깨와 오른쪽  손의 위치를 인식하고 입력한다.
                        Vector3 shoulderPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight);
                        handPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.HandRight);
                        //갭 차이(손까지의 y좌표값과 어깨까지의 y좌표값을 뻴셈한다)를 지정한다.

                        //이 부분부터 기존 스크립트에서 수정된 물수제비 코드-------------------------------------------------------------------------
                        //참고!! 키네틱 SDK 시스템에서 z축은 플레이어가 카메라를 바라보는 방향이다.
                        //손과 어깨의 간격을 알기 위해서는, 카메라부터 어깨까지의 길이(shoulderPos.z) - 카메라부터 손까지의 길이(handPos.z)으로 구할 수 있다.
                        //즉, 손을 앞으로 뻗으면 플레이어 기준(0,0,0)에서 양수값이 출력되고, 뒤로 뻗으면 음수값이 출력된다.
                        
                        //손과 어깨의 간격 = z축 어깨좌표값 - z축 손 좌표
                        handShoulderGap = shoulderPos.z - handPos.z ;

                        //만약 간격이 0보다 크면(앞으로 뻗으면)
                        if (handShoulderGap > 0f)
                        {
                            //공을 빨간색으로 바꾼다.
                            overlayObject.GetComponent<Renderer>().material.color = Color.red;
                            //만약 과거 손어깨 간격이 0보다 작았다면(공에 추진력을 가했을 때),
                            if (prevHandShoulderGap < 0)
                            {
                                //힘을 0으로 초기화한 후,
                                overlayObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                //현재 손 위치랑 과거 손 위치 값을 빼고, 벡터의 크기를 1로 만들어주는 normalized를 사용해서
                                //어떤 방향으로 던지고있는지 설정한다.
                                Vector3 force = (handPos - prevHandPos).normalized;

                                //방향 * 힘 을 통해 어떤방향으로 어느정도의 힘을 줄건지 설정한다.
                                overlayObject.GetComponent<Rigidbody>().AddForce(force * strength);

                                isThrown = true;
                                //원래 force = (handPos - prevHandPos) * strength; 이였지만, 벡터의 크기를 1로 만든 후 방향을 지정하면
                                //더 원활하게 날라가도록 조절해주기에 한번 사용해봤다.
                            }


                        }


                        //그 외에는 공이(초록)색을 띄도록 한다.
                        else
                        {
                            //오버레이오브젝트의 컴포넌트<렌더러> 중 메테리얼 컬러를 초록으로 변경.
                            overlayObject.GetComponent<Renderer>().material.color = Color.green;
                            //만약 y좌표(손을)가 -10보다 작게 내리면
                            if (overlayObject.position.z < 10f)
                            {
                                /*Instantiate(GreenBall, handPos, Quaternion.identity);*/
                                //던져짐 이 거짓으로 변경
                                isThrown = false;
                            }
                        }
                        //값을 가져오고 공이 할당되어 일어난 작업이 끝난 후
                        //과거 간격을 현재간격으로, 과거손좌표를 현재 손 좌표로 초기화하여,
                        //다음 작업이 실행될 시 새로운 값을 다시 갖기위해 실행한다.
                        prevHandShoulderGap = handShoulderGap;
                        prevHandPos = handPos;

                    }
                }
                //감지되지 않는상태면,
                else
                {
                    // make the overlay object invisible
                    //게임에서 안보여지게 설정됨.
                    //공이 할당되어있고, 공의 z좌표가 0보다 크면,
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
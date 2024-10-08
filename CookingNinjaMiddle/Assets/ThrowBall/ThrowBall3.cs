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

        //public UnityEngine.UI.Text debugText;

        [NonSerialized]
        public Quaternion initialRotation = Quaternion.identity;
        private bool objMirrored = false;

        // reference to KM
        private KinectManager kinectManager = null;

        // background rectangle
        private Rect backgroundRect = Rect.zero;

        //현재의 오른쪽 어깨와 오른손 사이의 값
        float handShoulderGap;
        //과거의 오른쪽 어깨와 오른손 사이의 값
        float prevHandShoulderGap;
        //공이 던져졌는지 확인하는 변수
        bool isThrown = false;
        //현재의 손 위치
        Vector3 handPos;
        //과거의 손 위치
        Vector3 prevHandPos;
        public void Start()
        {
            // get reference to KM
            //kinectManager 호출시 KinectManager의 변수 값을 받아온다.
            // Start 시 KinectManager가 활성화가 되고 인스턴스를 참조한다.
            //준비단계
            kinectManager = KinectManager.Instance;

            //if (!foregroundCamera)
            //{
            //    // by default - the main camera
            //    foregroundCamera = Camera.main;
            //}

            //만약 overlayObject의 값이 없다면
            if (overlayObject == null)
            {
                //overlayObject 는 transform이다.
                // by default - the current object
                overlayObject = transform;
            }
            else
            {
                //transform 포지션 값은 0이다.
                transform.position = Vector3.zero;
                //transform 회전값은 기본값이다.
                transform.rotation = Quaternion.identity;
            }
            //만약rotateObject 와 overlayObject 가 있다면
            if (rotateObject && overlayObject)
            {
                // always mirrored
                //거울과 같이 돌려라
                initialRotation = overlayObject.rotation; // Quaternion.Euler(new Vector3(0f, 180f, 0f));
                //vForward는 유니티 메인카메라가 있으면 유니티 메인카메라의 전방이고 아니면 3차원의 전방이다.
                Vector3 vForward = foregroundCamera ? foregroundCamera.transform.forward : Vector3.forward;
                // overlayObject가 vForward와 반대 방향인지 아닌지 판단하여 objMirrored는 true가 됩니다.
                objMirrored = (Vector3.Dot(overlayObject.forward, vForward) < 0);

                //overlayObject의 회전값은 기본값이다.
                overlayObject.rotation = Quaternion.identity;
            }
        }

        void Update()
        {
            //키넥트가 준비가 됐고 초기화가 다 되었다면?
            if (kinectManager && kinectManager.IsInitialized())
            {
                //유니티에 메인카메라가 연결되어 있느냐?
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

                //userId = 고유 넘버 아이디 값을 뜻함
                // overlay the joint
                ulong userId = kinectManager.GetUserIdByIndex(playerIndex);

                //오른손
                int iJointIndex = (int)trackedJoint;
                //오른쪽 허벅지
                int iJointIndex2 = (int)trackedJoint2;
                //userId 의 값을 통해 iJointIndex 즉 관절 정보를 받아온다.
                //현재 손과 어깨를 추적하기 위해 코드 작성
                if (kinectManager.IsJointTracked(userId, iJointIndex) && kinectManager.IsJointTracked(userId, iJointIndex2))
                {
                    //Vector3 posJoint는 증강현실의 위치정보를 유니티내 카메라를 통해 얻어온다
                    //아래의 코드에서 ? 는 if else 문을 한줄로 쓰는 방법중 하나 이다.
                    // 쉽게 설명 해서 조건문 요약버전이라고 생각하면된다.
                    // 1이 0보다 크면 2 작으면 1을 넣어라 라는 해설이 나온다.
                    //int a = 1 > 0 ? 2 : 1;
                    //이중 조건문을 사용한다고 보면 된다.
                    Vector3 posJoint = foregroundCamera ?
                        //GetJointPosColorOverlay는 유저의 아이디 와 관절 정보 키넥트카메라,특정 키넥트의 인덱스 값,유니티 게임신의 비율 정보를 찾아
                        //posJoint 안에 넣어준다.
                        //foregroundCamera: foregroundCamera가 true인 경우
                        //kinectManager.GetJointPosColorOverlay() 함수를 호출하여 사용자의 관절 위치를 가져온다.
                        //키네틱 카메라에서 메타 단위로(M) 찾아낸다.
                        //GetJointPosColorOverlay 가상 현실 세계에서의 위치
                        kinectManager.GetJointPosColorOverlay(userId, iJointIndex, sensorIndex, foregroundCamera, backgroundRect) :

                        //foregroundCamera: foregroundCamera가 false이고 sensorTransform이 true인 경우
                        //kinectManager.GetJointKinectPosition() 함수를 호출하여 사용자의 관절 위치를 Kinect 좌표계에서 가져오고
                        //이 함수는 사용자 ID(userId), 관절 인덱스(iJointIndex), Kinect 좌표계로 변환할지 여부(true) 등을 인자로 받는다.
                        sensorTransform ? kinectManager.GetJointKinectPosition(userId, iJointIndex, true) :

                        //그 외의 경우 (foregroundCamera가 false이고 sensorTransform도 false인 경우)
                        //kinectManager.GetJointPosition() 함수를 호출하여 사용자의 관절 위치를 Unity 월드 좌표계에서 가져고
                        //이 함수는 사용자 ID(userId)와 관절 인덱스(iJointIndex)만을 인자로 받는다.
                        //GetJointPosition 물리세계에서의 위치
                        kinectManager.GetJointPosition(userId, iJointIndex);

                    //만약 키넥트 카메라가 연결되어 있다면
                    if (sensorTransform)
                    {
                        //posJoint의 값은 sensorTransform 의 TransformPoint 관절 정보를 받아온다.
                        posJoint = sensorTransform.TransformPoint(posJoint);
                    }
                    //만약 posJoint 의 위치 정보가 0,0,0 이 아니고 overlayObject 즉 그린볼이 존재하고
                    //posJoint 가 실패 하지 않았다면
                    if (posJoint != Vector3.zero && overlayObject)
                    {
                        //horizontalOffset(X축) 의 값이 실수형 0f 가 아니면
                        //Offset은 원래의 위치에서 벗어난 값의 차이를 뜻함
                        //주로 벗어난 위치 값을 받아와 거기에 오브젝트를 부착 시켜 기존에 정했던 위치에서 벗어난 차이값을 구해 그린볼이 따라가기 위해 적용(추측)
                        //horizontal 가로 방향 vertical 는 세로 방향 forward는 전방을 뜻한다.
                        //horizontal  = X축  vertical = Y축 forward = Z축 을 뜻한다.
                        if (horizontalOffset != 0f)
                        {
                            // add the horizontal offset
                            //어깨 관절 의 위치 값은 GetJointPosition 에 관절 정보중 오른쪽 어깨의 값에서 왼쪽 어깨의 값을 뺀 사이 값이다.
                            //키네틱 카메라를 통해 사람의 어깨방향에 따라 가로 방향(X축)을 결정한다. 
                            Vector3 dirShoulders = kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight) -
                            kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderLeft);
                            //Vector3 dirHorizOfs = overlayObject.InverseTransformDirection(new Vector3(horizontalOffset, 0, 0));
                            //posJoint 값에 어깨 정보 값과 실수값을 곱한 값을 더한 값이다.(벡터곱 수식)
                            //dirShoulders의 길이를 1로 만든다. normalized(길이를 1로 만들때 사용)
                            posJoint += dirShoulders.normalized * horizontalOffset;  // dirHorizOfs;
                        }
                        // verticalOffset(Y축)
                        if (verticalOffset != 0f)
                        {
                            // add the vertical offset
                            //사람을 기준으로 척추 부터 목까지의 정보를 받아 세로 (Y축)을 결정
                            Vector3 dirSpine = kinectManager.GetJointPosition(userId, KinectInterop.JointType.Neck) -
                                kinectManager.GetJointPosition(userId, KinectInterop.JointType.Pelvis);
                            //Vector3 dirVertOfs = overlayObject.InverseTransformDirection(new Vector3(0, verticalOffset, 0));
                            posJoint += dirSpine.normalized * verticalOffset;  // dirVertOfs;
                        }
                        //forward(Z축)
                        if (forwardOffset != 0f)
                        {
                            // add the forward offset
                            //어깨 방향과 척추 방향을 벡터곱을 통해 길이를 1로 지정한 전방길이(Z축) 값을 얻을수 있다.
                            Vector3 dirShoulders = (kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight) -
                                kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderLeft)).normalized;
                            Vector3 dirSpine = (kinectManager.GetJointPosition(userId, KinectInterop.JointType.Neck) -
                                kinectManager.GetJointPosition(userId, KinectInterop.JointType.Pelvis)).normalized;
                            //Vector3.Cross는 벡터의 곱셈이다.
                            //앞서 X축과 Y축을 곱하여 Z축의 값을 받아온다.
                            Vector3 dirForward = Vector3.Cross(dirShoulders, dirSpine).normalized;
                            //Vector3 dirFwdOfs = overlayObject.InverseTransformDirection(new Vector3(0, 0, forwardOffset));
                            posJoint += dirForward * forwardOffset;  // dirFwdOfs;
                        }
                        //만약 공이 안던져졌다면
                        if (!isThrown)
                            //그린볼 포지션에 관절 정보를 입력해라.
                            overlayObject.position = posJoint;


                        //회전값을 적용하는 코드
                        if (rotateObject)
                        {
                            //GetJointOrientation 키넥트 내에서 회전값을 얻어오는 함수
                            Quaternion rotJoint = kinectManager.GetJointOrientation(userId, iJointIndex, !objMirrored);
                            rotJoint = initialRotation * rotJoint;

                            overlayObject.rotation = rotationSmoothFactor > 0f ?
                                Quaternion.Slerp(overlayObject.rotation, rotJoint, rotationSmoothFactor * Time.deltaTime) : rotJoint;
                        }
                        //오른쪽 어깨의 위치를 받아온다
                        Vector3 ShoulderPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight);
                        //오른손 의 위치를 받아온다.
                        handPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.HandRight);
                        //handShoulderGap의 값은 ShoulderPos.z 에서 handPos.z를 뺀 값이다.
                        //위치가 바뀐 이유는 z축을 구할때 X Y 를 곱해서 음수로 만들기 위함이다.
                        handShoulderGap = ShoulderPos.z - handPos.z;
                        //만약 손이 어깨 앞으로 갔다면
                        if (handShoulderGap > 0)
                        {
                            //그린볼은 빨간색으로 바꾸고
                            overlayObject.GetComponent<Renderer>().material.color = Color.red;
                            //만약 prevHandShoulderGap가 0 보다 작으면
                            if (prevHandShoulderGap < 0)
                            {
                                //그린볼의 속도값을 0으로 만들어라
                                overlayObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                //force의 값은 현재의 손 위치에서 과거의 손 위치 값을 뺀 값에 600f를 곱한값이다.
                                Vector3 force = (handPos - prevHandPos) * 1000f;
                                //force만큼 밀어내라
                                overlayObject.GetComponent<Rigidbody>().AddForce(force);
                                //isThrown를 참으로 변환
                                isThrown = true;
                            }

                        }
                        else
                        {
                            //아니라면 초록색으로 바꿔라
                            overlayObject.GetComponent<Renderer>().material.color = Color.green;
                            //만일 그린볼의 z축 값이  -10 보다 작으면
                            if (overlayObject.position.z < -10f)
                            {
                                //isThrown 거짓으로 변환
                                isThrown = false;
                            }

                        }
                        //명령이 끝나면 prevHandShoulderGap 은 handShoulderGap 이다.
                        prevHandShoulderGap = handShoulderGap;
                        //명령이 끝나면 prevHandPos의 값은 handPos 이다.
                        prevHandPos = handPos;
                        /*
                        overlayObject.GetComponent<Renderer>().material.color = handShoulderGap > 0 ? Color.red : Color.green;
                        */
                    }

                }
                else
                {
                    //overlayObject는 프로젝트 상의 GreenBall을 뜻함
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

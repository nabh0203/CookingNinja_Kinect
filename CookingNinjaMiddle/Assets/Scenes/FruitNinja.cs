using UnityEngine;
using System.Collections;
using System;
using com.rfilkov.kinect;


namespace com.rfilkov.components
{
    /// <summary>
    /// JointOverlayer overlays the given body joint with the given virtual object.
    /// </summary>
    public class FruitNinja : MonoBehaviour
    {
        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        public int playerIndex = 0;

        [Tooltip("오른손 관절")]
        public KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandRight;
        [Tooltip("오른쪽 어깨 관절")]
        public KinectInterop.JointType trackedJoint2 = KinectInterop.JointType.ShoulderRight;

        [Tooltip("Depth sensor index used for color camera overlay - 0 is the 1st one, 1 - the 2nd one, etc.")]
        public int sensorIndex = 0;

        [Tooltip("공")]
        public GameObject Ball;

        //현재 손과어깨 갭차이 위치값
        float handShoulderGap;
        //과거 손과어깨 갭차이 위치값
        float prevHandShoulderGap;
        
        //던져졌는지 안던져졌는지 bool 값
        bool isThrown;
        //현재 손 위치값
        Vector3 handPos;
        //과거 손 위치값
        Vector3 prevHandPos;

        //public UnityEngine.UI.Text debugText;
                
        // reference to KM
        private KinectManager kinectManager = null;
            
        public void Start()
        {
            // get reference to KM 키네틱매니저 시작
            kinectManager = KinectManager.Instance;
                                   
        }

        void Update()
        {
            if (kinectManager && kinectManager.IsInitialized())
            {

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

                    //카메라 기준으로 오른쪽 어깨와 오른쪽  손의 위치를 인식하고 입력한다.
                    Vector3 shoulderPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight);
                    handPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.HandRight);
                    //갭 차이(손까지의 y좌표값과 어깨까지의 y좌표값을 뻴셈한다)를 지정한다.
                    handShoulderGap = handPos.y - shoulderPos.y;
                    /* //카메라 기준으로 오른쪽 어깨와 오른쪽  손의 위치를 인식하고 입력한다.
                     Vector3 shoulderPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.ShoulderRight);
                         handPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.HandRight);
                         //갭 차이(손까지의 y좌표값과 어깨까지의 y좌표값을 뻴셈한다)를 지정한다.

                         handShoulderGap = handPos.y - shoulderPos.y;*/


                    
                    if (handShoulderGap > 0)//손이 어깨보다 높을 때,
                    {
                        //만약 어깨보다 위로 올라가면, 공을 빨간색으로 바꾼다.
                        Ball.GetComponent<Renderer>().material.color = Color.red;
                        //만약 과거 손어깨 간격이 0보다 작았다면(공에 추진력을 가했을 때),
                        if (prevHandShoulderGap < 0)
                        {
                            Ball.GetComponent<Rigidbody>().useGravity= true;
                            //힘을 0으로 초기화한 후,(속도를 잡아주는 코드)
                            Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            //현재 손 위치랑 과거 손 위치 값을 뺀 값의 (200f) 힘을 줌.
                            Vector3 force = (handPos - prevHandPos) * 500f;
                            //force z값은 음수로 지정하여 방향을 원래로 맞춘다.
                            force.z = -force.z;
                            //위에 계산된 값이 addforce값으로 적용.
                            Ball.GetComponent<Rigidbody>().AddForce(force);
                            //던져짐 bool이 트루로 변경.
                            isThrown = true;

                        }

                    }


                    //그 외에는 공이(초록)색을 띄도록 한다.
                    else
                    {
                        //오버레이오브젝트의 컴포넌트<렌더러> 중 메테리얼 컬러를 초록으로 변경.
                        Ball.GetComponent<Renderer>().material.color = Color.green;
                        //바닥 밖으로 떨어질 시 공이 재생성 되는 코드--------------------------------
                        //만약 공이 -5f(바닥 아래 y좌표)보다 아래로 내려간다면,
                        if (Ball.transform.position.y < -5f)
                        {
                            //중력을 없애서 던질 수 있는 상태로 대기시키고,
                            Ball.GetComponent<Rigidbody>().useGravity = false;
                            isThrown = false;
                            //공을 원위치로 이동시킨다.
                            Ball.transform.position = new Vector3(0, 1, 2);
                            //힘을 0으로 초기화하여 자연스럽게 던져진다.(속도를 잡아주는 코드)
                            Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;

                        }
                        //---------------------------------------------------------------------------
                    }

                    //값을 가져오고 공이 할당되어 일어난 작업이 끝난 후
                    //과거 간격을 현재간격으로, 과거손좌표를 현재 손 좌표로 초기화하여,
                    //다음 작업이 실행될 시 새로운 값을 다시 갖기위해 실행한다.
                    prevHandShoulderGap = handShoulderGap;
                    prevHandPos = handPos;

                }
            }
            

        }
    }

}

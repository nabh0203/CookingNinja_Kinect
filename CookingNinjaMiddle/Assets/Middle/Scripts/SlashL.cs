using UnityEngine;
using System.Collections;
using System;
using com.rfilkov.kinect;


namespace com.rfilkov.components
{
    /// <summary>
    /// JointOverlayer overlays the given body joint with the given virtual object.
    /// </summary>
    public class SlashL : MonoBehaviour
    {
        //플레이어 수를 정하는 변수
        [Tooltip("플레이어의 수")]
        public int playerIndex = 0;
        //KinectInterop.JointType를 통해 사용자의 오른손 관절 값을 받아오는 변수
        [Tooltip("왼손")]
        public KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandLeft;
        //사용자의 손에 부착된 오브젝트 변수
        [Tooltip("사용자의 손에 부착된 오브젝트 변수")]
        public GameObject Slashs;

        //KinectManager의 kinectManager를 받아오는 변수
        private KinectManager kinectManager = null;
        // x 좌표 스케일링 팩터
        public float scaleX = 1f;
        // y 좌표 스케일링 팩터
        public float scaleZ = 1f;
        // 오프셋 벡터
        public Vector3 offset = Vector3.zero;
        public void Start()
        {
            // get reference to KM
            //kinectManager 호출시 KinectManager의 변수 값을 받아온다.
            // Start 시 KinectManager가 활성화가 되고 인스턴스를 참조한다.
            //준비단계
            kinectManager = KinectManager.Instance;
        }
        void Update()
        {

            //키넥트가 준비가 됐고 초기화가 다 되었다면?
            if (kinectManager && kinectManager.IsInitialized())
            {
                //userId = 고유 넘버 아이디 값을 뜻함
                //플레이어 수를 받아옴
                ulong userId = kinectManager.GetUserIdByIndex(playerIndex);

                //왼손관절갯수를 받아오는 변수
                int iJointIndex = (int)trackedJoint;
                // 해당 관절이 추적되고 있다면, KinectManager의 GetJointKinectPosition 메소드를 사용하여 
                // 해당 관절의 3D 위치(Unity 좌표계 기준)를 가져온다.
                Vector3 posJoint = kinectManager.GetJointKinectPosition(userId, iJointIndex, true);
                //만약 posJoint 의 위치 정보가 0,0,0 이 아니라면
                if (posJoint != Vector3.zero)
                {
                    //userId 의 값을 통해 iJointIndex 즉 관절 정보를 받아온다.
                    if (kinectManager.IsJointTracked(userId, iJointIndex))
                    {
                        // Vector3 속성의 handPos변수 값을 왼손 관절 값이다.
                        Vector3 handPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.HandLeft);
                        //만약 Slashs가 없다면
                        if (Slashs != null)
                        {
                            //handPos.x는 해당 벡터의 x 좌표 값을 받아온다.
                            handPos.x *= scaleX;
                            //handPos.z는 해당 벡터의 z 좌표 값을 받아온다.
                            handPos.z *= scaleZ;
                            //handPos의 값은 offset의 값을 더한 값이다.
                            handPos += offset;
                            //slashPos의 값을 X Z 축만 움직이기 위해 y를 -1f 로 고정하여 값을 받아오고
                            Vector3 slashPos = new Vector3(handPos.x, -1f, handPos.z);
                            //Slashs 오브젝트의 포지션 값은 slashPos값이다.
                            Slashs.transform.position = slashPos;
                        }
                    }
                }
            }
        }
    }
}




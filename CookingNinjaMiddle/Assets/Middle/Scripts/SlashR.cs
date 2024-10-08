using UnityEngine;
using System.Collections;
using System;
using com.rfilkov.kinect;


namespace com.rfilkov.components
{
    /// <summary>
    /// JointOverlayer overlays the given body joint with the given virtual object.
    /// </summary>
    public class SlashR : MonoBehaviour
    {
        //�÷��̾� ���� ���ϴ� ����
        [Tooltip("�÷��̾��� ��")]
        public int playerIndex = 0;
        //KinectInterop.JointType�� ���� ������� ������ ���� ���� �޾ƿ��� ����
        [Tooltip("������")]
        public KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandRight;
        //������� �տ� ������ ������Ʈ ����
        [Tooltip("������� �տ� ������ ������Ʈ ����")]
        public GameObject Slashs;

        //KinectManager�� kinectManager�� �޾ƿ��� ����
        private KinectManager kinectManager = null;
        // x ��ǥ �����ϸ� ����
        public float scaleX = 1f;
        // y ��ǥ �����ϸ� ����
        public float scaleZ = 1f;
        // ������ ����
        public Vector3 offset = Vector3.zero;
        public void Start()
        {
            // get reference to KM
            //kinectManager ȣ��� KinectManager�� ���� ���� �޾ƿ´�.
            // Start �� KinectManager�� Ȱ��ȭ�� �ǰ� �ν��Ͻ��� �����Ѵ�.
            //�غ�ܰ�
            kinectManager = KinectManager.Instance;
        }
        void Update()
        {

            //Ű��Ʈ�� �غ� �ư� �ʱ�ȭ�� �� �Ǿ��ٸ�?
            if (kinectManager && kinectManager.IsInitialized())
            {
                //userId = ���� �ѹ� ���̵� ���� ����
                //�÷��̾� ���� �޾ƿ�
                ulong userId = kinectManager.GetUserIdByIndex(playerIndex);

                //�����հ��������� �޾ƿ��� ����
                int iJointIndex = (int)trackedJoint;
                // �ش� ������ �����ǰ� �ִٸ�, KinectManager�� GetJointKinectPosition �޼ҵ带 ����Ͽ� 
                // �ش� ������ 3D ��ġ(Unity ��ǥ�� ����)�� �����´�.
                Vector3 posJoint = kinectManager.GetJointKinectPosition(userId, iJointIndex, true);
                //���� posJoint �� ��ġ ������ 0,0,0 �� �ƴ϶��
                if (posJoint != Vector3.zero)
                {
                    //userId �� ���� ���� iJointIndex �� ���� ������ �޾ƿ´�.
                    if (kinectManager.IsJointTracked(userId, iJointIndex))
                    {
                        // Vector3 �Ӽ��� handPos���� ���� ������ ���� ���̴�.
                        Vector3 handPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.HandRight);
                        //���� Slashs�� ���ٸ�
                        if (Slashs != null)
                        {
                            //handPos.x�� �ش� ������ x ��ǥ ���� �޾ƿ´�.
                            handPos.x *= scaleX; // scaleX is a scaling factor for the x-coordinate
                                                 //handPos.z�� �ش� ������ z ��ǥ ���� �޾ƿ´�.
                            handPos.z *= scaleZ; // scaleY is a scaling factor for the y-coordinate
                                                 //handPos�� ���� offset�� ���� ���� ���̴�.
                            handPos += offset;   // offset is a Vector3 representing the offset in each direction
                                                 //slashPos�� ���� X Z �ุ �����̱� ���� y�� -1f �� �����Ͽ� ���� �޾ƿ���
                            Vector3 slashPos = new Vector3(handPos.x, -1f, handPos.z);
                            //Slashs ������Ʈ�� ������ ���� slashPos���̴�.
                            Slashs.transform.position = slashPos;
                        }
                    }
                }
            }
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    //ParticleSystem�Ӽ��� ���� Particle ����
    public ParticleSystem Particle;
    // Start is called before the first frame update
    void Start()
    {
        // �� ��ũ��Ʈ�� ���Ե� ���� ������Ʈ �Ǵ� �� �ڽ� ������Ʈ �߿��� 
        // ParticleSystem ������Ʈ�� ã�� 'Particle' ������ �Ҵ��Ѵ�.
        Particle = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        //���� Particle�� null �ƴ϶��
        if (Particle! == null)
        {
            //���� 'Particle'�� null�� �ƴ϶��, �ش� ��ƼŬ �ý����� MainModule�� �����´�.
            ParticleSystem.MainModule main = Particle.main;
            //���� ��ƼŬ �ý����� startRotation �Ӽ��� Constant ����
            if (main.startRotation.mode == ParticleSystemCurveMode.Constant)
            {
                //startRotation ���� ���� ���� ������Ʈ�� z�� ȸ�� ����(�� ����)�� ���� ������ ��ȯ�� ������ ����
                //����(radian)�� ���� ũ�⸦ �����ϴ� ���� �� �ϳ� Mathf.Deg2Rad�� ����Ͽ� �� ���� ���� ���� ���� ������ ��ȯ�ϱ� ���� ���
                main.startRotation = -transform.eulerAngles.z * Mathf.Deg2Rad;
            }
        }
    }
}

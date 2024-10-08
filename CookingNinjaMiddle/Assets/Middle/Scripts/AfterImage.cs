using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    //ParticleSystem속성의 변수 Particle 지정
    public ParticleSystem Particle;
    // Start is called before the first frame update
    void Start()
    {
        // 이 스크립트가 포함된 게임 오브젝트 또는 그 자식 오브젝트 중에서 
        // ParticleSystem 컴포넌트를 찾아 'Particle' 변수에 할당한다.
        Particle = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        //만약 Particle이 null 아니라면
        if (Particle! == null)
        {
            //만약 'Particle'이 null이 아니라면, 해당 파티클 시스템의 MainModule을 가져온다.
            ParticleSystem.MainModule main = Particle.main;
            //만약 파티클 시스템의 startRotation 속성이 Constant 모드면
            if (main.startRotation.mode == ParticleSystemCurveMode.Constant)
            {
                //startRotation 값을 현재 게임 오브젝트의 z축 회전 각도(도 단위)를 라디안 단위로 변환한 값으로 설정
                //라디안(radian)은 각의 크기를 측정하는 단위 중 하나 Mathf.Deg2Rad를 사용하여 도 단위 값을 라디안 단위 값으로 변환하기 위해 사용
                main.startRotation = -transform.eulerAngles.z * Mathf.Deg2Rad;
            }
        }
    }
}

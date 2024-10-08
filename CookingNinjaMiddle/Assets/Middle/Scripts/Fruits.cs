using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    //반으로 쪼개진 과일 모델링을 생성하는 변수생성
    public GameObject Fruit1;
    //반으로 쪼개진 과일 모델링을 생성하는 변수생성
    public GameObject Fruit2;
    //private bool isTriggered = false;  
    public GameObject Effects;

    //오디오를 재생 시켜주는 변수 생성 
    public AudioClip audioClip;

    //콜라이더 될때
    private void OnCollisionEnter(Collision other)
    {
        //만약 Slash태그가 붙은 오브젝트와 콜라이더 되면
        if (other.gameObject.CompareTag("Slash"))
        {
            //spawnPosition의 값은 transform.position 이고
            Vector3 spawnPosition = transform.position;
            //AudioPlayer의 인스턴스를 참조하여 오디오를 재생
            AudioPlayer.instance.Play(audioClip);

            //오브젝트를 파괴하고
            Destroy(gameObject);

            //ScoreManager를 찾아 IncreaseScore의 값을 10점 증가
            FindObjectOfType<ScoreManager>().IncreaseScore(10);  // 점수 증가

            //그리고 반으로 쪼개진 오브젝트를 표현하기위해 2개의 오브젝트를 복제
            Instantiate(Fruit1, spawnPosition, Quaternion.identity);
            Instantiate(Fruit2, spawnPosition, Quaternion.identity);
            //후에 쪼개진 이펙트를 넣기 위해 설정
            Instantiate(Effects, spawnPosition, Quaternion.identity);


        }
    }

}


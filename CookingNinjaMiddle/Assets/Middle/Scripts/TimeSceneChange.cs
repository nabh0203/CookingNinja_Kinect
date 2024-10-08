using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeSceneChange : MonoBehaviour
{
    //오디오 재생할수 있는 변수 지정
    public AudioClip audioClip;

    void OnTriggerEnter(Collider other)
    {
            //코루틴을 시작
            StartCoroutine(WaitAndLoadScene());
            //AudioPlayer 스크립트의 인스턴스를 참조하여 오디오를 재생시켜라
            AudioPlayer.instance.Play(audioClip);
    }

    IEnumerator WaitAndLoadScene()
    {
        // 3초간 대기
        yield return new WaitForSeconds(3f);
        // "Game"으로 이동
        SceneManager.LoadScene("Game");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 관리를 위한 네임스페이스

public class SceneChange : MonoBehaviour
{
    //씬을 이동할수 있는 문자멸 변수 지정
    public string SceneName;
    //오디오 재생할수 있는 변수 지정
    public AudioClip audioClip;
    //이 스크립트가 붙은 오브젝트가 콜라이더 되면
    void OnTriggerEnter(Collider other)
    {
        //내가 지정한 씬 이름의 씬으로 이동해고
        SceneManager.LoadScene(SceneName);
        //AudioPlayer 스크립트의 인스턴스를 참조하여 오디오를 재생시켜라
        AudioPlayer.instance.Play(audioClip);

    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // AudioPlayer 클래스의 인스턴스를 저장하는 static 변수.
    //static이란? 해당 변수, 메소드 또는 클래스가 인스턴스에 속하지 않고 클래스 자체에 속하도록 지정
    //좀 더 쉽게 말해 static을 사용하여 모든 인스턴스 공유가 가능하게 바꾼다.
    public static AudioPlayer instance;
    private AudioSource audioSource;

    //시작하면 바로 실행시키는 Awake함수 명령어
    // Awake 함수는? 객체가 생성될 때 호출되는 Unity의 내장 함수
    private void Awake()
    {
        // instance가 null이면, 현재 인스턴스(this)를 instance에 할당한다.
        // 이렇게 하면 다른 스크립트에서 이 클래스의 인스턴스에 접근할 수 있게 된다.
        if (instance == null)
            instance = this;
        //audioSource를 통해 이 스크립트가 붙은 오브젝트에 AudioSource가 붙어있는지 확인
        audioSource = GetComponent<AudioSource>();

        //만약 audioSource가 안 붙어있으면 
        if (audioSource == null)
            //디버그 오류를 통해 인스펙터창에 오류 "오디오가 없습니다."를 출력 
            Debug.LogError("오디오가 없습니다.");

    }
    // AudioClip을 재생하는 명령어
    public void Play(AudioClip clip)
    {
        // AudioSource.PlayOneShot을 통해 한번씩 재생하게 설정
        audioSource.PlayOneShot(clip);
    }
}


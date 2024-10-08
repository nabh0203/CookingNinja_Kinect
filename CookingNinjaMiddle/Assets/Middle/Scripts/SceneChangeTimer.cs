using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeTimer : MonoBehaviour
{
    // 타이머 설정 (30초)
    public float timer = 30f;
    // 고득점 시 로드할 씬의 이름.
    public string highScoreScene = "HighScoreScene";
    // 저득점 시 로드할 씬의 이름.
    public string lowScoreScene = "LowScoreScene";
    // UI Text에 대한 참조
    public Text timerText;
    // ScoreManager에 대한 참조
    public ScoreManager scoreManager;
    // 효과음 재생용 오디오 소스
    public AudioSource audioSource;
    // 효과음 클립
    public AudioClip endClip;
    // 오브젝트 스폰 관리자 참조
    public spawn fruitSpawnManager;
    // SceneFade 스크립트 참조
    public SceneFadeOut sceneFader;
    //SceneFadeOut 스크립트가 있는 오브젝트를 활성화 한다.
    public GameObject SceneFadeOut;

    private void Update()
    {
        //만약 timer가 0보다 크면
        if (timer > 0)
        {
            //timer의 값을 Time.deltaTime 만큼 빼라 즉 1초씩 감소 시켜라
            timer -= Time.deltaTime;
            //timerText.text에 "Time: " 에 timer의 소수점을 반올림 한 값을 입력 시켜라
            //Mathf.Round란? 입력된 실수를 가장 가까운 정수로 반올림하는 함수이다.
            timerText.text = "Time: " + Mathf.Round(timer);
        }
        //만약 오디오 소스가 플레이 중이 아니라면
        else if (!audioSource.isPlaying)
        {
            //endClip를 한번 재생하고
            audioSource.PlayOneShot(endClip);
            //endClip가 끝나면 WaitForSound 코루틴을 실행시켜라
            StartCoroutine(WaitForSound(endClip));
            // 타이머가 끝나면 스폰을 멈춰라
            fruitSpawnManager.StopSpawning();

            //SceneFadeOut가 비활성화 되어 있으니 활성화 시켜라
            SceneFadeOut.SetActive(true);
            // FadeImage 코루틴을 실행하기 위해 활성화
            sceneFader.gameObject.SetActive(true);
            // FadeImage 코루틴 시작
            StartCoroutine(sceneFader.FadeImage());
        }
    }
    //소리가 끝나면 시작되는 코루틴 명령어
    IEnumerator WaitForSound(AudioClip sound)
    {
        // 음악 길이만큼 대기한뒤
        yield return new WaitForSeconds(sound.length);

        //만약 scoreManager의 Score 가 5000보다 크면
        if (scoreManager.Score >= 2000)
            //highScoreScene 씬으로 이동하고
            SceneManager.LoadScene(highScoreScene);
        else
            //아니면 LowScoreScene 씬으로 이동해라
            SceneManager.LoadScene(lowScoreScene);
    }
}

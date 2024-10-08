using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeOut : MonoBehaviour
{
    // FadeOut 시킬 이미지 변수
    public Image fadeImage;
    //페이드 되는 속도
    public float fadeSpeed = 1f;

    public void Start()
    {
        //FadeImage 코루틴을 실행
        StartCoroutine(FadeImage());
    }

    //Fade 효과를 진행시킬 명령어 FadeImage
    public IEnumerator FadeImage()
    {
        // 알파값이 1이 될 때까지 반복.
        while (fadeImage.color.a < 1)
        {
            // Color.Lerp 함수는 두 색상 사이를 선형 보간합니다.
            // 여기서는 현재 알파값에서 목표 알파값인 1으로 보간하는데 사용되며 즉 컬러의 블랙값이 0에서 1사이의 값이면 색상을 변환한다.
            fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeSpeed * Time.deltaTime);

            yield return null;
        }

        // 모든 처리가 끝나면 이미지 컴포넌트를 활성화 한다.
        fadeImage.gameObject.SetActive(true);
    }
}

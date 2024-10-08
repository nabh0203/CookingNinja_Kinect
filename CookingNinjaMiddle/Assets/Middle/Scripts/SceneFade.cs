using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    // FadeOut ��ų �̹��� ����
    public Image fadeImage;
    //���̵� �Ǵ� �ӵ�
    public float fadeSpeed = 1f;

    public void Start()
    {
        //FadeImage �ڷ�ƾ�� ����
        StartCoroutine(FadeImage());
    }

    public IEnumerator FadeImage()
    {
        // ���İ��� 0�� �� ������ �ݺ�.
        while (fadeImage.color.a > 0)
        {
            // Color.Lerp �Լ��� �� ���� ���̸� ���� �����մϴ�.
            // ���⼭�� ���� ���İ����� ��ǥ ���İ��� 1���� �����ϴµ� ���Ǹ� �� �÷��� ���� 0���� 1������ ���̸� ������ ��ȯ�Ѵ�.
            fadeImage.color = Color.Lerp(fadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);

            yield return null;
        }

        // ��� ó���� ������ �̹��� ������Ʈ�� ��Ȱ��ȭ �Ѵ�.
        fadeImage.gameObject.SetActive(false);
    }
}

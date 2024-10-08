using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeTimer : MonoBehaviour
{
    // Ÿ�̸� ���� (30��)
    public float timer = 30f;
    // ����� �� �ε��� ���� �̸�.
    public string highScoreScene = "HighScoreScene";
    // ������ �� �ε��� ���� �̸�.
    public string lowScoreScene = "LowScoreScene";
    // UI Text�� ���� ����
    public Text timerText;
    // ScoreManager�� ���� ����
    public ScoreManager scoreManager;
    // ȿ���� ����� ����� �ҽ�
    public AudioSource audioSource;
    // ȿ���� Ŭ��
    public AudioClip endClip;
    // ������Ʈ ���� ������ ����
    public spawn fruitSpawnManager;
    // SceneFade ��ũ��Ʈ ����
    public SceneFadeOut sceneFader;
    //SceneFadeOut ��ũ��Ʈ�� �ִ� ������Ʈ�� Ȱ��ȭ �Ѵ�.
    public GameObject SceneFadeOut;

    private void Update()
    {
        //���� timer�� 0���� ũ��
        if (timer > 0)
        {
            //timer�� ���� Time.deltaTime ��ŭ ���� �� 1�ʾ� ���� ���Ѷ�
            timer -= Time.deltaTime;
            //timerText.text�� "Time: " �� timer�� �Ҽ����� �ݿø� �� ���� �Է� ���Ѷ�
            //Mathf.Round��? �Էµ� �Ǽ��� ���� ����� ������ �ݿø��ϴ� �Լ��̴�.
            timerText.text = "Time: " + Mathf.Round(timer);
        }
        //���� ����� �ҽ��� �÷��� ���� �ƴ϶��
        else if (!audioSource.isPlaying)
        {
            //endClip�� �ѹ� ����ϰ�
            audioSource.PlayOneShot(endClip);
            //endClip�� ������ WaitForSound �ڷ�ƾ�� ������Ѷ�
            StartCoroutine(WaitForSound(endClip));
            // Ÿ�̸Ӱ� ������ ������ �����
            fruitSpawnManager.StopSpawning();

            //SceneFadeOut�� ��Ȱ��ȭ �Ǿ� ������ Ȱ��ȭ ���Ѷ�
            SceneFadeOut.SetActive(true);
            // FadeImage �ڷ�ƾ�� �����ϱ� ���� Ȱ��ȭ
            sceneFader.gameObject.SetActive(true);
            // FadeImage �ڷ�ƾ ����
            StartCoroutine(sceneFader.FadeImage());
        }
    }
    //�Ҹ��� ������ ���۵Ǵ� �ڷ�ƾ ��ɾ�
    IEnumerator WaitForSound(AudioClip sound)
    {
        // ���� ���̸�ŭ ����ѵ�
        yield return new WaitForSeconds(sound.length);

        //���� scoreManager�� Score �� 5000���� ũ��
        if (scoreManager.Score >= 2000)
            //highScoreScene ������ �̵��ϰ�
            SceneManager.LoadScene(highScoreScene);
        else
            //�ƴϸ� LowScoreScene ������ �̵��ض�
            SceneManager.LoadScene(lowScoreScene);
    }
}

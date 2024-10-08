using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeSceneChange : MonoBehaviour
{
    //����� ����Ҽ� �ִ� ���� ����
    public AudioClip audioClip;

    void OnTriggerEnter(Collider other)
    {
            //�ڷ�ƾ�� ����
            StartCoroutine(WaitAndLoadScene());
            //AudioPlayer ��ũ��Ʈ�� �ν��Ͻ��� �����Ͽ� ������� ������Ѷ�
            AudioPlayer.instance.Play(audioClip);
    }

    IEnumerator WaitAndLoadScene()
    {
        // 3�ʰ� ���
        yield return new WaitForSeconds(3f);
        // "Game"���� �̵�
        SceneManager.LoadScene("Game");
    }
}

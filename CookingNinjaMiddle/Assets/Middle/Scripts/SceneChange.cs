using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // �� ������ ���� ���ӽ����̽�

public class SceneChange : MonoBehaviour
{
    //���� �̵��Ҽ� �ִ� ���ڸ� ���� ����
    public string SceneName;
    //����� ����Ҽ� �ִ� ���� ����
    public AudioClip audioClip;
    //�� ��ũ��Ʈ�� ���� ������Ʈ�� �ݶ��̴� �Ǹ�
    void OnTriggerEnter(Collider other)
    {
        //���� ������ �� �̸��� ������ �̵��ذ�
        SceneManager.LoadScene(SceneName);
        //AudioPlayer ��ũ��Ʈ�� �ν��Ͻ��� �����Ͽ� ������� ������Ѷ�
        AudioPlayer.instance.Play(audioClip);

    }

}


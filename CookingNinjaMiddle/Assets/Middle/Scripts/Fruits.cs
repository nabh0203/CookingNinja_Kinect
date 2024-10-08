using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    //������ �ɰ��� ���� �𵨸��� �����ϴ� ��������
    public GameObject Fruit1;
    //������ �ɰ��� ���� �𵨸��� �����ϴ� ��������
    public GameObject Fruit2;
    //private bool isTriggered = false;  
    public GameObject Effects;

    //������� ��� �����ִ� ���� ���� 
    public AudioClip audioClip;

    //�ݶ��̴� �ɶ�
    private void OnCollisionEnter(Collision other)
    {
        //���� Slash�±װ� ���� ������Ʈ�� �ݶ��̴� �Ǹ�
        if (other.gameObject.CompareTag("Slash"))
        {
            //spawnPosition�� ���� transform.position �̰�
            Vector3 spawnPosition = transform.position;
            //AudioPlayer�� �ν��Ͻ��� �����Ͽ� ������� ���
            AudioPlayer.instance.Play(audioClip);

            //������Ʈ�� �ı��ϰ�
            Destroy(gameObject);

            //ScoreManager�� ã�� IncreaseScore�� ���� 10�� ����
            FindObjectOfType<ScoreManager>().IncreaseScore(10);  // ���� ����

            //�׸��� ������ �ɰ��� ������Ʈ�� ǥ���ϱ����� 2���� ������Ʈ�� ����
            Instantiate(Fruit1, spawnPosition, Quaternion.identity);
            Instantiate(Fruit2, spawnPosition, Quaternion.identity);
            //�Ŀ� �ɰ��� ����Ʈ�� �ֱ� ���� ����
            Instantiate(Effects, spawnPosition, Quaternion.identity);


        }
    }

}


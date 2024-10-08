using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // AudioPlayer Ŭ������ �ν��Ͻ��� �����ϴ� static ����.
    //static�̶�? �ش� ����, �޼ҵ� �Ǵ� Ŭ������ �ν��Ͻ��� ������ �ʰ� Ŭ���� ��ü�� ���ϵ��� ����
    //�� �� ���� ���� static�� ����Ͽ� ��� �ν��Ͻ� ������ �����ϰ� �ٲ۴�.
    public static AudioPlayer instance;
    private AudioSource audioSource;

    //�����ϸ� �ٷ� �����Ű�� Awake�Լ� ��ɾ�
    // Awake �Լ���? ��ü�� ������ �� ȣ��Ǵ� Unity�� ���� �Լ�
    private void Awake()
    {
        // instance�� null�̸�, ���� �ν��Ͻ�(this)�� instance�� �Ҵ��Ѵ�.
        // �̷��� �ϸ� �ٸ� ��ũ��Ʈ���� �� Ŭ������ �ν��Ͻ��� ������ �� �ְ� �ȴ�.
        if (instance == null)
            instance = this;
        //audioSource�� ���� �� ��ũ��Ʈ�� ���� ������Ʈ�� AudioSource�� �پ��ִ��� Ȯ��
        audioSource = GetComponent<AudioSource>();

        //���� audioSource�� �� �پ������� 
        if (audioSource == null)
            //����� ������ ���� �ν�����â�� ���� "������� �����ϴ�."�� ��� 
            Debug.LogError("������� �����ϴ�.");

    }
    // AudioClip�� ����ϴ� ��ɾ�
    public void Play(AudioClip clip)
    {
        // AudioSource.PlayOneShot�� ���� �ѹ��� ����ϰ� ����
        audioSource.PlayOneShot(clip);
    }
}


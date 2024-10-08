using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // ���� ������ �����ϰ� private set�� ���� �ܺο��� ���� �������� ���ϰ� ��. �ʱⰪ�� 0.
    //{ get; private set; }���� get�� ������Ƽ�� ���� �о���� set�� �ش� ������Ƽ�� ���� �����Ҽ� �ְ� 
    // get; �տ��� �ƹ��� �����ڰ� ���� �ܺο��� ���� �޾ƿü� ������  private set;�� ���� ���ο��� ���� �������� ���ϰ� ������ �ִ�.
    public int Score { get; private set; } = 0;

    // UI Text ������Ʈ�� ����. �̰��� ������ ȭ�鿡 ǥ���ϴ� �� ���ȴ�.
    public Text scoreText;


    private void OnCollisionEnter(Collision other)
    {
        //���� Fruit�±װ� ���� ������Ʈ�� �ݶ��̴� �Ǹ�
        if (other.gameObject.CompareTag("Fruit"))
        {
            // ������ 5���� ����
            IncreaseScore(5);
            //����� ���
            Debug.Log("5��");
        }
        //���� FruitHalf�±װ� ���� ������Ʈ�� �ݶ��̴� �Ǹ�
        else if (other.gameObject.CompareTag("FruitHalf"))
        {
            // ������ 10���� ����
            IncreaseScore(10);
            //����� ���
            Debug.Log("10��");
        }
        //���� FruitQaud�±װ� ���� ������Ʈ�� �ݶ��̴� �Ǹ�
        else if (other.gameObject.CompareTag("FruitQaud"))
        {
            //������ 15���� ����
            IncreaseScore(15);
            //����� ���
            Debug.Log("15��");
        }
    }
    // ������ ������Ű�� �޼ҵ�. increment�� ������ų ���� ���� �޴´�.
    public void IncreaseScore(int increment)
    {
        // ���� increment��ŭ ������ ������Ų����.
        Score += increment;

        // UI Text ������Ʈ�� text �Ӽ��� ������Ʈ�Ͽ� ����� ������ ȭ�鿡 ǥ���Ѵ�.
        scoreText.text = "Score: " + Score;
    }

}

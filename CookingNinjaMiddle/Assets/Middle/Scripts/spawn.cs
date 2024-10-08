using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    // 3������ ������Ʈ�� �޾ƿ��� �迭 ���� ����
    public GameObject[] Fruits;
    // ������Ʈ�� ������ ������Ʈ �ϳ� ����
    public GameObject spawnPlane;
    //������Ʈ�� �����Ǵ� ������ ����
    public float spawnInterval = 1f;
    //������Ʈ�� �����Ǵ� ������ ���α��̸� ����
    public float planeWidth = 10f;
    //������Ʈ�� �����Ǵ� ������ ���α��̸� ����
    public float planeHeight = 10f;

    // �ܺο��� ���� �޾ƿ� ������Ʈ ������ ���߰� �ϴ� �����
    public bool isSpawning { get; private set; } = true;

    void Start()
    {
        //SpawnFruits �ڷ�ƾ ����
        StartCoroutine(SpawnFruits());
    }
    //SpawnFruits �ڷ�ƾ
    IEnumerator SpawnFruits()
    {
        //isSpawning�� ���̸� �ݺ�
        while (isSpawning)
        {
            //x��� z�࿡ ���� ������ ��ġ�� ����
            float x = Random.Range(-planeWidth / 2, planeWidth / 2);
            float z = Random.Range(-planeHeight / 2, planeHeight / 2);

            //spawnPosition�� ���� spawnPlane�� ��ġ�� x z ���� �����ǰ��� �ް� y�� 0���� ����
            Vector3 spawnPosition = spawnPlane.transform.position + new Vector3(x, 0f, z);

            // �������� �ϳ��� Fruit �����ؼ� ����
            int randomIndex = Random.Range(0, Fruits.Length);
            Instantiate(Fruits[randomIndex], spawnPosition, Quaternion.identity);

            //������ �ð��� ����� �� �ٽ� �����ϰ� �Ѵ�.
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // �ܺο��� ȣ���Ͽ� ������ ���� �� �ִ� �޼ҵ�
    public void StopSpawning()
    {
        isSpawning = false;
    }
}


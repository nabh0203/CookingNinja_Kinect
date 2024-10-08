using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    // 3가지의 오브젝트를 받아오는 배열 변수 지정
    public GameObject[] Fruits;
    // 오브젝트를 생성할 오브젝트 하나 지정
    public GameObject spawnPlane;
    //오브젝트가 생성되는 갯수를 지정
    public float spawnInterval = 1f;
    //오브젝트가 생성되는 구역의 가로길이를 지정
    public float planeWidth = 10f;
    //오브젝트가 생성되는 구역의 세로길이를 지정
    public float planeHeight = 10f;

    // 외부에서 값을 받아와 오브젝트 생성을 멈추게 하는 ㅂ녀수
    public bool isSpawning { get; private set; } = true;

    void Start()
    {
        //SpawnFruits 코루틴 시작
        StartCoroutine(SpawnFruits());
    }
    //SpawnFruits 코루틴
    IEnumerator SpawnFruits()
    {
        //isSpawning이 참이면 반복
        while (isSpawning)
        {
            //x축과 z축에 대한 랜덤한 위치를 생성
            float x = Random.Range(-planeWidth / 2, planeWidth / 2);
            float z = Random.Range(-planeHeight / 2, planeHeight / 2);

            //spawnPosition의 값은 spawnPlane의 위치에 x z 값을 포지션값을 받고 y는 0으로 고정
            Vector3 spawnPosition = spawnPlane.transform.position + new Vector3(x, 0f, z);

            // 랜덤으로 하나의 Fruit 선택해서 생성
            int randomIndex = Random.Range(0, Fruits.Length);
            Instantiate(Fruits[randomIndex], spawnPosition, Quaternion.identity);

            //지정된 시간이 경과한 후 다시 실행하게 한다.
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 외부에서 호출하여 스폰을 멈출 수 있는 메소드
    public void StopSpawning()
    {
        isSpawning = false;
    }
}


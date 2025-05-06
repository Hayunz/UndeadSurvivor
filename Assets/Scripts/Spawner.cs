using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>(); //다수를 가져오기
    }
    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length -1);
        //FloorToInt: 소수점 아래는 버리고 Int형으로 바꾸는 함수
        //CeilToInt : 소수점 아래를 올리고 Int형으로 바꾸는 함수

        if (timer > spawnData[level].spawnTime)
        {        
            timer = 0;
            Spawn();
        }

    }

    private void Spawn()
    {
       GameObject enemy= GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position; //자식 오브젝트에서만 선택되도록 랜덤 시작은 1부터
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable] //직렬화 구조
public class SpawnData
{
    public float spawnTime;
    public int spriteType;

    public int health;
    public float speed;
}
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
        spawnPoint = GetComponentsInChildren<Transform>(); //�ټ��� ��������
    }
    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length -1);
        //FloorToInt: �Ҽ��� �Ʒ��� ������ Int������ �ٲٴ� �Լ�
        //CeilToInt : �Ҽ��� �Ʒ��� �ø��� Int������ �ٲٴ� �Լ�

        if (timer > spawnData[level].spawnTime)
        {        
            timer = 0;
            Spawn();
        }

    }

    private void Spawn()
    {
       GameObject enemy= GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position; //�ڽ� ������Ʈ������ ���õǵ��� ���� ������ 1����
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable] //����ȭ ����
public class SpawnData
{
    public float spawnTime;
    public int spriteType;

    public int health;
    public float speed;
}
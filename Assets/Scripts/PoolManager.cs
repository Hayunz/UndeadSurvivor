using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PoolManager : MonoBehaviour
{
    //�������� ������ ����
    public GameObject[] prefabs;

    //�� ����� �ϴ� ����Ʈ�� 
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }

        //Debug.Log(pools.Length);
        Debug.Log($"<color=green>[SUCCESS]</color> {pools.Length}");
    }

    public GameObject Get(int index)
    {
        GameObject select = null; //���� ������Ʈ ���������� ������ �̸� �ۼ�

        //������ Ǯ�� ��� (��Ȱ��ȭ ��) �ִ� ���� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                //�߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }
        //...�� ã������?
       
        if (select == null)
        {
            //���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select); //������ ������Ʈ�� �ش� ������Ʈ Ǯ ����Ʈ�� add �Լ��� �߰�
        }

        return select;

    }

}

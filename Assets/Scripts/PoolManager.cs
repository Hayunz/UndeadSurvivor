using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PoolManager : MonoBehaviour
{
    //프리팹을 보관할 변수
    public GameObject[] prefabs;

    //플 담당을 하는 리스트들 
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
        GameObject select = null; //게임 오브젝트 지역변수와 리턴을 미리 작성

        //선택한 풀의 놀고 (비활성화 된) 있는 게임 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                //발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        //...못 찾았으면?
       
        if (select == null)
        {
            //새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select); //생성된 오브젝트는 해당 오브젝트 풀 리스트에 add 함수로 추가
        }

        return select;

    }

}

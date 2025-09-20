using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    // 풀링할 오브젝트들을 배열로 세팅하고 해당 오브젝트별로 리스트화 시켜서 생성/재사용합니다.
    // 기본 게임오브젝트 게터와 템플릿으로 바로 타입화하여 리턴시켜주기도 합니다.

    [SerializeField] GameObject[] prefabs; // 풀링 대상 프리펩
    List<GameObject>[] pools; // 풀링된 실제 오브젝트

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }
    void OnDisable()
    {
        for (int index = 0; index < pools.Length; index++)
        {
            foreach (GameObject obj in pools[index]) // 리스트
            {
                Destroy(obj);
            }
            pools[index].Clear();
        }
    }
    public GameObject GetObject(int _index, bool _inactive = false)
    {
        if (_index < 0)
        {
            Debug.LogError("pooling index approach below zero!");
            return null;
        }

        GameObject select = null;
        foreach (GameObject item in pools[_index]) // 선택한 프리팹에 접근
        {
            if (item.activeSelf == false)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null) // 없으면
        {
            select = Instantiate(prefabs[_index], transform);
            pools[_index].Add(select);
        }

        if (_inactive)
            select.SetActive(false);

        return select;
    }
    public T GetComponentDirect<T>(int _index, bool _inactive = false) where T : MonoBehaviour
    {
        if (_index < 0)
        {
            Debug.LogError("return T pooling index approach below zero!");
            return null;
        }

        return GetObject(_index, _inactive).GetComponent<T>();
    }
}

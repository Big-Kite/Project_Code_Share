using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs; // Ǯ�� ��� ������
    List<GameObject>[] pools; // Ǯ���� ���� ������Ʈ

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
            foreach (GameObject obj in pools[index]) // ����Ʈ
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
        foreach (GameObject item in pools[_index]) // ������ �����տ� ����
        {
            if (item.activeSelf == false)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null) // ������
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

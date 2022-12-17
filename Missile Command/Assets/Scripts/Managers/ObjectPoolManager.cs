using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private List<Pool> poolsData;

    private Dictionary<String, Queue<GameObject>> _poolsObjects = new Dictionary<string, Queue<GameObject>>();

    public static ObjectPoolManager Instance = null;
    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        Init();
    }
    
    
    private void Init()
    {
        foreach (Pool poolData in poolsData)
        {
            Queue<GameObject> pool = new Queue<GameObject>();

            for (int i = 0; i < poolData.maxSize; i++)
            {
                GameObject element = Instantiate(poolData.prefab, poolData.parentContainer);
                element.SetActive(false);
                pool.Enqueue(element);
            }
            _poolsObjects.Add(poolData.name, pool);
        }
    }

    public GameObject GetElementByName(string poolName)
    {
        Queue<GameObject> pool = _poolsObjects[poolName];
        GameObject element = pool.Dequeue();
        pool.Enqueue(element);
        return element;
    }
}

[System.Serializable]
class Pool
{
    public string name;
    public int maxSize;
    public Transform parentContainer;
    public GameObject prefab;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public Dictionary<string, List<GameObject>> ObjectPoolDictionary
    {
        get;
    } = new Dictionary<string, List<GameObject>>();

    public GameObject PoolObject(string key)
    {
        List<GameObject> list = ObjectPoolDictionary[key];
        if (list == null || list.Count == 0)
        {
            return null;
        }

        list[list.Count - 1].SetActive(true);
        return list[list.Count - 1];
    }

    public void SetObject(string key, GameObject gameObject)
    {
        List<GameObject> list = ObjectPoolDictionary[key];
        if (list == null)
        {
            list = new List<GameObject>();
            ObjectPoolDictionary.Add(key, list);
        }
        list.Add(gameObject);
    }
}

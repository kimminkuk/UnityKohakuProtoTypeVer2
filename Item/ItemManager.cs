using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
        //1) Save Prefabs Variables..
    public GameObject[] prefabs;

    //2) Pool that lists
    List<GameObject>[] pools;
    void Start()
    {
        //3) Make Pool
        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0 ; i < pools.Length; i++) {
            pools[i] = new List<GameObject>();
        }
    }
    public GameObject GetItemObject(int prefabId) {
        GameObject obj = null;
        //5) Check Pool
        foreach(GameObject poolObj in pools[prefabId]) {
            if (!poolObj.activeSelf) {
                obj = poolObj;
                obj.SetActive(true);
                return obj;
            }
        }
        //6) If Pool is Empty, Make New Object
        obj = Instantiate(prefabs[prefabId], transform);
        pools[prefabId].Add(obj);
        return obj;
    }
}

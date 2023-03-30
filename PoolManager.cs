using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //1) Save Prefabs Variables..
    public GameObject[] prefabs;

    //2) Pool that lists
    List<GameObject>[] pools;

    private void Awake() {
        //3) Make Pool
        pools = new List<GameObject>[prefabs.Length];
        for(int i = 0; i < pools.Length; i++) {
            pools[i] = new List<GameObject>();
        }
    }

    //4) Get Object from Pool
    public GameObject GetObject(int prefabId) {
        GameObject obj = null;
        //5) Check Pool
        foreach(GameObject poolObj in pools[prefabId]) {
            if (!poolObj.activeSelf) {
                obj = poolObj;
                obj.SetActive(true);
                return obj;
            }
        }

        // for(int i = 0; i < pools[prefabId].Count; i++) {
        //     if(!pools[prefabId][i].activeSelf) {
        //         pools[prefabId][i].SetActive(true);
        //         return pools[prefabId][i];
        //     }
        // }

        //6) If Pool is Empty, Make New Object
        obj = Instantiate(prefabs[prefabId]);
        pools[prefabId].Add(obj);
        return obj;
    }
}

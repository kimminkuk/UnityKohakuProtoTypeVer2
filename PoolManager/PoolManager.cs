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
        Debug.Log("GetObject Called : " + prefabId );
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
        Debug.Log("6) If Pool is Empty, Make New Object");
        //6) If Pool is Empty, Make New Object
        obj = Instantiate(prefabs[prefabId], transform);
        pools[prefabId].Add(obj);
        return obj;
    }
    public GameObject Get(int index)
    {
        GameObject select = null;
        //1) selected pool(None Active) Point
        //1-1) if, detected? -> select
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //2) find fail..?
        //2-1) new Make -> select
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }    
}

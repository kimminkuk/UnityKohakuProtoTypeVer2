using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolRangeTower : MonoBehaviour
{
    //1) Save Prefabs Variables..
    public GameObject[] prefabs;

    //2) Pool that lists
    public List<GameObject>[] pools;

    //3) Awake Make Pool

    void Awake() {
        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++) {
            pools[i] = new List<GameObject>();
        }
    }

    //4) Get Object from Pool
    public GameObject GetRangeTowerObject(int prefabId) {
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

    // Win Pose Call
    public void PoolRangeTowerWinPose() {
        //1. pools로 활성화 되어 있는 Object들의 WinPose를 호출
        for (int i = 0; i < pools.Length; i++) {
            foreach (GameObject obj in pools[i]) {
                if (obj.activeSelf) {
                    obj.GetComponent<RangeClassManager>().ClearPose();
                }
            }
        }
    }
}

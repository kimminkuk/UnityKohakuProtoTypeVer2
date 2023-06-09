using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCharcterManager : MonoBehaviour
{
    //1) Save Prefabs Variables..
    public GameObject[] prefabs;

    //2) Pool that lists
    public List<GameObject>[] pools;

    // Random Position Setting 3DVector, Z is 0

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
        
        //6) If Pool is Empty, Make New Object
        obj = Instantiate(prefabs[prefabId], transform);
        pools[prefabId].Add(obj);
        return obj;
    }
    
    // Win Pose Call
    public void MeleePoolsTowerWinPoseCall() {
        //1. pools로 활성화 되어 있는 Object들의 WinPose를 호출
        for (int i = 0; i < pools.Length; i++) {
            foreach (GameObject obj in pools[i]) {
                if (obj.activeSelf) {
                    //obj로 어떻게 Melee, Range를 구분할 수 있을까??
                    // 그냥 obj에서 함수 호출 못함??
                    obj.GetComponent<MeleeClassManager>().WinPose();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolEnemyManager : MonoBehaviour
{
    //1. Save Prefabs Variables
    public GameObject[] enemyPrefabs;
    //2. Pool that lists
    List<GameObject>[] enemyPools;
    //3. Make Pool -> Awake
    private void Awake() {
        enemyPools = new List<GameObject>[enemyPrefabs.Length];
        for (int i = 0; i < enemyPools.Length; i++) {
            enemyPools[i] = new List<GameObject>();
        }
    }
    //4. Get Object from Pool -> Function
    public GameObject GetEnemyObject(int prefabId) {
        //4-1. Check Pool -> foreach
        GameObject enemyObj = null;
        foreach(GameObject obj in enemyPools[prefabId]) {
            if (!obj.activeSelf) {
                enemyObj = obj;
                enemyObj.SetActive(true);
                return enemyObj;
            }
        }
        //4-2. If Pool is Empty, Make new Object -> Instantiate
        enemyObj = Instantiate(enemyPrefabs[prefabId], transform);
        enemyPools[prefabId].Add(enemyObj);
        return enemyObj;
    }
    
}

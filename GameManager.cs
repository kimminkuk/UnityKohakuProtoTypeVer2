using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public bool isGameStart = false;
    public int gameMode = 0; // 0 : Single, 1 : Multi

    [Header("# Game Statue")]
    public int gameStage = 0;

    [Header("# GameObject")]
    public PoolManager pool;
    public PoolCharcterManager poolCharcter;
    public PoolEnemyManager poolEnemy;
    public Player player;
    public ItemManager itemPool;

    public float makeIntervalEnemyTime = 0.5f;
    private float makeEnemyTimer = 0f;

    public float makeIntervalTime = 0.1f;

    // Respawn Pos List (GameObject, transform.position)
    public List<GameObject> respawnPosObjectList = new List<GameObject>();
    public List<GameObject> floortPosObjectList = new List<GameObject>();
    
    /*
    *    UI
    */
    public int gameLevel;
    public int kill;

    private void Awake() 
    {
        instance = this;
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        makeEnemyTimer -= Time.deltaTime;
        if (makeEnemyTimer <= 0f) {
            makeEnemyTimer = makeIntervalEnemyTime;
            GameObject obj = poolEnemy.GetEnemyObject(gameStage);
            int getRespawnPosIndex = Random.Range(0, respawnPosObjectList.Count);
            obj.transform.position = respawnPosObjectList[getRespawnPosIndex].transform.position;
            
        }
    
        //UI에서 버튼 클릭 시, Magician 케릭터를 FloorPos-1 위치에 생성
        //만약에 케릭터가 3개 이상이면, FloorPos-2에 생성
        //FloorPos-2에 케릭터가 3개 이상이면, FloorPos-3에 생성
        // a버튼을 누르면 나오게 만듭니다.
        
        //if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Get 0,1 Random Code
            // PoolCharacterManager 클래스의 pools list의 count를 가져옵니다.
            
            //int getClassChangeIndex = Random.Range(0, poolCharcter.pools.Length);
            int getClassChangeIndex = 1;
            int meleeCount = 2;
            int floorSelect = 0;
            if (getClassChangeIndex >= meleeCount) {
                floorSelect = 1;
            }
            GameObject obj = poolCharcter.GetObject(getClassChangeIndex);
            obj.transform.position = floortPosObjectList[floorSelect].transform.position;
        }  
    }

    // Level System 와꾸좀 잡아보자
    // 1. Lv UI 추가
    // 2. 돈 UI 추가
    // 3. 상점 버튼 및 UI 추가
}

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
    public int gameLevel = 0; // 0 : Easy, 1 : Normal, 2 : Hard


    [Header("# Player Info")]
    public int health;
    public int mana;
    public int maxHealth = 100; // 직업마다 달라질 예정
    public int maxMana = 100; // 직업마다 달라질 예정
    public int level;
    public int kill;

    [Header("# GameObject")]
    public PoolManager pool;
    public PoolCharcterManager poolCharcter;
    public Player player;
    public ItemManager itemPool;

    private float makeIntervalTime = 1f;
    private float makeTimer = 0f;

    private float makeIntervalItemTime = 3f;
    private float makeItemTimer = 0f;

    private void Awake() 
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
        mana = maxMana;
        
    }

    // Update is called once per frame
    void Update()
    {
        // 1. 10 Sec Inverval
        // 2. Get poolCharcter.GetObject(0)
        makeTimer -= Time.deltaTime;
        if (makeTimer <= 0f)
        {
            makeTimer = makeIntervalTime;
            GameObject obj = poolCharcter.GetObject(0);
            obj.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
        }

        // 1. 10 Sec Inverval
        // 2. Get poolCharcter.GetObject(0)
        makeItemTimer -= Time.deltaTime;
        if (makeItemTimer <= 0f)
        {
            makeItemTimer = makeIntervalItemTime;
            GameObject obj = itemPool.GetItemObject(0);
            obj.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]
    public int health;
    public int mana;
    public int maxHealth = 100; // 직업마다 달라질 예정
    public int maxMana = 100; // 직업마다 달라질 예정
    public int level;
    public int kill;

    [Header("# GameObject")]
    public PoolManager pool;
    public Player player;

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
        
    }
}

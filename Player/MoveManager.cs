using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [Header("# Player Move State")]
    public float speed = 5f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;
    public float jumpHeight = 2f;
    public float jumpTime = 0.5f;
    public float jumpVelocity = 0f;
    public float jumpTimeCounter = 0f;
    public bool isGrounded = false;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool isCrouching = false;
    public bool isRunning = false;
    public bool isSprinting = false;
    public bool isWalking = false;
    public bool isIdle = false;
    public bool isMoving = false;
    public bool isAttacking = false;

    //RididBody2D, SpriteRenderer, Anim Codes
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator anim;
    public Vector2 inputVec;
    public Hand[] hands;


    //Player 2D 8-direction
    // Enum for 8 possible directions
    private int lastDirection = 0;
    private Vector3 movingDirection3D = Vector3.zero;
    [SerializeField]
    public WeaponManager.WeaponType weaponType;

    [Header("# Player Stat Info")]
    public int health;
    public int mana;
    public int maxHealth; // 직업마다 달라질 예정
    public int maxMana; // 직업마다 달라질 예정
    public int level;
    public int kill;    

    public enum Direction2D {
        Right,
        UpRight,
        Up,
        UpLeft,
        Left,
        DownLeft,
        Down,
        DownRight
    }

    /*
        Random Auto Movement Code
    */
    public float timeBetweenMoves = 2f; // The time between each move
    public float moveTime = 3f; // The time it takes to complete a move
    public float maxDistance = 5f; // The maximum distance the character can move in one direction
    public bool canMove = true; // Whether the character can currently move or not

    private float moveTimer; // Timer for the current move
    private float betweenTimer; // Timer for time between moves
    private Vector2 moveDirection; // Direction of current move    
    private string folderPath = "Sprites/Class";
    private string spriteName;
    private void Awake() {
        weaponType = (WeaponManager.WeaponType)Random.Range(0, 3); 
        switch(weaponType) {
            case WeaponManager.WeaponType.Sword:
                spriteName = "Human_Sword";
                sr = GetComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
                anim = GetComponent<Animator>();
                maxHealth = 100;
                maxMana = 50;
                break;
            case WeaponManager.WeaponType.Bow:
                sr = GetComponent<SpriteRenderer>();
                anim = GetComponent<Animator>();
                break;
            case WeaponManager.WeaponType.Gun:
                sr = GetComponent<SpriteRenderer>();
                anim = GetComponent<Animator>();
                break;
            case WeaponManager.WeaponType.Staff:
                spriteName = "Human_Magician";
                sr = GetComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
                anim = GetComponent<Animator>();
                maxHealth = 50;
                maxMana = 100;
                break;
            case WeaponManager.WeaponType.Hammer:
                spriteName = "Human_Hammer";
                sr = GetComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
                anim = GetComponent<Animator>();
                maxHealth = 120;
                maxMana = 30;
                break;
            default:
                sr = GetComponent<SpriteRenderer>();
                anim = GetComponent<Animator>();
                break;
        }
        health = maxHealth;
        mana = maxMana;
        rb = GetComponent<Rigidbody2D>();
        hands = GetComponentsInChildren<Hand>(true);
        hands[0].weaponType = weaponType;

    }
    void Start()
    {
        moveTimer = moveTime;
        betweenTimer = timeBetweenMoves;        
        if (sr.flipX) {
            lastDirection = 2;
        } else {
            lastDirection = 6;
        }
    }    

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            // Countdown the timer for current move
            moveTimer -= Time.deltaTime;

            // If timer is done, start a new move
            if (moveTimer <= 0f)
            {
                // Get a new random direction
                moveDirection = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance)).normalized;
                lastDirection = Get2D8DirectionMoveToInt(moveDirection);
                movingDirection3D = Get2D8DirectionVector3(lastDirection);
                sr.flipX = moveDirection.x < 0;
                moveTimer = moveTime;
            }

            // Apply movement
            rb.velocity = moveDirection * speed;

            // Countdown the timer for time between moves
            betweenTimer -= Time.deltaTime;

            // If timer is done, stop moving
            if (betweenTimer <= 0f)
            {
                rb.velocity = Vector2.zero;
                betweenTimer = timeBetweenMoves;
            }
        }
        
    }
    public int Get2D8DirectionMoveToInt(Vector2 inputDir) {
        // Get angle between forward direction and input direction
        float angle = Vector2.SignedAngle(Vector2.up, inputDir);

        // Calculate index of 2D 8-direction based on angle
        int index = Mathf.RoundToInt(angle / 45.0f) % 8;
        if (index < 0) {
            index += 8;
        }

        // Return corresponding direction
        return index;
    }

    public Vector3 Get2D8DirectionVector3(int playerDirection) {
        Vector3 dir = Vector3.zero;
        switch (playerDirection) {
            case 0:
                dir = Vector3.up;
                break;
            case 1:
                dir = new Vector3(-1, 1).normalized; //left, up
                break;
            case 2:
                dir = Vector3.left;
                break;
            case 3: 
                dir = new Vector3(-1, -1).normalized; //left, down
                break;
            case 4:
                dir = Vector3.down;
                break;
            case 5: 
                dir = new Vector3(1, -1).normalized; //right, down
                break;
            case 6:
                dir = Vector3.right;
                break;
            case 7:
                dir = new Vector3(1, 1).normalized; //right, up
                break;
        }
        return dir;
    }
}

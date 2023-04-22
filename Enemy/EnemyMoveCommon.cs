using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveCommon : MonoBehaviour
{
    [Header("# Common Enemy Move State")]
    public float gravitySpeed;
    public float speed ;
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
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator anim;
    public Vector2 inputVec;

    //Enemy Hand로 변경 예정..
    //public Hand[] hands;

    private int lastDirection = 0;
    private Vector3 movingDirection3D = Vector3.zero;

    [Header("# Enemy Stat Info")]
        public int health;
    public int mana;
    public int maxHealth;
    public int maxMana;
    public int level;

    /*
        Auto Movement Code
    */
    public int isMoveLeft = 1;
    public bool ifFirstGround;
    public bool canMove; // Whether the character can currently move or not
    public bool isLeavingGround = false;
    private Vector2 moveDirection; // Direction of current move    

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
    *    Enemy Sprite, Animation
    */
    private string folderPath = "Sprites/Enemy";
    private string folderAnimPath = "Animation/Enemy";

    /*
        Common Take Damage Code
    */
    public float flashDuration = 0.1f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f);

    private void Awake() {
        health = maxHealth;
        mana = maxMana;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();        
    }

    void Start()
    {     
        if (sr.flipX) {
            lastDirection = 6;
        } else {
            lastDirection = 2;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrounded) {
            return;
        }
        
        if (canMove)
        {
            isMoving = true;
            Vector2 moveDirection = new Vector2(isMoveLeft, 0).normalized;
            sr.flipX = moveDirection.x > 0;
            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);

            // Animation
            anim.SetBool("isWalking", true);
        }
        else
        {
            isMoving = false;
            anim.SetBool("isWalking", false);
        }        
    }
    // Update is called once per frame
    private void FixedUpdate() {
        if (isGrounded || isMoving)
        {
            return;
        }
        Vector2 dropForce = new Vector2(0, -gravitySpeed);
        rb.velocity = dropForce;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Ground") {
            canMove = true;
            isMoveLeft *= -1;
            isGrounded = true;  
            isFalling = false;
            ifFirstGround = true;
            isMoving = true;
        } 
        else if (other.gameObject.tag == "EndPoint") {
            isGrounded = false;
            isLeavingGround = true;
            isMoving = false;
            isMoveLeft *= -1;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (!ifFirstGround) {
            return;
        }
        if (other.gameObject.tag == "GroundEdge") {
            {
                Debug.Log("falling");
                isGrounded = false;
                isLeavingGround = true;
                isMoving = false;
            }
        }
    }    

}

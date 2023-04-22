using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [Header("# Common Move State")]
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

    [Header("# Class Stat Info")]
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
    public bool canMove; // Whether the character can currently move or not

    private float moveTimer; // Timer for the current move
    private float betweenTimer; // Timer for time between moves
    private Vector2 moveDirection; // Direction of current move    
    private string folderPath = "Sprites/Class";
    private string folderAnimPath = "Animation";
    private string spriteName;

    /*
        Common Take Damage Code
    */
    public float flashDuration = 0.1f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f);

    /*
    *    Ground Position Check
    */
    public float leftBoundary;
    public float rightBoundary;    
    private float groundPositionY = 0f;
    public bool ifFirstGround;
    public int isMoveLeft = 1;
    public bool isEdgeBound = true;
    public bool isLeavingGround = false;
    public float leaveGroundTimer = 0.2f;    
    public float leaveGroundSaveTime = 0.2f;
    public List<float> fallXpos = new List<float>();

    private Collider2D moveCollider;

    private void Awake() {
        health = maxHealth;
        mana = maxMana;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        hands = GetComponentsInChildren<Hand>(true);
        fallXpos.Add(-14f);
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

    void Update()
    {   
        if (!isGrounded) {
            return; // wait for the enemy to touch the ground
        }
        
        if (canMove)
        {
            isMoving = true;
            isEdgeBound = true;
            Vector2 moveDirection = new Vector2(isMoveLeft, 0).normalized;
            sr.flipX = moveDirection.x < 0;
            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
        }
        else
        {
            isMoving = false;
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (isGrounded || isMoving)
        {
            return;
        }
        Vector2 dropForce = new Vector2(0, -gravitySpeed);
        
        //rb velocity로 dropForce를 날림
        rb.velocity = dropForce;

        //rb.AddForce(dropForce);
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
        // tag가 EndPoint면, MoveManager를 해제한다.
        else if (other.gameObject.tag == "EndPoint") {
            //MoveManager.instance.RemoveEnemy(this);
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
            //if (transform.position.x < fallXpos[0]) 
            {
                Debug.Log("falling");
                isGrounded = false;
                isLeavingGround = true;
                isMoving = false;
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

    public void ChangeClassType(WeaponManager.WeaponType weaponType) {
        switch(weaponType) {
            case WeaponManager.WeaponType.Sword:
                spriteName = "Human_Sword";
                maxHealth = 100;
                maxMana = 50;
                break;
            case WeaponManager.WeaponType.Staff:
                spriteName = "Human_Magician";
                maxHealth = 50;
                maxMana = 100;
                break;
            case WeaponManager.WeaponType.Hammer:
                spriteName = "Human_Hammer";
                maxHealth = 120;
                maxMana = 30;
                break;
        }
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
        anim = GetComponent<Animator>();
        health = maxHealth;
        mana = maxMana;
    }

    public void ChangeClassObject(WeaponManager.WeaponType weaponType) {
        string animName = "";
        switch(weaponType) {
            case WeaponManager.WeaponType.Sword:
                spriteName = "Human_Sword";
                animName = "AC_Sword";
                maxHealth = 100;
                maxMana = 50;
                break;
            case WeaponManager.WeaponType.Staff:
                spriteName = "Human_Magician";
                maxHealth = 50;
                maxMana = 100;
                break;
            case WeaponManager.WeaponType.Hammer:
                spriteName = "Human_Hammer";
                animName = "AC_Hammer";
                maxHealth = 120;
                maxMana = 30;
                break;
        }

        //1. sr object를 spriteName으로 변경
        //2. anim object를 spriteName으로 변경
        //3. health, mana를 maxHealth, maxMana로 변경
        //4. hands[0].weaponType 을 weaponType으로 변경
        sr.sprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(folderAnimPath + "/" + animName);
        health = maxHealth;
        mana = maxMana;

        hands[0].GetComponent<Hand>().ChangeWeaponAnim(weaponType);
    }


    // Take Damage
    public void TakeDamage(int damage) {
        //Apply Take to health
        health -= damage;
        if (health <= 0) {
            //Die
            //Die();
        }
        
        StartCoroutine(FlashSprite());


    }

    public void Die() {
        //Die
        Debug.Log("Enemy Died");
        //Disable the enemy
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private IEnumerator FlashSprite()
    {
        // Save original color
        Color originalColor = sr.color;

        // Set flash color
        sr.color = flashColor;

        // Wait for duration
        yield return new WaitForSeconds(flashDuration);

        // Restore original color
        sr.color = originalColor;
    }
}

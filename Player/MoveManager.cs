using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [Header("# Common Move State")]
    public float moveSpeed = 5f;
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

    /*
    *    Enemy Target, Attack Variables
    */
    public float enemySearchRange = 5f;
    public float attackRange = 0.5f;
    public string enemyTag = "Enemy";
    public float attackDelay = 1f;
    public float attackDamage = 10f;

    private Transform target;
    private float lastAttackTime;

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

        // 가장 가까운 거리의 Enemy Tag를 추적합니다.
        // attackRange 안에 들어 오면, 공격합니다.
        // 공격 후, attackDelay 만큼의 시간이 지나면 다시 공격합니다.
        // 가장 가까운 적을 추적합니다.
        Transform nearestEnemy = null;
        float distance = 0;
        if (Time.time > lastAttackTime + attackDelay)
        {
            // find the nearest enemy within range
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemySearchRange);
            float minDistance = float.MaxValue;
            

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag(enemyTag))
                {
                    distance = Mathf.Abs(transform.position.x - collider.transform.position.x);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = collider.transform;
                    }
                }
            }

            if (nearestEnemy != null)
            {
                // attack the nearest enemy
                Attack(nearestEnemy);
                lastAttackTime = Time.time;
            }
        }
        traceEnemy(nearestEnemy, distance);
    }

    private void traceEnemy(Transform nearestEnemy, float distance)
    {
        // nearestEnemy 가 없으면 return
        if (nearestEnemy == null)
        {
            return;
        }
        // nearest enemy를 추적합니다.
        // 추적하는 방향에 따라서, 방향을 바꿉니다.
        if (nearestEnemy.position.x > transform.position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
        // 쫒아갑니다.
        
        //transform.position = Vector2.MoveTowards(transform.position, nearestEnemy.position, moveSpeed * Time.deltaTime);
        Vector2 moveDirection = new Vector2(distance, 0).normalized;
        //rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        Debug.Log("transform.position : " + transform.position);
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (isMoving) {
            //rb.velocity is zero
            rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = new Vector2(0, -gravitySpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        // Floor 에서는 원거리 공격을 합니다.
        if (other.gameObject.tag == "Floor") {
            isMoving = true;
        } 
        
        // Ground 에서는 근거리 공격을 합니다.
        if (other.gameObject.tag == "Ground") {
            isMoving = true;
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

    private void Attack(Transform enemy)
    {
        // perform the attack here
        Debug.Log("Attacking enemy: " + enemy.name);
        // you can use attackDamage variable to set the amount of damage to inflict on the enemy
        hands[0].NormalAttackTriggerOn();

    }

    //draw a gizmo to visualize the attack range in the editor
    // private void OnDrawGizmosSelected()
    // {
    //     Debug.Log("Drawing gizmo");
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, attackRange);
    // }
}

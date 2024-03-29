using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeClassManager : MonoBehaviour
{
 [Header("# Common Move State")]
    public float attackDelayTime;
    public float attackSpeed;
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
    private Vector3 movingDirection3D = Vector3.zero;
    [SerializeField]
    public WeaponManager.WeaponType weaponType;
    public ClassManager.ClassType classType;
    

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
    public float maxDistance = 5f; // The maximum distance the character can move in one direction
    public bool canMove; // Whether the character can currently move or not

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
    public bool ifFirstGround;
    public int isMoveLeft = 1;
    public bool isEdgeBound = true;
    public bool isLeavingGround = false;
    public float leaveGroundTimer = 0.2f;    
    public float leaveGroundSaveTime = 0.2f;
    public List<float> fallXpos = new List<float>();

    private Collider2D moveCollider;

    /*x
    *    Enemy Target, Attack Variables
    */
    public List<Transform> waitPos = new List<Transform>();
    public float enemySearchRange = 5f;
    private float enemySearchTime = 0.3f;
    private float searchWaitTime = 0f;
    public float attackRange = 0.5f;
    public string enemyTag = "Enemy";
    public string enemyLayer = "Enemy";

    private Transform target;
    private float lastAttackTime;
    private bool walkAndEnemySearch = false;

    private int weaponTypeSelect = 0;
    const int RANGE_ATTACK_THRESHOLD = 100;

    /*
    *    Auto-Melee Attack Timer
    */
    private float AUTO_STOP = 0.1f;
    private float MeleeAttackTimer = 0f;

    private void Awake() {
        //GameManager의 floortPosObjectList를 가져옵니다.
        setDefault();
        health = maxHealth;
        mana = maxMana;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        hands = GetComponentsInChildren<Hand>(true);
        switch(classType) {
            case ClassManager.ClassType.Sword:
                weaponTypeSelect = 1;
                break;
            case ClassManager.ClassType.Hammer:
                weaponTypeSelect = 2;
                break;
            case ClassManager.ClassType.Staff:
                weaponTypeSelect = 100;
                break;
        }
    }
    void Start()
    {

    }    

    void Update()
    {   
        Transform nearestEnemy = null;
        nearestEnemy = findNearestEnemy(isAttacking, weaponTypeSelect);
        if (!isAttacking) {
            if (nearestEnemy) {
                MeleeAttackTimer += Time.deltaTime;
                if (Vector2.Distance(transform.position, nearestEnemy.position) <= attackRange) {
                    //StartCoroutine(MeleeNormalAttack(nearestEnemy));
                    if ((attackDelayTime / attackSpeed) < (MeleeAttackTimer + AUTO_STOP / attackSpeed)) {
                        MeleeNormalAttack(nearestEnemy, attackSpeed);
                        MeleeAttackTimer = 0f;
                    }
                    
                } else {
                    if (isAttacking) return;
                    Vector2 direction = (nearestEnemy.position - transform.position);
                    direction.y = 0f;           
                    Vector2 newPosition = new Vector2(nearestEnemy.position.x, transform.position.y);
                    rb.MovePosition(Vector2.Lerp(transform.position, newPosition, Time.deltaTime * moveSpeed));                
                }
            } 
            // else {
            //     // 1. enemySearchTime 시간이 지나면, waitPos[0] 위치로 서서히 이동한다.
            //     searchWaitTime += Time.deltaTime;
            //     if (searchWaitTime > enemySearchTime) {
            //         Vector2 direction = (waitPos[0].position - transform.position).normalized;
            //         direction.y = 0f;   
            //         Vector2 newPosition = new Vector2(transform.position.x + direction.x * moveSpeed * Time.deltaTime, transform.position.y);
            //         sr.flipX = direction.x < 0 ? true : false;
            //         hands[0].isLeft = sr.flipX;
            //         rb.MovePosition(newPosition);
            //         walkAndEnemySearch = true;
            //     }
            // }
        } 
    }

    void MeleeNormalAttack(Transform enemy, float attackSpeed)
    {
        walkAndEnemySearch = true;
        searchWaitTime = 0f;
        isAttacking = true;
        Attack(enemy, attackSpeed);
        isAttacking = false;
    }  

    // IEnumerator MeleeNormalAttack(Transform enemy)
    // {
    //     walkAndEnemySearch = true;
    //     searchWaitTime = 0f;
    //     isAttacking = true;
    //     Attack(enemy, attackSpeed);
    //     yield return new WaitForSeconds(attackDelayTime);
    //     StartCoroutine(AttackOff());
    //     isAttacking = false;
    // }    

    private Transform findNearestEnemy(bool isAttacking, int searchRangeSelect) {
        if (isAttacking) return null;
        enemySearchRange = walkAndEnemySearch == true ? enemySearchRange : attackRange * 2;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(enemySearchRange, 1f), 0, LayerMask.GetMask(enemyLayer));
        float minDistance = float.MaxValue;
        Transform nearestEnemy = null;
        
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(enemyTag))
            {
                float distance = Mathf.Abs(transform.position.x - collider.transform.position.x);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = collider.transform;
                    sr.flipX = nearestEnemy.position.x > transform.position.x ? false : true;
                    hands[0].isLeft = sr.flipX;
                }
            }
        }
        
        return nearestEnemy;        
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

    private void Attack(Transform enemy, float attackSpped)
    {
        // perform the attack here
        // you can use attackDamage variable to set the amount of damage to inflict on the enemy
        hands[0].meleeNormalAttackTriggerOn(enemy, attackSpped);
    }

    IEnumerator AttackOff() {
        yield return new WaitForSeconds(0.5f);
    }


    //draw a gizmo to visualize the attack range in the editor
    private void OnDrawGizmosSelected()
    {
        // Gizmos.color = Color.red;
        // Vector2 pos = new Vector2(enemySearchRange, 1f);
        // Gizmos.DrawWireCube(transform.position, new Vector3(pos.x, pos.y, 0));
    }
    void setDefault() {
        isGrounded = false;
        isJumping = false;
        isFalling = false;
        isCrouching = false;
        isRunning = false;
        isSprinting = false;
        isWalking = false;
        isIdle = false;
        isMoving = false;
        isAttacking = false;
        isMoveLeft = 1;
        ifFirstGround = false;
        canMove = false;
    }

    
    // 대략적인 Win Pose ok
    // 1. Score DB를 먼저 할까..? 
    //    로그인 화면부터 만들어야겟네
    //    1. 로그인 화면
    //    2. ID/PW 입력 or 가입
    //    3. FireBase에 저장
    //    4. 로그인 성공하면, ID가 가지고 있는 DB 정보들 가져와서 화면에 뿌려주기
    //       4-1. 돈, 스테이지, Tower 정보,
    //    후보, Redis와 Firebase
    //    일단 둘 다 뚫는걸로 합시다.

    // 2. 장비칸 만들기를 먼저 할까?
    //    아이템 간단하게 만들어서 장비에 넣고,
    //    외형 달라지기
    //    공속, 데미지 달라지기
    //    장비칸 누를 때, 샘플 다 초기화 되면 안되는거 확인하기..
    //    스테이지 전에 하니깐, 게임스탑 눌러놓고 만드는거니깐 상관없나?
    //    현재 데이터들을 기록해두는게 중요하다.

    public void WinPose() {
        Debug.Log("MeleeClassManager WinPose Call()");
        hands[0].ClearPose();
    }
}

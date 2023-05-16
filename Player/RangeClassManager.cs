using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeClassManager : MonoBehaviour
{
 [Header("# Common Move State")]
    public int bulletCommonDamage;
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


    /*
     *    Range Attack Direction
     *    Range Attack Variable
     */
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
    public float attackRange = 0.5f;
    public string enemyTag = "Enemy";
    public string enemyLayer = "Enemy";
    public float attackDelay = 1f;

    private Transform target;
    private float lastAttackTime;

    private int weaponTypeSelect = 0;
    const int RANGE_ATTACK_THRESHOLD = 100;

    /*
    *    Auto-Range Attack Timer
    */
    private float AUTO_STOP = 0.1f;
    private float RangeAttackTimer = 0f;

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
        setDefault();
    }    

    void Update()
    {   
        Transform nearestEnemy = null;
        nearestEnemy = findNearestEnemy(isAttacking, weaponTypeSelect);
        if (!isAttacking) {
            if (nearestEnemy) {
                RangeAttackTimer += Time.deltaTime;
                if (Vector2.Distance(transform.position, nearestEnemy.position) <= attackRange) {
                    if ((attackDelay / attackSpeed) < (RangeAttackTimer + AUTO_STOP / attackSpeed)) {
                        RangeNormalAttack(nearestEnemy, attackSpeed);
                        RangeAttackTimer = 0f;
                    }
                }
            }
        }
    }
    void RangeNormalAttack(Transform enemy, float attackSpeed)
    {
        isAttacking = true;
        Attack(enemy, attackSpeed);
        //LaunchMissileVer3(enemy, 0);
        isAttacking = false;
    }    
    // IEnumerator RangeNormalAttack(Transform enemy, float attackSpeed)
    // {
    //     isAttacking = true;
    //     //yield return new WaitForSeconds(2f);
    //     Attack(enemy, attackSpeed);
    //     yield return new WaitForSeconds(2f);
    //     LaunchMissileVer3(enemy, 0);
    //     isAttacking = false;
    // }

    private void Attack(Transform enemy, float attackSpped)
    {
        hands[0].rangeNormalAttackTriggerOn(enemy, attackSpped);
    }

    // IEnumerator LaunchMissileVer3(Transform enemyPos, int bulletStyle, float attackSpeed)
    // {
    //     yield return new WaitForSeconds(1f / attackSpeed);
    //     Vector2 direction = (Vector2)enemyPos.position - (Vector2)transform.position;
    //     direction.Normalize();
    //     Transform bullet = GameManager.instance.pool.GetObject(bulletStyle).transform;
    //     bullet.position = transform.position + (sr.flipX ? new Vector3(0.5f, 0, 0) : new Vector3(-0.5f, 0, 0));

    //     bullet.position = transform.position;
    //     bullet.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    //     bullet.GetComponent<Bullet>().InitVer2(bulletCommonDamage, 1, direction);
    //     StartCoroutine(MissileActiveFalse(bullet.gameObject));
    // }

    // void LaunchMissileVer3(Transform enemyPos, int bulletStyle)
    // {
    //     Vector2 direction = (Vector2)enemyPos.position - (Vector2)transform.position;
    //     direction.Normalize();
    //     Transform bullet = GameManager.instance.pool.GetObject(bulletStyle).transform;
    //     bullet.position = transform.position + (sr.flipX ? new Vector3(0.5f, 0, 0) : new Vector3(-0.5f, 0, 0));

    //     bullet.position = transform.position;
    //     bullet.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    //     bullet.GetComponent<Bullet>().InitVer2(bulletCommonDamage, 1, direction);
    //     StartCoroutine(MissileActiveFalse(bullet.gameObject));
    // }



    IEnumerator AttackStop(float attackSpeed)
    {
        yield return new WaitForSeconds(attackSpeed);
    }

    IEnumerator SetAttacking(bool isAttacking, float duration)
    {
        yield return new WaitForSeconds(duration);
        this.isAttacking = isAttacking;
    }


    private Transform findNearestEnemy(bool isAttacking, int searchRangeSelect) {
        if (isAttacking) return null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemySearchRange, LayerMask.GetMask(enemyLayer));

        float minDistance = float.MaxValue;
        Transform nearestEnemy = null;
        
        foreach (Collider2D collider in colliders)
        {
            // if, nearestEnemy is non-eqaul null, then return nearestEnemy
            if (nearestEnemy != null) {
                return nearestEnemy;
            }
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

        public int Get2D8DirectionToInt(Vector2 inputDir) {
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

    IEnumerator AttackOffTemp() {
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator AttackOff() {
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator MissileActiveFalse(GameObject missile) {
        yield return new WaitForSeconds(3.0f);
        missile.SetActive(false);
    }

    //draw a gizmo to visualize the attack range in the editor
    private void OnDrawGizmosSelected()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, attackRange);
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
}

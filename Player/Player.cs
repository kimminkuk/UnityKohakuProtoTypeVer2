using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    // Player Movement
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

    private bool attackInput = false;

    //임시로, 여기다 두고 나중에 Weapon쪽으로 옮길 예정
    public GameObject missilePrefab;
    private bool missileLaunched = false;
    public float missileSpeed = 10f;
    public Transform missileSpawnPoint;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        hands = GetComponentsInChildren<Hand>(true);
    }


    void Start()
    {
     
    }
    // Update is called once per frame
    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            attackInput = true; // Set the attack flag
        }

        //inputVec를 사용해서, 8방향의 값을 저장
        //inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //inputVec.Normalize(); //벡터의 크기를 1로 만들어줌

    }

    void FixedUpdate() 
    {
        //1. 힘을 준다
        //rb.AddForce(inputVec * 10f, ForceMode2D.Impulse);

        //2. 속도 제어
        //rb.velocity = Vector2.ClampMagnitude(rb.velocity, 5f);

        //3. 위치 이동
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        if (nextVec != Vector2.zero) {
            isMoving = true;
            isIdle = false;
        } else {
            isMoving = false;
            isIdle = true;
        }
        rb.MovePosition(rb.position + nextVec);


        // Normal Attack-Action
        // Space or Mouse Left Button 1Click
        if (attackInput && !isAttacking) {
            isAttacking = true;
            hands[0].NormalAttackOn();
            StartCoroutine(NormalAttackCasting());
            LaunchMissileVer2();
            anim.SetBool("NormalAttack", true);
            StartCoroutine(NormalAttack());     
            Debug.Log("Attack?");
        }
    }

    //프레임이 종료 되기 전 실행되는 함수
    void LateUpdate() 
    {
        if (inputVec.x != 0) {
            sr.flipX = inputVec.x < 0; //좌측이면, true flipX가 켜진거임 그리고 우측이면 false
        }
    }

    IEnumerator NormalAttackCasting() {
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator NormalAttack() {
        yield return new WaitForSeconds(0.25f);
        hands[0].NormalAttackOff();
        isAttacking = false;
        attackInput = false;
        
    }
    void LaunchMissile()
    {
        if (!missileLaunched) {
            // Create a new instance of the missilePrefab
            Transform bullet = GameManager.instance.pool.GetObject(0).transform;
            
            // Get the direction vector for the missile based on the player's transform
            Vector3 missileDirection = missileSpawnPoint.transform.position;
            Vector3 dir = missileDirection - transform.position;
            dir = dir.normalized;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(10, 1, dir); // -1 is Infinity Per.

            // Set the missile's initial direction and velocity
            // Rigidbody2D missileRb = bullet.GetComponent<Rigidbody2D>();
            // missileRb.velocity = dir * missileSpeed;
        }
    }
    
    //missile을 플레이어가 이동하는 방향으로 날리기
    void LaunchMissileVer2()
    {
        if (!missileLaunched) {
            // 1. Create a new instance of the missilePrefab
            Transform bullet = GameManager.instance.pool.GetObject(0).transform;
            // 2. Get the direction vector for the missile based on the player's transform
            Vector3 missileDirection = transform.position;
            // missileDirection을 플레이어가 이동하는 Vector3에 마이너스를 붙여서, 플레이어가 이동하는 방향으로 날리기
            // inputVec는 Vector2인데, 이걸 Vector3로 변환해야함
            
            Vector3 inputVec3 = new Vector3(inputVec.x, inputVec.y, 0);
            inputVec3.Normalize();
            Vector3 dir = missileDirection - inputVec3;
            //dir의 방향을 정규화
            dir = dir.normalized;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(10, 1, dir); // -1 is Infinity Per.
            // 3. Set the missile's initial direction and velocity
            // 4. Set the missileLaunched flag to true
            // 5. Start the LaunchMissileCooldown coroutine
            // 6. Play the missile launch sound effect
            // 7. Play the missile launch animation

        }
    }
}

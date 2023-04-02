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

    //Player 2D 8-direction
    // Enum for 8 possible directions
    private int lastDirection = 0;
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


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        hands = GetComponentsInChildren<Hand>(true);

        //Temp
        hands[0].weaponType = WeaponManager.WeaponType.Staff;
    }


    void Start()
    {
        //if Player's Sprite FlipX is true, then
        // lastDirection is Left  (2)
        //else 
        // lastDirection is Right (6)
        if (sr.flipX) {
            lastDirection = 2;
        } else {
            lastDirection = 6;
        }
    }
    // Update is called once per frame
    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            attackInput = true; // Set the attack flag
        }

        // if inputVec is not zero, then
        // Get2D8DirectionToInt(inputVec) is lastDirection
        if (inputVec != Vector2.zero) {
            lastDirection = Get2D8DirectionToInt(inputVec);
        }
                
        // Vector3 inputVec3 = new Vector3(inputVec.x, inputVec.y, 0).normalized;
        // if (inputVec3 != Vector3.zero) {
        //     lastDirection = Get2D8DirectionToInt(inputVec);
        // }
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
            //LaunchMissileVer2();
            LaunchMissileVer3(lastDirection);
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
            // missileRb.velocity = dir * missileSpeed;dd
        }
    }
    
    //missile을 플레이어가 이동하는 방향으로 날리기
    void LaunchMissileVer2()
    {
        if (!missileLaunched) {
            Transform bullet = GameManager.instance.pool.GetObject(0).transform;
            Vector3 missileDirection = transform.position;
            Vector3 inputVec3 = new Vector3(inputVec.x, inputVec.y, 0);
            inputVec3.Normalize();
            Vector3 dir = missileDirection - inputVec3;
            dir = dir.normalized;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(10, 1, dir); // -1 is Infinity Per.
        }
    }

    //missile을 플레이어가 이동하는 방향으로 날리기
    //1. 플레이어가 이동하는 방향을 구한다
    //2. 이동하는 값은 가속도가 아니라, 방향 자체를 알려준다.
    //3. Vector3로 변환해서, 2D 플레이어가 이동하는 방향으로 미사일을 날린다.
    void LaunchMissileVer3(int playerDirection)
    {
        if (!missileLaunched) {

            //Create New Instance of Missile Prefab
            Transform bullet = GameManager.instance.pool.GetObject(0).transform;

            //Get the direction Vector for the Missile based on the Player's Transform
            //Vector3 missileDirection = transform.position;

            //Get the inputVec normalized based on the Player's inputVec
            // Vector3 inputVec3 = new Vector3(inputVec.x, inputVec.y, 0).normalized;
            // if (inputVec3 != Vector3.zero) {
            //     lastDirection = Get2D8DirectionToInt(inputVec);
            // }
            Debug.Log("playerDirection : " + playerDirection );
            //lastDirection = Get2D8DirectionToInt(inputVec);
            //Debug.Log("sectionIndex : " + sectionIndex + " inputVec3: " + inputVec3);
            // calculate the direction based on the section index
            
            // 5 -> down-right
            // 6 -> right


            // 0 -> up
            // 1 -> up-left
            // 2 -> left
            // 3 -> down-left
            // 4 -> down
            // 5 -> down-right
            // 6 -> right
            // 7 -> up-right            

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
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(10, 1, dir); // -1 is Infinity Per.
            // 3sec After, missile is SetActive False
            StartCoroutine(MissileActiveFalse(bullet.gameObject));
        }
    }

    IEnumerator MissileActiveFalse(GameObject missile) {
        yield return new WaitForSeconds(3.0f);
        missile.SetActive(false);
    }

    // Function to get 2D 8-direction based on input direction
    public Direction2D Get2D8Direction(Vector2 inputDir) {
        // Get angle between forward direction and input direction
        float angle = Vector2.SignedAngle(Vector2.up, inputDir);

        // Calculate index of 2D 8-direction based on angle
        int index = Mathf.RoundToInt(angle / 45.0f) % 8;
        if (index < 0) {
            index += 8;
        }

        // Return corresponding direction
        return (Direction2D)index;
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
    

}
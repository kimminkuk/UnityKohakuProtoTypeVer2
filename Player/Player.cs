using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MoveManager
{
    //RididBody2D, SpriteRenderer, Anim Codes
    public Vector2 playerInputVec;

    private bool attackInput = false;

    //임시로, 여기다 두고 나중에 Weapon쪽으로 옮길 예정
    public GameObject missilePrefab;
    private bool missileLaunched = false;
    public float missileSpeed = 10f;
    public Transform missileSpawnPoint;

    //Player 2D 8-direction
    // Enum for 8 possible directions
    private int PlayerLastDirection = 0;

    private bool canPickup = false;
    private GameObject activeItem;
    private bool isPickingUp = false;
    private float pickupTime = 1.5f; // Adjust this to change the pickup time
    private float pickupTimer = 0f;

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
        if (sr.flipX) {
            PlayerLastDirection = 2;
        } else {
            PlayerLastDirection = 6;
        }
    }
    // Update is called once per frame
    void Update()
    {
        playerInputVec.x = Input.GetAxisRaw("Horizontal");
        playerInputVec.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            attackInput = true; // Set the attack flag
        }
        if (playerInputVec != Vector2.zero) {
            PlayerLastDirection = Get2D8DirectionToInt(playerInputVec);
        }

        //Input.GetKeyDown E
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            isPickingUp = true;
        }
        if (isPickingUp) {
            pickupTimer += Time.deltaTime;
            if (pickupTimer >= pickupTime) {
                weaponType = activeItem.GetComponent<ItemDropClass>().PickupItemDropClassVer2();            
                if (activeItem != null) {
                    activeItem.SetActive(false);
                }
                activeItem = null;
                Debug.Log("Item picked up and added to inventory");
                Debug.Log("Player weaponType: " + weaponType);                
                isPickingUp = false;
                pickupTimer = 0f;
            }
        } else {
            pickupTimer = 0f;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isPickingUp = false;
        }        
    }

    void FixedUpdate() 
    {
        Vector2 nextVec = playerInputVec.normalized * speed * Time.fixedDeltaTime;
        if (nextVec != Vector2.zero) {
            base.isMoving = true;
            base.isIdle = false;
        } else {
            
            base.isMoving = false;
            base.isIdle = true;
        }
        rb.MovePosition(rb.position + nextVec);


        // Normal Attack-Action
        // Space or Mouse Left Button 1Click
        if (attackInput && !isAttacking) {
            base.isAttacking = true;
            hands[0].NormalAttackOn();
            StartCoroutine(NormalAttackCasting());
            LaunchMissileVer3(PlayerLastDirection);
            anim.SetBool("NormalAttack", true);
            StartCoroutine(NormalAttack());     
            Debug.Log("Attack?");
        }
    }

    //프레임이 종료 되기 전 실행되는 함수
    void LateUpdate() 
    {
        if (playerInputVec.x != 0) {
            sr.flipX = playerInputVec.x < 0; //좌측이면, true flipX가 켜진거임 그리고 우측이면 false
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
    //missile을 플레이어가 이동하는 방향으로 날리기
    //1. 플레이어가 이동하는 방향을 구한다
    //2. 이동하는 값은 가속도가 아니라, 방향 자체를 알려준다.
    //3. Vector3로 변환해서, 2D 플레이어가 이동하는 방향으로 미사일을 날린다.
    void LaunchMissileVer3(int playerDirection)
    {
        if (!missileLaunched) {
            Transform bullet = GameManager.instance.pool.GetObject(0).transform;
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
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Item")) {
            canPickup = true;
            activeItem = collision.gameObject;
            Debug.Log("OnTriggerEnter2D Item");
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Item")) {
            Debug.Log("OnTriggerExit2D Item");
            canPickup = false;
        }
    }
    private void PickUpItem()
    {
        Debug.Log("How to Destroy?");
    }    
}
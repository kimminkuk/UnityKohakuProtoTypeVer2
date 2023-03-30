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

    //Attack IEnumerator
    // IEnumerator NormalAttack() {
    //     yield return new WaitForSeconds(0.5f);
    // }
    
    IEnumerator NormalAttack() {
        anim.SetBool("NormalAttack", true);
        hands[0].NormalAttackOn();
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("NormalAttack", false);
        hands[0].NormalAttackOff();
        isAttacking = false;
        attackInput = false;
    }

}

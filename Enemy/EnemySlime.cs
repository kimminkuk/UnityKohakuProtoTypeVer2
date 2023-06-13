using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyMoveCommon
{
    // Update is called once per frame
    protected float xOffsetJumpReady = 2f;
    protected float[] floorYpos = new float[3] { 5.5f, -5.5f, -15.5f };
    
    bool dontAction = false;
    void Update()
    {
        if (!isGrounded) {
            return;
        }
        base.Update();
        //isJumping SetTrigger로 추가 예정.
        // 1. Layer가 Tower인 오브젝트가 x좌표 1만큼 앞에 있으면, isJumping Trigger를 발동시킨다.
        //    Enmey의 움직이는 방향에 따라서, xOffsetJumpReady를 +로 할지 -로 할지 결정한다.
        //    Enemy가 Right To Left 방향으로 움직이면, xOffsetJumpReady를 -로 한다.
        //    sr.flipX가 false, Enemy가 Right To Left 방향으로 움직이고 있는 것이다.
        //    Enemy가 Left To Right 방향으로 움직이면, xOffsetJumpReady를 +로 한다.
        //    sr.flipX가 true이면, Enemy가 Left To Right 방향으로 움직이고 있는 것이다.
        // 2. Layer Tower 말고, Weapon으로 바꿔보자 조금 애매함
        if (!sr.flipX) xOffsetJumpReady *= -1;
        Vector2 xOffset = new Vector2(transform.position.x + xOffsetJumpReady, transform.position.y);
        Collider2D getCollider = Physics2D.OverlapPoint(xOffset, LayerMask.GetMask("Tower"));
        // 2. isJumping Trigger가 발동되면, isJumping을 true로 바꾼다.
        //    가장 처음, Tower에서만 isJumping Trigger가 발동되도록 한다.
        Debug.Log("xOffset.x: " + xOffset.x);
        if (getCollider && !dontAction) {
            anim.SetTrigger("isJumping");
            isJumping = true;
        }

        //지금 점프 무한으로 하는 버그있습니다.
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float jumpProgress = Mathf.Clamp01(jumpTimer / jumpDuration);

            // Calculate the vertical offset based on a jump curve or height value

            // Ypos = sin(PI * jumpProgress) + jumpHeight + transform.position.y 
            // 계속 커진다..?
            // 처음 데이터를 저장해둘까??  -> float ori = transform.position.y ;
            // Ypos = sin(PI * jumpProgress) + jumpHeight + ori;
            // new Vector2(x, Ypos);

            // 오, 지금 점프 괜찮았어! 근데 내려올 때, 다시 방향이 바뀌는 부분하고 (아마 Trigger 때문이겠지)
            // 점프가 끝났을 때, 자연스럽게 땅에 닿도록 유도해야한다.
            float verticalOffset = Mathf.Sin(jumpProgress * Mathf.PI) + floorYpos[0];
            Debug.Log(" Mathf.Sin(jumpProgress * Mathf.PI): " +  Mathf.Sin(jumpProgress * Mathf.PI) + " verticalOffset: " + verticalOffset + " jumpProgress:  " + jumpProgress);
            // Update the object's position with the horizontal movement and vertical offset
            Vector2 newPosition = new Vector2(transform.position.x + moveXpos, verticalOffset);
            rb.MovePosition(newPosition);

            // Check if the jump animation is complete
            if (jumpProgress >= 1f)
            {
                isJumping = false;
                jumpTimer = 0f;
                dontAction = true;
            }
            jumpCount++;
        }
    }
    private void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Call base method to invoke the parent's trigger enter event handling
        base.OnTriggerEnter2D(other);

        // Add additional functionality specific to the child class
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Call base method to invoke the parent's trigger exit event handling
        base.OnTriggerExit2D(other);

        // Add additional functionality specific to the child class
    }

}

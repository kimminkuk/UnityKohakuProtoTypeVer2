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
        xOffsetJumpReady *= isMoveLeft; //isMoveLeft: -1(left Move)  1(right Move)
        Vector2 xOffset = new Vector2(transform.position.x + xOffsetJumpReady, transform.position.y);
        Collider2D getCollider = Physics2D.OverlapPoint(xOffset, LayerMask.GetMask("Tower"));
        if (getCollider && !dontAction) {
            anim.SetTrigger("isJumping");
            dontAction = true;
            isJumping = true;
        }

        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float jumpProgress = Mathf.Clamp01(jumpTimer / jumpDuration);
            float verticalOffset = Mathf.Sin(jumpProgress * Mathf.PI) + floorYpos[0];
            Vector2 newPosition = new Vector2(transform.position.x + moveXpos, verticalOffset);
            rb.MovePosition(newPosition);
            if (jumpProgress >= 1f)
            {
                isJumping = false;
                jumpTimer = 0f;
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

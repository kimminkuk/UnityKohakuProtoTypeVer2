using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeGround : MonoBehaviour
{
    private Collider2D _collider;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered");
            Physics2D.IgnoreCollision(other, _collider, true);

            //MoveManager의 canMove를 true로 변경
            other.GetComponent<MoveManager>().canMove = true;
            other.GetComponent<MoveManager>().isMoveLeft *= -1;
            other.GetComponent<MoveManager>().isGrounded = true;
            other.GetComponent<MoveManager>().isFalling = false;
            
                //         canMove = true;
    //         isMoveLeft *= -1;
    //         isGrounded = true;  
    //         isFalling = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!other.GetComponent<MoveManager>().ifFirstGround) {
                return;
            }
            Debug.Log("Enemy exited");
            Physics2D.IgnoreCollision(other, _collider, false);

                //         isGrounded = false;
    //         isLeavingGround = true;         
            other.GetComponent<MoveManager>().isGrounded = false;
            other.GetComponent<MoveManager>().isLeavingGround = true;
        }
    }
}

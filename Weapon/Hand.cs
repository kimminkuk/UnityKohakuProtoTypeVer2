using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;
    Vector3 weaponPos = new Vector3(-0.5f, -0.1f, 0);
    Vector3 weaponPosReverse = new Vector3(0.5f, -0.1f, 0);
    Quaternion weaponRot = Quaternion.Euler(0, 0, 15);
    Quaternion weaponRotReverse = Quaternion.Euler(0, 0, -15);
    // Quaternion leftRot = Quaternion.Euler(0, 0, -15);
    // Quaternion leftRotReverse = Quaternion.Euler(0, 0, 15);

    Vector3 leftPos = new Vector3(-0.5f, -0.1f, 0);
    Vector3 leftPosReverse = new Vector3(0.1f, -0.3f, 0);

    //player.flipX = false
    //공격할 때, transfrom
    //weaponPos: -0.1, -0.1, 0
    //weaponRot: 0, 0, -60
    Vector3 weaponPosAttack = new Vector3(-0.1f, -0.1f, 0);
    Vector3 weaponPosAttackReverse = new Vector3(0.1f, -0.1f, 0);
    Quaternion weaponRotAttack = Quaternion.Euler(0, 0, -60);
    Quaternion weaponRotAttackReverse = Quaternion.Euler(0, 0, 60);

    private bool normalAttack = false;

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1]; //Parent Sprite is 1 ( myhand 0)

    }

    //Level Up 동작 시, 무기 이미지 변경
    public void ChangeWeaponImage(Sprite sprite)
    {
        spriter.sprite = sprite;
    }

    private void LateUpdate()
    {
        //Player의 isAttacking 상태에 따라 무기의 위치와 회전값을 변경
        bool isReverse = player.flipX;
        if (normalAttack)
        {
            NormalAttackMagicGuy(isReverse);
        }
        else
        {
            IdealState(isReverse);
        }
        // bool isReverse = player.flipX;
        // transform.localRotation = isReverse ? weaponRotReverse : weaponRot;
        // transform.localPosition = isReverse ? weaponPosReverse : weaponPos;
        //spriter.flipX = isReverse;
        //spriter.sortingOrder = isReverse ? 4 : 6;
    }

    public void NormalAttackOn()
    {
        normalAttack = true;
    }

    public void NormalAttackOff()
    {
        normalAttack = false;
    }

    public void NormalAttackMagicGuy(bool isReverse)
    {
        Debug.Log("here?"); 
        transform.localRotation = isReverse ? weaponRotAttackReverse : weaponRotAttack;
        transform.localPosition = isReverse ? weaponPosAttackReverse : weaponPosAttack;
    }

    public void IdealState(bool isReverse) {
        transform.localRotation = isReverse ? weaponRotReverse : weaponRot;
        transform.localPosition = isReverse ? weaponPosReverse : weaponPos;
    }
}

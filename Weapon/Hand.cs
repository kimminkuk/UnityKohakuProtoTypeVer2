using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float knockbackTime;
    public int weaponDamage;
    public bool isAttacking;
    public bool isLeft;
    public SpriteRenderer spriter;
    public Animator weaponAnim;
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
    
    // Enum Value Set, WeaponType
    [SerializeField]
    public WeaponManager.WeaponType weaponType;
    private Vector3[] idealPos = new Vector3[2];
    private Vector3[] normalAttackPos = new Vector3[2];
    private Quaternion[] idealRot = new Quaternion[2];
    private Quaternion[] normalAttackRot = new Quaternion[2];

    private enum E_flipXState { Normal = 0, Reverse }
    private string folderPath = "Sprites/Weapon";
    
    private string spriteName;

    private string folderAnimPath = "Animation/Weapon";
    private void Awake()
    {
        weaponAnim = GetComponent<Animator>();
        player = GetComponentsInParent<SpriteRenderer>()[1]; //Parent Sprite is 1 ( myhand 0)
        spriter = GetComponent<SpriteRenderer>();
        switch(weaponType) {
            case WeaponManager.WeaponType.Sword:
                spriteName = "Normal_Sword";
                break;
            case WeaponManager.WeaponType.Staff:
                spriteName = "Normal_Staff";
                break;
            case WeaponManager.WeaponType.Hammer:
                spriteName = "Normal_Hammer";
                break;
        }
        spriter.sprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
        // idealPos = WeaponTypeIdealPos(weaponType);
        // idealRot = WeaponTypeIdealRot(weaponType);
        // normalAttackPos = WeaponTypeNormalAttackPos(weaponType);
        // normalAttackRot = WeaponTypeNormalAttackRot(weaponType);
    }

    //Level Up 동작 시, 무기 이미지 변경
    public void ChangeWeaponImage(Sprite sprite)
    {
        spriter.sprite = sprite;
    }

    public IEnumerator NormalAttackBool()
    {
        weaponAnim.SetBool("NormalAttack", true);
        yield return new WaitForSeconds(0.5f);
        weaponAnim.SetBool("NormalAttack", false);
    }

    public void NormalAttackTriggerOn()
    {
        weaponAnim.speed = 5.0f;
        //weaponAnim.SetBool("FlipX", flip);
        weaponAnim.SetTrigger("N_Attack");
    }

    public void meleeNormalAttackTriggerOn(float speed)
    {
        weaponAnim.speed = speed;
        //weaponAnim.SetBool("FlipX", flip);
        weaponAnim.SetTrigger("N_Attack");
    }    

    public void rangeNormalAttackTriggerOn(float speed)
    {
        weaponAnim.speed = speed;
        //weaponAnim.SetBool("FlipX", flip);
        weaponAnim.SetTrigger("N_Attack");
    }    

    private void LateUpdate()
    {
        //1. isLeft true 이면, weaponAnim.SetBool("FlipX", true);
        //2. isLeft false 이면, weaponAnim.SetBool("FlipX", false);
        weaponAnim.SetBool("FlipX", isLeft);


        // isLeft 가 true면, weaponAnim을 왼쪽으로 한다.
        // isLeft 가 false면, weaponAnim을 오른쪽으로 한다.
        
    }



    public void normalAttackPosAndRot(bool isReverse)
    {
        transform.localRotation = isReverse ? normalAttackRot[1] : normalAttackRot[0];
        transform.localPosition = isReverse ? normalAttackPos[1] : normalAttackPos[0];
    }

    public void idealStatePosAndRot(bool isReverse)
    {
        transform.localRotation = isReverse ? idealRot[1] : idealRot[0];
        transform.localPosition = isReverse ? idealPos[1] : idealPos[0];
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

    // WeaponType's Vector3 Pos, Rot Set and Return
    public Vector3[] WeaponTypeNormalAttackPos(WeaponManager.WeaponType weaponType) {
        //0: flipX(x, right) 1: flipX(o, left)
        Vector3[] normalAttackPos = new Vector3[2];
        
        switch(weaponType) {
            case WeaponManager.WeaponType.Sword:
                normalAttackPos[(int)E_flipXState.Normal] = new Vector3(0.7f, -0.2f, 0f);
                normalAttackPos[(int)E_flipXState.Reverse] = new Vector3(0f, 0f, 0f);
                break;
            case WeaponManager.WeaponType.Staff:
                normalAttackPos[(int)E_flipXState.Normal] = new Vector3(-0.1f, -0.1f, 0);
                normalAttackPos[(int)E_flipXState.Reverse] = new Vector3(0.1f, -0.1f, 0);
                break;
            case WeaponManager.WeaponType.Hammer:
                normalAttackPos[(int)E_flipXState.Normal] = new Vector3(0.5f, 0f, 0f);
                normalAttackPos[(int)E_flipXState.Reverse] = new Vector3(0f, 0f, 0f);
                break;                
            default:
                Debug.LogError("Invalid weapon type!"); // handle invalid weapon types
                break;
        }
        
        return normalAttackPos;
    }

    public Vector3[] WeaponTypeIdealPos(WeaponManager.WeaponType weaponType) {
        //0: flipX(x, right) 1: flipX(o, left)
        Vector3[] idealPos = new Vector3[2];
        
        switch(weaponType) {
            case WeaponManager.WeaponType.Sword:
            case WeaponManager.WeaponType.Hammer:
                idealPos[(int)E_flipXState.Normal] = new Vector3(0.5f, 0f, 0f);
                idealPos[(int)E_flipXState.Reverse] = new Vector3(-0.5f, 0f, 0f);
                break;
            case WeaponManager.WeaponType.Staff:
                idealPos[(int)E_flipXState.Normal] = new Vector3(-0.5f, -0.1f, 0);
                idealPos[(int)E_flipXState.Reverse] = new Vector3(0.5f, -0.1f, 0);
                break;
            default:
                Debug.LogError("Invalid weapon type!"); // handle invalid weapon types
                break;
        }
        
        return idealPos;
    }

    public Quaternion[] WeaponTypeNormalAttackRot(WeaponManager.WeaponType weaponType) {
        //0: flipX(x, right) 1: flipX(o, left)
        Quaternion[] rot = new Quaternion[2];
        switch (weaponType) {
            case WeaponManager.WeaponType.Sword:
                rot[(int)E_flipXState.Normal] = Quaternion.Euler(0f, 0f, -80f);
                rot[(int)E_flipXState.Reverse] = Quaternion.Euler(0f, 180f, 0f);
                break;
            case WeaponManager.WeaponType.Staff:
                rot[(int)E_flipXState.Normal] = Quaternion.Euler(0, 0, -60);
                rot[(int)E_flipXState.Reverse] = Quaternion.Euler(0, 0, 60);
                break;
            case WeaponManager.WeaponType.Hammer:
                rot[(int)E_flipXState.Normal] = Quaternion.Euler(0f, 0f, 0f);
                rot[(int)E_flipXState.Reverse] = Quaternion.Euler(0f, 180f, 0f);
                break;
            default:
                rot[(int)E_flipXState.Normal] = Quaternion.Euler(0f, 0f, 0f);
                rot[(int)E_flipXState.Reverse] = Quaternion.Euler(0f, 0f, 0f);            
                Debug.LogError("Invalid weapon type!"); // handle invalid weapon types
                break;
        }
        return rot;
    }
    
    public Quaternion[] WeaponTypeIdealRot(WeaponManager.WeaponType weaponType) {
        //0: flipX(x, right) 1: flipX(o, left)
        Quaternion[] rot = new Quaternion[2];
        switch (weaponType) {
            case WeaponManager.WeaponType.Sword:
            case WeaponManager.WeaponType.Hammer:
                rot[(int)E_flipXState.Normal] = Quaternion.Euler(0f, 0f, -10f);
                rot[(int)E_flipXState.Reverse] = Quaternion.Euler(0f, 0f, -15f);
                break;
            case WeaponManager.WeaponType.Staff:
                rot[(int)E_flipXState.Normal] = Quaternion.Euler(0, 0, 15);
                rot[(int)E_flipXState.Reverse] = Quaternion.Euler(0, 0, -15);
                break;
            default:
                rot[(int)E_flipXState.Normal] = Quaternion.Euler(0f, 0f, 0f);
                rot[(int)E_flipXState.Reverse] = Quaternion.Euler(0f, 0f, 0f);            
                Debug.LogError("Invalid weapon type!"); // handle invalid weapon types
                break;
        }
        return rot;
    }

    public void ChangeWeaponType(WeaponManager.WeaponType weaponType)
    {
        player = GetComponentsInParent<SpriteRenderer>()[1]; //Parent Sprite is 1 ( myhand 0)
        spriter = GetComponent<SpriteRenderer>();
        switch(weaponType) {
            case WeaponManager.WeaponType.Sword:
                spriteName = "Normal_Sword";
                break;
            case WeaponManager.WeaponType.Staff:
                spriteName = "Normal_Staff";
                break;
            case WeaponManager.WeaponType.Hammer:
                spriteName = "Normal_Hammer";
                break;
        }
        spriter.sprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
        idealPos = WeaponTypeIdealPos(weaponType);
        idealRot = WeaponTypeIdealRot(weaponType);
        normalAttackPos = WeaponTypeNormalAttackPos(weaponType);
        normalAttackRot = WeaponTypeNormalAttackRot(weaponType);
    }

    public void ChangeWeaponAnim(WeaponManager.WeaponType weaponType) {
        string weaponName = "";
        switch(weaponType) {
            case WeaponManager.WeaponType.Sword:
                weaponName = "Normal_Attack_Sword";
                spriteName = "Normal_Sword";
                break;
            case WeaponManager.WeaponType.Staff:
                break;
            case WeaponManager.WeaponType.Hammer:
                weaponName = "Normal_Attack_Hammer";
                spriteName = "Normal_Hammer";
                break;
        }

        //뭐지?????? sprite가 안바뀜...????? Anim은 바꼇는데????
        spriter.sprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
        Debug.Log("spriteName: " + spriteName + "spriter.sprite: " + spriter.sprite);
        weaponAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(folderAnimPath + "/" + weaponName);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (isAttacking) {
            Debug.Log("Hands OnTriggerEnter2D");
            if (other.CompareTag("Enemy")) {
                //other.GetComponent<EnemyMoveCommon>().TakeDamage(weaponDamage);
                
                other.GetComponent<EnemyMoveCommon>().TakeDamage(weaponDamage, transform.position - other.transform.position , knockbackTime);
                
            }
        }
        // if (other.CompareTag("Enemy")) {
        //     other.GetComponent<EnemyMoveCommon>().TakeDamage(weaponDamage);
        // }        
    }
}

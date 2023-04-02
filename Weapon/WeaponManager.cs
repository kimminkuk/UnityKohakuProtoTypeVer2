using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //Weapon에서, 여러가지 무기를 만들어서, 그 무기들을 적용하고 싶다.
    //그러면, Weapon을 상속받는 클래스를 만들어서, 그 클래스에서 무기를 만들고, 그 무기를 적용하면 된다.
    //예를 들어, Sword, Bow, Gun, Wand, Staff, Axe, Hammer, Dagger, etc...
    //그리고, 그 무기들을 WeaponManager에서 관리하면 된다.
    //그러면, WeaponManager에서, 무기를 생성하고, 무기를 적용하고, 무기를 제거하고, 무기를 교체하고, 무기를 업그레이드하고, 무기를 다운그레이드하고, 무기를 파괴하고, 무기를 판매하고, 무기를 구매하고, 무기를 레벨업한다.
    //그리고, 무기를 생성할 때, 무기의 레벨을 지정할 수 있고, 무기의 레벨을 지정하지 않으면, 무기의 기본 레벨을 적용한다.

/*
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

*/


    public static WeaponManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum WeaponType { Sword, Staff, Hammer, Bow, Gun, Wand, Axe, Dagger }
}

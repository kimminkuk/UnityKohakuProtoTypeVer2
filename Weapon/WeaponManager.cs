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
    // 예시 코드
    // WeaponManager weaponManager = new WeaponManager();
    // weaponManager.CreateWeapon(WeaponType.Sword, 1);
    // weaponManager.CreateWeapon(WeaponType.Bow, 2);
    // weaponManager.CreateWeapon(WeaponType.Gun, 3);
    // weaponManager.CreateWeapon(WeaponType.Wand, 4);
    // weaponManager.CreateWeapon(WeaponType.Staff, 5);
    // weaponManager.CreateWeapon(WeaponType.Axe, 6);
    // weaponManager.CreateWeapon(WeaponType.Hammer, 7);
    // weaponManager.CreateWeapon(WeaponType.Dagger, 8);
    // weapon class에 적용할 수 있는 무기들을 정의한다.
    public enum WeaponType { Sword, Bow, Gun, Wand, Staff, Axe, Hammer, Dagger }
    
    // 무기를 생성한다.
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropClass : MonoBehaviour
{
    [SerializeField]
    public WeaponManager.WeaponType weaponType;

    private string folderPath = "Sprites/Weapon";
    private string spriteName;
    public SpriteRenderer sr;
    private void Awake() {

        //Sprite Renderer랑 일치하게 작성합니다.
        //weaponType = (WeaponManager.WeaponType)Random.Range(0, 3);
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
            default:
                spriteName = "Normal_Sword";
                break;
        }
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickupItemDropClass() {
        Debug.Log("ItemDropClass PickupItemDropClass");
        gameObject.SetActive(false);
    }

    // weaponType return
    public WeaponManager.WeaponType PickupItemDropClassVer2() {
        Debug.Log("ItemDropClass PickupItemDropClassVer2");
        gameObject.SetActive(false);        
        return weaponType;
    }

    //Trigger Entered Event with Tag.Player
    // it's SetActive false
    // private void OnTriggerEnter2D(Collider2D collision) {
    //     if (collision.CompareTag("Player")) {
    //         //collision.GetComponent<Player>().PickupItem(this);
    //         gameObject.SetActive(false);
    //     }
    // }
}

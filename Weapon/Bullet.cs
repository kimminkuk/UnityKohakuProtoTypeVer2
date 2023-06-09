using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    /*
    *    more bullet prefab...
    *    bullet, magic...
    */

    // Start is called before the first frame update
    public int damage;
    public int per;
    public float knockbackTime;

    Rigidbody2D rigid;
    public enum BulletType { Bullet, MagicMissile, Arrow }
    [SerializeField]
    public BulletType bulletType;

    private string folderPath = "Sprites/Bullet";
    private string folderAnimPath = "Animation/Bullet";

    private void Awake()
    {
        //Animation Add..
        switch (bulletType)
        {
            case BulletType.Bullet:
                knockbackTime = 0.2f;
                folderPath += "/Bullet";
                folderAnimPath += "/Bullet";
                break;
            case BulletType.MagicMissile:
                knockbackTime = 0.5f;
                folderPath += "/MagicMissile";
                folderAnimPath += "/MagicMissile";
                break;
            case BulletType.Arrow:
                knockbackTime = 0.3f;
                folderPath += "/Arrow";
                folderAnimPath += "/Arrow";
                break;
            default:
                break;
        }
        rigid = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Init, damage, per, Vector3를 설정하는 함수
    public void Init(int damage, int per, Vector3 dir) {
        this.damage = damage;
        this.per = per;

        //rigid.AddForce(dir * 10, ForceMode2D.Impulse);
        if (per > -1)
        {
            rigid.velocity = dir * 15f;
        }
    }

    public void InitVer2(int damage, int per, Vector2 dir) {
        this.damage = damage;
        this.per = per;

        //rigid.AddForce(dir * 10, ForceMode2D.Impulse);
        if (per > -1)
        {
            rigid.velocity = dir * 15f * 2; // 15f is speed
        }
    }    

    // 충돌이 일어났을 때, 호출되는 함수
    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.CompareTag("Enemy")) {
            collision.GetComponent<EnemyMoveCommon>().TakeDamage(this.damage, transform.position - collision.transform.position , knockbackTime);
            gameObject.SetActive(false);
        }

        // Enter에 넣으니, Enemy 판정과 뭔가 애매해진다.
        // if (collision.CompareTag("Ground")) {
        //     gameObject.SetActive(false);
        // }

        if (collision.CompareTag("GroundSecond")) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // if (collision.CompareTag("Ground")) {
        //     gameObject.SetActive(false);
        // }
    }
    IEnumerator DeactivateBullet(GameObject bulletObject, float time)
    {
        yield return new WaitForSeconds(time);
        
        //Destroy
        Destroy(bulletObject);
    }    
}

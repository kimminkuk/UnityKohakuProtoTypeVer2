using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage;
    public int per;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        // 
    }

    //Init, damage, per, Vector3를 설정하는 함수
    public void Init(float damage, int per, Vector3 dir) {
        this.damage = damage;
        this.per = per;

        //rigid.AddForce(dir * 10, ForceMode2D.Impulse);
        if (per > -1)
        {
            rigid.velocity = dir * 15f;
        }
    }

    // 충돌이 일어났을 때, 호출되는 함수
    private void OnTriggerEnter2D(Collider2D collision) {
        // // 충돌한 상대방의 태그가 "Enemy"이면
        // if(collision.CompareTag("Enemy") ) {
        //     // 상대방의 Enemy 컴포넌트를 가져온다.
        //     Enemy enemy = collision.GetComponent<Enemy>();
        //     // 상대방의 Enemy 컴포넌트의 OnDamaged 함수를 호출한다.
        //     enemy.OnDamaged(damage, per);
        //     // 자신의 Bullet 오브젝트를 파괴한다.
        //     Destroy(gameObject);
        // }
        //if (!collision.CompareTag("Enemy") || per == -1)
        // if (per == -1)
        //     return;
        // per--;

        // Layer is Greater than 9 (9 index is Enemy~)
        if (collision.gameObject.layer >= 9)
        {
            Debug.Log("Bullet OnTriggerEnter2D : " + collision.gameObject.layer);
            Debug.Log("Bullet OnTriggerEnter2D : " + collision.gameObject.name);
            Debug.Log("Bullet OnTriggerEnter2D : " + collision.gameObject.tag);
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }

        // anyWhere.. it's SetActive false
        // if tag is Not Player, then
    }

    void Fire()
    {
        //플레이가 바라보는 방향으로 발사
        Vector3 dir = GameManager.instance.player.transform.position - transform.position;
        
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.GetObject(0).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        Init(1, 1, dir); // -1 is Infinity Per.

        //i want bullet setActive false after 3 seconds
        StartCoroutine(DeactivateBullet(bullet.gameObject, 3f));
    }
    IEnumerator DeactivateBullet(GameObject bulletObject, float time)
    {
        yield return new WaitForSeconds(time);
        bulletObject.SetActive(false);
    }    
}

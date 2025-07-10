using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public GameObject bulletPrefab; //敵が撃つ弾のプレハブ
    public float fireRate = 1.5f; //弾を撃つ間隔(秒)
    public GameObject explosionPrefab; //爆発エフェクトのプレハブ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //敵が生成されたら、弾を撃つルーチンを開始
        StartCoroutine(ShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            //fireRate秒待つ
            yield return new WaitForSeconds(fireRate);

            //弾を生成
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            //生成した弾のタグを「EnemyBullet」に設定
            bullet.tag = "EnemyBullet";
            //生成した弾に下向きの力を与える
            bullet.GetComponent<BulletController>().SetDirection(Vector3.down);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //爆発エフェクトを、敵のいた場所に生成する
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            Destroy(other.gameObject); //弾を破壊する
            Destroy(gameObject); //この敵自身を破壊する
        }
    }
}

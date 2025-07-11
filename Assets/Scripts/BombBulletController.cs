using UnityEngine;

public class BombBulletController : MonoBehaviour
{
    public Transform orbitCenter; //回転の中心(プレイヤー)
    public float orbitSpeed = 200f; //回転速度
    public float orbitRadius = 1.5f; //回転の半径
    public float orbitDuration = 1.5f; //回転する時間

    public float homingSpeed = 15f; //ホーミング時の速度
    private Transform target; //追跡する敵
    private bool isHoming = false; //ホーミングモードに切り替わったかどうかの目印

    private float angle; //現在の角度

    public int damage = 10; //ボムの弾が与えるダメージ量

    public void SetInitialAngle(float initialAngle)
    {
        angle = initialAngle;
    }

    // Update is called once per frame
    void Update()
    {
        //もし回転時間がまだ残っていたら
        if (orbitDuration > 0) {
            //角度を時間とともに変化させる
            angle += orbitSpeed * Time.deltaTime;

            //新しい位置を計算
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;

            //中心の周りの位置に移動
            transform.position = orbitCenter.position + new Vector3(x, y, 0);

            //回転時間を減らしていく
            orbitDuration -= Time.deltaTime;
        }
        //オービットが終わったら
        else
        {
            if(!isHoming)
            {
                //最も近い敵を探して、自分のターゲットに設定する
                target = PlayerController.FindClosestEnemy(transform.position);
                isHoming=true;
            }

            //もしターゲットが見つかっていたら、その方向へ進む
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                transform.Translate(direction * homingSpeed * Time.deltaTime);
            }
            //もしターゲットがいなかったら、とりあえず真上に飛ぶ
            else
            {
                transform.Translate(Vector3.up * homingSpeed * Time.deltaTime);
            }
        }
    }

    //当たり判定のメソッド
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //相手のEnemyControllerを取得して、ダメージを与える
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            //自分自身を破壊する
            Destroy(gameObject);
        }
        else if (other.CompareTag("EnemyBullet"))
        {
            //敵の弾を破壊する
            Destroy(other.gameObject);

            //自分自身を破壊する
            // Destroy(gameObject);
        }
    }
}

using System.Collections;
using UnityEngine;

// abstract: このスクリプトは直接オブジェクトにはアタッチできず、必ず誰かに継承される「設計図専用」であることを示す
public abstract class BaseEnemy : MonoBehaviour
{
    [Header("基本ステータス")]
    public float speed = 3f;
    public int hp = 10;
    public int scoreValue = 10;
    public GameObject explosionPrefab;
    public AudioClip explosionSfx;

    [Header("射撃設定")]
    public GameObject bulletPrefab;
    public float fireRate = 1.5f;

    [Header("アイテムドロップ設定")]
    public GameObject powerUpPrefab;
    [Range(0, 100)]
    public int dropChancePercentage = 5; //アイテムをドロップする確率(%)

    // ゲーム開始時に一度だけ呼ばれる
    protected virtual void Start()
    {
        // 弾を撃つ敵なら、射撃ルーチンを開始する
        if (bulletPrefab != null)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    // 毎フレーム呼ばれる
    protected virtual void Update()
    {
        // ここに、全ての子孫が共通で行う動きなどを書ける
        // 今回は、動きは子孫に完全に任せるので、中身は空
    }

    // ダメージを受ける処理（全ての子孫で共通）
    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;
        if (hp <= 0)
        {
            Die();
        }
    }

    // 破壊される処理（全ての子孫で共通）
    protected virtual void Die()
    {
        // もしスコアマネージャーが存在すれば、スコアを加算
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        // もし爆発エフェクトが設定されていれば、生成
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        //ドロップ確率の抽選
        if (powerUpPrefab != null && Random.Range(1, 101) <= dropChancePercentage)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }

        SoundManager.instance.PlaySfx(explosionSfx);
        // 自身を破壊
        Destroy(gameObject);
    }

    // 弾を撃つ処理のコルーチン（全ての子孫で共通）
    protected virtual IEnumerator ShootRoutine()
    {
        // このメソッドは、弾を撃たない敵では使われない
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            if (bulletPrefab != null)
            {
                // ここでは、単純に真下に弾を撃つロジックを基本としておく
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.tag = "EnemyBullet";
                bullet.GetComponent<BaseBullet>()?.SetDirection(Vector3.down);
            }
        }
    }

    // プレイヤーの通常弾に当たった時の処理（全ての子孫で共通）
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            BaseBullet bullet = other.GetComponent<BaseBullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
            Destroy(other.gameObject);
        }
    }
}

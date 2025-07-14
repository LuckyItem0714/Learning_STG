using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseBoss : MonoBehaviour
{
    [Header("ボスステータス")]
    public float speed = 3f;
    public int maxHp = 500;
    protected int currentHp;
    public int scoreValue = 100;
    public GameObject explosionPrefab;
    public AudioClip explosionSfx;

    [Header("無敵設定")]
    public float spawnInvincibilityDuration = 1.0f; //登場の無敵時間
    protected bool isInvincible = false;

    [Header("攻撃設定")]
    public GameObject bulletPrefab; //ボスが撃つ弾のプレハブ

    //ボスの状態を管理するための変数(Phase'n')
    protected int currentBossPhase = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //体力を最大に設定
        currentHp = maxHp;

        //UIマネージャーにHPバーの表示と初期化を依頼
        if (BossUiManager.instance != null)
        {
            BossUiManager.instance.Show();
            BossUiManager.instance.UpdateHp(currentHp, maxHp);
        }

        //初期フェーズを設定し、ボスのAI行動ルーチンを開始する
        currentBossPhase = 1;
        StartCoroutine(BossAiRoutine());
        StartCoroutine(SpawnInvincibilityCoroutine()); //登場時の無敵コルーチンを開始
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //ここに、後からボスの移動や攻撃パターンの切り替え処理などを追加
    }

    //ダメージを受けるための公開メソッド
    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        //UIマネージャーにHPバーの更新を依頼
        if (BossUiManager.instance != null)
        {
            BossUiManager.instance.UpdateHp(currentHp, maxHp);
        }

        if (currentHp <= 0)
        {
            Die();
        }
    }

    //破壊される処理
    protected virtual void Die()
    {
        //UIマネージャーにHPバーの非表示を依頼
        if (BossUiManager.instance != null)
        {
            BossUiManager.instance.Hide();
        }

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

        SoundManager.instance.PlaySfx(explosionSfx);
        // 自身を破壊
        Destroy(gameObject);
    }

    //プレイヤーの弾に当たった時の処理
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("BombBullet"))
        {
            //弾からダメージ量を取得して、TakeDamageメソッドを呼び出す
            BaseBullet bullet = other.GetComponent<BaseBullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }

            //当たった弾を破壊する
            Destroy(other.gameObject);
        }
    }

    private IEnumerator SpawnInvincibilityCoroutine()
    {
        isInvincible = true; //無敵モードON
        yield return new WaitForSeconds(spawnInvincibilityDuration); //指定された秒数だけ待機
        isInvincible = false; //無敵モードOFF
    }

    protected virtual IEnumerator BossAiRoutine()
    {
        // このメソッドは、弾を撃たない敵では使われない
        while (true)
        {
            yield return new WaitForSeconds(1.5f);

            if (bulletPrefab != null)
            {
                // ここでは、単純に真下に弾を撃つロジックを基本としておく
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.tag = "EnemyBullet";
                bullet.GetComponent<BaseBullet>()?.SetDirection(Vector3.down);
            }
        }
    }
}

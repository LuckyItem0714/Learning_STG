using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("ボスステータス")]
    public int maxHp = 500;
    private int currentHp;

    [Header("無敵設定")]
    public float spawnInvincibilityDuration = 1.0f; //登場の無敵時間
    private bool isInvincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //体力を最大に設定
        currentHp = maxHp;

        //UIマネージャーにHPバーの表示と初期化を依頼
        if (BossUiManager.instance != null )
        {
            BossUiManager.instance.Show();
            BossUiManager.instance.UpdateHp(currentHp, maxHp);
        }

        StartCoroutine(SpawnInvincibilityCoroutine()); //登場時の無敵コルーチンを開始
    }

    // Update is called once per frame
    void Update()
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
    void Die()
    {
        //UIマネージャーにHPバーの非表示を依頼
        if (BossUiManager.instance != null)
        {
            BossUiManager.instance.Hide();
        }

        //ここに、後から爆発エフェクトやスコア加算処理を追加します
        Destroy(gameObject);
    }

    //プレイヤーの弾に当たった時の処理
    private void OnTriggerEnter2D(Collider2D other)
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

    private IEnumerator SpawnInvincibilityCoroutine()
    {
        isInvincible = true; //無敵モードON
        yield return new WaitForSeconds(spawnInvincibilityDuration); //指定された秒数だけ待機
        isInvincible = false; //無敵モードOFF
    }
}

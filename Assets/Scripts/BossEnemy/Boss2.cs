using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2 : BaseBoss
{
    [Header("System References")]
    public GameSettings gameSettings; //設定ファイルを格納する変数を定義

    private float rightBoundary;
    private float leftBoundary;

    [Header("移動設定")]
    public float horizontalSpeed = 2f; //横移動のスピード
    public float patrolWidth = 4f; //横移動する範囲の幅
    private Vector3 initialPosition;
    private int moveDirection = 1; //1なら右、-1なら左

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        initialPosition = transform.localPosition;//自身の初期位置を記憶

        if (gameSettings != null)
        {
            rightBoundary = Mathf.Min(initialPosition.x + patrolWidth / 2, gameSettings.playerMaxX);
            leftBoundary = Mathf.Max(initialPosition.x - patrolWidth / 2, gameSettings.playerMinX);
        }
        else
        {
            Debug.LogWarning("GameSettingsが設定されていません。パトロール範囲のみで移動します。");
            rightBoundary = initialPosition.x + patrolWidth / 2;
            leftBoundary = initialPosition.x - patrolWidth / 2;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        //ここに、後からボスの移動や攻撃パターンの切り替え処理などを追加
        if(!isInvincible)
        {
            transform.Translate(Vector3.right * horizontalSpeed * moveDirection * Time.deltaTime); //左右に移動

        
            //移動範囲の端に到達したら、方向を反転させる
            if (transform.localPosition.x > rightBoundary)
            {
                moveDirection = -1; //左に方向転換
            }
            else if (transform.localPosition.x < leftBoundary)
            {
                moveDirection = 1; //右に方向転換
            }
        }
    }

    //ボスの行動全体を管理するメインのコルーチン
    protected override IEnumerator BossAiRoutine()
    {
        //フェーズ1の攻撃を開始
        Coroutine phase1 = StartCoroutine(Phase1Attack());

        //HPが70%以下になるまで待機
        while (currentHp > maxHp * 0.7f)
        {
            yield return null;
        }

        isInvincible = true;
        yield return new WaitForSeconds(1f); //1秒待ってから
        isInvincible = false;

        //フェーズ1の攻撃を停止し、フェーズ2に移行
        Debug.Log("ボスが第2フェーズに移行!");
        StopCoroutine(phase1);
        currentBossPhase = 2;
        Coroutine phase2 = StartCoroutine(Phase2Attack());
    }

    //フェーズ1の攻撃パターン(例:シンプルなN-Way弾)
    private IEnumerator Phase1Attack()
    {
        while (true)
        {
            //5-Way,60度の弾を発射
            ShootNWay(5, 60f);
            yield return new WaitForSeconds(1.5f);
        }
    }

    //フェーズ2の攻撃パターン(例:回転する円形弾幕)
    private IEnumerator Phase2Attack()
    {
        float burstRotationSpeed = 15f;
        float currentBurstAngle = 0f;

        while (true)
        {
            //12方向の弾を、回転させながら発射
            ShootCircular(12, currentBurstAngle);
            currentBurstAngle += burstRotationSpeed;
            yield return new WaitForSeconds(0.8f);
        }
    }

    //フェーズ3の攻撃パターン(例:フェーズ1と2の組み合わせ)
    private IEnumerator Phase3Attack()
    {
        while (true)
        {
            //8-Way,90度の弾と
            ShootNWay(8, 90f);
            //16方向の弾を同時に発射
            ShootCircular(16, 0);
            yield return new WaitForSeconds(1.0f);
        }
    }

    //N-Way弾を発射するための共通メソッド
    void ShootNWay(int count, float angle)
    {
        float angleStep = angle / (count - 1);
        float startAngle = -angle / 2f;

        for (int i = 0; i < count; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            ShootBullet(currentAngle, Vector3.down);
        }
    }

    //円形弾を発射するための共通メソッド
    void ShootCircular(int count, float initialAngle)
    {
        float angleStep = 360f / count;
        for (int i = 0; i < count; i++)
        {
            float currentAngle = initialAngle + (angleStep * i);
            ShootBullet(currentAngle, Vector3.down);
        }
    }

    //実際に弾を1発生成するための共通メソッド
    void ShootBullet(float angle, Vector3 baseDirection)
    {
        if (bulletPrefab == null) return;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 bulletDirection = rotation * baseDirection;
        Vector3 spawnPosition = transform.position + bulletDirection * 0.7f; //少し前に出す
        GameObject bulletObj = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        bulletObj.GetComponent<BaseBullet>()?.SetDirection(bulletDirection);
    }

    protected override void HandleCollision(Collider2D other)
    {
        // フェーズ1の時、かつプレイヤーの弾に当たった場合
        if (currentBossPhase == 1 && other.CompareTag("Bullet"))
        {
            BaseBullet bullet = other.GetComponent<BaseBullet>();
            if (bullet != null)
            {
                // 1. ボスは半分のダメージを受ける
                TakeDamage(bullet.damage / 2);

                // 2. 弾のタグを"EnemyBullet"に変更する
                other.gameObject.tag = "EnemyBullet";

                // 3. 弾の進行方向ベクトルを真逆にする（これだけ！）
                bullet.moveDirection *= -1;
            }
        }
        else
        {
            base.HandleCollision(other);
        }
    }
}

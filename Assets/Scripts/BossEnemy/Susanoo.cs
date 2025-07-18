using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Susanoo : BaseBoss
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
        while (currentHp > maxHp * 0.9f)
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

    //フェーズ1の攻撃パターン(通常弾1)
    private IEnumerator Phase1Attack()
    {
        float coolTime1 = 0.8f;
        float coolTime2 = 1.2f;
        float timer1 = 0f;
        float timer2 = 0f;

        while (true)
        {
            timer1 += Time.deltaTime;
            timer2 += Time.deltaTime;

            if (timer1 >= coolTime1)
            {
                ShootCircular(20, 45f, 8f);
                timer1 = 0f;
            }
            if (timer2 >= coolTime2)
            {
                ShootCircular(20, 60f, 4f);
                timer2 = 0f;
            }
            yield return null;
        }
    }

    //フェーズ2の攻撃パターン(スキル1)
    private IEnumerator Phase2Attack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 PlayerPosition = player.transform.localPosition; //Playerの位置を記憶
        Vector3 EnemyPosition = transform.localPosition; //敵の位置を記憶
        Vector3 BufferPosition;

        while (true)
        {
            CircleBullet(20, 60f, 4f, 4f);

            PlayerPosition = player.transform.localPosition; //Playerの位置を記憶
            EnemyPosition = transform.localPosition; //敵の位置を記憶
            BufferPosition = PlayerPosition;
            PlayerPosition = EnemyPosition;
            EnemyPosition = BufferPosition;

            transform.Translate(EnemyPosition);
            player.transform.Translate(PlayerPosition);

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

    //速度指定版のN-Way弾メソッド
    void ShootNWay(int count, float angle, float speed)
    {
        float angleStep = angle / (count - 1);
        float startAngle = -angle / 2f;

        for (int i = 0; i < count; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            ShootBullet(currentAngle, Vector3.down, speed); //速度を渡す
        }
    }

    //速度指定版の円形弾メソッド
    void ShootCircular(int count, float initialAngle, float speed)
    {
        float angleStep = 360f / count;
        for (int i = 0; i < count; i++)
        {
            float currentAngle = initialAngle + (angleStep * i);
            ShootBullet(currentAngle, Vector3.down, speed); //速度を渡す
        }
    }

    //対象を中心とした円形に弾幕を出現するメソッド
    void CircleBullet(int count, float initialAngle, float radius, float speed)
    {
        float angleStep = 360f / count;

        float x = Mathf.Cos(initialAngle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(initialAngle * Mathf.Deg2Rad) * radius;

        for (int i = 0; i < count; i++)
        {
            float currentAngle = initialAngle + (angleStep * i);
            ShootBullet(currentAngle, Vector3.down, speed, false);
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

    //ボスから弾が出るのかどうかを判別したうえでの弾を1発生成するためのメソッド
    void ShootBullet(float angle, Vector3 baseDirection, float newSpeed, bool fromPrefab)
    {
        if (bulletPrefab == null) return;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 bulletDirection = rotation * baseDirection;
        Vector3 spawnPosition = transform.position;
        if (fromPrefab)
        {
            spawnPosition += (bulletDirection * 0.7f);
        }
        GameObject bulletObj = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        BaseBullet bullet = bulletObj.GetComponent<BaseBullet>();
        if (bullet != null)
        {
            bullet.SetDirection(bulletDirection);
            bullet.speed = newSpeed; //弾の速度を上書き
        }
    }

    //速度を指定して弾を1発生成するためのメソッド
    void ShootBullet(float angle, Vector3 baseDirection, float newSpeed)
    {
        if (bulletPrefab == null) return;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 bulletDirection = rotation * baseDirection;
        Vector3 spawnPosition = transform.position + bulletDirection * 0.7f;
        GameObject bulletObj = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        BaseBullet bullet = bulletObj.GetComponent<BaseBullet>();
        if (bullet != null)
        {
            bullet.SetDirection(bulletDirection);
            bullet.speed = newSpeed; //弾の速度を上書き
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
}

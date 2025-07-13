using System.Collections;
using UnityEngine;

public class SpinShooterEnemy : BaseEnemy
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

    [Header("円形弾幕設定")]
    public int bulletsPerBurst = 12; //一度に発射する弾の数
    public float burstRotationSpeed = 10f; //弾幕が回転するスピード
    private float currentBurstAngle = 0f; //現在の弾幕の基本角度

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start(); //まず親のStart処理を呼び出す(これにより射撃が開始される)
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

    //射撃ルーチンを上書きして、円形弾幕を実装
    protected override IEnumerator ShootRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(fireRate); //次の発射まで待機
            currentBurstAngle += burstRotationSpeed; //弾幕の角度を少しずつ回転させる(スパイラル効果)
            float angleStep = 360f / bulletsPerBurst; //360度を弾の数で割って、弾と弾の間の角度を計算

            //forループで円形に弾を発射
            for (int i = 0; i < bulletsPerBurst; i++)
            {
                float currentAngle = currentBurstAngle + (angleStep * i); //現在の弾の角度を計算(弾幕の回転+弾ごとの角度)
                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle); //角度から回転を生成
                Vector3 bulletDirection = rotation * Vector3.down; //基本方向(真下)に回転を適用

                //弾を生成し、進行方向を決定
                Vector3 spawnPosition = transform.position + bulletDirection * 0.5f;
                GameObject bulletObj = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                bulletObj.GetComponent<BaseBullet>()?.SetDirection(bulletDirection);
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class NWayShooterEnemy : BaseEnemy
{
    [Header("N-Way弾の設定")]
    public int numberOfBullets = 5; //発射する弾の数
    public float spreadAngle = 90f; //弾が広がる全体の角度

    protected override IEnumerator ShootRoutine()
    {
        //ゲームが続いている限り、無限に繰り返す
        while (true)
        {
            //次の発射まで待機
            yield return new WaitForSeconds(fireRate);

            //弾が1発以下の場合は何もしない(計算エラー防止)
            if (numberOfBullets <= 1)
            {
                continue;
            }

            float angleStep = spreadAngle / (numberOfBullets - 1); //弾と弾の間の角度を計算する
            float startAngle = -spreadAngle / 2f; //発射する弾の最初の角度を計算する

            //forループで、弾の数だけ弾を生成する
            for (int i = 0; i < numberOfBullets; i++)
            {
                float currentAngle = startAngle + (angleStep * i); //現在の弾の角度を計算する
                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle); //角度(オイラー角)から回転(クォータニオン)を生成
                Vector3 bulletDirection = rotation * Vector3.down; //基本となる発射方向(真下)に、計算した回転を適用する
                Vector3 spawnPosition = transform.position + bulletDirection * 0.5f; //弾の出現位置を、進行方向に少しだけずらす
                GameObject bulletObj = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity); //弾を生成
                bulletObj.GetComponent<BaseBullet>()?.SetDirection(bulletDirection); //弾の進行方向を設定
            }
        }
    }
}

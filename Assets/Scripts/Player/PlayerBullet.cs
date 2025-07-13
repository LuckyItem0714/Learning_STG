using UnityEngine;

public class PlayerBullet : BaseBullet
{
    // Update is called once per frame
    protected override void Update()
    {
        if (target != null)
        {
            //ターゲットの方向に向かうベクトルを計算
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            //現在の進行方向を、ターゲットの方向で上書き更新する
            moveDirection = directionToTarget;
        }

        base.Update();
    }
}

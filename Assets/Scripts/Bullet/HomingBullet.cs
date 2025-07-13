using UnityEngine;

public class HomingBullet : BaseBullet
{
    // Update is called once per frame
    protected override void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }

        if (target != null)
        {
            //ターゲットの方向に向かうベクトルを計算
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            //現在の進行方向を、ターゲットの方向で上書き更新する
            moveDirection = directionToTarget;
        }

        //常に、記憶されたmoveDirectionの方向へ移動する
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}

using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1; //この弾が与えるダメージ量
    public Transform target; //追跡する目標

    private Vector3 moveDirection = Vector3.up; //デフォルトは上方向

    // Update is called once per frame
    void Update()
    {
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

    //外部から弾の進行方向を設定するためのメソッド
    public void SetDirection(Vector3 newDirection)
    {
        moveDirection = newDirection;
    }
}

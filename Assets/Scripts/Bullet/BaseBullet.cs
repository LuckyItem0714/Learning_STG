using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1; //この弾が与えるダメージ量
    public Transform target; //追跡する目標

    public Vector3 moveDirection = Vector3.up; //デフォルトは上方向

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (moveDirection != Vector3.zero)
        {
            // moveDirectionから角度を計算し、Z軸周りの回転を生成する
            // Atan2はy,xの順で引数を渡す。-90fはスプライトの上が正面の場合の補正値
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    //外部から弾の進行方向を設定するためのメソッド
    public void SetDirection(Vector3 newDirection)
    {
        moveDirection = newDirection;
    }
}

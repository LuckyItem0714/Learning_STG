using UnityEngine;

public class BombBulletController : BaseBullet
{
    // --- 公開設定 ---
    public float orbitSpeed = 200f;
    public float orbitRadius = 1.5f;
    public float orbitDuration = 1.5f;
    public float homingSpeed = 15;

    // --- 内部変数 ---
    private Transform orbitCenter;
    private Vector3 lastKnownPosition;
    private float angle;

    //ボム弾の状態を管理するための「enum（列挙型）」
    private enum BombState { Orbiting, Homing, MovingToLastPos }
    private BombState currentState;

    // --- 初期設定 ---
    public void Initialize(Transform center, float initialAngle)
    {
        orbitCenter = center;
        angle = initialAngle;
        currentState = BombState.Orbiting; //初期状態は周回
    }

    // Update is called once per frame
    protected override void Update()
    {
        //現在の状態で、どの処理を行うか切り替える
        switch (currentState)
        {
            case BombState.Orbiting:
                HandleOrbiting();
                break;
            case BombState.Homing:
                HandleHoming();
                break;
            case BombState.MovingToLastPos:
                HandleMovingToLastPosition();
                break;
        }
    }

    // --- 各状態の処理 ---
    void HandleOrbiting()
    {
        if (orbitDuration > 0 && orbitCenter != null)
        {
            angle += orbitSpeed * Time.deltaTime;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;
            transform.position = orbitCenter.position + new Vector3(x, y, 0);
            orbitDuration -= Time.deltaTime;
        }
        else
        {
            //周回が終わったらホーミング状態へ移行
            target = PlayerController.FindClosestEnemy(transform.position);
            if (target != null)
            {
                lastKnownPosition = target.position; //最後の位置を記憶
                currentState = BombState.Homing;
            }
            else
            {
                //敵がいない場合は、まっすぐ上に飛んでいく
                lastKnownPosition = transform.position + Vector3.up * 10f;
                currentState = BombState.MovingToLastPos;
            }
        }
    }

    void HandleHoming()
    {
        if (target != null)
        {
            lastKnownPosition = target.position; //ターゲットの位置を追いかけながら記憶
            Vector3 direction = (lastKnownPosition - transform.position).normalized;
            transform.Translate(direction * homingSpeed * Time.deltaTime);
        }
        else
        {
            //ターゲットを見失ったら、最後の位置へ向かう状態に移行
            currentState = BombState.MovingToLastPos;
        }
    }

    void HandleMovingToLastPosition()
    {
        //最後の位置に向かってまっすぐ飛ぶ
        Vector3 direction = (lastKnownPosition - transform.position).normalized;
        transform.Translate(direction * homingSpeed * Time.deltaTime);

        //目的地に十分近づいたら、自分を破壊する
        if (Vector3.Distance(transform.position, lastKnownPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    //当たり判定のメソッド
    void OnTriggerEnter2D(Collider2D other)
    {
        if (currentState == BombState.Homing || currentState == BombState.MovingToLastPos)
        {
            if (other.CompareTag("Enemy"))
            {
                //カメラシェイクを呼び出す
                CameraShake.instance.TriggerShake(0.2f, 0.5f);

                other.GetComponent<BaseEnemy>()?.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        //状態に関わらず、敵の弾はいつでも消す
        if (other.CompareTag("EnemyBullet"))
        {
            //敵の弾を破壊する
            Destroy(other.gameObject);

            //自分自身を破壊する
            // Destroy(gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>()?.TakeDamage(damage);
        }
    }
}

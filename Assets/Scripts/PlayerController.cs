using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float normalSpeed = 5f; //通常スピード
    public float slowSpeed = 2.5f; //低速スピード
    public float moveSmoothing = 0.1f; //移動の滑らかさ(小さいほど機敏、大きいほど滑る)
    private Vector3 currentVelocity; //現在の移動速度と方向を保持する変数

    public int maxHp = 3; //最大HP
    private int currentHp; //現在のHP
    public HpUiManager hpUiManager;

    [Header("射撃設定")] //インスペクターに見出しを追加
    public GameObject bulletPrefab; //弾のプレハブを格納する変数
    public float fireRate = 0.25f; //弾の発射間隔(0.25秒に1発)
    public int normalShotDamage = 2; //通常弾のダメージ
    public int homingShotDamage = 1; //ホーミング弾のダメージ
    private float nextFireTime = 0f; //次に弾を発射できる時間

    [Header("ボム設定")]
    public GameObject bombBulletPrefab; //ボムの弾プレハブ
    public int bombBulletCount = 10; //生成するボムの弾の数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ゲーム開始時にHPを最大にする
        currentHp = maxHp;
        Debug.Log("プレイヤーのHP : " + currentHp);
        hpUiManager.UpdateHp(currentHp);

        //初期位置変更
        transform.position = new Vector3(0, -3.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //現在のフレームで適用するスピードを決定する
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? slowSpeed : normalSpeed;

        //プレイヤーの移動
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 targetVelocity = new Vector3(x, y, 0) * targetSpeed;

        //現在の速度(currentVelocity)を、目標の速度(targetVelocity)に滑らかに近づける
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 1 - Mathf.Pow(moveSmoothing, Time.deltaTime));

        Vector3 newPosition = transform.localPosition + currentVelocity * Time.deltaTime;

        //座標を制限
        newPosition.x = Mathf.Clamp(newPosition.x, -4.45f, 4.45f);
        newPosition.y = Mathf.Clamp(newPosition.y, -5f, 5f);

        transform.localPosition = newPosition;

        //弾を撃つ処理
        if (Input.GetKey(KeyCode.Z) && Time.time > nextFireTime)
        {
            //次に発射可能な時間を、現在の時間に発射間隔を加えたものに更新
            nextFireTime = Time.time + fireRate;

            //Shiftキーが押されているかで射撃モードを切り替え
            if(Input.GetKey(KeyCode.LeftShift))
            {
                //ホーミング弾を発射
                FireHomingBullet();
            }
            else
            {
                //通常弾を発射
                FireNormalBullet();
            }
        }

        //ボムの発動処理
        if (Input.GetKeyDown(KeyCode.X))
        {
            FireBomb();
        }
    }

    //通常弾を発射するメソッド
    void FireNormalBullet()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        BulletController bullet = bulletObj.GetComponent<BulletController>();

        //生成した弾のBulletControllerを取得し、進行方向を「上」に設定する
        bullet.SetDirection(Vector3.up);
        //通常弾のダメージを設定
        bullet.damage = normalShotDamage;
    }

    //ホーミング弾を発射するメソッド
    void FireHomingBullet()
    {
        //最も近い敵を探す
        Transform closestEnemy = FindClosestEnemy(transform.position);

        if (closestEnemy != null)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BulletController bullet = bulletObj.GetComponent<BulletController>();
            
            //弾のターゲットとして、見つけた最も近い敵を設定する
            bullet.target = closestEnemy;
            //ホーミング弾のダメージを設定
            bullet.damage = homingShotDamage;
        }
        else
        {
            //もし敵が一体もいなければ、代わりに通常弾を発射する
            FireNormalBullet();
        }
    }

    //最も近い敵を探し出すメソッド
    public static Transform FindClosestEnemy(Vector3 fromPosition)
    {
        //"Enemy"タグを持つすべてのオブジェクトを探して、配列に入れる
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity; //最も近い敵との距離の2乗を記録
        // Vector3 currentPosition = transform.position;

        //見つけた全ての敵に対してループ処理
        foreach (GameObject potentialEnemy in enemies)
        {
            Vector3 directionToTarget = potentialEnemy.transform.position - fromPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude; //距離の2乗を計算

            //もし、今までの最短距離よりも近ければ
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialEnemy.transform;
            }
        }

        return bestTarget; //見つけた最も近い敵を返す
    }

    //ボムを発射するメソッド
    void FireBomb()
    {
        //360度を、弾の数で均等に割る
        float angleStep = 360f / bombBulletCount;

        for (int i = 0; i < bombBulletCount; i++)
        {
            //現在の弾の角度を計算
            float currentAngle = i * angleStep;

            //弾をプレイヤーの位置に生成
            GameObject bomb = Instantiate(bombBulletPrefab, transform.position, Quaternion.identity);

            //弾のコントローラーを取得し、中心(自分自身)と初期角度をセットする
            BombBulletController bombController = bomb.GetComponent<BombBulletController>();
            if (bombController != null)
            {
                bombController.orbitCenter = transform; //「transform」はプレイヤー自身を指す
                bombController.SetInitialAngle(currentAngle); //各弾に、それぞれの発射角度を教える
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //ぶつかってきた相手のタグが「Enemy」または「EmenyBullet」だったら
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
        {
            //ぶつかってきたオブジェクト(敵か敵の弾)を破壊する
            Destroy(other.gameObject);

            //HPを1減らす
            currentHp--;
            Debug.Log("プレイヤーのHP : " + currentHp); //HPをコンソールに表示
            hpUiManager.UpdateHp(currentHp);

            //もしHPが0以下になったら
            if (currentHp <= 0)
            {
                //このプレイヤーオブジェクトを破壊する
                Destroy(gameObject);
            }
        }
    }
}

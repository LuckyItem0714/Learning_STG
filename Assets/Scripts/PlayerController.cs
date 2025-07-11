using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject bulletPrefab; //弾のプレハブを格納する変数

    public int maxHp = 3; //最大HP
    private int currentHp; //現在のHP

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ゲーム開始時にHPを最大にする
        currentHp = maxHp;
        Debug.Log("プレイヤーのHP : " + currentHp);
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの移動
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, y, 0);

        Vector3 newPosition = transform.localPosition + direction * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, -2f, 1.7f);
        newPosition.y = Mathf.Clamp(newPosition.y, -5f, 5f);

        transform.localPosition = newPosition;

        //弾を撃つ処理
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            //生成した弾のBulletControllerを取得し、進行方向を「上」に設定する
            bullet.GetComponent<BulletController>().SetDirection(Vector3.up);
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

            //もしHPが0以下になったら
            if (currentHp <= 0)
            {
                //このプレイヤーオブジェクトを破壊する
                Destroy(gameObject);
            }
        }
    }
}

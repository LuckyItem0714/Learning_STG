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
        
    }

    //外部から弾の進行方向を設定するためのメソッド
    public void SetDirection(Vector3 newDirection)
    {
        moveDirection = newDirection;
    }
}

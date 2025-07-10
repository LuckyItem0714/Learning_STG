using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 moveDirection = Vector3.up; //デフォルトは上方向

    // Update is called once per frame
    void Update()
    {
        //設定された方向に、まっすぐ進み続ける
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    //外部から弾の進行方向を設定するためのメソッド
    public void SetDirection(Vector3 newDirection)
    {
        moveDirection = newDirection;
    }
}

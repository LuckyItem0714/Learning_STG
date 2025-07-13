using UnityEngine;

public class StraightBullet : BaseBullet
{
    // Update is called once per frame
    protected override void Update()
    {
        //常に下方向へ移動する
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}

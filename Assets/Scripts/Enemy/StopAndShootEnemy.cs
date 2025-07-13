using UnityEngine;

public class StopAndShootEnemy : BaseEnemy
{
    [Header("停止設定")]
    public float stopYPosition = 3.0f; //敵が停止する画面のY座標

    private bool hasStopped = false; //敵がすでに停止したかどうかを記憶する旗(フラグ)

    // Update is called once per frame
    protected override void Update()
    {
        if (!hasStopped)
        {
            if (transform.position.y > stopYPosition)
            {
                transform.Translate(Vector3.down * speed *  Time.deltaTime);
            }
            else
            {
                hasStopped = true;
            }
        }
        //hasStoppedがtrueになった後は、このUpdateメソッドは何もしなくなる
    }
}

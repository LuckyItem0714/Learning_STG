using UnityEngine;

// MonoBehaviourの代わりに、先ほど作ったBaseEnemyを継承する
public class SineWaveEnemy : BaseEnemy
{
    [Header("サインカーブ移動の設定")]
    public float sineFrequency = 2f;
    public float sineMagnitude = 1f;

    private Vector3 startPosition;

    // BaseEnemyのStartメソッドを「上書き」して、独自の処理を追加する
    protected override void Start()
    {
        // まず、ご先祖様のStart()の処理を呼び出す（これにより射撃などが開始される）
        base.Start();

        // その上で、自分だけの初期設定を行う
        startPosition = transform.position;
    }

    // BaseEnemyのUpdateメソッドを「上書き」して、自分だけの動きを実装する
    protected override void Update()
    {
        // ご先祖様のUpdate()は空なので、呼び出さなくてもOK
        // base.Update();

        // Y軸方向（下）には、ご先祖様から受け継いだspeedでまっすぐ進む
        // ※この動きも共通にするなら、BaseEnemy側に書いても良い
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // X軸方向（横）には、サインカーブで揺れる
        float newX = startPosition.x + Mathf.Sin(Time.time * sineFrequency) * sineMagnitude;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
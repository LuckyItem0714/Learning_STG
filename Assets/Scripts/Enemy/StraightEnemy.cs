using UnityEngine;

public class StraightEnemy : BaseEnemy
{
    //個の敵は特別な設定は不要

    // Update is called once per frame
    protected override void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}

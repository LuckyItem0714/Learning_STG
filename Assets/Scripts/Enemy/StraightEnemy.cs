using UnityEngine;

public class StraightEnemy : BaseEnemy
{
    //�̓G�͓��ʂȐݒ�͕s�v

    // Update is called once per frame
    protected override void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}

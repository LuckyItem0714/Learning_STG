using UnityEngine;

public class StraightEnemy : BaseEnemy
{
    //ŒÂ‚Ì“G‚Í“Á•Ê‚Èİ’è‚Í•s—v

    // Update is called once per frame
    protected override void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}

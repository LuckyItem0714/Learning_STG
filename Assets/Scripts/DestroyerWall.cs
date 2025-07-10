using UnityEngine;

public class DestroyerWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //この壁に触れた、あらゆるオブジェクトを破壊する
        Destroy(other.gameObject);
    }
}

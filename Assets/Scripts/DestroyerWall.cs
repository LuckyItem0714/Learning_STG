using UnityEngine;

public class DestroyerWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //���̕ǂɐG�ꂽ�A������I�u�W�F�N�g��j�󂷂�
        Destroy(other.gameObject);
    }
}

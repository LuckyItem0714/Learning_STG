using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 0.5f; //0.5�b��ɏ�����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //lifetime�b��ɁA���̃I�u�W�F�N�g��j�󂷂�
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

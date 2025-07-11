using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 0.5f; //0.5秒後に消える

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //lifetime秒後に、このオブジェクトを破壊する
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

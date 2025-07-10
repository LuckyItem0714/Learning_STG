using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject bulletPrefab; //’e‚ÌƒvƒŒƒnƒu‚ğŠi”[‚·‚é•Ï”

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ƒvƒŒƒCƒ„[‚ÌˆÚ“®
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(x, y, 0);

        transform.Translate(direction * speed * Time.deltaTime);

        //’e‚ğŒ‚‚Âˆ—
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }
    }
}

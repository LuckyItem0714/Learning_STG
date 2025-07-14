using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [Tooltip("背景がスクロールする速さ")]
    public float scrollSpeed = 1.0f;

    [Tooltip("このY座標より下に行ったら、上にワープする")]
    public float resetBoundaryY = -15f;

    private float spriteHeight; //スプライトの縦の高さ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //自身の高さを記憶しておく
        spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime); //毎フレーム、真下にスクロールさせる

        //もし、画像1枚分が完全に見えなくなるくらい下に移動したら
        if (transform.position.y < resetBoundaryY)
        {
            transform.position += new Vector3(0, spriteHeight * 2f, 0); //画像2枚分の高さだけ、上に瞬間移動させる
        }
    }
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] animationFrames; //アニメーションさせるスプライトの配列
    public float framesPerSecond = 10f; //1秒あたりのフレーム数(アニメーションの速さ)
    public bool destroyOnCompletion = true; //アニメーション終了後にオブジェクトを破壊する

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        int frameIndex = 0;
        while (frameIndex < animationFrames.Length)
        {
            spriteRenderer.sprite = animationFrames[frameIndex]; //現在のフレームのスプライトを表示
            yield return new WaitForSeconds(1f / framesPerSecond); //次のフレームまで待機
            frameIndex++; //次のフレームへ
        }

        //アニメーションが終了し、破壊フラグがONなら自身を破壊
        if (destroyOnCompletion)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private bool isShaking = false; //現在シェイク中かの目印

    void Awake()
    {
        instance = this;
    }

    //このメソッドを外部から呼び出すように変更
    public void TriggerShake(float duration, float magnitude)
    {
        //もしシェイク中なければ、新しいシェイクを開始する
        if (!isShaking)
        {
            StartCoroutine(Shake(duration, magnitude));
        }
    }

    private IEnumerator Shake (float duration, float magnitude)
    {
        isShaking = true; //シェイク開始の旗を立てる
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null; //次のフレームまで待つ
        }

        transform.localPosition = originalPos; //カメラをもとの位置に戻す
        isShaking=false; //シェイク終了の旗を下ろす
    }
}

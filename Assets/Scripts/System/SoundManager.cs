using UnityEngine;
using UnityEngine.UI;

// [RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; //シングルトンインスタンス

    [Header("BGM設定")]
    public AudioSource bgmSource; //BGM再生用のスピーカー
    public bool useCustomLoop = false; //カスタムループ機能を使うかどうかのスイッチ
    [Tooltip("ループを開始する時間(秒)")]
    public float loopStartTime = 20.0f;
    [Tooltip("ループを終了し、開始地点に戻る時間(秒)")]
    public float loopEndTime = 80.0f;

    [Header("SFX設定")]
    public AudioSource sfxSource; //効果音再生用のスピーカー

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //シーンをまたいでも破壊されないようにする
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // BGMの再生を開始
        if (bgmSource != null)
        {
            bgmSource.Play();
        }
    }

    void Update()
    {
        // もしカスタムループが有効なら、ループ処理を実行
        if (useCustomLoop && bgmSource != null && bgmSource.isPlaying)
        {
            if (bgmSource.time >= loopEndTime)
            {
                bgmSource.time = loopStartTime;
            }
        }
    }

    //どこからでも効果音を再生できる公開メソッド
    public void PlaySfx(AudioClip clip)
    {
        //sfxSourceが設定されていれば、PlayOneShotで音を再生
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}

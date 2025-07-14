using UnityEngine;
using UnityEngine.UI;

public class BossUiManager : MonoBehaviour
{
    public static BossUiManager instance; //シングルトンインスタンス

    public GameObject bossUiContainer; //HPバー全体を格納する親オブジェクト
    public Slider hpSlider; //HPを表示するスライダー

    void Awake()
    {
        //シングルトンの設定
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hide(); //開始時は非表示にしておく
    }

    // Update is called once per frame
    public void UpdateHp(int currentHp, int maxHp)
    {
        hpSlider.value = (float)currentHp / maxHp; //スライダーの値を、現在のHPの割合(0.0～1.0)に設定する
    }

    //HPバーを表示するメソッド
    public void Show()
    {
        bossUiContainer.SetActive(true);
    }

    //HPバーを非表示にするメソッド
    public void Hide()
    {
        bossUiContainer.SetActive(false);
    }
}

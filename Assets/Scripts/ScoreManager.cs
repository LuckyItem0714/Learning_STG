using UnityEngine;
using TMPro; //TextMeshProを扱うために必要

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; //他スクリプトからアクセスするための参照
    public TextMeshProUGUI scoreText; //スコアを表示するUIテキスト要素

    private int score = 0;

    private void Awake()
    {
        //インスタンスの参照を設定する
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
        //スコア表示を初期化する
        scoreText.text = "Score : " + score.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score : " + score.ToString();
    }
}

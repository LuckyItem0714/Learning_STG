using UnityEngine;
using TMPro; //TextMeshPro���������߂ɕK�v

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; //���X�N���v�g����A�N�Z�X���邽�߂̎Q��
    public TextMeshProUGUI scoreText; //�X�R�A��\������UI�e�L�X�g�v�f

    private int score = 0;

    private void Awake()
    {
        //�C���X�^���X�̎Q�Ƃ�ݒ肷��
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
        //�X�R�A�\��������������
        scoreText.text = "Score : " + score.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score : " + score.ToString();
    }
}

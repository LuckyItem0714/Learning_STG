using System.Collections;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spaenInterval = 2f;
    public float spawnAreaWidth = 8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�Q�[�����n�܂�����A�G�𐶐����郋�[�`�����J�n����
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true) //����Ŗ����ɓG�𐶐���������
        {
            //��ʏ㕔�́A���E�����_���Ȉʒu���v�Z
            float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
            Vector3 spawnPosition = new Vector3(randomX, 3f, 0);

            //�G�𐶐�
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            //�w�肵���b�������A�������ꎞ��~����
            yield return new WaitForSeconds(spaenInterval);
        }
    }
}

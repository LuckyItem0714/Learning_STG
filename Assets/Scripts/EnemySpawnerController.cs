using System.Collections;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
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
            Vector3 localSpawnPosition = new Vector3(randomX, 4f, 0);
            Vector3 worldSpawnPosition = transform.position + localSpawnPosition;

            //�G�𐶐�
            Instantiate(enemyPrefab, worldSpawnPosition, Quaternion.identity);

            //�w�肵���b�������A�������ꎞ��~����
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

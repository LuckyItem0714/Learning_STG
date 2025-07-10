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
        //ゲームが始まったら、敵を生成するルーチンを開始する
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true) //これで無限に敵を生成し続ける
        {
            //画面上部の、左右ランダムな位置を計算
            float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
            Vector3 spawnPosition = new Vector3(randomX, 3f, 0);

            //敵を生成
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            //指定した秒数だけ、処理を一時停止する
            yield return new WaitForSeconds(spaenInterval);
        }
    }
}

using System.Collections;
using UnityEngine;

public class RandomEnemySpawner : BaseEnemySpawner
{
    [Header("生成設定")]
    public Transform gameWorld;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
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
            Vector3 localSpawnPosition = new Vector3(randomX, 4f, 0);
            Vector3 worldSpawnPosition = transform.position + localSpawnPosition;

            //敵のリストの中から、ランダムに1種類を選ぶ
            GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            //敵を生成
            GameObject enemyObj = Instantiate(enemyToSpawn, worldSpawnPosition, Quaternion.identity);

            //もしgameWorldが設定されていたら、それを親にする
            if (gameWorld != null)
            {
                enemyObj.transform.SetParent(gameWorld);
            }

            //指定した秒数だけ、処理を一時停止する
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

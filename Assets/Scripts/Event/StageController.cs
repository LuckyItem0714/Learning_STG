using System.Collections;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("このステージの出現イベントリスト")]
    public SpawnEvent[] spawnEvents;

    [Header("親となるGameWorld")]
    public Transform gameWorld; //敵をGameWorldの子にするため

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RunStage()); //ステージ実行のコルーチンを開始
    }

    IEnumerator RunStage()
    {
        //リストにある全てのイベントを順番に実行する
        foreach (var currentEvent in spawnEvents)
        {
            yield return StartCoroutine(ExecuteEvent(currentEvent)); //現在のイベントを実行するコルーチンを呼び出し、それが終わるまで待つ
            yield return new WaitForSeconds(currentEvent.delayAfterEvent);
        }

        Debug.Log("ステージクリア!");
        //ここに、将来的にボス出現などの処理を追加していく
    }

    IEnumerator ExecuteEvent(SpawnEvent spawnEvent)
    {
        //中ボスかどうかを判定(敵が一体だけのイベントを中ボスとみなす)
        bool isBossEvent = spawnEvent.eventType == SpawnEvent.EventType.MidBoss ||
                           spawnEvent.eventType == SpawnEvent.EventType.FinalBoss;

        GameObject bossInstance = null;

        //指定された数だけ、敵を一体ずつ出現させる
        for (int i = 0; i < spawnEvent.enemyCount; i++)
        {
            Vector3 worldSpawnPos = gameWorld.TransformPoint(spawnEvent.spawnPosition); //SpawnEventに設定されたローカル座標を、ワールド座標に変換する

            GameObject enemyObj = Instantiate(spawnEvent.enemyPrefab, worldSpawnPos, Quaternion.identity); //敵を生成

            if (isBossEvent)
            {
                bossInstance = enemyObj; //中ボスのインスタンスを記憶
            }

            //GameWorldの子にする
            if (gameWorld != null)
            {
                enemyObj.transform.SetParent(gameWorld);
            }

            yield return new WaitForSeconds(spawnEvent.spawnInterval); //次の敵を出現させるまで待機
        }

        //もし中ボスイベントだったら、それが破壊されるまで待つ
        if (isBossEvent && bossInstance != null)
        {
            while (bossInstance != null)
            {
                yield return null;
            }
        } 
    }
}

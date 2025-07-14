using UnityEngine;

[CreateAssetMenu(fileName = "SpawnEvent", menuName = "Scriptable Objects/SpawnEvent", order = 2)]
public class SpawnEvent : ScriptableObject
{
    public enum EventType { Noramal, MidBoss, FinalBoss}
    [Header("イベントの種類")]
    public EventType eventType = EventType.Noramal;

    [Header("出現させる敵の種類")]
    public GameObject enemyPrefab;

    [Header("出現させる数")]
    public int enemyCount = 5;

    [Header("出現させる場所")]
    public Vector3 spawnPosition = new Vector3(0, 5.5f, 0);

    [Header("次の敵が出現するまでの間隔(秒)")]
    public float spawnInterval = 0.5f;

    [Header("このイベントが終了してから、次のイベントが始まるまでの待機時間(秒)")]
    public float delayAfterEvent = 2f;
}

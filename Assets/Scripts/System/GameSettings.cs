using UnityEngine;

// この行により、アセットメニューからこのオブジェクトを作成できるようになります
[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    [Header("Player Movement Boundaries")]
    public float playerMinX = -4.45f;
    public float playerMaxX = 4.45f;
    public float playerMinY = -5f;
    public float playerMaxY = 5f;

    // 将来、他の共通設定もここに追加できます
    // public float defaultEnemySpeed = 2f;
}

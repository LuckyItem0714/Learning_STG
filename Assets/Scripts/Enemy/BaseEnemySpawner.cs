using System.Collections;
using UnityEngine;

public class BaseEnemySpawner : MonoBehaviour
{
    // public GameObject enemyPrefab;
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 2f;
    public float spawnAreaWidth = 8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
    }
}

using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // GameObject _poolManager;
    [SerializeField]
    PoolManager poolManager;

    Transform[] spawns = new Transform[9];

    void Awake()
    {
        // poolManager = _poolManager.GetComponent<PoolManager>();
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i=1; i < children.Length; i++) {
            spawns[i-1] = children[i];
        }
    }

    public void StartSpawn(int stage)
    {
        int enemyCount = (stage / 2) + 3;

        for (int i=0; i < enemyCount; i++) {
            // 적 랜덤으로 가져오기
            int ranIndex = Enum.GetNames(typeof(PoolManager.Enemys)).Length;
            int ranEnemy = UnityEngine.Random.Range(0, ranIndex);
            GameObject enemy = poolManager.GetEnemy(ranEnemy);
            // GameObject enemy = poolManager.GetEnemy(PoolManager.Enemys.Mayo);

            // 랜덤 스포너 + 약간의 랜덤성 위치
            ranIndex = UnityEngine.Random.Range(0, spawns.Length);
            float x = UnityEngine.Random.Range(-1.0f, 1.0f);
            float y = UnityEngine.Random.Range(-1.0f, 1.0f);
            Vector3 ranpos = new Vector3(x, y, 0);
            enemy.transform.position = spawns[ranIndex].position + ranpos;

            // 적 카운트
            GameManager.instance.EnemyCountIncrease();
        }
    }
}

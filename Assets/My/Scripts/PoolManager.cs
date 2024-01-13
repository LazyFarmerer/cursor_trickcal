using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public enum Enemys {
        Amelia,
        Erpin,
        Kidion,
        Juvie,
        Mayo,
        Tig,
    }
    public enum Bullets {
        ErpinBullet,
        JuvieBullet,
        MayoBullet,
    }

    // 보관할 자식 오브젝트
    GameObject enemysObject;
    GameObject bulletsObject;

    // .. 프리팹들을 보관할 변수
    [Header("적")]
    public GameObject[] EnemysPrefabs;
    [Header("투사체")]
    public GameObject[] BulletsPrefabs;

    // .. 풀 담당을 하는 리스트들
    List<GameObject>[] EnemysPools;
    List<GameObject>[] BulletsPools;

    void Awake()
    {
        enemysObject = transform.GetChild(0).gameObject;
        bulletsObject = transform.GetChild(1).gameObject;

        EnemysPools = new List<GameObject>[EnemysPrefabs.Length];
        BulletsPools = new List<GameObject>[BulletsPrefabs.Length];

        for(int index = 0; index < EnemysPools.Length; index++)
        {
            EnemysPools[index] = new List<GameObject>();
        }
        for(int index = 0; index < BulletsPools.Length; index++)
        {
            BulletsPools[index] = new List<GameObject>();
        }
    }

    public GameObject GetEnemy(Enemys enemy) => GetEnemy((int)enemy);
    public GameObject GetEnemy(int index)
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고 (비활성화 된) 있는 게임오브젝트 접근
        foreach(GameObject item in EnemysPools[index])
        {
            if (!item.activeSelf)
            {
                // ... 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ... 못 찾았으면?
        if (!select)
        {
            // ... 새롭게 생성하고 select 변수에 할당  // Poolmanager에 넣겠다.
            select = Instantiate(EnemysPrefabs[index], enemysObject.transform);
            EnemysPools[index].Add(select);
        }

        return select;
    }

    public GameObject GetBullet(Bullets bullet, Vector3 position) => GetBullet((int)bullet, position);
    public GameObject GetBullet(int index, Vector3 position)
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고 (비활성화 된) 있는 게임오브젝트 접근
        foreach(GameObject item in BulletsPools[index])
        {
            if (!item.activeSelf)
            {
                // ... 발견하면 select 변수에 할당
                select = item;
                select.transform.position = position;
                select.SetActive(true);
                break;
            }
        }

        // ... 못 찾았으면?
        if (!select)
        {
            // ... 새롭게 생성하고 select 변수에 할당  // Poolmanager에 넣겠다.
            select = Instantiate(BulletsPrefabs[index], position, Quaternion.identity, bulletsObject.transform);
            BulletsPools[index].Add(select);
        }

        return select;
    }
}

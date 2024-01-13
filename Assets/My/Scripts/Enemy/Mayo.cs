using System.Collections;
using UnityEngine;

public class Mayo : Enemy
{
    SpriteRenderer spriteRenderer;
    GameObject target;
    int BulletsCount;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameManager.instance.player;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // 스킬 시전중일 때 죽을 경우 대비해서 초기화
        int stage = GameManager.instance.stage;
        BulletsCount = Mathf.Min(10, (stage / 2) + 3);
        Init();
    }

    void Init()
    {
        spriteRenderer.color = data.colorDeactive;
    }

    protected override void Skill()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        // 공격 전 저격포시
        // 빨개짐
        spriteRenderer.color = data.colorActive;
        yield return new WaitForSeconds(1.5f);

        // 공격 시작
        StartCoroutine(Fire());

        yield return new WaitForSeconds(1.0f);
        Init();
        base.Skill();
    }

    IEnumerator Fire()
    {
        for (int i=0; i < BulletsCount; i++) {
            Vector3 vec = transform.position + (target.transform.position - transform.position).normalized * 1;
            GameObject bullet = GameManager.instance.poolManager.GetBullet(PoolManager.Bullets.MayoBullet, vec);


            yield return new WaitForSeconds(0.2f);
        }
    }
}

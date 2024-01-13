using System.Collections;
using UnityEngine;

public class Erpin : Enemy
{
    SpriteRenderer spriteRenderer;
    int BulletsCount;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        // 공격 전
        // 빨개짐
        spriteRenderer.color = data.colorActive;
        yield return new WaitForSeconds(1.5f);

        // 공격 활성화
        // 여기서 투사체 발사시킴
        StartCoroutine(Fire());
        Init();

        base.Skill();
    }

    IEnumerator Fire()
    {
        for (int i=0; i < BulletsCount; i++) {
            Vector3 vec = transform.position + Vector3.up;
            GameObject bullet = GameManager.instance.poolManager.GetBullet(PoolManager.Bullets.ErpinBullet, vec);

            yield return new WaitForSeconds(0.2f);
        }
    }
}

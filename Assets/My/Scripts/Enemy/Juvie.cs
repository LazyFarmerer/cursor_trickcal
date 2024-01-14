using System.Collections;
using UnityEngine;

public class Juvie : Enemy
{
    SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

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

        // 공격 시작
        Vector3 target = GameManager.instance.player.transform.position;
        Vector3 vec = transform.position + (target - transform.position).normalized * 1;
        GameObject bullet = GameManager.instance.poolManager.GetBullet(PoolManager.Bullets.JuvieBullet, vec);
        SkillAudio();
        // 위치 지정
        Init();
        base.Skill();
    }
}

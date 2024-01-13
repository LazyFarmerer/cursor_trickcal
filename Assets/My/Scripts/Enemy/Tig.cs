using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Tig : Enemy
{
    GameObject skillObject;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        skillObject = transform.Find("Skill").gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // 스킬 시전중일 때 죽을 경우 대비해서 초기화
        Init();
    }

    void Init()
    {
        spriteRenderer.color = data.colorDeactive;
        skillObject.SetActive(false);
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

        // 공격 시작 DoTween 이용 한바퀴 회전
        skillObject.SetActive(true);
        skillObject.transform.DORotate(new Vector3(0f, 0f, 360f), 1.0f, RotateMode.FastBeyond360)
            // .SetAutoKill(false)
            .SetEase(Ease.Linear)
            .OnComplete(
                () => skillObject.SetActive(false)
            );

        yield return new WaitForSeconds(1.0f);
        Init();
        base.Skill();
    }
}

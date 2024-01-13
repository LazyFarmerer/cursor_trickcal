using System.Collections;
using UnityEngine;

public class Amelia : Enemy
{
    GameObject skillObject;
    Collider2D skillCollider;
    SpriteRenderer skillBox;

    protected override void Awake()
    {
        base.Awake();
        skillObject = transform.Find("Skill").gameObject;
        skillCollider = skillObject.GetComponent<Collider2D>();
        skillBox = skillObject.GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // 스킬 시전중일 때 죽을 경우 대비해서 초기화
        Init();
    }

    void Init()
    {
        skillObject.SetActive(false);
        skillCollider.enabled = false;
        skillBox.color = data.colorDeactive;
    }

    protected override void Skill()
    {
        StartCoroutine(Attack());
    }

    void LookAtMouseCursor()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector3 direction = target - transform.position;
        // 마우스를 바라보도록 방향 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        skillObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 위치 지정
        skillObject.transform.position = transform.position + (target - transform.position).normalized * 6;
    }

    IEnumerator Attack()
    {
        LookAtMouseCursor();
        skillObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        // 공격 활성화
        skillCollider.enabled = true;
        skillBox.color = data.colorActive;
        SkillAudio();
        yield return new WaitForSeconds(1.0f);

        // 공격 비활성화
        Init();
        StopSkillAudio();

        base.Skill();
    }
}

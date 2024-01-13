using System.Collections;
using UnityEngine;

public class Kidion : Enemy
{
    GameObject snipeObject;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        snipeObject = transform.Find("snipe").gameObject;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Init();
        TagLayoutChange(false);
    }

    void Init()
    {
        snipeObject.transform.position = transform.position;
        spriteRenderer.color = data.colorDeactive;
    }

    void TagLayoutChange(bool isAttack)
    {
        if (isAttack) {
            gameObject.layer = LayerMask.NameToLayer("EnemySkill");
            gameObject.tag = "EnemySkill";
            return;
        }

        gameObject.layer = LayerMask.NameToLayer("Enemy");
        gameObject.tag = "Enemy";
    }

    protected override void Skill()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        // 공격 전 저격포시
        // 빨개짐
        snipeObject.transform.position = GameManager.instance.player.transform.position;
        spriteRenderer.color = data.colorActive;
        yield return new WaitForSeconds(1.5f);

        // 공격 시작
        TagLayoutChange(true);
        Vector2 vec = snipeObject.transform.position - transform.position;
        rigid.AddForce(vec.normalized * 10, ForceMode2D.Impulse);
        snipeObject.transform.position = transform.position;

        yield return new WaitForSeconds(1.0f);
        Init();
        TagLayoutChange(false);
        base.Skill();
    }
}

using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State { idle, attack }
    State state;
    [SerializeField] protected DamageNumbers damageNumber;
    [SerializeField] protected EnemyDatas data;

    [SerializeField] protected float hp;

    protected virtual void OnEnable()
    {
        // 기본 설정
        hp = data.Hp(GameManager.instance.stage);
        int startTime = GameManager.instance.enemyCount % 5;
        float ranWaitTime = Random.Range(0.0f, 0.5f);
        StartCoroutine(ChangeState(State.idle, startTime + ranWaitTime));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator ChangeState(State nextState, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        state = nextState;

        switch (state)
        {
            case State.idle:
                waitTime = data.WaitTimeAttack();
                StartCoroutine(ChangeState(State.attack, waitTime));
                break;
            case State.attack:
                Skill();
                break;
        }
    }

    public void Hit(float damage, bool isCritical)
    {
        damageNumber.Print(transform.position, damage, isCritical);
        hp -= damage;

        if (hp < 0) {
            Dead();
        }
    }

    protected virtual void Skill()
    {
        // 스킬 쓰고 난 직후
        // 자식 Skill 스킬 사용 후 마지막에 base.Skill(); 호출
        StartCoroutine(ChangeState(State.idle, 0f));
    }

    void Dead()
    {
        GameManager.instance.EnemyCountDecrease();
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}

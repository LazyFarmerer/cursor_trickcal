using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State { idle, attack }
    State state;
    [SerializeField] protected DamageNumbers damageNumber;
    [SerializeField] protected EnemyDatas data;

    AudioSource audioSource;
    AudioSource voiceSource;
    [SerializeField] protected float hp;

    protected virtual void Awake()
    {
        voiceSource = transform.Find("Voice Source").GetComponent<AudioSource>();;
        voiceSource.volume = GameManager.instance.data.sfxVolume.Get() / 100;
        voiceSource.clip = data.voiceAuio;

        audioSource = transform.Find("Audio Source").GetComponent<AudioSource>();;
        audioSource.volume = GameManager.instance.data.sfxVolume.Get() / 100;
    }

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
                VoiceAudio();
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

    protected void VoiceAudio()
    {
        voiceSource.Play();
    }

    protected void SkillAudio()
    {
        audioSource.Stop();
        audioSource.clip = data.skillAuio;
        audioSource.Play();
    }
    protected void StopSkillAudio()
    {
        audioSource.Stop();
    }

    void Dead()
    {
        GameManager.instance.EnemyCountDecrease();
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}

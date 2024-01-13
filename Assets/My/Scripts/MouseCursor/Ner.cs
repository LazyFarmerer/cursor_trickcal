using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ner : MouseCursor
{
    bool skillDamageUp;
    [SerializeField] ParticleSystem skillVisualizer;
    WaitForSeconds skillDuration = new WaitForSeconds(5.0f);

    protected override void OnEnable()
    {
        base.OnEnable();

        skillDamageUp = false;
    }

    protected override void Skill()
    {
        base.Skill();

        StartCoroutine(SkillCoroutine());
    }

    IEnumerator SkillCoroutine()
    {
        skillDamageUp = true;
        skillVisualizer.Play();
        yield return skillDuration;
        skillDamageUp = false;
        skillVisualizer.Stop();
    }

    /// <summary>
    /// 스킬 사용시 데미지 1.5배 증가
    /// </summary>
    /// <returns></returns>
    protected override float Damage()
    {
        if (skillDamageUp) {
            return base.Damage() * 1.5f;
        }
        return base.Damage();
    }

    /// <summary>
    /// 스킬 사용시 데미지 1.5배 증가
    /// </summary>
    /// <returns></returns>
    protected override float CriticalDamage()
    {
        if (skillDamageUp) {
            return base.CriticalDamage() * 1.5f;
        }
        return base.CriticalDamage();
    }
}

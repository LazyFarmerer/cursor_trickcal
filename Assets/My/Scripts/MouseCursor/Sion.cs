using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sion : MouseCursor
{
    [SerializeField] ParticleSystem skillEnergyParticle;
    WaitForSeconds skillParticleDuration = new WaitForSeconds(2.0f);
    WaitForSeconds skillDuration = new WaitForSeconds(3.0f);

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Skill()
    {
        base.Skill();

        StartCoroutine(SkillCoroutine());
    }

    IEnumerator SkillCoroutine()
    {
        skillEnergyParticle.Play();
        yield return skillParticleDuration;
        skillEnergyParticle.Stop();
        yield return skillDuration;
        // 여기서 스킬 발사하면 될거 같음
    }

    protected override float Damage()
    {
        return base.Damage();
    }

    protected override float CriticalDamage()
    {
        return base.CriticalDamage();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Sion : MouseCursor
{
    [SerializeField] ParticleSystem skillEnergyParticle;
    WaitForSeconds skillParticleDuration = new WaitForSeconds(0.5f);
    WaitForSeconds skillDuration = new WaitForSeconds(0.5f);

    [Header("스킬용")]
    [SerializeField] GameObject sniperPrefab;
    GameObject sniper;
    SionSniper sionSniper;


    protected override void Awake()
    {
        base.Awake();
        // 시작하면 만들고 비활성화
        sniper = Instantiate(sniperPrefab, GameObject.Find("Pool Manager").transform.GetChild(1));
        sniper.SetActive(false);
        sionSniper = sniper.GetComponent<SionSniper>();
        sionSniper.sion = this;
    }

    /// <summary>
    /// 스킬 다시 사용
    /// </summary>
    public void ReloadSkill() => StartCoroutine(SkillCoroutine());
    protected override void Skill()
    {
        base.Skill();
        // 스킬 시전시 리로드 2번 주어짐
        sionSniper.reload = 2;

        StartCoroutine(SkillCoroutine());
    }

    IEnumerator SkillCoroutine()
    {
        skillEnergyParticle.Play();
        yield return skillParticleDuration;
        skillEnergyParticle.Stop();
        yield return skillDuration;
        // 여기서 스킬 발사하면 될거 같음
        // 활성화만 하고 나머진 알아서 추적 할거임
        float skillDamage = CriticalDamage() * 3;
        sniper.transform.position = transform.position;
        sionSniper.damage = skillDamage;
        sniper.SetActive(true);
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

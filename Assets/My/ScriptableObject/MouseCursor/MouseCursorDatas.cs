using UnityEngine;

[CreateAssetMenu(fileName = "Cursor Data", menuName = "Scriptable Object/Cursor Data", order = int.MaxValue)]
public class MouseCursorDatas : ScriptableObject
{
    [Header("이름")]
    public string Name;
    [Header("이미지")]
    public Sprite sprite;
    public Sprite skill;
    [Header("정보")]
    public int maxHp;
    [SerializeField] int hp;
    [Header("데미지")]
    [SerializeField] float baseDamage;
    public float levelAdjustedDamage;
    [Header("크리티컬")]
    [SerializeField] float baseCritical;
    public float levelAdjustedCritical;
    [Header("크리티컬 데미지")]
    public float baseCriticalDamage;
    public float levelAdjustedCriticalDamage;
    [Header("스킬 쿨타임")]
    public float skillCoolTime;

    [Header("캐릭터 설명")]
    [TextArea]
    public string description;
    [Header("캐릭터 스킬 설명")]
    public string skillDescription;

    public int Hp() {
        int _h = GameManager.instance.data.hpLevel.Get();
        return Mathf.Min(hp + _h, maxHp);
    }

    public float Damage(int damageLevel) {
        int _d = GameManager.instance.data.damageLevel.Get();
        return baseDamage + ((damageLevel + (_d*0.3f)) * levelAdjustedDamage);
    }
    public float CriticalChance(int criticalChanceLevel) {
        float _cc = GameManager.instance.data.criticalChanceLevel.Get();
        return baseCritical + ((criticalChanceLevel + (_cc*0.3f)) * levelAdjustedCritical);
    }
    public float CriticalDamage(int damageLevel, int criticalDamageLevel) {
        return Damage(damageLevel) * CriticalDamagePercent(criticalDamageLevel);
    }

    /// <summary>
    /// 크리티컬 데미지 비율
    /// </summary>
    /// <returns></returns>
    public float CriticalDamagePercent(int criticalDamageLevel)
    {
        float _cd = GameManager.instance.data.criticalDamageLevel.Get();
        float cd = (criticalDamageLevel + (_cd*0.3f)) * levelAdjustedCriticalDamage;
        return (1 + (baseCriticalDamage + cd));
    }
}

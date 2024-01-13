using UnityEngine;
using TMPro;

public class NextStageWaitWindow : MonoBehaviour
{
    MouseCursorDatas mouseCursorData;

    TextMeshProUGUI hp;
    TextMeshProUGUI maxHp;
    TextMeshProUGUI damage;
    TextMeshProUGUI criticalChance;
    TextMeshProUGUI criticalDamage;

    void Awake()
    {
        Transform statsUPObject = transform.GetChild(0);

        Transform hpObject = statsUPObject.GetChild(0);
        hp = hpObject.GetChild(1).GetComponent<TextMeshProUGUI>();

        Transform maxHpObject = statsUPObject.GetChild(1);
        maxHp = maxHpObject.GetChild(1).GetComponent<TextMeshProUGUI>();

        Transform damageObject = statsUPObject.GetChild(2);
        damage = damageObject.GetChild(1).GetComponent<TextMeshProUGUI>();

        Transform criticalChanceObject = statsUPObject.GetChild(3);
        criticalChance = criticalChanceObject.GetChild(1).GetComponent<TextMeshProUGUI>();

        Transform criticalDamageObject = statsUPObject.GetChild(4);
        criticalDamage = criticalDamageObject.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if (mouseCursorData == null) {
            // int index = GameManager.instance.playerIndex;
            // mouseCursorData = GameManager.instance.mouseCursorDatas[index];
            mouseCursorData = GameManager.instance.player.transform.GetChild(0).GetComponent<MouseCursor>().mouseCursorData;
        }

        int damageLevel = GameManager.instance.damageLevel;
        int criticalChanceLevel = GameManager.instance.criticalChanceLevel;
        int criticalDamageLevel = GameManager.instance.criticalDamageLevel;

        damage.text = string.Format(
            "공격력 {0} 증가\n(총 {1})",
            mouseCursorData.levelAdjustedDamage,
            mouseCursorData.Damage(damageLevel + 1)
        );
        criticalChance.text = string.Format(
            "치명타확률 {0}% 증가\n(총 {1}%)",
            mouseCursorData.levelAdjustedCritical * 100,
            mouseCursorData.CriticalChance(criticalChanceLevel + 1)
        );
        criticalDamage.text = string.Format(
            "치명타 공격력 {0}% 증가\n(총 {1}%)",
            mouseCursorData.levelAdjustedCriticalDamage * 100,
            mouseCursorData.CriticalDamagePercent(criticalDamageLevel) * 100
        );
    }
}

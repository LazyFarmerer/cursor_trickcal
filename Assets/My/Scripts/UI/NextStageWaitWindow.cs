using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NextStageWaitWindow : MonoBehaviour
{
    MouseCursorDatas mouseCursorData;

    TextMeshProUGUI hp;
    TextMeshProUGUI maxHp;
    TextMeshProUGUI damage;
    TextMeshProUGUI criticalChance;
    TextMeshProUGUI criticalDamage;
    Button[] NextStageButtons = new Button[6];

    void Awake()
    {
        NextStageButtons[0] = transform.GetChild(1).GetComponent<Button>();

        Transform statsUPObject = transform.GetChild(0);

        Transform hpObject = statsUPObject.GetChild(0);
        hp = hpObject.GetChild(1).GetComponent<TextMeshProUGUI>();
        NextStageButtons[1] = hpObject.GetComponent<Button>();

        Transform maxHpObject = statsUPObject.GetChild(1);
        maxHp = maxHpObject.GetChild(1).GetComponent<TextMeshProUGUI>();
        NextStageButtons[2] = hpObject.GetComponent<Button>();

        Transform damageObject = statsUPObject.GetChild(2);
        damage = damageObject.GetChild(1).GetComponent<TextMeshProUGUI>();
        NextStageButtons[3] = hpObject.GetComponent<Button>();

        Transform criticalChanceObject = statsUPObject.GetChild(3);
        criticalChance = criticalChanceObject.GetChild(1).GetComponent<TextMeshProUGUI>();
        NextStageButtons[4] = hpObject.GetComponent<Button>();

        Transform criticalDamageObject = statsUPObject.GetChild(4);
        criticalDamage = criticalDamageObject.GetChild(1).GetComponent<TextMeshProUGUI>();
        NextStageButtons[5] = hpObject.GetComponent<Button>();

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

    /// <summary>
    /// 업그레이드창에서 버튼들을 활성화 여부(중복클릭 방지)
    /// </summary>
    /// <param name="active"></param>
    public void ButtonsSetActive(bool active) {
        for (int i=0; i < NextStageButtons.Length; i++) {
            NextStageButtons[i].enabled = active;
        }
    }
}

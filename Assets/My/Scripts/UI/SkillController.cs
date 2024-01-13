using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillController : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] MouseCursor player;
    [Header("쿨타임")]
    [SerializeField] Image skillImage;
    [SerializeField] Image coolTimeImage;
    [SerializeField] TextMeshProUGUI coolTimeText;

    void Start()
    {
        int index = GameManager.instance.playerIndex;
        player = GameManager.instance.player.transform.GetChild(0).GetComponent<MouseCursor>();

        MouseCursorDatas mouseCursorData = player.mouseCursorData;

        skillImage.sprite = mouseCursorData.skill;
    }

    void LateUpdate()
    {
        float coolTime = player.skillCoolTime / player.maxSkillCoolTime;

        coolTimeImage.fillAmount = coolTime;
        if (player.skillCoolTime == 0) {
            coolTimeText.text = "";
            return;
        }
        coolTimeText.text = player.skillCoolTime.ToString("0.0");
    }
}

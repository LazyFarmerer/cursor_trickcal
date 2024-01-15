using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeButtons : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinCount;

    string[] description = new string[4] {
        "최대체력 증가\n(최대 10개)\n레벨: {0}",
        "공격력 증가\n레벨: {0}",
        "치명타 확률 증가\n레벨: {0}",
        "치명타 공격력 증가\n레벨: {0}"
    };
    [SerializeField] GameObject[] buttons = new GameObject[4];
    TextMeshProUGUI[] descriptionText = new TextMeshProUGUI[4];
    TextMeshProUGUI[] coinPrice = new TextMeshProUGUI[4];

    int coin;
    int hpLevel;
    int damageLevel;
    int criticalChanceLevel;
    int criticalDamageLevel;

    [SerializeField] ShowCharacterChoice showCharacterChoice;

    public void Start()
    {
        coin = GameManager.instance.data.coin.Get();
        hpLevel = GameManager.instance.data.hpLevel.Get();
        damageLevel = GameManager.instance.data.damageLevel.Get();
        criticalChanceLevel = GameManager.instance.data.criticalChanceLevel.Get();
        criticalDamageLevel = GameManager.instance.data.criticalDamageLevel.Get();

        for (int i=0; i < buttons.Length; i++) {
            descriptionText[i] = buttons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            coinPrice[i] = buttons[i].transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();

        }

        descriptionText[0].text = string.Format(description[0], hpLevel);
        coinPrice[0].text = Price(hpLevel * 10).ToString("n0");
        descriptionText[1].text = string.Format(description[1], damageLevel);
        coinPrice[1].text = Price(damageLevel).ToString("n0");
        descriptionText[2].text = string.Format(description[2], criticalChanceLevel);
        coinPrice[2].text = Price(criticalChanceLevel).ToString("n0");
        descriptionText[3].text = string.Format(description[3], criticalDamageLevel);
        coinPrice[3].text = Price(criticalDamageLevel).ToString("n0");
    }

    int Price(int level)
    {
        float result = 100 * Mathf.Pow(1.15f, level);
        return (int)result;
    }

    public void Buy(int index) {
        int price;

        switch (index)
        {
            case 0:
                price = Price(hpLevel * 10);
                if (coin - price < 0) {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonClickFail);
                    return;
                }
                coin -= price;
                hpLevel++;

                GameManager.instance.data.hpLevel.Save(hpLevel);
                descriptionText[index].text = string.Format(description[index], hpLevel);
                coinPrice[index].text = Price(hpLevel * 10).ToString("n0");
                break;
            case 1:
                price = Price(damageLevel);
                if (coin - price < 0) {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonClickFail);
                    return;
                }
                coin -= price;
                damageLevel++;

                GameManager.instance.data.damageLevel.Save(damageLevel);
                descriptionText[index].text = string.Format(description[index], damageLevel);
                coinPrice[index].text = Price(damageLevel).ToString("n0");
                break;
            case 2:
                price = Price(criticalChanceLevel);
                if (coin - price < 0) {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonClickFail);
                    return;
                }
                coin -= price;
                criticalChanceLevel++;

                GameManager.instance.data.criticalChanceLevel.Save(criticalChanceLevel);
                descriptionText[index].text = string.Format(description[index], criticalChanceLevel);
                coinPrice[index].text = Price(criticalChanceLevel).ToString("n0");
                break;
            case 3:
                price = Price(criticalDamageLevel);
                if (coin - price < 0) {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonClickFail);
                    return;
                }
                coin -= price;
                criticalDamageLevel++;

                GameManager.instance.data.criticalDamageLevel.Save(criticalDamageLevel);
                descriptionText[index].text = string.Format(description[index], criticalDamageLevel);
                coinPrice[index].text = Price(criticalDamageLevel).ToString("n0");
                break;
        }

        coinCount.text = coin.ToString("n0");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.buy);
        GameManager.instance.data.coin.Save(coin);

        // 마지막으로 캐릭 선택 창 새로고침
        showCharacterChoice.Init();
    }
}

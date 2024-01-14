using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowCharacterChoice : MonoBehaviour
{
    [SerializeField]
    GameObject characterWindow;
    [SerializeField]
    GameObject clickBlockPannel;

    void Start()
    {
        MouseCursorDatas[] mouseCursorDatas = GameManager.instance.mouseCursorDatas;

        for (int index=0; index< mouseCursorDatas.Length; index++) {
            int i = index;
            Transform character = Instantiate(characterWindow, transform).transform;
            character.GetComponent<Button>().onClick.AddListener(() =>
                AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonClick)
            );
            character.GetComponent<Button>().onClick.AddListener(() =>
                GameManager.instance.StartGame(i)
            );
            character.GetComponent<Button>().onClick.AddListener(() =>
                GameManager.instance.uIEvent.CharacterChoice2InGame()
            );
            character.GetComponent<Button>().onClick.AddListener(() =>
                clickBlockPannel.SetActive(true)
            );
            character.GetChild(0).GetComponent<Image>().sprite = mouseCursorDatas[index].sprite;
            character.GetChild(1).GetComponent<TextMeshProUGUI>().text = mouseCursorDatas[index].Name;

            // Description 부분
            Transform descriptionObject = character.GetChild(2);
            // Name UI
            descriptionObject.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = mouseCursorDatas[index].Name;
            // Character Image UI
            descriptionObject.GetChild(1).GetChild(1).GetComponent<Image>().sprite = mouseCursorDatas[index].sprite;
        }

        Init();
    }

    public void Init()
    {
        MouseCursorDatas[] mouseCursorDatas = GameManager.instance.mouseCursorDatas;

        for (int index=0; index < transform.childCount; index++) {
            // Description 부분
            Transform descriptionObject = transform.GetChild(index).GetChild(2);

            // Description Text
            TextMeshProUGUI descriptionTextObject = descriptionObject.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            string description = string.Format(
                mouseCursorDatas[index].description,
                mouseCursorDatas[index].Hp(),
                mouseCursorDatas[index].maxHp,
                mouseCursorDatas[index].Damage(0),
                mouseCursorDatas[index].levelAdjustedDamage,
                mouseCursorDatas[index].CriticalChance(0) * 100,
                mouseCursorDatas[index].levelAdjustedCritical * 100,
                mouseCursorDatas[index].CriticalDamagePercent(0) * 100,
                mouseCursorDatas[index].levelAdjustedCriticalDamage * 100,
                mouseCursorDatas[index].skillDescription
            );
            descriptionTextObject.text = description;
        }
    }
}

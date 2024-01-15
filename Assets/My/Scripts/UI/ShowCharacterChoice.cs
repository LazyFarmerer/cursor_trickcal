using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowCharacterChoice : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinCount;
    [SerializeField]
    GameObject characterWindow;
    [SerializeField]
    GameObject clickBlockPannel;

    // 시온 비공개를 위한 변수
    bool[] hiddenCharacter = new bool[] {true, true, false};

    void Start()
    {
        MouseCursorDatas[] mouseCursorDatas = GameManager.instance.mouseCursorDatas;
        hiddenCharacter[2] = GameManager.instance.data.sion.Get()==1;

        for (int index=0; index< mouseCursorDatas.Length; index++) {
            MouseCursorDatas cursorData = mouseCursorDatas[index];
            if (hiddenCharacter[index]) {
                CreateWindow(index, cursorData);
                continue;
            }
            CreateHiddenWindow(index, cursorData);
        }

        Init();
    }

    public void Init()
    {
        MouseCursorDatas[] mouseCursorDatas = GameManager.instance.mouseCursorDatas;

        for (int index=0; index < transform.childCount; index++) {
            if (!hiddenCharacter[index])
                continue;
            CharacterStatus(index, mouseCursorDatas[index]);
        }
    }

    /// <summary>
    /// 공개되어 있는 캐릭터 보여줌
    /// </summary>
    /// <param name="index"></param>
    /// <param name="mouseCursorData"></param>
    /// <returns></returns>
    Transform CreateWindow(int index, MouseCursorDatas mouseCursorData)
    {
        Transform character = Instantiate(characterWindow, transform).transform;
        Button characterButton = character.GetComponent<Button>();
        characterButton.onClick.AddListener(() =>
            AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonClick)
        );
        characterButton.onClick.AddListener(() =>
            GameManager.instance.StartGame(index)
        );
        characterButton.onClick.AddListener(() =>
            GameManager.instance.uIEvent.CharacterChoice2InGame()
        );
        characterButton.onClick.AddListener(() =>
            clickBlockPannel.SetActive(true)
        );
        character.GetChild(0).GetComponent<Image>().sprite = mouseCursorData.sprite;
        character.GetChild(1).GetComponent<TextMeshProUGUI>().text = mouseCursorData.Name;

        // Description 부분
        Transform descriptionObject = character.GetChild(2);
        // Name UI
        descriptionObject.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = mouseCursorData.Name;
        // Character Image UI
        descriptionObject.GetChild(1).GetChild(1).GetComponent<Image>().sprite = mouseCursorData.sprite;
        return character;
    }

    /// <summary>
    /// 공개되어있지않은 캐릭터 보여줌
    /// </summary>
    /// <param name="index"></param>
    /// <param name="mouseCursorData"></param>
    /// <returns></returns>
    Transform CreateHiddenWindow(int index, MouseCursorDatas mouseCursorData)
    {
        Transform character = Instantiate(characterWindow, transform).transform;
        Button characterButton = character.GetComponent<Button>();
        characterButton.onClick.AddListener(() => {
            int coin = GameManager.instance.data.coin.Get();
            coin -= 500;
            if (coin < 0) {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonClickFail);
                return;
            }
            // 구매 완료
            GameManager.instance.data.coin.Save(coin);
            coinCount.text = coin.ToString("n0");
            // @@@ 숨겨진 캐릭이 하나뿐이라 이렇게 하는데 여러명일 땐 배열 이용하기 @@@
            GameManager.instance.data.sion.Save(1);
            Destroy(character.gameObject);
            CreateWindow(index, mouseCursorData).SetSiblingIndex(index);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.buy);
        });
        Image spriteImage = character.GetChild(0).GetComponent<Image>();
        spriteImage.sprite = mouseCursorData.sprite;
        spriteImage.color = new Color(0, 0, 0, 1);
        character.GetChild(1).GetComponent<TextMeshProUGUI>().text = "??";

        // Description 부분
        Transform descriptionObject = character.GetChild(2);
        // Name UI
        descriptionObject.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "??";
        // Character Image UI
        spriteImage = descriptionObject.GetChild(1).GetChild(1).GetComponent<Image>();
        spriteImage.sprite = mouseCursorData.sprite;
        spriteImage.color = new Color(0, 0, 0, 1);

        // Description Text
        TextMeshProUGUI descriptionTextObject = descriptionObject.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        descriptionTextObject.text = "<size=+10>500 코인 필요</size>";

        return character;
    }

    /// <summary>
    /// Init 함수에서 캐릭터 글 새로고침 용
    /// </summary>
    /// <param name="index"></param>
    /// <param name="mouseCursorData"></param>
    void CharacterStatus(int index, MouseCursorDatas mouseCursorData)
    {
        // Description 부분
        Transform descriptionObject = transform.GetChild(index).GetChild(2);

        // Description Text
        TextMeshProUGUI descriptionTextObject = descriptionObject.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        string description = string.Format(
            mouseCursorData.description,
            mouseCursorData.Hp(),
            mouseCursorData.maxHp,
            mouseCursorData.Damage(0),
            mouseCursorData.levelAdjustedDamage,
            mouseCursorData.CriticalChance(0) * 100,
            mouseCursorData.levelAdjustedCritical * 100,
            mouseCursorData.CriticalDamagePercent(0) * 100,
            mouseCursorData.levelAdjustedCriticalDamage * 100,
            mouseCursorData.skillDescription
        );
        descriptionTextObject.text = description;
    }
}

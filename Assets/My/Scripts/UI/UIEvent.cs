using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIEvent : MonoBehaviour
{
    [Header("UI 에니메이션을 위한 UI 오브젝트")]
    [Header("In Game UI 오브젝트")]
    [SerializeField]
    GameObject _inGameUi;
    [Header("Title UI 오브젝트")]
    [SerializeField]
    GameObject _titleUi;
    [SerializeField]
    GameObject _gameTitle;
    RectTransform gameTitle;
    TextMeshProUGUI gameTitleText;
    [SerializeField]
    GameObject _menu;
    RectTransform menu;
    [Header("사도 선택")]
    [SerializeField]
    GameObject _characterChoice;
    RectTransform characterChoice;
    [Header("모카롱 먹기")]
    [SerializeField]
    GameObject _upgrade;
    RectTransform upgrade;
    [SerializeField]
    GameObject _coin;
    RectTransform coin;
    [Header("세팅")]
    [SerializeField]
    GameObject _setting;
    RectTransform setting;

    void Start()
    {
        gameTitle = _gameTitle.GetComponent<RectTransform>();
        gameTitleText = _gameTitle.GetComponent<TextMeshProUGUI>();
        menu = _menu.GetComponent<RectTransform>();
        characterChoice = _characterChoice.GetComponent<RectTransform>();
        coin = _coin.GetComponent<RectTransform>();
        upgrade = _upgrade.GetComponent<RectTransform>();
        setting = _setting.GetComponent<RectTransform>();
    }

    public void Title2CharacterChoice()
    {
        OutGameTitle()
            .OnComplete(() => InGameTitle("사도 선택"));

        OutMenu()
            .OnComplete(() => InCharacterChoice());
    }
    public void CharacterChoice2Title()
    {
        OutGameTitle()
            .OnComplete(() => InGameTitle("커서 트릭컬"));

        OutCharacterChoice()
            .OnComplete(() => InMenu());
    }

    public void CharacterChoice2InGame()
    {
        OutGameTitle();

        OutCharacterChoice()
            .OnComplete(InGame);
    }

    public void Title2Upgrade()
    {
        OutGameTitle()
            .OnComplete(() => InGameTitle("비밀의 베이커리"));

        OutMenu()
            .OnComplete(()=> {
                InUpgrade();
                InCoin();
            });
    }
    public void Upgrade2Title()
    {
        OutGameTitle()
            .OnComplete(() => InGameTitle("커서 트릭컬"));
        
        OutUpgrade()
            .OnPlay(()=> OutCoin())
            .OnComplete(()=> InMenu());
    }

    public void Setting2Title()
    {
        OutGameTitle()
            .OnComplete(() => InGameTitle("커서 트릭컬"));

        OutSetting()
            .OnComplete(() => InMenu());
    }
    public void Title2Setting()
    {
        OutGameTitle()
            .OnComplete(() => InGameTitle("설정"));

        OutMenu()
            .OnComplete(() => InSetting());
    }

    /// <summary>
    /// 사도 선택시 실행
    /// </summary>
    void InGame()
    {
        // _pannel.GetComponent<DOTweenAnimation>().DORestartById("off");
        _titleUi.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(1,1,1,0), 0.5f)
            .OnComplete(()=> {
                _inGameUi.SetActive(true);
                _titleUi.SetActive(false);
            });
    }

    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    InGameTitle(string titleText)
    {
        gameTitleText.text = titleText;
        return gameTitle.DOAnchorPosY(-50, 0.8f)
            .SetEase(Ease.OutBack);;
    }
    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    OutGameTitle()
    {
        return gameTitle.DOAnchorPosY(100, 0.8f)
            .SetEase(Ease.InBack);
    }

    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    InMenu()
    {
        return menu.DOAnchorPosX(20, 0.8f)
            .SetEase(Ease.OutBack);
    }
    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    OutMenu()
    {
        return menu.DOAnchorPosX(-220, 0.8f)
            .SetEase(Ease.InBack);
    }

    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    InCharacterChoice()
    {
        return characterChoice.DOAnchorPosX(0, 1f)
            .SetEase(Ease.OutBack);
    }
    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    OutCharacterChoice()
    {
        return characterChoice.DOAnchorPosX(-1200, 1f)
            .SetEase(Ease.InBack);
    }

    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    InUpgrade()
    {
        return upgrade.DOAnchorPosX(0, 1f)
            .SetEase(Ease.OutBack);
    }
    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    OutUpgrade()
    {
        return upgrade.DOAnchorPosX(-1200, 1f)
            .SetEase(Ease.InBack);
    }

    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    InCoin()
    {
        return coin.DOAnchorPosX(0, 0.8f)
            .SetEase(Ease.OutBack);
    }
    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    OutCoin()
    {
        return coin.DOAnchorPosX(200, 0.8f)
            .SetEase(Ease.InBack);
    }

    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    InSetting()
    {
        return setting.DOAnchorPosX(0, 1f)
            .SetEase(Ease.OutBack);
        
    }
    DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>
    OutSetting()
    {
        return setting.DOAnchorPosX(-1200, 1f)
            .SetEase(Ease.InBack);
    }

    /// <summary>
    /// 게임 스톱 창을 열 경우(창 크기 1)
    /// </summary>
    public void ShowGamePauseWindow()
    {
        GameManager.instance.uIManager._gamePauseWindow.transform.DOScale(1.0f, 0.5f)
            .SetUpdate(true)
            .SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 게임 스톱 창을 닫을 경우(창 크기 0)
    /// </summary>
    public void HiddenGamePauseWindow()
    {
        GameManager.instance.uIManager._gamePauseWindow.transform.DOScale(0.0f, 0.5f)
            .SetEase(Ease.InBack)
            .SetUpdate(true);
    }
}

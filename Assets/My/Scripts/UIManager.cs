using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Title UI 오브젝트")]
    [SerializeField]
    GameObject _coin;
    RectTransform coin;
    TextMeshProUGUI coinCount;
    [Header("In Game UI 오브젝트")]
    [Header("스테이지 표시하는 UI")]
    [SerializeField]
    GameObject _stageCount;
    TextMeshProUGUI stageCount;

    [Header("점수 표시하는 UI")]
    [SerializeField]
    GameObject _scoreCount;
    TextMeshProUGUI scoreCount;

    [Header("적 숫자 표시하는 UI")]
    [SerializeField]
    GameObject _enemyCount;
    TextMeshProUGUI enemyCount;

    [Header("게임시작과 끝을 알려주는 UI")]
    [SerializeField]
    GameObject _stageWaitTimeCount;
    RectTransform stageWaitTimeCount;
    TextMeshProUGUI stageWaitTimeCountText;

    [Header("스테이지 클리어 후 나오는 창 UI")]
    public GameObject _nextStageWaitWindow;
    // public NextStageWaitWindow nextStageWaitWindowButtons;

    [Header("게임 일시정지 시 나오는 창 UI")]
    public GameObject _gamePauseWindow;

    [Header("스테이지 클리어 후 나오는 창 UI")]
    public GameObject _gameOverWindow;

    [Header("체력 갯수 표시 UI")]
    [SerializeField]
    GameObject[] _hpBars;
    Image[] hpBarHearts = new Image[10];
    // [Header("Title UI 부분")]
    // [SerializeField]
    // GameObject _titleUi;

    [Header("기타 변수 부분")]
    Color Active = new Color32(255, 255, 255, 255);
    Color Deactive = new Color32(255, 255, 255, 100);

    void Awake()
    {
        coin = _coin.GetComponent<RectTransform>();
        coinCount = _coin.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        // nextStageWaitWindowButtons = _nextStageWaitWindow.GetComponent<NextStageWaitWindow>();

        stageCount = _stageCount.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        scoreCount = _scoreCount.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        enemyCount = _enemyCount.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        stageWaitTimeCount = _stageWaitTimeCount.GetComponent<RectTransform>();
        stageWaitTimeCountText = _stageWaitTimeCount.GetComponent<TextMeshProUGUI>();

        for (int i=0; i < 10; i++) {
            hpBarHearts[i] = _hpBars[i].GetComponent<Image>();
        }
    }

    public void Start()
    {
        coinCount.text = GameManager.instance.data.coin.Get().ToString("n0");
    }

    /// <summary>
    /// 스테이지 정보를 표시
    /// </summary>
    /// <param name="stage"></param>
    public void StageCount(int stage)
    {
        stageCount.text = stage.ToString();
    }

    /// <summary>
    /// 점수를 표시
    /// </summary>
    /// <param name="score"></param>
    public void ScoreCount(int score)
    {
        scoreCount.text = score.ToString("n0");
    }

    /// <summary>
    /// 적의 남은수를 표시
    /// </summary>
    /// <param name="enemyCount"></param>
    public void EnemyCount(int enemyCount)
    {
        this.enemyCount.text = enemyCount.ToString();
    }

    /// <summary>
    /// 스테이지 시작 전 남은시간 표시
    /// </summary>
    /// <param name="TimeCount"></param>
    public void StageWaitTimeCount(int TimeCount)
    {
        stageWaitTimeCount.anchoredPosition = Vector2.zero;
        stageWaitTimeCount.anchoredPosition = Vector2.right * 35;
        stageWaitTimeCount.DOLocalMoveX(-35, 1.0f).SetEase(Ease.InOutQuint);

        string textView = TimeCount != 0? string.Format("{0}", TimeCount) : "시작!";
        stageWaitTimeCountText.text = textView;
    }
    /// <summary>
    /// 위 함수와 동일하나 스테이지 클리어만 표시해줌
    /// </summary>
    public void StageClear()
    {
        stageWaitTimeCount.anchoredPosition = Vector2.zero;
        stageWaitTimeCountText.text = "스테이지 클리어!";
    }

    /// <summary>
    /// 스테이지 시작 전 남은시간 표시 UI 켜고 끄기
    /// </summary>
    /// <param name="isActive"></param>
    public void StageWaitTimeCountSetActive(bool isActive)
    {
        _stageWaitTimeCount.SetActive(isActive);
    }

    public void HpBar(int currHp, int maxHp)
    {
        for (int i=0; i < 10; i++) {
            _hpBars[i].SetActive(false);
        }
        for (int i=0; i < maxHp; i++) {
            _hpBars[i].SetActive(true);
            hpBarHearts[i].color = Deactive;
        }
        for (int i=0; i < currHp; i++) {
            hpBarHearts[i].color = Active;
        }
    }

    public void GameOver()
    {
        _gameOverWindow.SetActive(true);

        Transform textGrup = _gameOverWindow.transform.GetChild(0).GetChild(2);

        TextMeshProUGUI highScore = textGrup.GetChild(0).GetComponent<TextMeshProUGUI>();
        highScore.text = string.Format("최고 점수: {0:n0}", GameManager.instance.highScore);

        TextMeshProUGUI score = textGrup.GetChild(1).GetComponent<TextMeshProUGUI>();
        score.text = string.Format("점수: {0:n0}", GameManager.instance.score);
    }
}

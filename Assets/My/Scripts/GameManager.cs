using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

// "https://velog.io/@soos00soo/Unity-Assembly-CSharp.csproj-is-in-unsupported-format-for-example-a-traditional-.NET-Framework-project.-It-need-be-converted-to-new-SDK-style-to-work-in-C-Dev-Kit";

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public enum GameState
    {
        WindowOpen, // 게임 내 창 띄어져 있음 (타이틀 등)
        Playing, // 게임중
        Paused // 게임 일시정지 상태
    }

    [Header("게임 조작을 위한 변수들")]
    [HideInInspector] public GameState gameState; // 게임 상태여부
    [HideInInspector] public int stage = 1;
    [HideInInspector] public int enemyCount;

    [HideInInspector] public int damageLevel;
    [HideInInspector] public int criticalChanceLevel;
    [HideInInspector] public int criticalDamageLevel;

    // 점수
    [HideInInspector] public int highScore;
    [HideInInspector] public int score;

    [Header("게임 조작을 위한 스크립트")]
    // 저장 데이터들
    public DataController data = new DataController();
    public PoolManager poolManager;
    [SerializeField]
    EnemySpawner enemySpawner;
    public UIManager uIManager;
    public UIEvent uIEvent;
    [Header("마우스 커서 데이터")]
    // 선택 된 마우스 커서
    // player.transform.GetChild(0);
    public MouseCursorDatas[] mouseCursorDatas;
    public GameObject player;
    public int playerIndex;
    [SerializeField] GameObject[] mouseCursors;

    void Awake()
    {
        instance = this;

        poolManager = GameObject.Find("Pool Manager").GetComponent<PoolManager>();
        enemySpawner = GameObject.Find("Enemy Spawner").GetComponent<EnemySpawner>();
        uIManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        uIEvent = GameObject.Find("UI Manager").GetComponent<UIEvent>();
        player = GameObject.Find("MouseCursor").gameObject;

        // 저장된 데이터 가져오기
        if (!data.bgmVolume.HasKey()) {
            data.bgmVolume.Save(0.7f);
        }
        if (!data.sfxVolume.HasKey()) {
            data.sfxVolume.Save(0.7f);
        }

        highScore = data.highScore.Get();
        score = 0;

        Init();
    }

    void Init()
    {
        gameState = GameState.WindowOpen;
        stage = 0; // 게임 시작 시 NextStage() 호출하면서 + 1
        enemyCount = 0;

        // 마우스 한게임당 임시
        damageLevel = 0;
        criticalChanceLevel = 0;
        criticalDamageLevel = 0;
    }

    /// <summary>
    /// 게임의 현재 상태를 확인하는 함수
    /// </summary>
    /// <param name="gameState"></param>
    /// <returns></returns>
    public bool IsGameState(GameState gameState) => this.gameState == gameState;

    public void StartGame(int index)
    {
        // 마우스 커서 만들기
        Instantiate(mouseCursors[index], player.transform);

        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.GameStart);
        yield return new WaitForSeconds(1.5f);
        NextStage(0.0f);
    }

    public void NextStage(float time)
    {
        if (gameState == GameState.Playing)
            return;

        stage++;
        StartCoroutine(StageCount(time));
    }

    /// <summary>
    /// 3... 2... 1... start!
    /// </summary>
    /// <returns></returns>
    IEnumerator StageCount(float time)
    {
        yield return new WaitForSeconds(time);
        gameState = GameState.Playing;
        AudioManager.instance.StopBGM();
        uIManager._nextStageWaitWindow.SetActive(false);
        // 스테이지 표시
        uIManager.StageCount(stage);

        AudioManager.instance.PlayBGM(AudioManager.BGM.Battle);
        yield return new WaitForSeconds(1.0f);
        uIManager.StageWaitTimeCountSetActive(true);
        uIManager.StageWaitTimeCount(3);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Count);
        yield return new WaitForSeconds(1.0f);
        uIManager.StageWaitTimeCount(2);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Count);
        yield return new WaitForSeconds(1.0f);
        uIManager.StageWaitTimeCount(1);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Count);
        yield return new WaitForSeconds(1.0f);
        uIManager.StageWaitTimeCount(0);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.CountFinal);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BattleStartExclamation);
        yield return new WaitForSeconds(1.0f);
        uIManager.StageWaitTimeCountSetActive(false);
        // 스폰 시작
        enemySpawner.StartSpawn(stage);
    }

    public void PauseGame()
    {
        gameState = GameState.Paused;
        uIEvent.ShowGamePauseWindow();
        SlowMotionStart();
    }

    public void ContinueGame()
    {
        player.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        gameState = GameState.Playing;
        uIEvent.HiddenGamePauseWindow();
        SlowMotionStart(1.0f);
    }

    public void NewGame()
    {
        DOTween.KillAll();
        SlowMotionStart(1.0f);
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// <para>gameState 바꿔서 화면 정지</para>
    /// <para>점수와 코인 반영</para>
    /// </summary>
    public void GameOver()
    {
        int coin = data.coin.Get();
        data.coin.Save(coin + score);

        if (highScore < score) {
            highScore = score;
            data.highScore.Save(highScore);
        }
        gameState = GameState.WindowOpen;
    }

    public void StopGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 데이터 초기화
    /// </summary>
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        uIManager.Start();
    }

    /// <summary>
    /// <para>적이 얼마나 남았는지 체크하기 위한 용도</para>
    /// <para>함수는 적 한명 늘리기</para>
    /// </summary>
    public void EnemyCountIncrease()
    {
        enemyCount++;
        uIManager.EnemyCount(enemyCount);
    }
    /// <summary>
    /// <para>적이 얼마나 남았는지 체크하기 위한 용도</para>
    /// <para>점수 10점 올리고</para>
    /// <para>적 한명 줄이기</para>
    /// </summary>
    public void EnemyCountDecrease()
    {
        enemyCount--;
        uIManager.EnemyCount(enemyCount);
        score += 10;
        uIManager.ScoreCount(score);

        // 만약 적을 다 없앴다면
        if (enemyCount == 0) {
            // 적 미사일 모두 제거
            int enemyBulletsCount = poolManager.transform.GetChild(1).childCount;
            for (int i=0; i < enemyBulletsCount; i++) {
                poolManager.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }
            // 클리어 표시
            StartCoroutine(StageClear());
        }
    }

    /// <summary>
    /// <para>스테이지 클리어시 실행</para>
    /// <para>이후 대기화면 등장</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator StageClear()
    {
        gameState = GameState.WindowOpen;

        uIManager.StageWaitTimeCountSetActive(true);
        uIManager.StageClear();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.ActivityClear);
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BattleEndExclamation);
        yield return new WaitForSeconds(2.5f);
        uIManager.StageWaitTimeCountSetActive(false);
        uIManager._nextStageWaitWindow.SetActive(true);
        // uIManager.nextStageWaitWindowButtons.ButtonsSetActive(true);

        AudioManager.instance.StopAllSfx();
        AudioManager.instance.PlayBGM(AudioManager.BGM.Bakery);
    }

    /// <summary>
    /// 시간비율을 조절하는 함수 (0.0f ~ 1.0f)
    /// </summary>
    /// <param name="timeScale">0.0f ~ 1.0f</param>
    public void SlowMotionStart(float timeScale=0.0f)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    /// <summary>
    /// <para>시간비율을 일정시간에 걸쳐 천천히 증가시키는 함수</para>
    /// <para>현재 설정된 시간은 4초</para>
    /// </summary>
    public void SlowMotionEndOverTime()
    {
        Time.timeScale += (1f / 4f) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    /// <summary>
    /// 데미지 증가 버튼
    /// </summary>
    public void DamageLevelUp()
    {
        if (gameState == GameState.Playing)
            return;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.buy);
        damageLevel++;
    }
    /// <summary>
    /// 크리티컬 확률 증가 버튼
    /// </summary>
    public void CriticalChanceLevelUp()
    {
        if (gameState == GameState.Playing)
            return;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.buy);
        criticalChanceLevel++;
    }
    /// <summary>
    /// 크리티컬 데미지 증가 버튼
    /// </summary>
    public void CriticalDamageLevelUp()
    {
        if (gameState == GameState.Playing)
            return;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.buy);
        criticalDamageLevel++;
    }
    public void HpUp(int hp)
    {
        if (gameState == GameState.Playing)
            return;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.buy);
        player.transform.GetChild(0).GetComponent<MouseCursor>().Heal(hp, 0);
    }
    public void MaxHpUp(int maxHp)
    {
        if (gameState == GameState.Playing)
            return;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.buy);
        player.transform.GetChild(0).GetComponent<MouseCursor>().Heal(0, maxHp);
    }
    public void ScoreUp()
    {
        if (gameState == GameState.Playing)
            return;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.buy);
        score += 50;
        uIManager.ScoreCount(score);
    }
}

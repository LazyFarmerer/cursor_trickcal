using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MouseCursor : MonoBehaviour
{
    [Header("게임 오브젝트")]
    protected SpriteRenderer characterImage;
    [SerializeField]
    public MouseCursorDatas mouseCursorData;
    [SerializeField]
    GameObject hitFxObject;
    ParticleSystem hitFx;


    [Header("각종 게임 변수")]
    protected bool isHit;
    WaitForSeconds hitIgnoreTime = new WaitForSeconds(4.0f);
    protected Color hitActive = new Color32(255, 255, 255, 100);
    protected Color hitDeactive = new Color32(255, 255, 255, 255);
    protected int maxHp;
    protected int hp;

    public float maxSkillCoolTime;
    public float skillCoolTime;

    protected virtual void Awake()
    {
        Transform childObject = transform.Find("Character Image");
        characterImage = childObject.GetComponent<SpriteRenderer>();

        hitFxObject = GameManager.instance.player.transform.parent.Find("Hit Effect").gameObject;
        hitFx = hitFxObject.GetComponent<ParticleSystem>();

        isHit = false;
        maxHp = mouseCursorData.Hp();
        hp = maxHp;

        maxSkillCoolTime = mouseCursorData.skillCoolTime;
        skillCoolTime = 0;
    }

    void Update()
    {
        if (!GameManager.instance.IsGameState(GameManager.GameState.Playing))
            return;
    
        // 여기서 호출하는 이유:
        // Update 함수 사용하는데가 마우스밖에 없음
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.instance.PauseGame();

        Vector2 PlayerPos = transform.position;
        Vector2 MousePos = GetMousePosition();
        if (0.3f <= Vector2.Distance(PlayerPos, MousePos)) {
            Vector2 direction = MousePos - PlayerPos;
            transform.parent.Translate(direction.normalized * Time.deltaTime * 45);
        } else {
            transform.parent.position = GetMousePosition();
        }

        // 스킬 쿨타임 관련
        skillCoolTime -= Time.deltaTime;
        skillCoolTime = Mathf.Max(0, skillCoolTime);
        if (Input.GetMouseButtonDown(0) && skillCoolTime == 0 && GameManager.instance.IsGameState(GameManager.GameState.Playing)) {
            Skill();
        }

        GameManager.instance.SlowMotionEndOverTime();
    }

    protected virtual void OnEnable()
    {
        GameManager.instance.uIManager.HpBar(hp, maxHp);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) {

            bool isCritical = CriticalChance() <= Random.Range(0, 1.0f);
            float damage = isCritical? Damage(): CriticalDamage();

            other.GetComponent<Enemy>().Hit(damage, isCritical);
        } else if (other.CompareTag("EnemySkill") && !isHit) {
            Hit();
        }
    }

    protected virtual void Skill()
    {
        skillCoolTime = mouseCursorData.skillCoolTime;
    }

    protected virtual float CriticalChance()
    {
        int criticalChanceLevel = GameManager.instance.criticalChanceLevel;
        return mouseCursorData.CriticalChance(criticalChanceLevel);
    }

    protected virtual float Damage()
    {
        int damageLevel = GameManager.instance.damageLevel;
        return mouseCursorData.Damage(damageLevel);
    }

    protected virtual float CriticalDamage()
    {
        int damageLevel = GameManager.instance.damageLevel;
        int criticalDamageLevel = GameManager.instance.criticalDamageLevel;
        return mouseCursorData.CriticalDamage(damageLevel, criticalDamageLevel);
    }

    /// <summary>
    /// 마우스 위치 리턴
    /// </summary>
    /// <returns>마우스 위치 리턴</returns>
    Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
    }

    /// <summary>
    /// <para>피격시 실행</para>
    /// <para>체력을 깍고</para>
    /// <para>무적으로 만들고 흐리게 만듦</para>
    /// </summary>
    void Hit()
    {
        hp--;
        isHit = true;
        characterImage.color = hitActive;
        HitEffect();
        GameManager.instance.SlowMotionStart();
        StartCoroutine(HitIgnoreTime());

        GameManager.instance.uIManager.HpBar(hp, maxHp);

        if (hp == 0) {
            // 게임 오버
            GameManager.instance.GameOver();
            DeadAnimation();
        }
    }

    /// <summary>
    /// <para>피격시 실횅</para>
    /// <para>hitIgnoreTime 만큼 무적시간 부여</para>
    /// </summary>
    /// <returns></returns>
    protected IEnumerator HitIgnoreTime()
    {
        yield return hitIgnoreTime;
        isHit = false;
        characterImage.color = hitDeactive;
    }

    void HitEffect()
    {
        hitFxObject.transform.position = transform.position;
        hitFx.Play();
    }

    public void Heal(int hp, int maxHp)
    {
        this.hp += hp;
        this.maxHp += maxHp;
        this.maxHp = Mathf.Min(this.maxHp, mouseCursorData.maxHp);
        if (this.maxHp < this.hp) {
            this.hp = this.maxHp;
        }
        GameManager.instance.uIManager.HpBar(this.hp, this.maxHp);
    }

    void DeadAnimation()
    {
        Sequence deadAnimationSequence = DOTween.Sequence()
            .SetUpdate(true);
        Transform characterImage = transform.GetChild(0);

        deadAnimationSequence.Prepend(
            characterImage.DORotate(new Vector3(0,0,360), 1.0f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(5, LoopType.Incremental)
        )
        .Insert(
            1.5f,
            characterImage.DOMoveY(-10, 2.0f)
                .SetEase(Ease.InBack)
                .SetRelative()
                .OnComplete(() => GameManager.instance.uIManager.GameOver())
        );
    }
}

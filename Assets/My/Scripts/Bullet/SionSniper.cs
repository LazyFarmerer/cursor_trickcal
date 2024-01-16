using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SionSniper : MonoBehaviour
{
    public Sion sion;
    SpriteRenderer sprite;
    GameObject colliderObject;
    Rigidbody2D rigid;
    // 끝까지 남아서 있을 파티클
    [SerializeField] GameObject effectObjectPrefab;
    GameObject effectObject;
    ParticleSystem effectParticle;

    // 스캔 관련 변수
    Scanner scanner;
    Transform target;
    WaitForSeconds scanWiteTime = new WaitForSeconds(0.2f);

    // 게임 관련 변수
    bool isDisposable;
    readonly float speed = 7.0f;
    public float damage;
    // 스킬 다시 사용하기 위한 변수
    bool isReload; // 스킬 재장전 여부
    public int reload; // 스킬 재장전 가능 횟수

    Color colorActive = new Color32(100, 150, 225, 255);
    Color colorDeactive = new Color32(100, 150, 225, 0);

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        colliderObject = transform.Find("Collider").gameObject;
        rigid = GetComponent<Rigidbody2D>();
        effectObject = Instantiate(effectObjectPrefab);
        effectParticle = effectObject.GetComponent<ParticleSystem>();

        scanner = GetComponent<Scanner>();
        reload = 2;
    }

    void OnEnable()
    {
        isReload = true;
        isDisposable = true;
        sprite.color = colorActive;

        StartCoroutine(GameObjectDeactive(5.0f));
        StartCoroutine(Scan());
    }

    void OnDisable()
    {
        colliderObject.transform.localScale = new Vector3(1,1,1);
        StopAllCoroutines();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDisposable) {
            isDisposable = false;
            colliderObject.transform.localScale = new Vector3(5,5,5);
            Fire();
        }
        else if (other.CompareTag("Enemy")) {
            bool isDead = other.GetComponent<Enemy>().Hit(damage, true);
            if (isReload && isDead) {
                isReload = false;
                if (0 < reload)
                    sion.ReloadSkill();
                reload--; // 2->1 , 1->0
            }
        }
    }

    void Fire()
    {
        rigid.velocity = Vector2.zero;
        sprite.color = colorDeactive;
        PlayEffect();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Explosion);

        StopAllCoroutines();
        StartCoroutine(GameObjectDeactive());
    }

    void PlayEffect()
    {
        effectObject.transform.position = transform.position;
        effectParticle.Play();
    }

    /// <summary>
    /// 지속적으로 스캔하다가 발견하면 돌진
    /// </summary>
    /// <returns></returns>
    IEnumerator Scan()
    {
        while (true) {
            target = scanner.Scan();

            if (target != null) {
                break;
            }

            yield return scanWiteTime;
        }

        Vector2 vec = (target.position - transform.position).normalized;
        rigid.AddForce(vec * speed, ForceMode2D.Impulse);
    }
    
    IEnumerator GameObjectDeactive(float liveTime = 0.8f)
    {
        yield return  new WaitForSeconds(liveTime);
        gameObject.SetActive(false);
    }
}

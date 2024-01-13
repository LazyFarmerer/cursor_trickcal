using System.Collections;
using UnityEngine;

public class JuvieBullet : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    GameObject target;

    WaitForSeconds activeTime;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        target = GameManager.instance.player;
    }

    void OnEnable()
    {
        int stage = GameManager.instance.stage;
        int time = Mathf.Min(15, stage * 2);
        activeTime = new WaitForSeconds(time);

        StartCoroutine(ActiveTime());
    }

    void OnDisable()
    {
        rigid.velocity = Vector2.zero;
    }

    void FixedUpdate()
    {
        Guide();
    }

    void Guide()
    {
        Vector3 direction = target.transform.position - transform.position;
        rigid.AddForce(direction.normalized * 3, ForceMode2D.Force);

        if (0 <= rigid.velocity.x) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }

    IEnumerator ActiveTime()
    {
        yield return activeTime;
        gameObject.SetActive(false);
    }
}

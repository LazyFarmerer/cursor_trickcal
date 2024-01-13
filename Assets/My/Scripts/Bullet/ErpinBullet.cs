using UnityEngine;

public class ErpinBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    TrailRenderer trailRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void OnEnable()
    {
        float x = Random.Range(-3.0f, 3.0f);
        float y = Random.Range(4.0f, 10.0f);
        Vector2 vec = new Vector2(x, y);
        rigid.AddForce(vec, ForceMode2D.Impulse);
    }

    void OnDisable()
    {
        rigid.velocity = Vector2.zero;
        trailRenderer.Clear();
    }
}

using UnityEngine;

public class MayoBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    TrailRenderer trailRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void OnDisable()
    {
        rigid.velocity = Vector2.zero;
        trailRenderer.Clear();
    }
}

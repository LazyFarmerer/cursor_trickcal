using UnityEngine;

public class MayoBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    TrailRenderer trailRenderer;
    GameObject target;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        target = GameManager.instance.player;
    }

    void OnEnable()
    {
        float ranX = Random.Range(-1.0f, 1.0f);
        float rany = Random.Range(-1.0f, 1.0f);
        float bulletSpeed = Random.Range(2.0f, 10.0f);

        Vector3 vec = target.transform.position - transform.position;
        Vector3 ranTargetPos = vec + new Vector3(ranX, rany, 0);

        rigid.AddForce(ranTargetPos.normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    void OnDisable()
    {
        rigid.velocity = Vector2.zero;
        trailRenderer.Clear();
    }
}

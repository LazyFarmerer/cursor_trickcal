using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemySkill")) {
            other.gameObject.SetActive(false);
            other.transform.position = Vector2.zero;
        }
    }
}

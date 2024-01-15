using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScore : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemySkill")) {
            GameManager.instance.score += 1;
            GameManager.instance.uIManager.ScoreCount(GameManager.instance.score);
        }
    }
}

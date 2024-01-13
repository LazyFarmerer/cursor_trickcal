using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyDatas : ScriptableObject
{
    [SerializeField]
    private string enemyName;
    public string Name { get { return enemyName; } }
    [Header("기본 데이터")]
    [SerializeField]
    private float baseHp;
    [SerializeField]
    private float levelAdjustedHp;
    [SerializeField]
    private float waitTimeAttack;

    [Header("소리")]
    public AudioClip voiceAuio;
    public AudioClip skillAuio;

    [Header("색")]
    public Color colorActive = new Color32(150, 25, 25, 255);
    public Color colorDeactive = new Color32(255, 255, 255, 255);

    public float Hp(int stage) {
        return (baseHp + (stage * levelAdjustedHp)) * Mathf.Pow(1.05f, stage);
    }

    public float WaitTimeAttack() {
        float addRandomTime = Random.Range(0, 1.0f);
        return waitTimeAttack + addRandomTime;
    }
}

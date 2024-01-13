using UnityEngine;
using DamageNumbersPro;

[CreateAssetMenu(fileName = "DamageNumbers", menuName = "Scriptable Object/DamageNumbers", order = int.MaxValue)]
public class DamageNumbers : ScriptableObject
{
    [SerializeField]
    private DamageNumber nomal;
    [SerializeField]
    private DamageNumber critical;
    // public void PrintNomalDamage(Vector3 pos, float damage) => nomal.Spawn(pos, damage);
    // public void PrintCriticalDamage(Vector3 pos, float damage) => critical.Spawn(pos, damage);

    public void Print(Vector3 pos, float damage, bool isCritical=false)
    {
        if (isCritical) {
            nomal.Spawn(pos, damage);
        }
        else {
            critical.Spawn(pos, damage);
        }
    }
}

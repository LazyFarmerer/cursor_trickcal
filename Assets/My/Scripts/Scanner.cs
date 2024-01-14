using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float range;

    public Transform Scan()
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, targetLayer);
        return DistalDistance(targets);
    }

    Transform DistalDistance(RaycastHit2D[] targets)
    {
        Transform result = null;

        float distalDistance = 0;
        foreach (RaycastHit2D target in targets) {
            float targetDistance = Vector3.Distance(transform.position, target.transform.position);
            if (distalDistance < targetDistance) {
                distalDistance = targetDistance;
                result = target.transform;
            }
        }

        return result;
    }
}

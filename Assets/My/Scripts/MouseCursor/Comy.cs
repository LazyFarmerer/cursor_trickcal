using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Comy : MouseCursor
{
    Sequence skillAnimation;

    protected override void Awake()
    {
        base.Awake();

        skillAnimation = DOTween.Sequence()
            .SetAutoKill(false)
            .SetUpdate(true);
        skillAnimation
        .Append(
            transform.DOScale(2, 0.5f)
        )
        .Append(
            transform.DOScale(1, 0.5f)
                .SetDelay(3.0f)
        )
        .Rewind();
    }

    protected override void Skill()
    {
        base.Skill();

        isHit = true;
        StartCoroutine(HitIgnoreTime());
        skillAnimation.Restart();
    }
}

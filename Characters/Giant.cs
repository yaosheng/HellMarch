using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Giant : Character
{

    public Giant()
    {
        attArea = new Point[4] { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1) };
        attackPoint = 1;
        moveSpeed = 10;
    }

    public override void JobEffectForMonsterLife(Point[] hitAreaPoint)
    {

    }

    public override void JobEffectForMonsterDead()
    {

    }

    public override void HitEffect()
    {

    }
}

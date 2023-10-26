using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ninja : Character {

    public Ninja()
    {
        attArea = new Point[1] { new Point(0, 0) };
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

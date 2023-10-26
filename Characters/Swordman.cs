using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swordman : Character {

    public Swordman()
    {
        attArea = new Point[3] { new Point(-1, 0), new Point(0, 0), new Point(1, 0) };
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

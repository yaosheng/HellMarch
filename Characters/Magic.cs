using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Magic : Character {

    protected override Point[] attArea
    {
        get
        {
            Point[] p = new Point[Data.tileWidth];
            for (int i = 0; i < Data.tileWidth; i++)
            {
                p[i] = new Point(i, 0);
            }
            return p;
        }
    }

    public Magic()
    {
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dwarf : Character {

    public Dwarf()
    {
        attArea = new Point[7] { new Point(-1, 0), new Point(0, 0), new Point(1, 0) ,
                                new Point(-1, -1), new Point(1, -2), new Point(1, -1),
                                new Point(-1, -2) };
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

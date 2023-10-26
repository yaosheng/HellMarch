using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Knight : Character 
{
    public Knight()
    {
        attArea = new Point[]{ new Point(0, 0), new Point(0, 1), new Point(0, 2) };
        attackPoint = 1;
        moveSpeed = 10;
    }

    public override void JobEffectForMonsterLife(Point[] hitAreaPoint)
    {
        if (lifeMonsters.Count > 0)
        {
            TileMatch[] tempTileMatch = lifeMonsters.ToArray();
            tempTileMatch = Tools.SortTileMatchTopDown(tempTileMatch);
            Debug.Log("tempTileMatch.Length : " + tempTileMatch.Length);
            for (int i = 0; i < tempTileMatch.Length; i++)
            {
                Debug.Log("x : " + tempTileMatch[i].point.x + ", " + tempTileMatch[i].point.y + ", " + tempTileMatch[i].cell.randomTile);
                if (tempTileMatch[i].gameObject.activeSelf)
                {
                    //Debug.Log(tempTileMatch[i].cell.randomTile);
                    int temp = 0;
                    int maxX = 0, maxY = 0;
                    if (tempTileMatch[i].extraPoint != null && tempTileMatch[i].extraPoint.Length >= 1)
                    {
                        for (int k = 0; k < tempTileMatch[i].extraPoint.Length; k++)
                        {
                            if (maxX < tempTileMatch[i].extraPoint[k].x) maxX = tempTileMatch[i].extraPoint[k].x;
                            if (maxY < tempTileMatch[i].extraPoint[k].y) maxY = tempTileMatch[i].extraPoint[k].y;
                        }
                        //Debug.Log("maxX , maxY : " + maxX + ", " + maxY);
                        for (int k = 0; k < tempTileMatch[i].extraPoint.Length; k++)
                        {
                            if (tempTileMatch[i].extraPoint[k].y == maxY)
                            {
                                if (ArmyManager.cells[tempTileMatch[i].point.x + tempTileMatch[i].extraPoint[k].x,
                                    tempTileMatch[i].point.y + tempTileMatch[i].extraPoint[k].y + 1].IsEmpty) temp++;
                                //Debug.Log("temp : " + temp);
                            }
                        }
                        if (maxY == 0)
                        {
                            if (ArmyManager.cells[tempTileMatch[i].point.x, tempTileMatch[i].point.y + 1].IsEmpty) temp++;
                        }

                        if (temp < maxX + 1)//!=
                        {
                            Debug.Log("multi don't move");
                            iTween.Stop(tempTileMatch[i].gameObject);
                            MonsterBeAttackedMotion(tempTileMatch[i]);
                            continue;
                        }
                        else MoveAndResetPoint(tempTileMatch[i], 1, "y", maxX, maxY);
                    }
                    else
                    {
                        if (!ArmyManager.cells[tempTileMatch[i].point.x, tempTileMatch[i].point.y + 1].IsEmpty)
                        {
                            Debug.Log("single don't move");
                            iTween.Stop(tempTileMatch[i].gameObject);
                            MonsterBeAttackedMotion(tempTileMatch[i]);
                            continue;
                        }
                        else
                        {
                            MoveAndResetPoint(tempTileMatch[i], 1, "y", maxX, maxY);
                        }
                    }
                }

            }
        }

    }

    public override void JobEffectForMonsterDead()
    {

    }

    public override void HitEffect()
    {

    }
}

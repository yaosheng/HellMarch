using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{

    public string tagName = "";
    protected virtual Point[] attArea
    {
        get
        {
            tagName = this.gameObject.tag;
            Point[] p = new Point[0];
            switch (tagName)
            {
                case "swordman":
                    p = new Point[3] { new Point(-1, 0), new Point(0, 0), new Point(1, 0) };
                    break;
                case "ninja":
                    p = new Point[1] { new Point(0, 0) };
                    break;
                case "magic":
                    p = new Point[Data.tileWidth];
                    for (int i = 0; i < Data.tileWidth; i++)
                    {
                        p[i] = new Point(i, 0);
                    }
                    break;
                case "giant":
                    p = new Point[4] { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1) };
                    break;
                case "knight":
                    p = new Point[3] { new Point(0, 0), new Point(0, 1), new Point(0, 2) };
                    break;
                case "dwarf":
                    p = new Point[7] { new Point(-1, 0), new Point(0, 0), new Point(1, 0) ,
                                       new Point(-1, -1), new Point(1, -2), new Point(1, -1),
                                       new Point(-1, -2) };
                    break;
            }

            return p;
        }
        set { }
    }
    protected int attackPoint = 1;
    protected int moveSpeed = 10;

    protected List<TileMatch> lifeMonsters = new List<TileMatch>();
    protected List<TileMatch> deadMonsters = new List<TileMatch>();

    private RaycastHit2D hit, hit2;
    private int lineCheck = -1;
    private const float hitmotionTime = 0.05f;
    private const float movemotionTime = 1.2f;
    private const float hitmoveTransform = 0.25f;
    private const float hitDistence = 1.3f;
    private const float showDistence = 100.0f;

    public void FixedUpdate()
    {
        hit = Physics2D.Raycast(this.transform.position, transform.up, hitDistence);
        hit2 = Physics2D.Raycast(this.transform.position, transform.up, showDistence);
        if (TouchManager.isCreated && hit2 && lineCheck != TouchManager.line)
        {
            Tools.ClosedShowHitMonsters();
            TileMatch tm2 = hit2.collider.GetComponent<TileMatch>();
            ShowReadyToHit(tm2);
            lineCheck = TouchManager.line;
        }
        if (TouchManager.isFinished == false && TouchManager.isCreated == false)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
        if (hit)
        {
            TileMatch tm1 = hit.collider.GetComponent<TileMatch>();
            HitSelfEffect();
            HitOtherEffect(tm1);
            Tools.ClosedShowHitMonsters();
        }
    }

    public void HitSelfEffect()
    {
        this.gameObject.SetActive(false);
        TouchManager.isFinished = true;
    }

    public void ShowReadyToHit(TileMatch tm)
    {
        int hitX = tm.point.x;
        int hitY = tm.point.y;
        Point[] hitPoint = Tools.FindAttArea(this.gameObject, hitX, hitY, tm, attArea);
        Tools.ShowAttPoint(hitX, hitY, attArea);
        Tools.ShowHitMonsters(tm);
        foreach (TileMatch tm1 in ArmyManager.tiles)
        {
            if (tm1.gameObject.activeSelf)
            {
                if (tm1.extraPoint != null && tm1.extraPoint.Length >= 1)
                {
                    for (int i = 0; i < hitPoint.Length; i++)
                    {
                        for (int j = 0; j < tm1.extraPoint.Length; j++)
                        {
                            if (hitPoint[i].x == tm1.point.x + tm1.extraPoint[j].x && hitPoint[i].y == tm1.point.y + tm1.extraPoint[j].y)
                            {
                                Tools.ShowHitMonsters(tm1);
                            }
                            if (hitPoint[i].x == tm1.point.x && hitPoint[i].y == tm1.point.y)
                            {
                                Tools.ShowHitMonsters(tm1);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < hitPoint.Length; i++)
                    {
                        if (hitPoint[i].x == tm1.point.x && hitPoint[i].y == tm1.point.y)
                        {
                            Tools.ShowHitMonsters(tm1);
                        }
                    }
                }
            }
        }
    }

    public void HitOtherEffect(TileMatch tm)
    {
        int hitX = tm.point.x;
        int hitY = tm.point.y;
        Point[] hitPoint = Tools.FindAttArea(this.gameObject, hitX, hitY, tm, attArea);
        foreach (TileMatch tm1 in ArmyManager.tiles)
        {
            if (tm1.gameObject.activeSelf)
            {
                if (tm1.extraPoint != null && tm1.extraPoint.Length >= 1)
                {
                    int temp = 0;
                    for (int i = 0; i < hitPoint.Length; i++)
                    {
                        if (hitPoint[i].x == tm1.point.x && hitPoint[i].y == tm1.point.y)
                        {
                            HitMonster(tm1);
                            temp++;
                        }

                        for (int j = 0; j < tm1.extraPoint.Length; j++)
                        {
                            if (hitPoint[i].x == tm1.point.x + tm1.extraPoint[j].x && hitPoint[i].y == tm1.point.y + tm1.extraPoint[j].y)
                            {
                                HitMonster(tm1);
                                temp++;
                            }
                        }
                    }
                    if (temp > 0)
                    {
                        lifeMonsters.Add(tm1);
                    }
                }
                else
                {
                    for (int i = 0; i < hitPoint.Length; i++)
                    {
                        if (hitPoint[i].x == tm1.point.x && hitPoint[i].y == tm1.point.y)
                        {
                            HitMonster(tm1);
                            lifeMonsters.Add(tm1);
                        }
                    }
                }
            }
        }
        JobEffectForMonsterLife(hitPoint);
        JobEffectForMonsterDead();
    }

    public void MonsterBeAttackedMotion(TileMatch tm)
    {
        iTween.MoveTo(tm.gameObject, iTween.Hash("y", tm.originalPosition.y + hitmoveTransform, "islocal", true, "time", hitmotionTime, "easeType", "easeOutExpo"));
        iTween.MoveTo(tm.gameObject, iTween.Hash("y", tm.originalPosition.y, "islocal", true, "time", hitmotionTime, "delay", hitmotionTime, "easeType", "easeinExpo"));
    }

    public void HitMonster(TileMatch tm)
    {
        tm.HP -= attackPoint;
        if (!tm.gameObject.activeSelf || tm.HP <= 0)
        {
            deadMonsters.Add(tm);
            ArmyManager.cells[tm.point.x, tm.point.y].randomTile = Data.tileTypes.empty;
            if (tm.extraPoint != null && tm.extraPoint.Length > 0)
            {
                for (int i = 0; i < tm.extraPoint.Length; i++)
                {
                    ArmyManager.cells[tm.point.x + tm.extraPoint[i].x, tm.point.y + tm.extraPoint[i].y].randomTile = Data.tileTypes.empty;
                }
            }
            tm.gameObject.SetActive(false);
        }
    }

    public virtual void JobEffectForMonsterLife(Point[] hitAreaPoint)
    {
        if (lifeMonsters.Count > 0)
        {
            TileMatch[] tempTileMatch = lifeMonsters.ToArray();
            tempTileMatch = Tools.SortTileMatchTopDown(tempTileMatch);
            Debug.Log("tempTileMatch.Length : " + tempTileMatch.Length);
            switch (tagName)
            {
                case "swordman":
                    for (int i = tempTileMatch.Length - 1; i >= 0; i--)
                    {
                        MonsterBeAttackedMotion(tempTileMatch[i]);
                    }
                    break;
                case "ninja":
                    for (int i = tempTileMatch.Length - 1; i >= 0; i--)
                    {
                        MonsterBeAttackedMotion(tempTileMatch[i]);
                    }
                    break;
                case "magic":
                    for (int i = tempTileMatch.Length - 1; i >= 0; i--)
                    {
                        MonsterBeAttackedMotion(tempTileMatch[i]);
                    }
                    break;
                case "giant":
                    for (int i = tempTileMatch.Length - 1; i >= 0; i--)
                    {
                        if (tempTileMatch[i].gameObject.activeSelf)
                        {
                            int maxX = 0, maxY = 0;
                            if (tempTileMatch[i].extraPoint != null && tempTileMatch[i].extraPoint.Length >= 1)
                            {
                                for (int k = 0; k < tempTileMatch[i].extraPoint.Length; k++)
                                {
                                    if (maxX < tempTileMatch[i].extraPoint[k].x) maxX = tempTileMatch[i].extraPoint[k].x;
                                    if (maxY < tempTileMatch[i].extraPoint[k].y) maxY = tempTileMatch[i].extraPoint[k].y;
                                }
                                if (lineCheck == tempTileMatch[i].point.x - 1)
                                {
                                    CheckMonsterMoveOrNot(tempTileMatch[i], 1, "x", maxX, maxY);
                                }
                                if(lineCheck == tempTileMatch[i].point.x + maxX)
                                {
                                    CheckMonsterMoveOrNot(tempTileMatch[i], -1, "x", maxX, maxY);
                                }
                            }
                            else
                            {
                                if (tempTileMatch[i].point.x == lineCheck)
                                {
                                    if ((tempTileMatch[i].point.x != 0 && !ArmyManager.cells[tempTileMatch[i].point.x - 1, tempTileMatch[i].point.y].IsEmpty) || tempTileMatch[i].point.x == 0)
                                    {
                                        iTween.Stop(tempTileMatch[i].gameObject);
                                        MonsterBeAttackedMotion(tempTileMatch[i]);
                                        continue;
                                    }
                                    else
                                    {
                                        MoveAndResetPoint(tempTileMatch[i], -1, "x", maxX, maxY);
                                    }
                                }
                                else
                                {
                                    if ((tempTileMatch[i].point.x != Data.tileWidth - 1 && !ArmyManager.cells[tempTileMatch[i].point.x + 1, tempTileMatch[i].point.y].IsEmpty) || tempTileMatch[i].point.x == Data.tileWidth - 1)
                                    {
                                        iTween.Stop(tempTileMatch[i].gameObject);
                                        MonsterBeAttackedMotion(tempTileMatch[i]);
                                        continue;
                                    }
                                    else
                                    {
                                        MoveAndResetPoint(tempTileMatch[i], 1, "x", maxX, maxY);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "knight":
                    //Point[] newPoint = { hitAreaPoint[1], hitAreaPoint[2], new Point(hitAreaPoint[0].x, hitAreaPoint[2].y + 1) };
                    for (int i = 0; i < tempTileMatch.Length; i++)
                    {
                        Debug.Log("x : " + tempTileMatch[i].point.x + ", " + tempTileMatch[i].point.y + ", " + tempTileMatch[i].cell.randomTile);
                        if (tempTileMatch[i].gameObject.activeSelf)
                        {
                            //Debug.Log(tempTileMatch[i].cell.randomTile);
                            int maxX = 0, maxY = 0;
                            if (tempTileMatch[i].extraPoint != null && tempTileMatch[i].extraPoint.Length >= 1)
                            {
                                for (int k = 0; k < tempTileMatch[i].extraPoint.Length; k++)
                                {
                                    if (maxX < tempTileMatch[i].extraPoint[k].x) maxX = tempTileMatch[i].extraPoint[k].x;
                                    if (maxY < tempTileMatch[i].extraPoint[k].y) maxY = tempTileMatch[i].extraPoint[k].y;
                                }
                                CheckMonsterMoveOrNot(tempTileMatch[i], 1, "y", maxX, maxY);
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
                    break;
                case "dwarf":
                    for (int i = tempTileMatch.Length - 1; i >= 0; i--)
                    {
                        MonsterBeAttackedMotion(tempTileMatch[i]);
                    }
                    break;
            }
            lifeMonsters.Clear();
        }
    }

    public void CheckMonsterMoveOrNot(TileMatch tm, int distance, string direction, int maxX, int maxY)
    {
        int temp = 0;
        switch (direction)
        {
            case "y":
                for (int k = 0; k < tm.extraPoint.Length; k++)
                {
                    if (tm.extraPoint[k].y == maxY)
                    {
                        if (ArmyManager.cells[tm.point.x + tm.extraPoint[k].x,
                            tm.point.y + tm.extraPoint[k].y + distance].IsEmpty) temp++;
                    }
                }
                if (maxY == 0) if (ArmyManager.cells[tm.point.x, tm.point.y + distance].IsEmpty) temp++;
                if (temp < maxX + distance)//!=
                {
                    Debug.Log("multi don't move");
                    iTween.Stop(tm.gameObject);
                    MonsterBeAttackedMotion(tm);
                }
                else MoveAndResetPoint(tm, distance, "y", maxX, maxY);

                break;
            case "x":
                for (int k = 0; k < tm.extraPoint.Length; k++)
                {
                    if (tm.extraPoint[k].x == maxX)
                    {
                        if (ArmyManager.cells[tm.point.x + tm.extraPoint[k].x + distance,
                            tm.point.y + tm.extraPoint[k].y].IsEmpty) temp++;
                    }
                }
                if (maxX == 0) if (ArmyManager.cells[tm.point.x + distance, tm.point.y].IsEmpty) temp++;
                if (temp < maxY + distance)//!=
                {
                    Debug.Log("multi don't move");
                    iTween.Stop(tm.gameObject);
                    MonsterBeAttackedMotion(tm);
                }
                else MoveAndResetPoint(tm, distance, "x", maxX, maxY);
                break;
        }



    }

    public void MoveAndResetPoint(TileMatch tm, int distance, string direction, int maxX, int maxY)
    {
        Debug.Log("move");
        iTween.Stop(tm.gameObject);
        switch (direction)
        {
            case "y":
                ArmyManager.cells[tm.point.x, tm.point.y + distance].randomTile = ArmyManager.cells[tm.point.x, tm.point.y].randomTile;
                if (tm.extraPoint != null && tm.extraPoint.Length >= 1)
                {
                    for (int i = 0; i < tm.extraPoint.Length; i++)
                    {
                        ArmyManager.cells[tm.point.x + tm.extraPoint[i].x, tm.point.y + tm.extraPoint[i].y + distance].randomTile = ArmyManager.cells[tm.point.x + tm.extraPoint[i].x, tm.point.y + tm.extraPoint[i].y].randomTile;
                    }
                }
                for (int i = 0; i <= maxX; i++)
                {
                    Debug.Log("set empty");
                    ArmyManager.cells[tm.point.x + i, tm.point.y].randomTile = Data.tileTypes.empty;
                }
                iTween.MoveTo(tm.gameObject, iTween.Hash("y", (tm.point.y + distance) * ArmyManager.cellHeight, "islocal", true, "time", movemotionTime, "easeType", "easeOutBack"));
                tm.originalPosition = new Vector2(tm.point.x * ArmyManager.cellWidth, (tm.point.y + distance) * ArmyManager.cellHeight);
                tm.point = new Point(tm.point.x, tm.point.y + distance);
                break;

            case "x":
                ArmyManager.cells[tm.point.x + distance, tm.point.y].randomTile = ArmyManager.cells[tm.point.x, tm.point.y].randomTile;
                if (tm.extraPoint != null && tm.extraPoint.Length >= 1)
                {
                    for (int i = 0; i < tm.extraPoint.Length; i++)
                    {
                        ArmyManager.cells[tm.point.x + tm.extraPoint[i].x + distance, tm.point.y + tm.extraPoint[i].y].randomTile = ArmyManager.cells[tm.point.x + tm.extraPoint[i].x, tm.point.y + tm.extraPoint[i].y].randomTile;

                    }
                }
                for (int i = 0; i <= maxY; i++)
                {
                    ArmyManager.cells[tm.point.x, tm.point.y + i].randomTile = Data.tileTypes.empty;
                }
                iTween.MoveTo(tm.gameObject, iTween.Hash("x", (tm.point.x + distance) * ArmyManager.cellWidth, "islocal", true, "time", movemotionTime, "easeType", "easeOutBack"));
                tm.originalPosition = new Vector2((tm.point.x + distance) * ArmyManager.cellWidth, tm.point.y * ArmyManager.cellHeight);
                tm.point = new Point(tm.point.x + distance, tm.point.y);
                break;
        }
        Debug.Log("x : " + tm.point.x + ", " + tm.point.y + ", " + tm.cell.randomTile);
    }

    public virtual void JobEffectForMonsterDead()
    {

    }

    public virtual void HitEffect()
    {

    }


}

using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour {

    public static void ShowHitMonsters(TileMatch tm)
    {
        tm.GetComponent<SpriteRenderer>().color = new Color32(20, 20, 20, 255);
    }

    public static void ClosedShowHitMonsters()
    {
        foreach (TileMatch tm in ArmyManager.tiles)
        {
            tm.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
    }

    public static void showCells()
    {
        for (int x = 0; x < Data.tileWidth; x++)
        {
            for (int y = 0; y < Data.tileHeight; y++)
            {
                Debug.Log("cells : " + "( " + x + " , " + y + ")" + " : " + ArmyManager.cells[x, y].randomTile);
            }
        }
    }

    public static Point[] FindAttArea(GameObject go, int x, int y, TileMatch tm, Point[] attArea)
    {
        Point[] hitPoint = new Point[attArea.Length];

        if (go.tag != "magic")
        {
            for (int i = 0; i < attArea.Length; i++)
            {
                hitPoint[i] = new Point(TouchManager.line + attArea[i].x, y + attArea[i].y);
            }
        }
        else
        {
            for (int i = 0; i < attArea.Length; i++)
            {
                hitPoint[i] = new Point(attArea[i].x, y + attArea[i].y);
            }
        }

        return hitPoint;
    }

    public static void ShowAttPoint(int x, int y, Point[] p)
    {
        string st = "";
        for (int i = 0; i < p.Length; i++)
        {
            int sumX = x + p[i].x;
            int sumY = y + p[i].y;
            st += "(" + sumX + ", " + sumY + ")";
        }
        Debug.Log("st : " + st);
    }

    public static TileMatch[] SortTileMatchTopDown(TileMatch[] tilematchArray)
    {
        TileMatch[] tmArray = new TileMatch[tilematchArray.Length];
        int temp = 0;
        for (int y = Data.tileHeight - 1; y >= 0; y--)
        {
            for (int x = Data.tileWidth - 1; x >= 0; x--)
            {
                foreach(TileMatch tm in tilematchArray)
                {
                    if(tm.point.x == x && tm.point.y == y)
                    {
                        tmArray[temp] = tm;
                        temp++;
                    }
                }
            }
        }
        return tmArray;
    }
}

using UnityEngine;
using System.Collections;

public enum TestType
{
    full,
    someEmpty,
}

public struct Point
{
    public int x, y;
    public Point(int pointX, int pointY)
    {
        x = pointX;
        y = pointY;
    }
    public override string ToString()
    {
        return "(" + x + " , " + y + ")";
    }
}

public class Data
{
    public const int tileWidth = 6;
    public const int tileHeight = 40;
    public enum tileTypes { part, empty, monster1, monster2, monster3, monster4, monster5, monster6, monster7, monster8, monster9 };
}

public class Cell
{
    public Data.tileTypes randomTile = Data.tileTypes.empty;
    public bool IsEmpty
    {
        get { return randomTile == Data.tileTypes.empty; }
    }
    public void SetRandomTile(int total, int specialType)
    {
        int temp1 = UnityEngine.Random.Range(0, 20);
        int temp2 = 0;
        int temp3 = 2;
        if (ArmyManager.gameType == TestType.full)
        {
            temp2 = 2;
            if (temp1 > 2)
            {
                randomTile = (Data.tileTypes)UnityEngine.Random.Range(0, total - 2) + temp2;
            }
            else
            {
                randomTile = (Data.tileTypes)UnityEngine.Random.Range(7, total) + temp2;
            }
        }
        else
        {
            temp2 = UnityEngine.Random.Range(0, 10);
            if (temp2 > 3)
            {
                if (temp1 > 2)
                {
                    randomTile = (Data.tileTypes)UnityEngine.Random.Range(1, 9);
                }
                else
                {
                    randomTile = (Data.tileTypes)UnityEngine.Random.Range(7, total) + temp3;
                }
            }
            else
            {
                randomTile = (Data.tileTypes)(1);
            }
        }
    }
}
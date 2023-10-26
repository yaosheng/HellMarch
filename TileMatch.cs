using UnityEngine;
using System.Collections;

public class TileMatch : MonoBehaviour {

    public int HP;
    public int container;
    public Point point;
    public int pointX;
    public int pointY;
    public string thisCell;
    public Vector2 originalPosition;
    public Point[] extraPoint
    {
        get
        {
            Point[] ep = new Point[container - 1];
            if (this.tag == "Monster2.2")
            {
                ep[0] = new Point(0, 1);
                ep[1] = new Point(1, 0);
                ep[2] = new Point(1, 1);
            }
            else if (this.tag == "Monster1.2")
            {
                ep[0] = new Point(0, 1);
            }
            else if (this.tag == "Monster2.1")
            {
                ep[0] = new Point(1, 0);
            }
            else if (this.tag == "Monster1.3")
            {
                ep[0] = new Point(0, 1);
                ep[1] = new Point(0, 2);
            }
            else return null;
            return ep;
        }
    }
    public Cell cell;

    void Start()
    {
        thisCell = cell.randomTile.ToString();
    }
    void FixedUpdate()
    {
        pointX = point.x;
        pointY = point.y;
    }
}

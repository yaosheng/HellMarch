using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmyManager : MonoBehaviour {

    public Transform armyRoot;
    public SpriteRenderer[] armyArray;
    public static float cellWidth = 1.6f;
    public static float cellHeight = 1.2f;
    public static Cell[,] cells = new Cell[Data.tileWidth, Data.tileHeight];
    public static List<TileMatch> tiles;
    public static GameObject[,] SpriteArmy = new GameObject[Data.tileWidth, Data.tileHeight];
    public static float armySpeed = 0.2f;
    private int normalMaster = 7;
    private int specialMaster = 2;
    public static TestType gameType = TestType.full;

    public void CreatTileGrid()
    {
        for (int x = 0; x < Data.tileWidth; x++)
        {
            for (int y = 0; y < Data.tileHeight; y++)
            {
                cells[x, y] = new Cell();
            }
        }
    }

    public void showCells()
    {
        for (int x = 0; x < Data.tileWidth; x++)
        {
            for (int y = 0; y < Data.tileHeight; y++)
            {
                Debug.Log("cells : " + "( " + x + " , " + y + ")" + " : " + cells[x, y].randomTile);
            }
        }
    }

    public void InitTileGrid()
    {
        for (int x = 0; x < Data.tileWidth; x++)
        {
            for (int y = 0; y < Data.tileHeight; y++)
            {
                cells[x, y] = new Cell();
                cells[x, y].SetRandomTile(normalMaster + specialMaster, specialMaster);
            }
        }
    }

    public void CheckTile()
    {
        for (int y = 0; y < Data.tileHeight; y++)
        {
            for (int x = 0; x < Data.tileWidth; x++)
            {
                if(cells[x, y].randomTile == Data.tileTypes.monster8)
                {
                    if(x > 0 && y < Data.tileHeight - 1)
                    {
                        if (cells[x - 1, y + 1].randomTile == Data.tileTypes.monster8 || cells[x - 1, y + 1].randomTile == Data.tileTypes.monster9)
                        {
                            Debug.Log("bug : " + new Point(x, y));
                            cells[x - 1, y + 1].SetRandomTile(normalMaster, specialMaster);
                            Debug.Log("tile : " + cells[x, y].randomTile);
                            Debug.Log("tile : " + cells[x - 1, y + 1].randomTile);
                        }
                    }
                }
            }
        }
    }

    public void DisplayTileGrid()
    {
        tiles = new List<TileMatch>();
        for (int y = 0; y < Data.tileHeight; y++)
        {
            for (int x = 0; x < Data.tileWidth; x++)
            {
                if (cells[x, y].randomTile == Data.tileTypes.part) continue;
                int type = (int)cells[x, y].randomTile - 2;//2
                if (type >= normalMaster && x == Data.tileWidth - 1)
                {
                    if (x == Data.tileWidth - 1 || y == Data.tileHeight - 1) type = UnityEngine.Random.Range(0, normalMaster);
                }
                if(type == -1)
                {
                    continue;
                }
                SpriteRenderer instance = Instantiate(armyArray[(type)]) as SpriteRenderer;
                instance.transform.parent = armyRoot;
                instance.sortingOrder = Data.tileHeight - y;
                
                TileMatch tile = instance.GetComponent<TileMatch>();
                tile.cell = cells[x, y];
                tile.point = new Point(x, y);
                tiles.Add(tile);
                SpriteArmy[x, y] = instance.gameObject;

                if (type >= normalMaster && x < Data.tileWidth - 1)//fixed temporarily
                {
                    int mulitX = x;
                    //int check = 0;
                    if (tile.container > 1)
                    {
                        for (int i = 0; i < tile.container - 1; i++)
                        {
                            int x1 = x + tile.extraPoint[i].x;
                            int y1 = y + tile.extraPoint[i].y;
                            if (x1 <= Data.tileWidth - 1 && y1 <= Data.tileHeight - 1)
                            {
                                if (cells[x1, y1].randomTile != Data.tileTypes.part) cells[x1, y1].randomTile = Data.tileTypes.part;
                                //else check++;
                            }
                            if (tile.extraPoint[i].x == 1 && tile.extraPoint[i].y == 0) mulitX += tile.extraPoint[i].x;
                        }
                        //if (check > 0)
                        //{
                        //    //int tempType = UnityEngine.Random.Range(0, normalMaster);
                        //    //instance = armyArray[(tempType)];
                        //    isFixed = false;
                        //    Debug.Log("conflict");
                        //}
                    }
                    instance.transform.localPosition = new Vector3(((x + mulitX) * cellWidth) / 2, y * cellHeight, 0f);
                    tile.originalPosition = instance.transform.localPosition;
                }
                else instance.transform.localPosition = new Vector3(x * cellWidth, y * cellHeight, 0f);
                tile.originalPosition = instance.transform.localPosition;
            }
        }
    }

    void Update () {
        transform.Translate(Vector2.down * armySpeed * Time.deltaTime);
	}

    void Start()
    {
        CreatTileGrid();
        InitTileGrid();
        CheckTile();
        DisplayTileGrid();
        showCells();
    }
}

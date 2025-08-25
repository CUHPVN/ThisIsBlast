using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GridParent : MonoBehaviour
{
    public static GridParent Instance { get; private set; }
    [SerializeField] private ColorTile tilePrefab;
    [SerializeField] private ColorShooter shooterPrefab;
    [SerializeField] private Transform tileParentPrefab;
    [SerializeField] private Transform shooterParentPrefab;
    [SerializeField] private int colorCode = 0;
    [SerializeField] private int count = 1;
    [SerializeField] private int tileRow = 10;
    [SerializeField] private int tileCol = 13;
    [SerializeField] private int shooterRow = 5;
    [SerializeField] private int shooterCol = 5;
    public bool isTile = true;
    public int[] colorCount= new int[10];
    [SerializeField] private List<List<ColorTile>> tiles = new();
    [SerializeField] private List<List<ColorShooter>> shooters = new();
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        GenTile();
        GenShooter();
    }

    void Update()
    {
        
    }
    void GenTile()
    {
        for(int i = 0; i < tileCol; i++)
        {
            List<ColorTile> list = new List<ColorTile>();
            for(int j=0; j < tileRow; j++)
            {
                ColorTile colorTile = Instantiate(tilePrefab,GenPos(i,j),Quaternion.identity);
                colorTile.transform.parent = tileParentPrefab;
                list.Add(colorTile);
            }
            tiles.Add(list);
        }
        UpdateState();
    }
    void GenShooter()
    {
        for (int i = 0; i < shooterCol; i++)
        {
            List<ColorShooter> list = new List<ColorShooter>();
            for (int j = 0; j < shooterRow; j++)
            {
                ColorShooter colorShooter = Instantiate(shooterPrefab, GenShooterPos(i, j), Quaternion.identity);
                colorShooter.transform.parent = shooterParentPrefab;
                list.Add(colorShooter);
            }
            shooters.Add(list);
        }
        UpdateState();
    }
    private void UpdateState()
    {
        tileParentPrefab.gameObject.SetActive(isTile);
        shooterParentPrefab.gameObject.SetActive(!isTile);
    }
    public void SetDrawMode(bool value)
    {
        isTile = value;
        UpdateState();
    }
    public void Export()
    {
        List<List<int>> tileColorList = new();
        List<List<int>> tileCountList = new();
        List<List<int>> shooterColorList = new();
        List<List<int>> shooterCountList = new();
        List<List<bool>> shooterHiddenList = new();

        for (int i = 0; i < tileCol; i++)
        {
            List<int> list1 = new();
            List<int> list2 = new();

            for (int j = 0; j < tileRow; j++)
            {
                list1.Add(tiles[i][j].colorCode);
                list2.Add(tiles[i][j].count);
            }
            tileColorList.Add(list1);
            tileCountList.Add(list2);
        }
        for (int i = 0; i < shooterCol; i++)
        {
            List<int> list1 = new();
            List<int> list2 = new();
            List<bool> list3 = new();


            for (int j = 0; j < shooterRow; j++)
            {
                list1.Add(shooters[i][j].colorCode);
                list2.Add(shooters[i][j].count);
                list3.Add(shooters[i][j].isHidden);
            }
            shooterColorList.Add(list1);
            shooterCountList.Add(list2);
            shooterHiddenList.Add(list3);
        }
        TileData tileData = new TileData()
        {
            tileColor = LevelData.ConvertToIntList(tileColorList),
            tileCount = LevelData.ConvertToIntList(tileCountList)
        };
        ShooterData shooterData = new ShooterData()
        {
            shooterColor = LevelData.ConvertToIntList(shooterColorList),
            shooterCount = LevelData.ConvertToIntList(shooterCountList),
            shooterHidden = LevelData.ConvertToBoolList(shooterHiddenList)
        };
        LevelData.ExportColorList(tileData,shooterData);
    }
    void Clear()
    {
        for (int i = 0; i < tileCol; i++)
        {
            for (int j = 0; j < tileRow; j++)
            {
                Destroy(tiles[i][j].gameObject);
            }
        }
        tiles.Clear();
        for (int i = 0; i < shooterCol; i++)
        {
            for (int j = 0; j < shooterRow; j++)
            {
                Destroy(shooters[i][j].gameObject);
            }
        }
        shooters.Clear();
    }
    internal void Import((TileData,ShooterData) value)
    {
        List<List<int>> tileColorList = LevelData.ConvertFromIntList(value.Item1.tileColor);
        List<List<int>> tileCountList = LevelData.ConvertFromIntList(value.Item1.tileCount);
        List<List<int>> shooterColorList = LevelData.ConvertFromIntList(value.Item2.shooterColor);
        List<List<int>> shooterCountList = LevelData.ConvertFromIntList(value.Item2.shooterCount);
        List<List<bool>> shooterHiddenList = LevelData.ConvertFromBoolList(value.Item2.shooterHidden);
        Clear();
        tileCol = tileColorList.Count;
        GenTile();

        shooterCol = shooterColorList.Count;
        shooterRow = shooterColorList[0].Count;
        GenShooter();

        for (int i = 0; i < tileCol; i++)
        {
            for (int j = 0; j < tileRow; j++)
            {
                tiles[i][j].SetColor(tileColorList[i][j]);
                tiles[i][j].SetCount(tileCountList[i][j]);
            }
        }
        for (int i = 0; i < shooterCol; i++)
        {
            for (int j = 0; j < shooterRow; j++)
            {
                shooters[i][j].SetColor(shooterColorList[i][j]);
                shooters[i][j].SetCount(shooterCountList[i][j]);
                shooters[i][j].SetHidden(shooterHiddenList[i][j]);
                if (shooterColorList[i][j] < 99)
                {
                    colorCount[shooterColorList[i][j]] += shooterCountList[i][j];
                }
            }
        }
        ToolUI.Instance.UpdateText();
    }
    public void SetColorCode(int colorcode)
    {
        colorCode = colorcode;
    }
    public void SetCount(int cou)
    {
        count = cou; 
    }
    public void Paint(int x,int y)
    {
        if (isTile)
        {
            PaintTile(x, y);
        }
        else PaintShooter(x, y);
        ToolUI.Instance.UpdateText();
    }
    public void Erase(int x,int y)
    {
        if (isTile)
        {
            EraseTile(x, y);
        }
        else EraseShooter(x, y);
        ToolUI.Instance.UpdateText();
    }
    public void SetHidden(int x, int y)
    {
        y = -y;
        if (y + shooterCol / 2 < 0 || y + shooterCol / 2 >= shooterCol || x + shooterRow / 2 < 0 || x + shooterRow / 2 >= shooterRow) return;
        shooters[y + shooterCol / 2][x + shooterRow / 2].SetHidden(!shooters[y + shooterCol / 2][x + shooterRow / 2].GetHidden());
    }

    public void PaintTile(int x,int y)
    {
        if (y + tileCol / 2 <0|| y + tileCol / 2 >= tileCol || x + tileRow / 2 <0|| x + tileRow / 2 >= tileRow) return;
        if(tiles[y + tileCol / 2][x + tileRow / 2].GetColor()==colorCode&&tiles[y + tileCol / 2][x + tileRow / 2].GetCount()==this.count) return;
        int code = tiles[y + tileCol / 2][x + tileRow / 2].GetColor();
        int count = tiles[y + tileCol / 2][x + tileRow / 2].GetCount();
        if (code != 99) colorCount[code]-=count;
        tiles[y+tileCol/2][x+tileRow/2].SetColor(colorCode);
        tiles[y + tileCol / 2][x + tileRow / 2].SetCount(this.count);
        colorCount[colorCode]+=this.count;
    }
    public void EraseTile(int x, int y)
    {
        if (y + tileCol / 2 < 0 || y + tileCol / 2 >= tileCol || x + tileRow / 2 < 0 || x + tileRow / 2 >= tileRow) return;
        if(tiles[y + tileCol / 2][x + tileRow / 2].GetColor()==99) return;
        int code = tiles[y + tileCol / 2][x + tileRow / 2].GetColor();
        int count = tiles[y + tileCol / 2][x + tileRow / 2].GetCount();
        if (code != 99) colorCount[code]-=count;
        tiles[y + tileCol / 2][x + tileRow / 2].SetColor(99);
        tiles[y + tileCol / 2][x + tileRow / 2].SetCount(0);
    }
    public void PaintShooter(int x, int y)
    {
        y = -y;
        if (y + shooterCol / 2 < 0 || y + shooterCol / 2 >= shooterCol || x + shooterRow / 2 < 0 || x + shooterRow / 2 >= shooterRow) return;
        shooters[y + shooterCol / 2][x + shooterRow / 2].SetColor(colorCode);
        shooters[y + shooterCol / 2][x + shooterRow / 2].SetCount(count);
    }
    public void EraseShooter(int x, int y)
    {
        y = -y;
        if (y + shooterCol / 2 < 0 || y + shooterCol / 2 >= shooterCol || x + shooterRow / 2 < 0 || x + shooterRow / 2 >= tileRow) return;
        shooters[y + shooterCol / 2][x + shooterRow / 2].SetColor(99);
        shooters[y + shooterCol / 2][x + shooterRow / 2].SetCount(0);
    }
    Vector3 GenPos(int z,int x)
    {
        return new Vector3((float)x-tileRow/2+0.5f,0, (float)z-tileCol/2+0.5f);
    }
    Vector3 GenShooterPos(int z, int x)
    {
        return new Vector3((float)x - shooterRow / 2 + 0.5f, 0, -(float)z + shooterCol / 2 + 0.5f);
    }


}

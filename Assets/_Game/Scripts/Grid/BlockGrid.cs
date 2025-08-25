using Tech.Singleton;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using System;

public class BlockGrid : Tech.Singleton.Singleton<BlockGrid>
{
    [SerializeField] private int blockLength=13;
    [SerializeField] private int blockWidth=10;
    [SerializeField] private int shooterLength = 5;
    [SerializeField] private int shooterWidth = 5;
    [SerializeField] private int shooterListLength=5;
    [SerializeField] private Block block;
    [SerializeField] private Shooter shooter;
    [SerializeField] private Transform ShooterGrid;
    [SerializeField] private Transform ShooterSpawnPos;
    private List<List<int>> tileColorList = new();
    private List<List<int>> tileCountList = new();
    private List<List<int>> shooterColorList = new();
    private List<List<int>> shooterCountList = new();
    private List<List<bool>> shooterHiddenList = new();

    private List<List<Block>> blocks = new();
    private List<List<Shooter>> shooters = new();
    public List<Shooter> shootersList = new();
    [SerializeField] private int[] pivot= new int[10];

    private void OnValidate()
    {
    }
    
    private void Start()
    {
        TileData tileData = new TileData();
        ShooterData shooterData =new ShooterData();
        (tileData,shooterData) = LevelData.ImportColorList();
        tileColorList = LevelData.ConvertFromIntList(tileData.tileColor);
        tileCountList = LevelData.ConvertFromIntList(tileData.tileCount);
        shooterColorList = LevelData.ConvertFromIntList(shooterData.shooterColor);
        shooterCountList = LevelData.ConvertFromIntList(shooterData.shooterCount);
        shooterHiddenList = LevelData.ConvertFromBoolList(shooterData.shooterHidden);
        blocks.Clear();
        
        for(int i=0; i < shooterListLength; i++)
        {
            shootersList.Add(null);
        }
        for (int i = 0; i < blockLength; i++)
        {
            List<Block> row = new();
            for (int j = 0; j < blockWidth; j++)
            {
                Vector3 vector3 = new Vector3(j+transform.position.x,0, i+transform.position.z);
                Block blockCls = Instantiate(block,vector3,Quaternion.identity,transform);
                blockCls.SetHeight(tileCountList[i][j]);
                blockCls.SetColorID(tileColorList[i][j]);
                blockCls.SpawnVisual();
                pivot[j]++;
                row.Add(blockCls);     
            }
            blocks.Add(row);
        }
        for (int i = 0; i < shooterLength; i++)
        {
            List<Shooter> row = new();
            for (int j = 0; j < shooterWidth; j++)
            {
                Vector3 vector3 = new Vector3(ShooterSpawnPos.position.x+j-shooterWidth/2,0,ShooterSpawnPos.position.z-i+shooterLength/2)*1.5f;
                Shooter shooterCls = Instantiate(shooter, vector3, Quaternion.identity, ShooterSpawnPos);
                shooterCls.SetCount(shooterCountList[i][j]);
                shooterCls.SetColorID(shooterColorList[i][j]);
                shooterCls.SetDarkMode(shooterHiddenList[i][j]);
                row.Add(shooterCls);
            }
            shooters.Add(row);
        }
        for (int i = 0; i < shooterLength; i++)
        {
            if (shooters[0][i] != null) shooters[0][i].SetChosed();
        }
        StartCoroutine(CheckShoot());
    }
    private void Update()
    {
        
    }
    public IEnumerator CheckShoot()
    {
        WaitForSeconds waitForSeconds = new(0.02f);
        while (true)
        {
            for (int i = 0; i < blockWidth; i++)
            {
                if (blocks[0][i] != null)
                {
                    int id = blocks[0][i].GetColorID();
                    int min = int.MaxValue;
                    Shooter chose = null;
                    foreach (Shooter shooter in shootersList)
                    {
                        if (shooter == null) continue;
                        if (shooter.GetColorID() == id)
                        {
                            if (min > shooter.GetBulletCount())
                            {
                                min = shooter.GetBulletCount();
                                chose = shooter;
                            }
                        }
                    }
                    if (chose != null)
                    {
                        chose.Shoot(i);
                        yield return waitForSeconds;
                    }
                }
            }
            yield return null;

        }
    }
    public bool AddShooter(Shooter shooter)
    {
        int d = 0;
        for (int i= 0; i<shooterListLength;i++)
        {
            if (shootersList[i] == null)
            {
                d= i;
                shootersList[i] = shooter;
                shootersList[i].MoveTo(ShooterGrid.position + new Vector3((d - (float)shooterListLength / 2 + 0.5f) * 1.5f, 0, 0));
                return true;
            }
        }
        return false;
    }
    public void RemoveShooter(Shooter shooter)
    {
        for (int i = 0; i < shooterListLength; i++)
        {
            if (shootersList[i] == shooter)
            {
                shootersList[i] = null;
                return;
            }
        }
    }

    public int FindColorID(int colorID)
    {
        return -1;
    }
    public void UpdateShooter(Shooter shooter)
    {
        int col=default;
        for(int i=0;i<shooterLength;i++)
        {
            for(int j = 0; j < shooterWidth; j++)
            {
                if (shooter == shooters[i][j])
                {
                    shooters[i][j] = null;
                    col= j; break;
                }
            }
            if (col != default) break;
        }
        for (int i = 0; i < shooterLength; i++)
        {
            if (i != shooterLength - 1)
                shooters[i][col] = shooters[i + 1][col];
            else shooters[i][col] = null;
            if (shooters[i][col] != null) shooters[i][col].MoveTo(GetShooterPos(i, col));
        }
        
        if (shooters[0][col] != null && shooters[0][col].GetCount() == 0)
            shooters[0][col] = null;
        for (int i = 0; i < shooterLength; i++)
        {
            if (shooters[0][i] != null) shooters[0][i].SetChosed();
        }
    }
    public void Shoot(int col)
    {
        if(blocks[0][col]==null || !blocks[0][col].Shoot()) return;
        for(int i = 0; i < blockLength;i++)
        {
            if(i!=blockLength-1)
            blocks[i][col] = blocks[i + 1][col];
            else blocks[i][col] = null;
            if (blocks[i][col] != null) blocks[i][col].MoveTo(GetPos(i, col), blocks[i][col].GetHeight());
        }
        if (blocks[0][col]!=null&& blocks[0][col].GetHeight() == 0)
            blocks[0][col] = null;
        if (pivot[col] >= tileColorList.Count) return;
        blocks[blockLength - 1][col] = SpawnBlock(blockLength - 1, col);
        pivot[col]++;
    }
    private Block SpawnBlock(int i,int j)
    {
        Vector3 vector3 = new Vector3(j + transform.position.x, 0, i + transform.position.z);
        Block blockCls = Instantiate(block, vector3, Quaternion.identity, transform);
        blockCls.SetHeight(tileCountList[pivot[j]][j]);
        blockCls.SetColorID(tileColorList[pivot[j]][j]);
        blockCls.SpawnVisual();
        return blockCls;
    }
    private Vector3 GetPos(int i,int j)
    {
        Vector3 vector3 = new Vector3(j + transform.position.x, 0, i + transform.position.z);
        return vector3;
    }
    private Vector3 GetShooterPos(int i,int j)
    {
        Vector3 vector3 = new Vector3(ShooterSpawnPos.position.x + j - shooterWidth / 2, 0, ShooterSpawnPos.position.z - i + shooterLength / 2) * 1.5f;
        return vector3;
    }
}

using Tech.Singleton;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using System;

public class GridManager : Tech.Singleton.Singleton<GridManager>
{
    [SerializeField] private int blockLength = 13;
    [SerializeField] private int blockWidth = 10;
    [SerializeField] private int shooterLength = 5;
    [SerializeField] private int shooterWidth = 5;
    [SerializeField] private int shooterListLength = 5; //Stack
    [SerializeField] private Block block;
    [SerializeField] private Shooter shooter;
    [SerializeField] private Transform ShooterGrid;
    [SerializeField] private Transform ShooterSpawnPos;
    public List<Shooter> shootersList = new();

    private BlockManager blockManager;
    private ShooterManager shooterManager;
    private void OnValidate()
    {
    }

    private void Start()
    {
        TileData tileData = new TileData();
        ShooterData shooterData = new ShooterData();
        (tileData, shooterData) = LevelData.ImportColorList();
        blockManager = new BlockManager(blockLength, blockWidth, block, tileData, this.transform);
        shooterManager = new ShooterManager(shooterLength, shooterWidth, shooter, shooterData, ShooterSpawnPos);
        for (int i = 0; i < shooterListLength; i++)
        {
            shootersList.Add(null);
        }
        StartCoroutine(blockManager.CheckShoot(shootersList));
    }    
    public void CheckTripple()
    {
        int[] count = new int[9];
        for (int i = 0; i < shootersList.Count; i++)
        {
            if (shootersList[i] == null) continue;
            int ID = shootersList[i].GetColorID();
            count[ID]++;
            if (count[ID] == 3)
            {
                int dem = 0;
                for (int j = 0; j < shooterLength; j++)
                {
                    if (shootersList[j].GetColorID() == ID)
                    {
                        dem++;
                    }
                    if (dem == 2)
                    {
                        Merge(j, ID);
                        break;
                    }
                }
                break;
            }
        }
    }
    public void Merge(int pos, int id)
    {
        int count = 0;
        for (int i = 0; i < shooterLength; i++)
        {
            if (shootersList[i] == null) continue;
            if (i != pos && shootersList[i].GetColorID() == id)
            {
                count += shootersList[i].Clear();
            }
        }
        shootersList[pos].AddCount(count);
    }
    public bool AddShooter(Shooter shooter)
    {
        int d = 0;
        for (int i = 0; i < shooterListLength; i++)
        {
            if (shootersList[i] == null)
            {
                d = i;
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
    public void UpdateShooter(Shooter shooter)
    {
        shooterManager.UpdateShooter(shooter);
        CheckTripple();
    }
    public void Shoot(int value)
    {
        blockManager.Shoot(value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterManager
{
    private int shooterLength = 5;
    private int shooterWidth = 5;
    private Shooter shooter;
    private Transform parent;
    private List<List<int>> shooterColorList = new();
    private List<List<int>> shooterCountList = new();
    private List<List<bool>> shooterHiddenList = new();
    private List<List<Shooter>> shooters = new();
    
    public ShooterManager(int lenght,int width,Shooter shooter, ShooterData shooterData, Transform spawnPos)
    {
        shooterColorList = LevelData.ConvertFromIntList(shooterData.shooterColor);
        shooterCountList = LevelData.ConvertFromIntList(shooterData.shooterCount);
        shooterHiddenList = LevelData.ConvertFromBoolList(shooterData.shooterHidden);
        this.shooter = shooter;
        this.parent = spawnPos;
        for (int i = 0; i < shooterLength; i++)
        {
            List<Shooter> row = new();
            for (int j = 0; j < shooterWidth; j++)
            {
                Vector3 vector3 = new Vector3(spawnPos.position.x + j - shooterWidth / 2, 0, spawnPos.position.z - i + shooterLength / 2) * 1.5f;
                Shooter shooterCls = Object.Instantiate(shooter, vector3, Quaternion.identity, spawnPos);
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
    }
    public void UpdateShooter(Shooter shooter)
    {
        int col = default;
        for (int i = 0; i < shooterLength; i++)
        {
            for (int j = 0; j < shooterWidth; j++)
            {
                if (shooter == shooters[i][j])
                {
                    shooters[i][j] = null;
                    col = j; break;
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
    private Vector3 GetShooterPos(int i, int j)
    {
        Vector3 vector3 = new Vector3(parent.position.x + j - shooterWidth / 2, 0, parent.position.z - i + shooterLength / 2) * 1.5f;
        return vector3;
    }
}

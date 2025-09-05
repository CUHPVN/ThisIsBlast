using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager
{
    private int blockLength = 13;
    private int blockWidth = 10;
    private Block block;
    private Transform parent;
    private List<List<int>> tileColorList = new();
    private List<List<int>> tileCountList = new();
    private List<List<Block>> blocks = new();
    private int[] pivot = new int[10];

    public BlockManager(int lenght, int width, Block block, TileData blockData, Transform parent)
    {
        tileColorList = LevelData.ConvertFromIntList(blockData.tileColor);
        tileCountList = LevelData.ConvertFromIntList(blockData.tileCount);
        this.block = block;
        this.parent = parent;
        blocks.Clear();
        for (int i = 0; i < blockLength; i++)
        {
            List<Block> row = new();
            for (int j = 0; j < blockWidth; j++)
            {
                Vector3 vector3 = new Vector3(j + parent.position.x, 0, i + parent.position.z);
                Block blockCls = Object.Instantiate(block, vector3, Quaternion.identity, parent);
                blockCls.SetHeight(tileCountList[i][j]);
                blockCls.SetColorID(tileColorList[i][j]);
                blockCls.SpawnVisual();
                pivot[j]++;
                row.Add(blockCls);
            }
            blocks.Add(row);
        }
    }

    public IEnumerator CheckShoot(List<Shooter> shootersList)
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
    public void Shoot(int col)
    {
        if (blocks[0][col] == null || !blocks[0][col].Shoot()) return;
        for (int i = 0; i < blockLength; i++)
        {
            if (i != blockLength - 1)
                blocks[i][col] = blocks[i + 1][col];
            else blocks[i][col] = null;
            if (blocks[i][col] != null) blocks[i][col].MoveTo(GetPos(i, col), blocks[i][col].GetHeight());
        }
        if (blocks[0][col] != null && blocks[0][col].GetHeight() == 0)
            blocks[0][col] = null;
        if (pivot[col] >= tileColorList.Count) return;
        blocks[blockLength - 1][col] = SpawnBlock(blockLength - 1, col);
        pivot[col]++;
    }
    private Vector3 GetPos(int i, int j)
    {
        Vector3 vector3 = new Vector3(j + parent.position.x, 0, i + parent.position.z);
        return vector3;
    }
    private Block SpawnBlock(int i, int j)
    {
        Vector3 vector3 = new Vector3(j + parent.position.x, 0, i + parent.position.z);
        Block blockCls = Object.Instantiate(block, vector3, Quaternion.identity, parent);
        blockCls.SetHeight(tileCountList[pivot[j]][j]);
        blockCls.SetColorID(tileColorList[pivot[j]][j]);
        blockCls.SpawnVisual();
        return blockCls;
    }
}

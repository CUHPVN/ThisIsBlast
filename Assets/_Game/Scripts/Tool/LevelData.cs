using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class IntList
{
    public List<int> values;
}
[System.Serializable]
public class BoolList
{
    public List<bool> values;
}
[System.Serializable]
public class LevelWrapper
{
    public TileData tileData;
    public ShooterData shooterData;
}
[System.Serializable]
public class TileData
{
    public List<IntList> tileColor;
    public List<IntList> tileCount;
}
[System.Serializable]
public class ShooterData
{
    public List<IntList> shooterColor;
    public List<IntList> shooterCount;
    public List<BoolList> shooterHidden;
}
public static class LevelData
{
    static string path = Path.Combine(Application.streamingAssetsPath, "colorList.json");

    public static List<IntList> ConvertToIntList(List<List<int>> source)
    {
        List<IntList> result = new List<IntList>();
        foreach (var subList in source)
        {
            result.Add(new IntList { values = subList });
        }
        return result;
    }
    public static List<List<int>> ConvertFromIntList(List<IntList> source)
    {
        var result = new List<List<int>>();
        foreach (var intList in source)
        {
            result.Add(new List<int>(intList.values));
        }
        return result;
    }
    public static List<BoolList> ConvertToBoolList(List<List<bool>> source)
    {
        var result = new List<BoolList>();
        foreach (var subList in source)
        {
            result.Add(new BoolList { values = new List<bool>(subList) });
        }
        return result;
    }

    public static List<List<bool>> ConvertFromBoolList(List<BoolList> source)
    {
        var result = new List<List<bool>>();
        foreach (var boolList in source)
        {
            result.Add(new List<bool>(boolList.values));
        }
        return result;
    }

    public static void ExportColorList(TileData tileData,ShooterData shooterData)
    {
        LevelWrapper wrapper = new LevelWrapper();
        wrapper.tileData = tileData;
        wrapper.shooterData= shooterData;
        Debug.Log(wrapper.tileData.tileColor.Count);
        string json = JsonUtility.ToJson(wrapper, false);
        File.WriteAllText(path, json);
    }

    public static (TileData,ShooterData) ImportColorList()
    {
        //path = Application.dataPath + "/colorList.json";
        if (!File.Exists(path))
        {
            Debug.LogError("File not found: " + path);
            return (null,null);
        }

        string json = File.ReadAllText(path);
        LevelWrapper wrapper = JsonUtility.FromJson<LevelWrapper>(json);

        TileData tileData = wrapper.tileData;
        ShooterData shooterData = wrapper.shooterData;
        

        return (tileData,shooterData) ;
    }
}

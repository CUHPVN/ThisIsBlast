using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorPallet
{
    public static Color GetColorByID(int id)
    {
        switch (id)
        {
            case 0:
                return Color.red;
            case 1:
                return Color.green;
            case 2:
                return Color.blue;
            case 3:
                return Color.yellow;
            case 4:
                return Color.cyan;
            case 5:
                return Color.magenta;
            case 6:
                return Color.gray;
            case 7:
                return Color.black;
            case 8:
                return Color.white;
        }
        return Color.clear;
    }
}

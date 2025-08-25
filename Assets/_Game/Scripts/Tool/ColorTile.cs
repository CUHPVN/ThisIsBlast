using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorTile : MonoBehaviour
{
    public int colorCode=99;
    public MeshRenderer mesh;
    public TMP_Text countText;
    public int count;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public int GetColor() => colorCode;
    public int GetCount() => count;
    public void SetColor(int id)
    {
        colorCode = id;
        mesh.material.color = ColorPallet.GetColorByID(colorCode);
    }
    public void SetCount(int id)
    {
        count = id;
        countText.text= count.ToString();
    }
}

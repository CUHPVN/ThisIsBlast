using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorShooter : MonoBehaviour
{
    public int colorCode = 99;
    public MeshRenderer mesh;
    public TMP_Text countText;
    public int count;
    public bool isHidden=false;
    [SerializeField] private Shader darkShader;
    [SerializeField] private Shader lightShader;
    void Awake()
    {
        lightShader = Shader.Find("Universal Render Pipeline/Lit");
    }
    void Update()
    {

    }
    public void SetColor(int id)
    {
        colorCode = id;
        mesh.material.color = ColorPallet.GetColorByID(colorCode);
    }
    public void SetCount(int id)
    {
        count = id;
        countText.text = count.ToString();
    }
    public void SetHidden(bool hidden)
    {
        isHidden = hidden;
        if (isHidden)
        {
            mesh.material.shader =darkShader;
        }
        else
        {
            mesh.material.shader = lightShader;
        }
    }
    public bool GetHidden() => isHidden;
}

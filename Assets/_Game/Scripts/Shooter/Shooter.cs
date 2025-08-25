using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private int bulletCount = 20;
    [SerializeField] private int colorID = 0;
    [SerializeField] private bool Chosed = true;
    [SerializeField] public bool darkMode = true;
    [SerializeField] private SkinnedMeshRenderer MeshRenderer;
    [SerializeField] private Shader darkShader;
    [SerializeField] private Shader lightShader;
    [SerializeField] private TMP_Text countText;
    public void Awake()
    {
        lightShader= Shader.Find("Universal Render Pipeline/Lit");
    }
    public void Start()
    {
        MeshRenderer.material.color = ColorPallet.GetColorByID(colorID);
        countText.text = bulletCount.ToString();
        
    }
    public void Chose()
    {
        if (Chosed|| !BlockGrid.Instance.AddShooter(this)) return;
        BlockGrid.Instance.UpdateShooter(this);
        Chosed=true;
    }
    public void Shoot(int col)
    {
        BlockGrid.Instance.Shoot(col);
        bulletCount--;
        countText.text = bulletCount.ToString();
        if (bulletCount == 0)
        {
            BlockGrid.Instance.RemoveShooter(this);
            transform.gameObject.SetActive(false);
        }
    }
    public void SpawnVisual()
    {

    }
    public void MoveTo(Vector3 pos)
    {
        Vector3 vector = new Vector3(pos.x, 0, pos.z);
        transform.DOMove(vector, 0.5f);
    }
    public bool GetChosed() => this.Chosed;
    public void SetDarkMode(bool value)
    {
        darkMode = value;
        if (darkMode)
        {
            MeshRenderer.material.shader = darkShader;

            countText.gameObject.SetActive(false);
        }
    }
    public void SetChosed()
    {
        Chosed = false;
        if(darkMode)
        {
            MeshRenderer.material.shader= lightShader;
            MeshRenderer.material.color = ColorPallet.GetColorByID(colorID);

            countText.gameObject.SetActive(true);
            darkMode=false;
        }
    }
    public int GetColorID() => this.colorID;
    public void SetColorID(int colorID) { this.colorID = colorID; }
    public int GetCount() => this.bulletCount;
    public void SetCount(int count) { this.bulletCount = count; }
    public int GetBulletCount()
     { return bulletCount; }
}

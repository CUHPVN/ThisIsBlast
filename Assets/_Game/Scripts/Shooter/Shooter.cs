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
    [SerializeField] public bool hiddenMode = true;
    [SerializeField] private SkinnedMeshRenderer MeshRenderer;
    [SerializeField] private Material hiddenMaterial;
    [SerializeField] private Material normalMaterial;

    [SerializeField] private TMP_Text countText;
    public void Awake()
    {
    }
    public void Start()
    {
        MeshRenderer.material.color = ColorPallet.GetColorByID(colorID);
        countText.text = bulletCount.ToString();
    }
    public void Chose()
    {
        if (Chosed|| !GridManager.Instance.AddShooter(this)) return;
        GridManager.Instance.UpdateShooter(this);
        Chosed=true;
    }
    public void Shoot(int col)
    {
        GridManager.Instance.Shoot(col);
        bulletCount--;
        countText.text = bulletCount.ToString();
        if (bulletCount == 0)
        {
            GridManager.Instance.RemoveShooter(this);
            transform.gameObject.SetActive(false);
        }
    }
    public int Clear()
    {
        int count = bulletCount;
        bulletCount=0;
        countText.text = bulletCount.ToString();
        GridManager.Instance.RemoveShooter(this);
        transform.gameObject.SetActive(false);
        return count;
    }
    public void AddCount(int value)
    {
        bulletCount += value;
        countText.text = bulletCount.ToString();
    }
    public void MoveTo(Vector3 pos)
    {
        Vector3 vector = new Vector3(pos.x, 0, pos.z);
        transform.DOMove(vector, 0.5f);
    }
    public bool GetChosed() => this.Chosed;
    public void SetDarkMode(bool value)
    {
        hiddenMode = value;
        if (hiddenMode)
        {
            MeshRenderer.material = hiddenMaterial;

            countText.gameObject.SetActive(false);
        }
    }
    public void SetChosed()
    {
        Chosed = false;
        if(hiddenMode)
        {
            MeshRenderer.material= normalMaterial;
            MeshRenderer.material.color = ColorPallet.GetColorByID(colorID);

            countText.gameObject.SetActive(true);
            hiddenMode=false;
        }
    }
    public int GetColorID() => this.colorID;
    public void SetColorID(int colorID) { this.colorID = colorID; }
    public int GetCount() => this.bulletCount;
    public void SetCount(int count) { this.bulletCount = count; }
    public int GetBulletCount()
     { return bulletCount; }
}

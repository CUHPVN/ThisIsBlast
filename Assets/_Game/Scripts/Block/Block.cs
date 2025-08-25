using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Block : MonoBehaviour
{
    [SerializeField] private BlockVisual blockVisual;
    [SerializeField] private List<BlockVisual> visual;
    [SerializeField] private int height=1;
    [SerializeField] private int colorID = 0;   
    [SerializeField] private Color color = Color.white;

    public void Awake()
    {
        color = ColorPallet.GetColorByID(colorID);
    }
    public void SetColor(Color color)
    {
        this.color = color;
    }
    public void SetColorID(int colorID)
    {
        this.colorID = colorID;
        color = ColorPallet.GetColorByID(colorID);
    }
    public void SetHeight(int height)
    {
        this.height = height;
    }
    public void AddVisual(BlockVisual gameObject)
    {
        visual.Add(gameObject);
    }
    public void SpawnVisual()
    {
        for (int i=0; i<height; i++)
        {
            Vector3 vector = new Vector3(transform.position.x, i, transform.position.z);
            BlockVisual newblockVisual = Instantiate(blockVisual,vector,Quaternion.identity,transform);
            newblockVisual.transform.parent = this.transform;
            newblockVisual.ChangeColor(color);
            visual.Add(newblockVisual);
        }
    }
    public bool Shoot()
    {
        if (height >= 1)
        {
            BlockVisual last = visual[visual.Count - 1];
            visual.Remove(last);
            last.animator.SetBool("isShooted", true);
            height--;
            if(height == 0)
            {
                StartCoroutine(Destroy());
                return true;
            }
        }
            return false;
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this);
    }
    public Color GetColor() => this.color;
    public int GetColorID() => this.colorID;
    public int GetHeight() => this.height;
    public List<BlockVisual> GetVisual() => this.visual;
    public void MoveTo(Vector3 pos,int k)
    {
        for(int i = 0; i < k; i++)
        {
            Vector3 vector = new Vector3(pos.x,i,pos.z);
            visual[i].transform.DOMove(vector, 0.5f);
        }
    }
}

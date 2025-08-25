using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockVisual : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Animator animator;
    MaterialPropertyBlock mpb;
    public void ChangeColor(Color color)
    {
        mpb = new MaterialPropertyBlock();

        skinnedMeshRenderer.GetPropertyBlock(mpb);

        mpb.SetColor("_BaseColor", color);

        skinnedMeshRenderer.SetPropertyBlock(mpb);
    }
}

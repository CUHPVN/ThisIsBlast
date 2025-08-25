using System;
using UnityEngine;

public class SetWorldBounds : MonoBehaviour
{
    private void Awake()
    {
        var bounds = GetComponent<MeshRenderer>().bounds;
        Globals.WorldBounds = bounds;
    }
}
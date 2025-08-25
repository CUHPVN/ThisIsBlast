using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem instance;
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private GameObject cursoIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] PaintMode paintMode = PaintMode.Erase;
    public enum PaintMode
    {
        Erase,
        Paint,
        Hidden,
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetMouseColor(0);
    }
    void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cursoIndicator.transform.position = grid.CellToWorld(gridPosition);
        if (Input.GetMouseButton(0))
        {
            OnClick((int)cursoIndicator.transform.position.x,(int)cursoIndicator.transform.position.z);
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnClickOnce((int)cursoIndicator.transform.position.x, (int)cursoIndicator.transform.position.z);
        }
    }
    void OnClick(int x,int z)
    {
        if (paintMode==PaintMode.Paint)
        {
            GridParent.Instance.Paint(x,z);
        }
        if (paintMode==PaintMode.Erase)
        {
            GridParent.Instance.Erase(x, z);
        }
    }
    void OnClickOnce(int x, int z)
    {
        if (paintMode == PaintMode.Hidden)
        {
            GridParent.Instance.SetHidden(x, z);
        }
    }
    public void SetPaint(PaintMode Paint)
    {
        paintMode = Paint;
    }
    public void SetMouseColor(int id)
    {
        cursoIndicator.GetComponentInChildren<MeshRenderer>().material.color = ColorPallet.GetColorByID(id);
    }
}

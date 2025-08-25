using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolUI : MonoBehaviour
{
    public static ToolUI Instance { get; private set; }
    public GameObject colorPaleteUI;
    public GameObject colorButtonPrefab;
    public GameObject colorButtonParent;
    public TMP_InputField inputField;
    public List<TMP_Text> colorCount;
    private void Awake()
    {
        Instance=this;
    }

    void Start()
    {
        GenColor();
        LoadComponent();
    }
    void LoadComponent()
    {
        inputField.onEndEdit.AddListener(
             value => UpdateCount(int.Parse(value))
         );
    }
    void UpdateCount(int value)
    {
        GridParent.Instance.SetCount(value);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void GenColor()
    {
        for(int i = 0; i < 9; i++)
        {
            GameObject gameObject= Instantiate(colorButtonPrefab);
            gameObject.GetComponent<Image>().color = ColorPallet.GetColorByID(i);
            int tmp = i;
            gameObject.GetComponent<Button>().onClick.AddListener(()=>SetPaintColor(tmp));
            colorCount.Add(gameObject.GetComponentInChildren<TMP_Text>());
            gameObject.transform.parent = colorButtonParent.transform;
        }
    }
    private void SetPaintColor(int color)
    {
        GridParent.Instance.SetColorCode(color);
        PlacementSystem.instance.SetMouseColor(color);
    }
    public void UpdateCount()
    {
        if (!colorPaleteUI.activeSelf) return;

    }
    public void UpdateText()
    {
        for (int i = 0;i<colorCount.Count;i++)
        {
            colorCount[i].text = GridParent.Instance.colorCount[i].ToString();
        }
    }
    public void SetColorPalete()
    {
        colorPaleteUI.SetActive(!colorPaleteUI.activeSelf);
    }
    public void SetPaint(int paint)
    {
        PlacementSystem.instance.SetPaint(PlacementSystem.PaintMode.Erase+paint);
    }
    public void Import()
    {
        GridParent.Instance.Import(LevelData.ImportColorList());
    }
    public void Export()
    {
        GridParent.Instance.Export();
    }
    public void SetDrawMode(bool value)
    {
        GridParent.Instance.SetDrawMode(value);
    }
}

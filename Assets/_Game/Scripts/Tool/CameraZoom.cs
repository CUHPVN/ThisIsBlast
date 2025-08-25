using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float zoomSmoothTime = 0.1f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 7f;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private GameInput gameInput;

    private Camera cam;
    private float targetZoom = 5f;
    private float zoomVelocity = 0f;
    private void Awake()
    {
        cam = Camera.main;
        LoadComponent();
    }
    private void Start()
    {
        cameraMovement = GetComponent<CameraMovement>();
    }
    private void LoadComponent()
    {
        targetZoom = 5f;
    }
    public void Update()
    {
        float scroll = gameInput.GetScrollY();
        if (scroll != 0f)
        {
            targetZoom -= scroll * zoomSpeed/10f;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
        if (Mathf.Abs(cam.orthographicSize - targetZoom) > 0.01f)
        {
            cam.orthographicSize = Mathf.SmoothDamp(
                cam.orthographicSize,
                targetZoom,
                ref zoomVelocity,
                zoomSmoothTime
            );

            //cameraMovement.Calculate();
            //cameraMovement.FixCamera();
        }
    }

   


 
}

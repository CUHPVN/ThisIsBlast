using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    #region Variables

    private Vector3 _origin;
    private Vector3 _difference;

    private Camera _mainCamera;

    private bool _isDragging;

    private Bounds _cameraBounds;
    private Vector3 _targetPosition;
    [SerializeField] private GameInput gameInput;

    #endregion

    private void Awake() => _mainCamera = Camera.main;

    private void Start()
    {
        gameInput.OnDragAction += GameInput_OnDragAction;
        gameInput.EndDragAction += GameInput_EndDragAction;
        Calculate();
    }
    public void Calculate()
    {
        var height = _mainCamera.orthographicSize;
        var width = height * _mainCamera.aspect;

        var minX = Globals.WorldBounds.min.x + width;
        var maxX = Globals.WorldBounds.extents.x - width;

        //var minY = Globals.WorldBounds.min.y + height;
        //var maxY = Globals.WorldBounds.extents.y - height;
        var minY = Globals.WorldBounds.min.z + height;
        var maxY = Globals.WorldBounds.extents.z - height;

        _cameraBounds = new Bounds();
        _cameraBounds.SetMinMax(
            //new Vector3(minX, minY, 0.0f),
            //new Vector3(maxX, maxY, 0.0f)
            new Vector3(minX,0.0f, minY),
            new Vector3(maxX,0.0f, maxY)
            );
    }
    public void FixCamera()
    {
        _difference = transform.position - transform.position;
        _targetPosition = transform.position - _difference;
        _targetPosition = GetCameraBounds();
        transform.position = _targetPosition;
    }
    public void Return()
    {
        transform.position = new Vector3(0, transform.position.y, 0);
    }
    public void GameInput_OnDragAction(object sender, System.EventArgs e)
    {
        _origin = GetMousePosition;
        _isDragging = true;
    }
    public void GameInput_EndDragAction(object sender, System.EventArgs e)
    {
        _isDragging = false;
    }
    private void LateUpdate()
    {
        if (!_isDragging) return;
        _difference = GetMousePosition - transform.position;
        _targetPosition = _origin-_difference;
        _targetPosition = GetCameraBounds();
        transform.position = ChangeYtoZ(_targetPosition);
    }

    private Vector3 GetCameraBounds()
    {
        return new Vector3(
            Mathf.Clamp(_targetPosition.x, _cameraBounds.min.x, _cameraBounds.max.x),
            Mathf.Clamp(_targetPosition.z, _cameraBounds.min.z, _cameraBounds.max.z),
            transform.position.y
        //transform.position.z
        );
    }
    private Vector3 ChangeYtoZ( Vector3 pos)
    {
        return new Vector3(pos.x,pos.z,pos.y);
    }

    private Vector3 GetMousePosition => _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}
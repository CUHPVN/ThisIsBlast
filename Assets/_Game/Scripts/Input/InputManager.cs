using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayermask;
    [SerializeField] private LayerMask shoterLayermask;

    [SerializeField] private Vector3 lastPosition;
   
    public Vector3 GetSelectedMapPosition()
    {
        Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
    public Shooter GetShooter()
    {
        Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, shoterLayermask))
        {
            return hit.collider.GetComponent<Shooter>();
        }
        return null;
    }
    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shooter shooter = GetShooter();
            if (shooter != null)
            {
                shooter.Chose();
            }
        }
    }
}

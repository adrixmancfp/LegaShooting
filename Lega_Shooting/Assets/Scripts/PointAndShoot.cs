using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndShoot : MonoBehaviour
{
    public GameObject crosshair;
    
    private Vector3 target;

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4));
        crosshair.transform.position = new Vector3(target.x, target.y, target.z);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;


    private void Update()
    {
        //currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomspeed;
        //currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    //    yawInput -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;

    }
    void LateUpdate()
    {
        transform.position = target.position + offset;
     //   transform.LookAt(target.position + Vector3.up * pitch);


        //transform.RotateAround(target.position, Vector3.up, yawInput);
    }
    private void OnValidate()
    {
        transform.position = target.transform.position + offset;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(target.transform.position + offset, .5f);
    }

}

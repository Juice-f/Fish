using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [SerializeField] public float cameraRotation = 0;
    [SerializeField] public float xRotation = 0;

    private void Update()
    {
        //currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomspeed;
        //currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        //    yawInput -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        cameraRotation += Input.GetAxis("Mouse ScrollWheel") * 60;
    }
    void LateUpdate()
    {
        transform.position = target.position + Quaternion.Euler(0, cameraRotation, 0) * offset;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraRotation, transform.eulerAngles.z);
        
     //   transform.LookAt(target.position + Vector3.up * pitch);


        //transform.RotateAround(target.position, Vector3.up, yawInput);
    }
    private void OnValidate()
    {
        transform.position = target.transform.position + Quaternion.Euler(0,cameraRotation,0) *  offset;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraRotation, transform.eulerAngles.z);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(target.transform.position + offset, .5f);
    }

}

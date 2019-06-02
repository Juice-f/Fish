using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ProbeMove : MonoBehaviour
{
    float floorHeight;
    GameObject player;
    private void Update()
    {
        player = FindObjectOfType<playercontroller>().gameObject;
        float floorHeight = player.transform.position.y - player.GetComponent<NavMeshAgent>().baseOffset;
        float cameraDistFromGround = Mathf.Abs(Camera.main.transform.position.y- floorHeight);
        transform.position = new Vector3(Camera.main.transform.position.x, floorHeight-cameraDistFromGround, Camera.main.transform.position.z);
    }


    private void OnValidate()
    {
        player = FindObjectOfType<playercontroller>().gameObject;
        float floorHeight = player.transform.position.y - player.GetComponent<NavMeshAgent>().baseOffset;
        float cameraDistFromGround = Mathf.Abs(Camera.main.transform.position.y - floorHeight);
        transform.position = new Vector3(Camera.main.transform.position.x, floorHeight - cameraDistFromGround, Camera.main.transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector3(player.transform.position.x, player.transform.position.y - player.GetComponent<NavMeshAgent>().baseOffset, player.transform.position.z), Vector3.one);
    }

}

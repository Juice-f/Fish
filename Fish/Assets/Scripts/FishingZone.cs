using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FishingZone : MonoBehaviour
{
    [SerializeField] public float fishingZoneRadius = 10;
    [SerializeField] public Vector3 fishingZoneCenterOffset = new Vector3(0, 0, 0);

    public Vector3 FishCenter { get => transform.position + fishingZoneCenterOffset; }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + fishingZoneCenterOffset, -fishingZoneRadius);
    }

}

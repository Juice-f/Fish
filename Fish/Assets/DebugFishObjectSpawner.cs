using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFishObjectSpawner : MonoBehaviour
{
    [SerializeField]
    Object[] objects;
    private void Start()
    {

        objects = Resources.LoadAll("Fish", typeof(FishInfo));

        foreach (FishInfo item in objects)
        {
            Instantiate(item.caughtObject, Vector3.zero, Quaternion.identity, transform);
        }

    }
}

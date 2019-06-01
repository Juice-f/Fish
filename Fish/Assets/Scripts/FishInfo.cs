using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new fish", menuName = "Create new fish", order = 0)]
public class FishInfo : ScriptableObject
{
    public float probabilityWeight = .1f;
    public string fishname = "John Standardfish";
    public GameObject caughtObject;



}

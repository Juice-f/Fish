using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new fish", menuName = "Create new fish", order = 0)]
public class FishInfo : ScriptableObject
{
    public float probabilityWeight = .1f;
    public string fishname = "John Standardfish";
    public GameObject caughtObject;
    public float passiveDrain = 3;
    public FishAttackDamage diveAttackDamage;
    public FishAttackDamage yeetAttackDamage;
    public float maxFishStamina = 50;
    public AudioClip song;
    public float fishRegen = 1;

}
[System.Serializable]
public class FishAttackDamage
{
    public float stmnDamage = 1;
    public float lineDamage = 1;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClub", menuName = "Clubs/ClubScriptableObject")]
public class Club : ScriptableObject
{
    public string clubName;
    public int[] dates;
    public int[] times;
    public string description;
}
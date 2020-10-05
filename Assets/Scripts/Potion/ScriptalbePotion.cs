using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Potions/Potion")]
public class ScriptalbePotion : ScriptableObject
{
    public string Name;
    public string description;

   // public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Material[] material;

    public Sprite icon;
    public int tier;

    public Stats per_stats;

    public bool has_temp_effects;
    public Stats temp_stats;
    public int effectTime;
    public int coolDownTime;


    public Stats GetTempStats()
    { return temp_stats; }

    public Stats GetPerStats()
    { return per_stats; }

    public int GetTime()
    { return effectTime; }
}

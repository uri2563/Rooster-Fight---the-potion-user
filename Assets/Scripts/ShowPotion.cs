using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPotion : MonoBehaviour
{
    public ScriptalbePotion scriptalbePotion;
    // Start is called before the first frame update
    void Start()
    {
        SetPotionPrefab();
    }

    public bool SetPotionPrefab()
    {
        this.gameObject.name = scriptalbePotion.name;

        this.GetComponentInChildren<MeshFilter>().sharedMesh = scriptalbePotion.meshFilter.sharedMesh;
        this.GetComponentInChildren<MeshRenderer>().sharedMaterials = scriptalbePotion.material;

        return true;
    }
}

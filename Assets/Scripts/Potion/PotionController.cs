using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshFilter))]
public class PotionController : MonoBehaviour
{
    private ScriptalbePotion scriptalbePotion;
    public ParticleSystem shatteredGlass;

    public ScriptalbePotion GetScriptablePotion()
    {
        return scriptalbePotion;
    }

    public bool SetPotionPrefab(ScriptalbePotion _scriptalbePotion)
    {
        scriptalbePotion = _scriptalbePotion;
        this.gameObject.name = scriptalbePotion.name;

        this.GetComponentInChildren<MeshFilter>().sharedMesh = scriptalbePotion.meshFilter.sharedMesh;
        this.GetComponentInChildren<MeshRenderer>().sharedMaterials = scriptalbePotion.material;

        return true;
    }

    void OnCollisionEnter(Collision collision)
    {
        ParticleSystem prefabParticals = Instantiate(shatteredGlass, transform.position, Quaternion.identity);
        AudioManager.Instance.Play("Glass");

        Destroy(prefabParticals.gameObject, 1f);
        Destroy(this.gameObject);
    }
}

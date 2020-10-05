using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionBuff : MonoBehaviour
{
    public ChickenController chicken;
    public string buffName = "";
    private ScriptalbePotion scriptalbePotion;

    public ParticleSystem shatteredGlass;

    public float timer;

    //public TextMeshProUGUI timerText;
    public Image timerImage;

    private float effectTime;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        //timerText.text = "" + Math.Round(timer, 1);
        timerImage.fillAmount -= 1.0f / effectTime * Time.deltaTime;

        //face cam

        if ((!buffName.Equals("")) && (0f >= timer))
        {
            EndBuff();
        }
    }

    public void StartBuff(ScriptalbePotion _scriptalbePotion)
    {
        scriptalbePotion = _scriptalbePotion;

        if(scriptalbePotion.has_temp_effects)
        {
            effectTime = scriptalbePotion.GetTime();
            timerImage.fillAmount = 1f;
            timer = scriptalbePotion.GetTime();

            buffName = scriptalbePotion.Name;

            this.GetComponentInChildren<MeshFilter>().sharedMesh = scriptalbePotion.meshFilter.sharedMesh;
            this.GetComponentInChildren<MeshRenderer>().sharedMaterials = scriptalbePotion.material;

            gameObject.SetActive(true);

            chicken.stats.SumStats(scriptalbePotion.GetTempStats());
        }

        chicken.stats.SumStats(scriptalbePotion.GetPerStats());

    }

    public void EndBuff()
    {
        chicken.stats.ReduceStats(scriptalbePotion.GetTempStats());
        chicken.currentBuffs.Remove(name);

        gameObject.SetActive(false);

        //end buff visual + audio:
        //ParticleSystem prefabParticals = Instantiate(shatteredGlass, transform.position, Quaternion.identity);
        AudioManager.Instance.Play("Bloop");

        buffName = "";
    }

    public void ResetPotionEffect()
    {
        timerImage.fillAmount = 1f;
        timer = scriptalbePotion.GetTime();

        chicken.stats.SumStats(scriptalbePotion.GetPerStats());
    }
}

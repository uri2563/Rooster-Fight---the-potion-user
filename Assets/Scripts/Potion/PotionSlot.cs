
using UnityEngine;
using System;
using UnityEngine.UI;


public class PotionSlot : MonoBehaviour
{
    Potion potion;
    public Text Description;//Debug: check if needed
    public Text Name;

    
    private void Awake()
    {
        potion = GetComponentInChildren<Potion>();
    }

    //setup potion slot and if needed prefub
    public bool setPotionSlot(ScriptalbePotion scriptablePotion)
    {
        if (scriptablePotion != null)
        {
            Description.text = scriptablePotion.description;
            Name.text = scriptablePotion.Name;
        }
        else
        {
            EmptySlot();
        }
        return potion.SetPotionView(scriptablePotion);
    }

    private void EmptySlot()
    {
        Description.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);
    }

    private float timer;
    private float totalTime;
    private bool isCooling;

    public Text CoolDownTime;
    public Image HoldImage;

    private void Update()
    {
        if(isCooling && timer > 0)
        {
            timer -= Time.deltaTime;
            CoolDownTime.text = Math.Ceiling(timer) + "";
            HoldImage.fillAmount -= 1.0f / totalTime * Time.deltaTime;
        }
        else if (isCooling && timer <= 0)
        {
            EndCoolDown();
        }
    }

    public void StartCoolDown(int time)
    {
        CoolDownTime.gameObject.SetActive(true);
        HoldImage.gameObject.SetActive(true);

        isCooling = true;
        timer = totalTime = time;
    }

    private void EndCoolDown()
    {
        CoolDownTime.gameObject.SetActive(false);
        HoldImage.gameObject.SetActive(false);

        HoldImage.fillAmount = 1f;
        isCooling = false;

        potion.SetTouch(true);
    }

    public ScriptalbePotion GetScriptablePotion()
    {
        return potion.scriptalbePotion;
    }

}
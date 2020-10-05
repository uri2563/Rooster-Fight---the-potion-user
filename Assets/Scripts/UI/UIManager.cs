using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private PlayerManager manager;
    PotionSlot[] PotionSlots; 

    ScriptalbePotion[] potions;

    private void Start()
    {
        manager = PlayerManager.Instance;
        potions = manager.playerPotions;
        PotionSlots = this.GetComponentsInChildren<PotionSlot>();
        initiatePotionSlots();
    }

    public void initiatePotionSlots()
    {
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i] != null)
            {
                PotionSlots[i].setPotionSlot(potions[i]);
            }
            else { PotionSlots[i].setPotionSlot(null); }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<ScriptalbePotion> allPotions;
    public List<int> ShownPotions;

    public ScriptalbePotion[] playerPotions;

    public static PlayerManager Instance;


    public GameObject playerInventory;
    public GameObject shopInventory;//only to init
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("more then 1 instance of playermanager had been found");
        }
        Instance = this;
    }

    public void SetNewPlayerPotionsList()
    {
        int i = 0;
       foreach(DragDrop slot in playerInventory.GetComponentsInChildren<DragDrop>())
        {
            playerPotions[i] = slot.thisPotion;
            i++;
        }
    }

    public List<ScriptalbePotion> getShopPotions()
    {
        int tier = GameUIManager.Instance.level - 2;//-2 - level up and the tier level
        List<ScriptalbePotion> newShop = new List<ScriptalbePotion>();

        int num;
        //make sure you dont get into ifenent loop
        List<int> checkedPotions = new List<int>();
        while (newShop.Count < 3)
        {
            if(allPotions.Count == ShownPotions.Count || checkedPotions.Count == allPotions.Count)
            {
                Debug.Log("ResetShop");
                ShownPotions = new List<int>();
            }

            num = Random.Range(0, allPotions.Count);

            if(!checkedPotions.Contains(num))
            { checkedPotions.Add(num); }

            ScriptalbePotion pot = allPotions[num];
            if (pot.tier <= tier && !ShownPotions.Contains(num) && !newShop.Contains(pot) && !playerPotions.Contains(pot))
            {
                newShop.Add(pot);
                ShownPotions.Add(num);
            }
        }
        return newShop;
    }

    public static ScriptalbePotion getScriptablePotion(string potionName)
    {
        ScriptalbePotion scriptablePotion = Resources.Load<ScriptalbePotion>("ScriptablePotions/" + potionName);
        //check for error
        if (scriptablePotion == null)
        {
            Debug.LogError("there is no scriptable object containing this name!");
            return null;
        }
        return scriptablePotion;
    }
}

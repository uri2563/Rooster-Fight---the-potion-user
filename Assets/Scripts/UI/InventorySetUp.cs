
using UnityEngine;

public class InventorySetUp : MonoBehaviour
{
    [SerializeField] private GameObject playerInventory;
    [SerializeField] private GameObject ShopInventory;

    public void SetUpInventory()
    {
        ScriptalbePotion[] shopPotions = PlayerManager.Instance.getShopPotions().ToArray();
        ScriptalbePotion[] playerPotions = PlayerManager.Instance.playerPotions;

        int i = 0;
        foreach (DragDrop slot in playerInventory.GetComponentsInChildren<DragDrop>())
        {
            slot.SetSlot(playerPotions[i]);
            i++;
        }

        i = 0;
        foreach (DragDrop slot in ShopInventory.GetComponentsInChildren<DragDrop>())
        {
            slot.SetSlot(shopPotions[i]);
            i++;
        }
    }
}

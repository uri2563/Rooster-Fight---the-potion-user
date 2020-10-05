using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textDescription;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void PopUp(ScriptalbePotion scriptalbePotion)
    {
        string description = scriptalbePotion.description + "\n" +
            "CoolDown: " + scriptalbePotion.coolDownTime +"[Sec]"+ "\n\n"+
            "Increate Stats Permenent By:\n" + scriptalbePotion.per_stats.toString() +
            "\n\n" + "Increate Stats Temp By:\n" + "Duration: " + scriptalbePotion.effectTime +"[Sec]"+ "\n" +
            scriptalbePotion.temp_stats.toString();

        textName.text = scriptalbePotion.Name;
        textDescription.text = description;

        gameObject.SetActive(true);
    }

    public void PopDown()
    {
        gameObject.SetActive(false);
    }
}

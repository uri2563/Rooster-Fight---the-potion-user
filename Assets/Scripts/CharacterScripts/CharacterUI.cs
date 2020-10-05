using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    public ChickenController character;
    public Bar health;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //change - should call barupdate only when stats changes!
        UpdateBars();
    }

    void UpdateBars()
    {
        //UpdateHealth
        health.UpdateBar(character.stats.Max_health, character.stats.Curr_health);

    }

}

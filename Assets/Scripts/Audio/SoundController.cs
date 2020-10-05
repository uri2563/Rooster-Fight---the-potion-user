using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private GameObject muteBtn;
    private GameObject unmuteBtn;

    // Start is called before the first frame update
    void Start()
    {
        muteBtn = GameObject.Find("MuteButton");
        unmuteBtn = GameObject.Find("UnmuteButton");

        if (isMuted)
        {
            muteBtn.SetActive(false);
            unmuteBtn.SetActive(true);
        }
        else
        {
            muteBtn.SetActive(true);
            unmuteBtn.SetActive(false);
        }
    }

    private static bool isMuted = false;
    public void Mute()
    {
        if (isMuted)//unmute
        {
            AudioManager.Instance.gameObject.SetActive(true);
            isMuted = false;
        }
        else//mute
        {
            AudioManager.Instance.gameObject.SetActive(false);
            isMuted = true;
        }
    }

    public void ClickSound()
    {
        AudioManager.Instance.Play("Click");
    }
}

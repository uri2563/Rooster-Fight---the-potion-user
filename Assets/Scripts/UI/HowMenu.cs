using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HowMenu : MonoBehaviour
{
    public Sprite[] pages;
    private int pointer;

    public Image image;

    public GameObject next;
    public GameObject before;

    public TextMeshProUGUI pageNum;

    public void StartImage()
    {
        image.sprite = pages[0];
        pointer = 0;
        UpdatePageNum();

        before.SetActive(false);
        next.SetActive(true);
    }

    public void NextPage()
    {
        pointer++;
        before.SetActive(true);
        image.sprite = pages[pointer];
        UpdatePageNum();

        if (pointer == pages.Length - 1)
        {
            next.SetActive(false);
        }
        else
        {
            next.SetActive(true);
        }
    }

    public void BackPage()
    {
        pointer--;
        next.SetActive(true);
        image.sprite = pages[pointer];
        UpdatePageNum();

        if (pointer == 0)
        {
            before.SetActive(false);
        }
        else
        {
            before.SetActive(true);
        }
    }

    private void UpdatePageNum()
    {
        pageNum.text = (pointer + 1) + "/" + pages.Length;
    }
}

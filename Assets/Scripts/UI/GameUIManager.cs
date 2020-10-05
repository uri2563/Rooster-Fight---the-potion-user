using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    public GamesManager gamesManager;
    public InventorySetUp inventorySetUp;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public Text timeText;

    private float timer;
    public int level = 0;

    private int timeScale = 1;

    private int timeSecs;
    private int timemili;

    public GameObject win;
    public GameObject lose;

    public TextMeshProUGUI scoreText;

    public GameObject adsBtn;

    public GameObject showWindow;
    public GameObject playWindow;
    public GameObject losserWindow;
    public GameObject shopWindow;

    public PopUpWindow popUpWindowShop;
    public PopUpWindow popUpWindowPlay;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LevelUp();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Time.timeScale = timeScale;
    }

    private void FixedUpdate()
    {
        timeSecs = (int)timer;
        timemili = (int)(((timer) * 100f) % 100);

        timerText.text = timeSecs + ":" + timemili;
    }

    private void OnEnable()
    {
        timer = 0;
    }

    private void LevelUp()
    {
        level++;
        levelText.text = "LVL " + level;
    }

    public void ChangeTimeScale()
    {
        switch (timeScale)
        {
            case 1:
                timeScale = 2;
                break;
            case 2:
                timeScale = 3;
                break;
            case 3:
                timeScale = 5;
                break;
            case 5:
                timeScale = 1;
                break;
        }
        timeText.text = "X" + timeScale;
    }

    public void QuitBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Winner()
    {
        LevelUp();
        //save high score?
        playWindow.SetActive(false);
        win.SetActive(true);
        LeanTween.scale(win, new Vector3(1.5f, 1.5f, 1.5f), 2f).setLoopPingPong(1).setOnComplete(CompleteWinner);
    }
    void CompleteWinner()
    {
        win.SetActive(false);
        inventorySetUp.SetUpInventory();
        popUpWindowShop.gameObject.SetActive(false);
        shopWindow.SetActive(true);
    }

    public void Losser()
    {
        playWindow.SetActive(false);
        lose.SetActive(true);
        LeanTween.scale(lose, new Vector3(1.5f, 1.5f, 1.5f), 2f).setLoopPingPong(1).setOnComplete(CompleteLosser);
    }
    void CompleteLosser()
    {
        lose.SetActive(false);
        int score = level - 1;
        //save high score
        if(SaveHighScore.LoadScore() < score)
        {
            SaveHighScore.SaveScore(score);
        }
        //set text score
        scoreText.text = "Score: "+ score;

        //if it is the second time it revived
        if (AdsManager.Instance.revived)
        {
            adsBtn.SetActive(false);
        }

        losserWindow.SetActive(true);
    }

    public void PlayAgine()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Revive()
    {
        losserWindow.SetActive(false);
        gamesManager.SwitchCams();
        gamesManager.StartRound();
        showWindow.SetActive(true);
    }

    public void ResetTimer()
    {
        timer = 0;
    }

}

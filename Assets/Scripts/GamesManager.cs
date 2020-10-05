using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GamesManager : MonoBehaviour
{
    public GameUIManager gameUIManager;

    public GameObject gameCam;
    public GameObject showCam;

    public GameObject potionBar;

    enum Players { PLAYER1, PLAYER2 }
    enum State { START, INGAME, SHOW, ENDGAME }

    private State currState;

    public float MAX_GAME_TIME = 240f;//in seconds

    public GameObject characterPrefab;
    public GameObject showPrefab;

    private GameObject player1;
    private GameObject player2;

    private ChickenController controller1;
    private ChickenController controller2;

    public int number_of_nets = 100;//has to be even

    public static int current_chicken_number1;
    public static int current_chicken_number2;

    private int color1;
    private int color2;

    // Start is called before the first frame update
    void Start()
    {
        currState = State.START;
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case State.START:
                StartRound();
                break;
            case State.INGAME:
                ListenToGame();
                break;
            default:
                break;
        }
    }

    public void StartRound()
    {
        gameUIManager.enabled = true;
        currState = State.SHOW;

        current_chicken_number1 = GetRandom(0, number_of_nets);
        current_chicken_number2 = GetRandom(0, number_of_nets, current_chicken_number1);

        Debug.Log("chiken numbers:" + current_chicken_number1 + ", " + current_chicken_number2);

        color1 = GetRandom(0, 9);
        color2 = GetRandom(0, 9, color1);

        ShowChicken();
    }

    public void SwitchCams()
    {
        showCam.SetActive(!showCam.activeSelf);//!showCam.isActiveAndEnabled;
        gameCam.SetActive(!gameCam.activeSelf);
    }

    public GameObject showChick;
    public Animator rotateCam;
    //player allways want player1 to win!
    private void ShowChicken()
    {
        showChick = Instantiate(showPrefab, new Vector3(0,1.4f,0) , Quaternion.identity);
        showChick.GetComponentInChildren<ChickenRandererManager>().ChangeChickenColor(color1);
    }

    public void StartGame()
    {
        SwitchCams();
        Destroy(showChick);
        GameUIManager.Instance.ResetTimer();
        //Enable GameUI

        //init prefab - player1 can be on both sides!
        Vector3 Side1 = new Vector3(5, 1, 0);
        Vector3 Side2 = new Vector3(-5, 1, 0);

        //pick a the starting side of chickens
        float randomSide = UnityEngine.Random.Range(0f, 1f);
        if (randomSide < 0.5f)
        {
            Side1 = new Vector3(-5, 1, 0);
            Side2 = new Vector3(5, 1, 0);
        }


        player1 = Instantiate(characterPrefab, Side1, Quaternion.identity);
        player2 = Instantiate(characterPrefab, Side2, Quaternion.identity);

        //change color
        player1.GetComponentInChildren<ChickenRandererManager>().ChangeChickenColor(color1);
        player2.GetComponentInChildren<ChickenRandererManager>().ChangeChickenColor(color2);
        //get controllers
        controller1 = player1.GetComponent<ChickenController>();
        controller2 = player2.GetComponent<ChickenController>();
        Debug.Log("Level:" + LevelAddDifficulty(GameUIManager.Instance.level));

        //set stats
        controller1.stats = new Stats(1f);
        controller2.stats = new Stats(1f + LevelAddDifficulty(GameUIManager.Instance.level));
        Debug.Log("Level stats :" + controller2.stats.toString());

        //set NN
        setNet(current_chicken_number1, controller1);
        setNet(current_chicken_number2, controller2);
        currState = State.INGAME;
    }
    //
    //returns the amount of power that will be add to opponent chicken 
    private static float LevelAddDifficulty(int x)
    {
        return ((float)Math.Sqrt((float)x) / 7f);
    }

    int GetRandom(int min, int max, int usedNumber)
    {
        int rand = UnityEngine.Random.Range(min, max);
        while (rand == usedNumber)
            rand = UnityEngine.Random.Range(min, max);
        usedNumber = rand;
        return rand;
    }
    int GetRandom(int min, int max)
    {
        int rand = UnityEngine.Random.Range(min, max);
        return rand;
    }

    private void setNet(int numberNet, ChickenController controller_)
    {
        NeuralNet net = null;

        net = SaveNet.LoadResorcesNN(numberNet).getDataAsNet();
        controller_.setNet(net);
    }

    IEnumerator EndGame(Players winner)
    {
        currState = State.ENDGAME;
        potionBar.SetActive(false);
        gameUIManager.enabled = false;
        yield return new WaitForSeconds(1f);//playing dead animation

        //pass to next window
        if (winner.Equals(Players.PLAYER1))
        {
            AudioManager.Instance.Play("Win");
            AudioManager.Instance.Play("Claps");
            GameUIManager.Instance.Winner();
        }
        else
        {
            AudioManager.Instance.Play("Lose");
            GameUIManager.Instance.Losser();
        }

        yield return new WaitForSeconds(2f);//playing dead animation
        Destroy(player1);
        Destroy(player2);
    }

    //checks if the game ended
    private void ListenToGame()
    {
        if (controller1 != null && controller2 != null)
        {
            if (controller1.stats.CheckDead())
            {
                StartCoroutine(EndGame(Players.PLAYER2));
            }
            else if (controller2.stats.CheckDead())
            {
                StartCoroutine(EndGame(Players.PLAYER1));
            }
        }
    }
}

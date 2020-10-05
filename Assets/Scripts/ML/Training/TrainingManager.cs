using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random;

public class TrainingManager : MonoBehaviour
{
    public bool restart_nets = false;

    public float surviveRate = 0.6f;

    public float MAX_GAME_TIME = 240f;//in seconds

    public float timeScale = 1;

    public GameObject characterPrefab;

    private GameObject player1;
    private GameObject player2;

    private ChickenController controller1;
    private ChickenController controller2;

    public bool game_runs = false;
    public int rounds_to_go = 10;

    private float timer;
    //NN
    private const int inputN = 5;
    private const int outputN = 6;
    private const int hiddenLayersN = 2;
    private const int hiddenN = 5;

    //train
    public int number_of_games = 0;//debug
    public int number_of_nets = 100;//has to be even

    public static int current_chicken_number1;
    public static int current_chicken_number2;

    private List<int> net_pool;
    private List<NeuralNet> nets_played;
    private List<float> nets_scores;

    public Material mat; // delete

    // Start is called before the first frame update
    void Start()
    {
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeScale;

        timer += Time.deltaTime;
        ListenToGame();
    }

    private void StartRound()
    {
        if(rounds_to_go == 0)
        {
            Debug.Log("Finished training");
            return;
        }

        rounds_to_go--;
        GeneratePool();
        nets_played = new List<NeuralNet>();
        nets_scores = new List<float>();
        StartGame();
    }

    private void GeneratePool()
    {
        net_pool = new List<int>();
        for (int i = 0; i < number_of_nets; i++)
        {
            net_pool.Add(i);
        }
    }

    private int ChooseFromPool()
    {
        int index = Random.Range(0,net_pool.Count-1);
        int r = net_pool[index];
        net_pool.RemoveAt(index);
        return r;
    }

    private void StartGame()
    {
        //if ended all games this round
        if (nets_played.Count == number_of_nets)
        {
            EndRound();
            return;
        }
        Debug.Log("nets_played.Count:" + nets_played.Count + " number_of_nets:"+ number_of_nets);
        number_of_games++;//debug

        current_chicken_number1 = ChooseFromPool();
        current_chicken_number2 = ChooseFromPool();
        Debug.Log("chiken numbers:" + current_chicken_number1 + ", " + current_chicken_number2);
        
        timer = 0;

        player1 = Instantiate(characterPrefab, new Vector3(5, 1, 0), Quaternion.identity);
        player2 = Instantiate(characterPrefab, new Vector3(-5, 1, 0), Quaternion.identity);

        controller1 = player1.GetComponent<ChickenController>();
        controller2 = player2.GetComponent<ChickenController>();

        setNet(current_chicken_number1, controller1,restart_nets);
        setNet(current_chicken_number2, controller2, restart_nets);
    }

    private void setNet(int numberNet, ChickenController controller_,bool restart_all_nets)
    {
        NeuralNet net;

       // path = Application.persistentDataPath + "/Net" + numberNet + ".net";
        string path = Application.dataPath + "/BieryFiles/Net" + numberNet + ".net";

        if (File.Exists(path) && !restart_all_nets)
        {
            net = SaveNet.LoadNN(numberNet).getDataAsNet();
            Debug.Log("From path");
        }
        else
        {
            net = new NeuralNet(inputN, outputN, hiddenLayersN, hiddenN);
            Debug.Log("new chick");
        }

        nets_played.Add(net);
        controller_.setNet(net);
    }

    private void EndGame(float bonus)
    {
        nets_scores.Add(getScore(controller1, controller2) + bonus);
        nets_scores.Add(getScore(controller2, controller1) + bonus);

        Destroy(player1);
        Destroy(player2);
        controller1 = null;
        controller2 = null;
        
        StartGame();
    }


    private void EndRound()
    {
        UpdateGenes(nets_played,nets_scores);
        StartRound();
    }

    //checks if the game ended
    private void ListenToGame()
    {
        if(controller1 != null && controller2 != null)
        {
            if(controller1.stats.CheckDead() || controller2.stats.CheckDead())
            {
                EndGame(0);
            }
            else if (timer >= MAX_GAME_TIME)
            {
                Debug.Log("Times up: " + timer);
                EndGame(-100f);
            }
        }
    }

    private float getScore(ChickenController myController ,ChickenController oppenentController)
    {
        float damageFromOp = 100 - myController.stats.Curr_health;
        float damageToOp = 100 - oppenentController.stats.Curr_health;
        return damageToOp - damageFromOp - timer / 10f;
    }

    //evolve genes of the nets - will be called after training round
    private void UpdateGenes(List<NeuralNet> nets, List<float> scores)
    {
        List<List<float>> genes = new List<List<float>>();
        for (int i = 0; i < nets.Count; i++)
        {
            genes.Add(nets[i].getWieghtGenome());
        }

        List<List<float>> newGenes = new List<List<float>>();

        List<List<float>> chosen_genes = Genetics.Roulette(genes,scores, (int)(number_of_nets * surviveRate));
        List<List<float>> chosen_genes_copy;

        int num = 0;
        int key1 = 0;
        int key2;
        while(num < number_of_nets)
        {
            chosen_genes_copy = new List<List<float>>(chosen_genes);

            while (chosen_genes_copy.Count > 0)
            {
                num++;
                //if there is no partner for the last one
                if (chosen_genes_copy.Count == 1)
                {
                    key2 = Random.Range(1, chosen_genes.Count-1);//key2 will be taken from initial list
                    newGenes.Add(Genetics.Produce(chosen_genes_copy[key1], chosen_genes[key2]));
                    chosen_genes_copy.RemoveAt(key1);
                }
                else
                {
                    key2 = Random.Range(1, chosen_genes_copy.Count-1);
                    newGenes.Add(Genetics.Produce(chosen_genes_copy[key1], chosen_genes_copy[key2]));
                    chosen_genes_copy.RemoveAt(key2);
                    chosen_genes_copy.RemoveAt(key1);
                }
            }
        }
        saveGenes(genes, nets);
    }

    private void saveGenes(List<List<float>> genes, List<NeuralNet> nets)
    {
        Debug.Log("saving training");
        for (int i = 0; i < genes.Count; i++)
        {
            if(i > nets.Count)
            {
                nets.Add(new NeuralNet(inputN, outputN, hiddenLayersN, hiddenN));
            }

            nets[i].InsertNewGenome(genes[i]);
            SaveNet.SaveNN(nets[i], i);
        }
    }

}

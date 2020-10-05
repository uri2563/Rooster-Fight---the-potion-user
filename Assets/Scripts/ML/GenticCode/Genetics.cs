using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SocialPlatforms;

public static class Genetics {

    public static int mutationRate = 50; //(1/rate)
    public static float crossoverRate = 0.7f; //less then 1
    public static float MaxValue = 1; //maximum value a gene can hold
    public static float MinValue = -1;//min value a gene can hold

    public static List<float> Produce(List<float> geneKey1, List<float> geneKey2)
    {
        List<float> geneKeyNew = new List<float>();
        geneKeyNew = Crossover(crossoverRate, geneKey1, geneKey2);
        geneKeyNew = Mutation(mutationRate, geneKeyNew);
        Debug.Log("Produce:" + geneKeyNew.Count);
        return geneKeyNew;
    }


    /// <summary>
    /// takes the 2 geneKey and combains the 2 together the cut depends on the rate.
    /// </summary>
    /// <param name="rate"></param>
    /// <param name="geneKey1"></param>
    /// <param name="geneKey2"></param>
    /// <returns></returns>
    public static List<float> Crossover(float rate, List<float> geneKey1, List<float> geneKey2)
    {
        if(rate > 1)
        {
            Debug.LogError("Crossover rate cant be more then 1");
            return null;
        }

        List<float> geneKeyNew = new List<float>();

        int len = geneKey1.Count;
        int part1 = (int)(len * rate);

        //choos wich perent will be dominent
        if (Random.Range(0f, 2f) < 1)
        {
            geneKeyNew = geneKey1.GetRange(0, part1);
            geneKeyNew.AddRange(geneKey2.GetRange(part1, len - part1));
        }
        else
        {
            geneKeyNew = geneKey2.GetRange(0, part1);
            geneKeyNew.AddRange(geneKey1.GetRange(part1, len - part1));
        }

        CheckIfEqualLists(len, geneKeyNew.Count);//check if 2 keys has the same length
        return geneKeyNew;
    }

    /// <summary>
    /// the mutation takes every bit in the key and check if change it -controled by the rate
    /// </summary>
    /// <param name="rate">(1-range)</param>
    /// <param name="geneKeyNew">the genekey after crossover</param>
    /// <returns>theMutate key</returns>
    public static List<float> Mutation(int rate, List<float> geneKeyNew)
    {
        int len = geneKeyNew.Count;

        for (int i = 0; i < len; i++)
        {
            if (Random.Range(0, rate) == 0)//if it is a mutate gene
            {
                geneKeyNew[i] = Random.Range(MinValue, MaxValue);
                Debug.Log("check that the max value here is currect");
            }
        }

        return geneKeyNew;
    }

    //DebugFuncs
    public static void CheckIfEqualLists(int len1, int len2)
    {
        int counter = 1;//the num of this Debug

        if (len1 != len2)
        {
            Debug.LogError("The 2 Lists length is NOT the same! - " + counter);
        }

        counter++;
    }

    public static void PrintGeneKey(List<float> geneKey)
    {
        for (int i = 0; i < geneKey.Count; i++)
        {
            Debug.Log("The key-" + geneKey[i] + " num:" + i);
        }
    }

    /// <summary>
    /// gets list of genes and the scores in another list returns the surviving genes
    /// </summary>
    /// <param name="genes"></param>
    /// <param name="scores">scores of each gene, can be negative</param>
    /// <param name="num_choosen_genes">number of genes to renturn from rollet</param>
    /// <returns></returns>
    public static List<T> Roulette<T>(List<T> genes, List<float> scores, int num_choosen_genes)
    {
        if(genes.Count < num_choosen_genes)
        {
            Debug.LogError("num_choosen_genes is less then the total number of genes");
            return null;
        }

        List<T> chosenGenes = new List<T>();
        //copys
        List<T> old_genes = new List<T>(genes);
        List<float> genes_score= new List<float>(scores);

        float min = genes_score[0];
        float sum = 0;
        //find min and get sum
        for (int i = 0; i < genes_score.Count; i++)
        {
            sum += genes_score[i];
            if(min > genes_score[i])
            {
                min = genes_score[i];
            }
        }

        //if there is non posative score
        if(min <= 0)
        {
            sum += (-min+1) * genes_score.Count;
            for (int i = 0; i < genes_score.Count; i++)
            {
                genes_score[i] += (-min + 1);
            }
        }

        float choose;
        int index;
        for (int i = 0; i < num_choosen_genes; i++)
        {
            index = 0;
            choose = Random.Range(0f,sum);
            while (choose > 0 && index < old_genes.Count - 1)
            {
                sum += genes_score[index];
                index++;
            }
            //assure no genes is choosen twise
            chosenGenes.Add(old_genes[index]);
            sum -= genes_score[index];

            old_genes.RemoveAt(index);
            genes_score.RemoveAt(index);
        }

        return chosenGenes;
    }
}

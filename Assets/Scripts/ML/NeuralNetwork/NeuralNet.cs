using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NeuralNet
{
    private int inputNum;
    private int outputNum;
    private int hiddenLayersNum;
    private int hiddenNeuronNum;

    private Layer layers;//the NN (list of layers)
    private List<float> wieghtGenome;//list of all the wieghts in the NN

    //Constractor creates the layers and make a wieght list. 
    public NeuralNet(int inputN, int outputN, int hiddenLayersN,int hiddenNeuronN)
    {
        inputNum = inputN;
        outputNum = outputN;
        hiddenLayersNum = hiddenLayersN;
        hiddenNeuronNum = hiddenNeuronN;

        layers = new Layer(LayersPropMaker(inputN, outputN, hiddenLayersN, hiddenNeuronN));//makes int list for how the NN will look like
        wieghtGenome = ReturnNetWieghtGenome();
    }

    //Creates a list(int) of the num of neurons in each layer.
    private List<int> LayersPropMaker(int inputN, int outputN, int hiddenLayersN, int hiddenNeuronN)
    {
        List<int> prop = new List<int>();
        prop.Add(inputN);
        for (int i = 0; i < hiddenLayersN; i++)
        {prop.Add(hiddenNeuronN);}
        prop.Add(outputN);
        return prop;
    }

    public int InputNum { get => inputNum; set => inputNum = value; }
    public int OutputNum { get => outputNum; set => outputNum = value; }
    public int HiddenLayersNum { get => hiddenLayersNum; set => hiddenLayersNum = value; }
    public int HiddenNeuronNum { get => hiddenNeuronNum; set => hiddenNeuronNum = value; }
    public List<float> getWieghtGenome()
    {
        return wieghtGenome;
    }

    //Insert the new genome wieghts - list of all the wieghts and bias
    //wieghts should be in range ~-4, 4
    public void InsertNewGenome(List<float> newGenome)
    {
        layers.EnterNewWieghts(newGenome);
        wieghtGenome = ReturnNetWieghtGenome();//constract the new genom (wieght list inserted)
    }

    //enter an input and find the result
    public List<float> Input(List<float> inputs)
    {
        if(inputs.Count != inputNum)
        {
            Debug.LogError("number of input desnt match the net, input layer count: " + inputNum + " input count: " + inputs.Count);
        }
        return layers.Insert(inputs);
    }
    //collect all the wieghts to list of wieghts
    private List<float> ReturnNetWieghtGenome()
    {
        List<float> wieghetsGenome = new List<float>();
        layers.ReturnWieghtGenome(wieghetsGenome);
        return wieghetsGenome;
    }
    //prints the wieghtGenom
    public void PrintWieghtGenom()
    {
        Debug.Log("count: " + wieghtGenome.Count + "should be: " + NetworkWieghtsCounter(inputNum, outputNum, hiddenLayersNum, hiddenNeuronNum));

        for (int i = 0; i < wieghtGenome.Count; i++)
        {
           // Debug.Log("num: " + i);
            Debug.Log(wieghtGenome[i]);
        }
    }

    //counts the wieghts in the net
    private int NetworkWieghtsCounter(int inputN, int outputN, int hiddenLayersN, int hiddenNeuronN)
    {
        int sum = 0;
        sum += inputN * hiddenNeuronN; // wieghts of first layer

        sum += hiddenNeuronN * hiddenNeuronN * (hiddenLayersN - 1);
        sum +=  hiddenNeuronN * outputN; // wieghts of hidden layers
        sum += outputN * 0;//output Dosnt have wieghts
        sum += outputN + hiddenNeuronN * hiddenLayersN + inputN;// add bias
        return sum;
    }




}

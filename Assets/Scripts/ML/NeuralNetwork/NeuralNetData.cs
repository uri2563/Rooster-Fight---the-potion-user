using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuralNetData
{
    private int inputNum;
    private int outputNum;
    private int hiddenLayersNum;
    private int hiddenNeuronNum;

    private List<float> wieghtGenome;//list of all the wieghts in the NN

    public NeuralNetData(NeuralNet neuralNet)
    {
        inputNum = neuralNet.InputNum;
        outputNum = neuralNet.OutputNum;
        hiddenLayersNum = neuralNet.HiddenLayersNum;
        hiddenNeuronNum = neuralNet.HiddenNeuronNum;
        wieghtGenome = neuralNet.getWieghtGenome();


    }

    public NeuralNet getDataAsNet()
    {
        if(wieghtGenome == null)
        {
            Debug.LogError("uninisialized NN data");
        }

        NeuralNet neuralNet = new NeuralNet(inputNum, outputNum, hiddenLayersNum, hiddenNeuronNum);


        neuralNet.InsertNewGenome(wieghtGenome);

        return neuralNet;
    }

    public int InputNum { get => inputNum; set => inputNum = value; }
    public int OutputNum { get => outputNum; set => outputNum = value; }
    public int HiddenLayersNum { get => hiddenLayersNum; set => hiddenLayersNum = value; }
    public int HiddenNeuronNum { get => hiddenNeuronNum; set => hiddenNeuronNum = value; }
    public List<float> WieghtGenome { get => wieghtGenome; set => wieghtGenome = value; }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Neuron {

    private int numWieghts;//the number of wieghts in this nueron = the num of neurons in the next layer
    private List<float> neuronWieghts;
    private float bias;

    //constructore
    public Neuron(int wieghtNum)
    {
        neuronWieghts = new List<float>();
        numWieghts = wieghtNum;
        //+1 for bias
        //Creates the wieght list the random first time - the range depend on the input range
        for (int i = 0; i < wieghtNum + 1;i++)
        {
            //Debug.Log("think about the wieght range and change it");----------------------------
            neuronWieghts.Add(Random.Range(-1f,1f));
        }
        //bias is the last wieght
        bias = neuronWieghts[wieghtNum];
    }

    //set input and get output list after bieng sigmoid - list of outputs one for each line
    public List<float> InsertNeuron(List<float> input)
    {
        float ac = (SumTheInputs(input) + bias);//the num after suming and add bias //should be with Sigmoid here

        List<float> activationList = new List<float>();//list of results that will go out of the neuron
        for (int i = 0; i < numWieghts; i++)//for each wieght ather result
        {
            activationList.Add(neuronWieghts[i] * ac);//double the wieghts with the result of the neuron
        }
        return activationList;
    }
    //sums the list of inputs
    private float SumTheInputs(List<float> input)
    {
        float sum = 0;
        for (int i = 0; i < input.Count; i++)
        {
            sum += input[i];
        }
        return sum;
    }

    private float Sigmoid(float input)
    {
        return 1.0f / (1.0f + (float)Mathf.Exp(-input));
    }

    public float finalOutput(List<float> input)
    {
       return Sigmoid(SumTheInputs(input) + bias);
    }

    public List<float> NeuronWieghts
    {
        set { neuronWieghts = value; }
        get { return neuronWieghts; }
    }
    public int NumWieghts
    {
        set { numWieghts = value; }
        get { return numWieghts; }
    }

}

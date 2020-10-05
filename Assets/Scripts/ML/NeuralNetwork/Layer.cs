using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Layer
{
    private Layer layers;//pointer to the next layer
    private List<Neuron> neurons;//the list of neurons that represent this layer
    private int wieghtNum;//the number of the next layer neurons = the num of wieghts in this layer

    public List<Neuron> Neurons
        {
        set { neurons = value; }
        get { return neurons; }
        }
    //create the NN
    public Layer(List<int> layersP)
    {
        //checks if its the last layer
        if (layersP.Count == 1)
        {
            neurons = new List<Neuron>();
            for (int i = 0; i < layersP[0]; i++)
            {
                neurons.Add(new Neuron(0));
            }
        }
        //if it is not last layer
        else
        {
            wieghtNum = layersP[1]; //the num of wights of each neuron in this layer is the num of neurons in the next layer 
            neurons = new List<Neuron>();
            for (int i = 0; i < layersP[0]; i++) //creates the neurons in this layer
            {
                neurons.Add(new Neuron(wieghtNum));//adds a neuron to the layer
            }
            
            layersP.RemoveAt(0);//remove the layer finished build in this play
            layers = new Layer(layersP); //saves the neurons in list "neurons" and the variable "layers" points to the next layer
        }
    }
    //inserts the input and find the result
    public List<float> Insert(List<float> input)
    {
        List<List<float>> newInput = new List<List<float>>();//
        for (int i = 0;i<input.Count;i++)//for each input 
        {
            newInput.Add(new List<float>());
            newInput[i].Add(input[i]);
        }
        List<float> result = new List<float>(); // create empty list for result
        InsertToNet(newInput,result);
        return result;
    }

    private void InsertToNet(List<List<float>> input, List<float> result)
    {
        List<List<float>> output = new List<List<float>>();
        //if its the output layer 
        if (layers == null)
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                result.Add(neurons[i].finalOutput(input[i]));//the result of each neuron in a list
            }
        }
        //if it is not the output layer
        else
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                output.Add(neurons[i].InsertNeuron(input[i]));//- for each neuron (outputline) create list of outputs numbers that will go to the next layer after double with the wieghts((((not true the input[i]? ))))
            }

            layers.InsertToNet(RearangeOutput(output), result);
        }
    }
    //rearange the list form list for every neuron in this layer to list of results for every neuron on the next layer
    private List<List<float>> RearangeOutput(List<List<float>> output)
    {
        List<List<float>> newOutput = new List<List<float>>();
        for (int i = 0; i < wieghtNum; i++)
        {
            newOutput.Add(new List<float>());
        }
        for (int i = 0; i < wieghtNum; i++)
        {
            for (int j = 0; j < neurons.Count; j++)
            {
                newOutput[i].Add(output[j][0]);
                output[j].RemoveAt(0);
            }
        }

        return newOutput;

    }
    //enters new wieghts
    public void EnterNewWieghts (List<float> wieghts)
    {
        for (int i = 0; i < neurons.Count; i++)//foreach neuron in the layer
        {
            for (int j = 0; j < neurons[i].NeuronWieghts.Count; j++)//each wieght of every neuron
            {
                neurons[i].NeuronWieghts[j] = wieghts[0];//enter the new wieght
                wieghts.RemoveAt(0);//remove the new wieght from the list of new wieghts
            }
        }
        if (layers != null)
        {
            layers.EnterNewWieghts(wieghts);
        }
    }
   
    public void ReturnWieghtGenome(List<float> wieghtList)
    {
        //create 2 variables to replace in the for func 
            for (int i = 0; i < neurons.Count; i++) //For each neuron in the layer
        {
                for (int j = 0; j < neurons[i].NeuronWieghts.Count; j++)//can be changed to wieghtNum + 1?
            {
                    wieghtList.Add(neurons[i].NeuronWieghts[j]);//adds up all the wieghts inside the neurons
                }
            }

        if (layers != null)//if its the last layer
        { layers.ReturnWieghtGenome(wieghtList); }
    }

}

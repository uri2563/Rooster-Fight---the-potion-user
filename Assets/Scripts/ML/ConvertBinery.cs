using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertBinery : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i < 100; i++)
        {
            NeuralNet net = SaveNet.LoadNN(i).getDataAsNet();
            SaveNet.SaveNN(net, i);
        }
        Debug.Log("done");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

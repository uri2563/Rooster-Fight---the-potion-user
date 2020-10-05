using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveNet
{
    public static void SaveNN(NeuralNet neuralNet,int numberNet)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        //string path = Application.persistentDataPath + "/Net" + numberNet + ".net";
        //string path = Application.dataPath + "/BieryFiles/Net" + numberNet + ".net";
        
        string path = Application.dataPath + "/Resources/BineryFiles2/Net" + numberNet + ".bytes";//for saving in diractory

        FileStream stream = new FileStream(path, FileMode.Create);//Debug: note that if i want to stack nn change create command

        NeuralNetData neuralNetData= new NeuralNetData(neuralNet);

        formatter.Serialize(stream, neuralNetData);
        stream.Close();
    }
    public static NeuralNetData LoadNN(int numberNet)
    {
        //string path = Application.persistentDataPath + "/Net" + numberNet + ".net";
        string path = Application.dataPath + "/BieryFiles/Net" + numberNet + ".net";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            NeuralNetData netData = formatter.Deserialize(stream) as NeuralNetData;
            stream.Close();

            return netData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static NeuralNetData LoadResorcesNN(int numberNet)
    {
        TextAsset textAsset = Resources.Load("BineryFiles2/Net" + numberNet) as TextAsset;
        Stream stream = new MemoryStream(textAsset.bytes);
        BinaryFormatter formatter = new BinaryFormatter();
        NeuralNetData myInstance = formatter.Deserialize(stream) as NeuralNetData;
        return myInstance;
    }



}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveHighScore
{
    public static void SaveScore(int score)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/HighScore.score";

        FileStream stream = new FileStream(path, FileMode.Create);//Debug: note that if i want to stack nn change create command

        formatter.Serialize(stream, score);
        stream.Close();
    }

    public static int LoadScore()
    {
        string path = Application.persistentDataPath + "/HighScore.score";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            int score = (int)formatter.Deserialize(stream);
            stream.Close();

            return score;
        }
        else
        {
            return 0;
        }
    }
}

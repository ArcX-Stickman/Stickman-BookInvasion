using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class SaveSystem 
{
    public static void SaveProgess(Movement Player)
    {
        BinaryFormatter Formatter =  new BinaryFormatter();
        string placeTosave = Application.persistentDataPath + "/save.stick";
        FileStream fileStream = new FileStream(placeTosave, FileMode.Create);

        PlayerData playerData = new PlayerData(Player);

        Formatter.Serialize(fileStream, playerData);
        fileStream.Close();

        
    }
    public static PlayerData LoadSave()
    {
        string placeTosave = Application.persistentDataPath + "/save.stick";
        if (File.Exists(placeTosave))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(placeTosave,FileMode.Open);
            
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("uhhh i dont know what to tell you buddy");
            return null;
        }

    }
    public static void ResetGame()
    {
        string placeTosave = Application.persistentDataPath + "/save.stick";
        if (File.Exists(placeTosave))
        {
            File.Delete(placeTosave);
        }
        else
        {
            Debug.Log("Uhh What?");
        }


    }


}

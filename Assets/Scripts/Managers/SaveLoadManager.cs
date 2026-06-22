using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    public string folderName = "SaveFiles";
    
    [Header("Spawn")] 
    public string fileName = "SpawnPoint.json";
    
    [Header("Checkpoint")]
    public string fileCheckPoint = "CheckPoint.json";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save<T>(T dataToSave, string folderName, string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath,folderName, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        File.WriteAllText(savePath, JsonUtility.ToJson(dataToSave, true));
    }

    public void Load<T>(T dataToLoad, string folderName, string fileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(loadPath))
        {
            string loadDataString = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(loadDataString, dataToLoad);
        }
    }

    public void DeleteSaveFile(string folderName, string fileName)
    {
        string dataPath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(dataPath))
            File.Delete(dataPath); 
    }

    public void DeleteFolder(string folderName)
    {
        string dataPath = Path.Combine(Application.persistentDataPath, folderName);
        if (Directory.Exists(dataPath))
            Directory.Delete(dataPath, true);
    }
}

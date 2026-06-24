using System.IO;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{

    private Transform player;
    [SerializeField] private SpawnIdentifier[] spawnPoints;
    [SerializeField] private SpawnIdentifier[] checkPoints;
    private SpawnData spawnData = new SpawnData();
    private CheckpointData checkData = new CheckpointData();
    
    
    void Start()
    {
        // SaveLoadManager.instance.DeleteFolder(SaveLoadManager.instance.folderName);
        player = FindAnyObjectByType<Player>().transform;


        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        if (SpawnMode.spawnFromCheckPoint && File.Exists(loadPath))
        {
            SaveLoadManager.instance.Load(checkData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
            foreach (SpawnIdentifier spawnID in checkPoints)
            {
                if (spawnID.spawnKey == checkData.checkpointKey)
                {
                    player.transform.position = spawnID.transform.position;
                    break;
                }
            }

            if (checkData.facingRight == false)
            {
                player.GetComponent<Player>().ForceFlip();
            }
            SpawnMode.spawnFromCheckPoint = false;
        }
        else
        {
            SaveLoadManager.instance.Load(spawnData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileName);

            foreach (SpawnIdentifier spawnID in spawnPoints)
            {
                if (spawnID.spawnKey == spawnData.spawnPointKey)
                {
                    player.transform.position = spawnID.transform.position;
                    break;
                }
            }

            if (spawnData.facingRight == false)
            {
                player.GetComponent<Player>().ForceFlip();
            }
        }
    }
}

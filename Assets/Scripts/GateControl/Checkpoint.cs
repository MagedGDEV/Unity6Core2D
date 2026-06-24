using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriter;
    [SerializeField] private Sprite spriteDisabled;
    [SerializeField] private Sprite spriteEnabled;
    [SerializeField] private Collider2D boxCol;
    [SerializeField] private CheckpointData checkpointData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriter.sprite = spriteEnabled;
            // Save data
            SaveLoadManager.instance.Save(checkpointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        }
    }
}

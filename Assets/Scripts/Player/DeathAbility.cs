using System.IO;
using System.IO;
using UnityEngine;

public class DeathAbility : BaseAbility
{
    private string deathAnimParameterName= "Death";
    private int deathParameterInt;


    public override void EnterAbility()
    {
        SpawnMode.spawnFromCheckPoint = true;
        player.gatherInput.DisablePlayerInput();
        linkedPhysicsControl.ResetVelocity();
    }

    protected override void Initialization()
    {
        base.Initialization();
        deathParameterInt = Animator.StringToHash(deathAnimParameterName);
    }

    public override void UpdateAnimator()
    {
        if(linkedPhysicsControl.grounded)
            linkedAnimator.SetBool(deathParameterInt, linkedStateMachine.currentState == PlayerStates.State.Death);
        else
        {
            // TODO - Add air death animation
            linkedAnimator.SetBool(deathParameterInt, linkedStateMachine.currentState == PlayerStates.State.Death);
        }
    }

    public void ResetGame()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        if (File.Exists(loadPath))
        {
            CheckpointData checkData = new CheckpointData();
            SaveLoadManager.instance.Load(checkData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileName);
            LevelManager.instance.LoadLevel(checkData.sceneToLoad);
        }
        else
        {
            LevelManager.instance.RestartLevel();
        }
    }
}

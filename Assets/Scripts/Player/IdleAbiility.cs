using UnityEngine;

public class IdleAbiility : BaseAbility
{
    protected override void Initialization()
    {
        base.Initialization();
        // add more things
    }

    public override void ProcessAbility()
    {
        Debug.Log("This is IDLE ability");
    }
}

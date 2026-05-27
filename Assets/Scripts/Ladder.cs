using System;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private LadderAbility ladderAbility;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ladderAbility = collision.GetComponent<LadderAbility>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ladderAbility == null)
            return;
        
        if (ladderAbility.isPermitted)
            ladderAbility.canGoOnLadder = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ladderAbility == null)
            return;
        
        if (ladderAbility.isPermitted)
            ladderAbility.canGoOnLadder = false;
    }
}

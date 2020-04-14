using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInactiveState : State
{
    PlayerController playerController;

    public override void BeginState()
    {
        playerController = this.gameObject.GetComponent<PlayerController>();
    }
}

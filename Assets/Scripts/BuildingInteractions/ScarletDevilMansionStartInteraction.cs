using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarletDevilMansionInteraction : InteractableCollider
{
    protected override void OnInteract()
    {
        GridSystem gd = GameInformation.instance.gd;
        if(GameInformation.instance.MainNetwork.IsServer)
        {
            if (gd.CurrentRoomCompleted)
            {
                PacketSend.Server_Send_StartRoom();
                gd.StartGridSystem(true);
            }
            else
            {
                GameUIManager.instance.NewMessage("Not everyone is ready");
            }
        } else
        {
            GameUIManager.instance.NewMessage("Only the host can enter for you");

        }

    }
}

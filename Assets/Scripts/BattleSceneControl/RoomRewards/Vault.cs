using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : RoomReward
{
    public override void OnRoomComplete()
    {
        for (int i = 0; i < 50; i++)
        {
            Vector3 droppos = Vector3.zero;
            droppos.y = GameInformation.instance.LocalPlayer.transform.position.y;
            GameSystem.instance.SpawnPointDrops(droppos);

        }
    }
}

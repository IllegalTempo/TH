using Assets.Scripts.BattleSceneControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomReward : MonoBehaviour
{
    protected Room mainroom;
    public void init(Room r)
    {
        mainroom = r;
    }
    public abstract void OnRoomComplete();

}

using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PacketHandles_Method
{
    public static GameServer server;
    public static GameClient client;
    public static void Server_Handle_test(NetworkPlayer p, packet packet)
    {
        string text = packet.ReadstringUNICODE();
        long clienttime = packet.Readlong();
        if (text == PacketSend.TestRandomUnicode)
        {
            Debug.Log($"{clienttime} Confirmed {p.SteamName}, successfully connected, delay:{(DateTime.Now.Ticks - clienttime)/10000}ms");

        } else
        {
            Debug.Log($"Check Code Mismatched Client Message: {text}");
        }
    }
    public static void Server_Handle_ReadyUpdate(NetworkPlayer p, packet packet)
    {
        bool ready = packet.Readbool();
        p.MovementUpdateReady = ready;
        Debug.Log($"Player {p.SteamName} is ready for receiving pos informations!");
    }
    public static void Server_Handle_AnimationState(NetworkPlayer p, packet packet)
    {
        float movex = packet.Readfloat();
        float movey = packet.Readfloat();
        p.player.playermovement.SetAnimation(movex, movey);
        PacketSend.Server_DistributePlayerAnimationState(p.NetworkID,movex,movey);
    }
    public static void Server_Handle_PosUpdate(NetworkPlayer p, packet packet)
    {
        Vector3 pos = packet.Readvector3();
        Quaternion rot = packet.Readquaternion();
        Quaternion yrot = packet.Readquaternion();
        p.player.playermovement.SetMovement(pos,rot,yrot);
        PacketSend.Server_DistributeMovement(p.NetworkID,pos,rot,yrot);
    }



    public static async void Client_Handle_test(Connection c, packet packet)
    {
        int NetworkID = packet.Readint();
        string text = packet.ReadstringUNICODE();
        long Servertime = packet.Readlong();
        GameInformation.instance.MainNetwork.client.NetworkID = NetworkID;

        if (text == PacketSend.TestRandomUnicode)
        {
            Debug.Log($"{Servertime} Confirmed connected from server. delay:{(DateTime.Now.Ticks - Servertime)/10000}ms");

        } else {
            Debug.Log($"Check Code Mismatched Server Message: {text}");

        }
        await Task.Delay(5);
        PacketSend.Client_Send_test();
    }
    public static async void Client_Handle_InitRoomInfo(Connection c, packet packet)
    {
        int numplayer = packet.Readint();
        GameClient client = GameInformation.instance.MainNetwork.client;
        for (int i = 0; i < numplayer; i++)
        {
            int NetworkID = packet.Readint();
            ulong steamid = packet.Readulong();
            Debug.Log($"Spawning Player {NetworkID} {steamid}");
            client.GetPlayerByNetworkID.Add(NetworkID,GameSystem.instance.SpawnPlayer(client.IsLocal(NetworkID), NetworkID,steamid));
            

        }
        await Task.Delay(1000);
        PacketSend.Client_Send_ReadyUpdate();
    }
    public static void Client_Handle_NewPlayerJoin(Connection c, packet packet)
    {
        ulong playerid = packet.Readulong();
        int supposeNetworkID = packet.Readint();


        

        GameInformation.instance.MainNetwork.client.GetPlayerByNetworkID.Add(supposeNetworkID, GameSystem.instance.SpawnPlayer(false, supposeNetworkID, playerid));
    }
    public static void Client_Handle_PlayerQuit(Connection c, packet packet)
    {
        GameClient cl = GameInformation.instance.MainNetwork.client;
        int NetworkID = packet.Readint();
        cl.GetPlayerByNetworkID[NetworkID].Disconnect();
        cl.GetPlayerByNetworkID.Remove(NetworkID);
    }
    public static void Client_Handle_ReceivedPlayerMovement(Connection c,packet packet)
    {
        int NetworkID = packet.Readint();

        Vector3 pos = packet.Readvector3();
        Quaternion headrot = packet.Readquaternion();
        Quaternion bodyrot = packet.Readquaternion();
        GameInformation.instance.MainNetwork.client.GetPlayerByNetworkID[NetworkID].playermovement.SetMovement(pos, headrot, bodyrot);
    }
    public static async void Client_Handle_ReceivedTransferWorld(Connection c,packet packet)
    {
        int seed = packet.Readint();
        GameSystem.instance.LoadSceneAction("InBattle", false);
        GameUIManager.instance.NewMessage("Transferring To Battle...");
        await Task.Delay(5000);
        GameInformation.instance.gd.ClientInitGridSystem(seed);
    }
    public static void Client_Handle_ReceivedPlayerAnimation(Connection c,packet packet)
    {
        int NetworkID = packet.Readint();
        float x = packet.Readfloat();
        float y = packet.Readfloat();
        GameInformation.instance.MainNetwork.client.GetPlayerByNetworkID[NetworkID].playermovement.SetAnimation(x, y);
    }
}



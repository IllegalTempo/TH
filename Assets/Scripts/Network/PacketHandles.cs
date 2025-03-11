using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PacketHandles_Method
{
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
    }
    public static void Server_Handle_PosUpdate(NetworkPlayer p, packet packet)
    {
        Vector3 pos = packet.Readvector3();
        Quaternion rot = packet.Readquaternion();
        float yrot = packet.Readfloat();
        p.player.playermovement.SetMovement(pos,rot,yrot);
        PacketSend.Server_DistributeMovement(p.NetworkID,pos,rot,yrot);
    }



    public static async void Client_Handle_test(Connection c, packet packet)
    {
        int NetworkID = packet.Readint();
        string text = packet.ReadstringUNICODE();
        long Servertime = packet.Readlong();
        SteamManager.client.NetworkID = NetworkID;

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
        GameClient client = SteamManager.client;
        for (int i = 0; i < numplayer; i++)
        {
            int NetworkID = packet.Readint();
            ulong steamid = packet.Readulong();
            Debug.Log($"Spawning Player {NetworkID} {steamid}");
            SteamManager.client.GetPlayerByNetworkID.Add(NetworkID,GameSystem.instance.SpawnPlayer(client.IsLocal(NetworkID), NetworkID,steamid));
            

        }
        await Task.Delay(1000);
        PacketSend.Client_Send_ReadyUpdate();
    }
    public static void Client_Handle_NewPlayerJoin(Connection c, packet packet)
    {
        ulong playerid = packet.Readulong();
        int supposeNetworkID = packet.Readint();


        

        SteamManager.client.GetPlayerByNetworkID.Add(supposeNetworkID, GameSystem.instance.SpawnPlayer(false, supposeNetworkID, playerid));
    }
    public static void Client_Handle_PlayerQuit(Connection c, packet packet)
    { 
        int NetworkID = packet.Readint();
        SteamManager.client.GetPlayerByNetworkID[NetworkID].Disconnect();
        SteamManager.client.GetPlayerByNetworkID.Remove(NetworkID);
    }
    public static void Client_Handle_ReceivedPlayerMovement(Connection c,packet packet)
    {
        int NetworkID = packet.Readint();
        Debug.Log($"Updating {NetworkID} Position.");

        Vector3 pos = packet.Readvector3();
        Quaternion headrot = packet.Readquaternion();
        float yrot = packet.Readfloat();
        SteamManager.client.GetPlayerByNetworkID[NetworkID].playermovement.SetMovement(pos, headrot, yrot);
    }
}



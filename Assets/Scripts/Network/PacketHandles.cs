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
        float clienttime = packet.Readfloat();
        if (text == PacketSend.TestRandomUnicode)
        {
            Debug.Log($"Confirmed {p.SteamName}, successfully connected, delay:{DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - clienttime}ms");

        }
    }




    public static async void Client_Handle_test(Connection c, packet packet)
    {
        string text = packet.ReadstringUNICODE();
        float Servertime = packet.Readfloat();

        if (text == PacketSend.TestRandomUnicode)
        {
            Debug.Log($"Confirmed connected from server. delay:{DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - Servertime}ms");

        }
        await Task.Delay(5);
        Debug.Log("Sending Test Packet");
        PacketSend.Client_Send_test();

    }
    public static void Client_Handle_InitRoomInfo(Connection c, packet packet)
    {
        int numplayer = packet.Readint();

        for (int i = 0; i < numplayer; i++)
        {
            GameObject instance = Resources.Load<GameObject>("Assets/Character/Reimu/prefab/Reimu");

            ulong playerid = packet.Readulong();
            PlayerMain p = UnityEngine.Object.Instantiate(instance, Vector3.zero, Quaternion.identity).GetComponent<PlayerMain>();
            p.NetworkID = i;
            p.PlayerID = playerid;
            SteamManager.client.GetPlayerByNetworkID.Add(i, p);
        }
    }
    public static void Client_Handle_NewPlayerJoin(Connection c, packet packet)
    {
        ulong playerid = packet.Readulong();
        GameObject instance = Resources.Load<GameObject>("Assets/Character/Reimu/prefab/Reimu");

        PlayerMain p = UnityEngine.Object.Instantiate(instance, Vector3.zero, Quaternion.identity).GetComponent<PlayerMain>();
        int supposeNetworkID = SteamManager.client.GetPlayerByNetworkID.Count;
        p.NetworkID = supposeNetworkID;
        p.PlayerID = playerid;
        SteamManager.client.GetPlayerByNetworkID.Add(supposeNetworkID, p);
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
        Vector3 pos = packet.Readvector3();
        SteamManager.client.GetPlayerByNetworkID[NetworkID].transform.position = pos;
    }
}



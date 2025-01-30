using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PacketHandles_Method
{
    public static void Server_Handle_test(NetworkPlayer p, packet packet)
    {
        Debug.Log($"Received Message from {p.SteamName} : {packet.ReadstringUNICODE()}");
    }
    public static async void Client_Handle_test(Connection c,packet packet)
    {
        Debug.Log($"Received Message from Server : {packet.ReadstringUNICODE()}");
        await Task.Delay(5);
        Debug.Log("Sending Test Packet");
        PacketSend.Client_Send_test(c);

    }

}

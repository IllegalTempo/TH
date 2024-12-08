using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public class GameServer : SocketManager
{
    int maxplayer;
    int currentplayer;
    public static Dictionary<ulong,NetworkPlayer> players = new Dictionary<ulong, NetworkPlayer>();

    private delegate void PacketHandle(NetworkPlayer n ,packet p);
    private static Dictionary<int, PacketHandle> ServerPacketHandles;
    public GameServer()
    {
        this.maxplayer = 8;
    }
    public void DisconnectAll()
    {
        players.Clear();
        foreach (Connection item in Connected)
        {
            item.Close();
        }
    }
    public Result SendPacket(ulong steamid,packet p)
    {
        return players[steamid].SendPacket(p);
    }
    private void initiatelizePacketHandler()
    {
        ServerPacketHandles = new Dictionary<int, PacketHandle>()
        {
            { 0,PacketHandles_Method.Server_Handle_test },
        };
    }
    
    public override async void OnConnected(Connection connection, ConnectionInfo info)
    {
        initiatelizePacketHandler();
        base.OnConnected(connection, info);
        Debug.Log(new Friend(info.Identity.SteamId).Name + " is Connected!");
        await Task.Delay(5);
        Debug.Log("Sending Test Packet");

        PacketSend.Server_Send_test(players[info.Identity.SteamId.Value]);
    }
    public override void OnConnecting(Connection connection, ConnectionInfo info)
    {
        base.OnConnecting(connection, info);
        
        if (currentplayer < maxplayer)
        {
            currentplayer++;
            
            Debug.Log(new Friend(info.Identity.SteamId).Name + " is connecting");
            players.Add(info.Identity.SteamId.Value, new NetworkPlayer(info.Identity.SteamId, connection));
            connection.Accept();
        } else
        {
            Debug.Log(new Friend(info.Identity.SteamId).Name + " cannot connected as the server is full");
            connection.Close();
        }


    }
    public override void OnDisconnected(Connection connection, ConnectionInfo info)
    {
        base.OnDisconnected(connection, info);
        Debug.Log(new Friend(info.Identity.SteamId).Name + " is Disconnected.");
        players.Remove(info.Identity.SteamId.Value);

    }
    public override unsafe void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        base.OnMessage(connection,identity,data,size,messageNum,recvTime,channel);
        byte[] bytedata = new byte[size];
        Marshal.Copy(data, bytedata, 0, size);
        using (packet packet = new packet(bytedata))
        {

            ServerPacketHandles[packet.Readint()](players[identity.SteamId],packet);

        }
    }

}

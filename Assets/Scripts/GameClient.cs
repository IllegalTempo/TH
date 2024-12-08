using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System;
using Connection = Steamworks.Data.Connection;
using System.Runtime.InteropServices;

public class GameClient : ConnectionManager
{
    private delegate void PacketHandle(Connection c,packet p);
    private static Dictionary<int, PacketHandle> ClientPacketHandles;
    private void initiatelizePacketHandler()
    {
        ClientPacketHandles = new Dictionary<int, PacketHandle>()
        {
            { 0,PacketHandles_Method.Client_Handle_test },
        };
    }
    public override void OnConnected(ConnectionInfo info)
    {
        base.OnConnected(info);
        Debug.Log("Successfully Connected to " + new Friend(info.Identity.SteamId).Name);
        initiatelizePacketHandler();

    }
    public override void OnConnecting(ConnectionInfo info)
    {
        base.OnConnecting(info);
        Debug.Log("Connecting to " + new Friend(info.Identity.SteamId).Name + "...");


    }
    public override void OnDisconnected(ConnectionInfo info)
    {
        base.OnDisconnected(info);
        Debug.Log("Disconnected to " + new Friend(info.Identity.SteamId).Name);
    }
    public override unsafe void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        base.OnMessage(data, size, messageNum, recvTime, channel);


        byte* bytepointer = (byte*)data.ToPointer();
        byte[] bytedata = new byte[size];
        Marshal.Copy(data, bytedata, 0, size);
        using (packet packet = new packet(bytedata))
        {
            int packetid = packet.Readint();
            Debug.Log("Received packet: " + packetid);
            ClientPacketHandles[packetid](Connection, packet);

        }
    }
}

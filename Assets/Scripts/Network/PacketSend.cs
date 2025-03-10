using Steamworks;
using Steamworks.Data;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PacketSend
{
    public enum ServerPackets
    {
        Test_Packet = 0,
        RoomInfoOnPlayerEnterRoom = 1,
        UpdatePlayerEnterRoomForExistingPlayer = 2,
        PlayerQuit = 3,
        DistributeMovement = 4,
    };
    public static string TestRandomUnicode = "幻想鄉是一個與外界隔絕的神秘之地，其存在自古以來便被視為傳說而流傳。";
    public static Result Server_Send_test(NetworkPlayer pl)
    {
        using (packet p = new packet((int)ServerPackets.Test_Packet))
        {
            p.Write(pl.NetworkID);
            p.WriteUNICODE(TestRandomUnicode);
            p.Write(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            return pl.SendPacket(p);

        };
    }
    public static Result Server_DistributeMovement(int SourceNetworkID, Vector3 pos)
    {
        using (packet p = new packet((int)ServerPackets.DistributeMovement))
        {
            p.Write(SourceNetworkID);
            p.Write(pos);

            return BroadcastPacket(SourceNetworkID,p);

        };
    }
    public static Result Server_Send_NewPlayerJoined(ConnectionInfo newplayer)
    {
        using (packet p = new packet((int)ServerPackets.UpdatePlayerEnterRoomForExistingPlayer))
        {
            p.Write(newplayer.Identity.SteamId);
            p.Write(SteamManager.server.GetPlayer(newplayer).NetworkID);
            return BroadcastPacket(newplayer, p);

        };
    }
    public static Result Server_Send_PlayerQuit(int NetworkID) //who quitted
    {
        using (packet p = new packet((int)ServerPackets.PlayerQuit))
        {
            p.Write(NetworkID);

            return BroadcastPacket(p);

        };
    }
    private static Result BroadcastPacket(packet p)
    {
        return BroadcastPacket(9999, p);
    }
    private static Result BroadcastPacket(ConnectionInfo i, packet p)
    {
        return BroadcastPacket(SteamManager.server.GetPlayer(i).NetworkID, p);
    }
    private static Result BroadcastPacket(ulong ExcludeID, packet p)
    {
        return BroadcastPacket(SteamManager.server.players[ExcludeID].NetworkID, p);
    }
    private static Result BroadcastPacket(int excludeid, packet p)
    {
        for (int i = 0; i < SteamManager.server.currentplayer; i++)
        {
            NetworkPlayer sendtarget = SteamManager.server.GetPlayer(i);
            if (sendtarget.NetworkID != excludeid)
            {
                if (sendtarget.SendPacket(p) != Result.OK)
                {
                    Debug.Log("Result Error when broadcasting packet");
                    return Result.Cancelled;
                }
            }

        }
        return Result.OK;
    }

    public static Result Server_Send_InitRoomInfo(NetworkPlayer target, int NumPlayer)
    {
        using (packet p = new packet((int)ServerPackets.RoomInfoOnPlayerEnterRoom))
        {
            p.Write(NumPlayer);
            p.Write(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            for (int i = 0; i < NumPlayer; i++)
            {
                p.Write(SteamManager.server.GetSteamID.ElementAt(i).Key);
                p.Write(SteamManager.server.GetSteamID[i]); //given information (SteamID)
            }
            return target.SendPacket(p);
        }
    }



    //Area for client Packets!
    public enum ClientPackets
    {
        Test_Packet = 0,
        SendPosition = 1,
    };
    public static Result Client_Send_test()
    {
        using (packet p = new packet((int)ClientPackets.Test_Packet))
        {
            p.WriteUNICODE(TestRandomUnicode);

            return SendToServer(p);


        };
    }
    public static Result Client_Send_Position(Vector3 pos)
    {
        using (packet p = new packet((int)ClientPackets.SendPosition))
        {
            p.Write(pos);
            return SendToServer(p);
        }
    }
    private static Result SendToServer(packet p)
    {
        return PacketSendingUtils.SendPacketToConnection(SteamManager.client.GetServer(), p);
    }
}

public class PacketSendingUtils
{
    public static Result SendPacketToConnection(Connection c, packet p)
    {
        byte[] data = p.GetPacketData();
        IntPtr datapointer = Marshal.AllocHGlobal(data.Length);
        Marshal.Copy(data, 0, datapointer, data.Length);
        Result r = c.SendMessage(datapointer, data.Length, SendType.Reliable);
        Marshal.FreeHGlobal(datapointer); //Free memory allocated
        return r;
    }
}

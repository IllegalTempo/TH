using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Unity.VisualScripting;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using Steamworks.Data;
using UnityEditor;

public class SteamManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
    }
    private async void Start()
    {
        if (GameInformation.instance.MainNetwork == null)
        {
            GameInformation.instance.MainNetwork = this;
        }
        else
        {
            Debug.Log("SteamManager Already Exist");
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
        try
        {
            SteamClient.Init(480, true);
            SteamNetworkingUtils.InitRelayNetworkAccess();

        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
#if UNITY_EDITOR

        EditorApplication.playModeStateChanged += OnExit;
#endif

        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyGameCreated += OnLobbyGameCreated;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += OnFriendJoinLobby;
        bool success = await CreateLobby();
        if(!success)
        {
            Debug.Log("Create Lobby Failed");
        }
    }

#if UNITY_EDITOR
    private void OnExit(PlayModeStateChange change)
    {
        if(change == PlayModeStateChange.ExitingPlayMode)
        {
            OnDestroy();
        }
    }
#endif
    private void OnApplicationQuit()
    {
        OnDestroy();
    }


    private void OnDestroy()
    {

        if (server != null)
        {
            server.DisconnectAll();
            server = null;
        }
        if(client != null)
        {
            client.Close();
            client = null;
        }
        SteamClient.Shutdown();
    }
    public async Task<bool> CreateLobby()
    {
        try
        {
            var createLobbyOutput = await SteamMatchmaking.CreateLobbyAsync(8);
            if (!createLobbyOutput.HasValue)
            {
                Debug.Log("Lobby created but not correctly instantiated");
                throw new Exception();
            }

            Debug.Log("Successfully Created Lobby");

            return true;
        }
        catch (Exception exception)
        {
            Debug.Log("Failed to create multiplayer lobby");
            Debug.Log(exception.ToString());
            return false;
        }
    }
    private void Update()
    {
        if(server!=null)
        {
            server.Receive();
        }
        if(client != null)
        {
            client.Receive();
        }
    }

    public static GameServer server;
    public static GameClient client;
    private void OnLobbyCreated(Result r,Lobby l)
    {
        l.SetFriendsOnly();
        l.SetJoinable(true);
        Debug.Log($"Lobby ID: {l} Result: {r} Starting Game Server...");
        
        server = SteamNetworkingSockets.CreateRelaySocket<GameServer>(8964);
        GameSystem.instance.SpawnPlayer(true, 0, SteamClient.SteamId);

        l.SetGameServer(SteamClient.SteamId);
    }
    private void OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId id)
    {
        if (id == SteamClient.SteamId) return;
        Debug.Log($"Connecting To Relay Server: {ip}:{port}, {id}");
        if (client == null)
        {
            client = SteamNetworkingSockets.ConnectRelay<GameClient>(id);
        }
    }
    private void OnLobbyEntered(Lobby l)
    {
        if (l.Owner.Id == SteamClient.SteamId) return;
        if(client == null)
        {
            SteamId serverid = new SteamId();
            uint ip = 0;
            ushort port = 0;
            l.GetGameServer(ref ip,ref port,ref serverid);
            Debug.Log($"Connecting To Relay Server: {ip}:{port}, {serverid}");

            client = SteamNetworkingSockets.ConnectRelay<GameClient>(serverid, 8964);
        }
    }
    private async void OnFriendJoinLobby(Lobby lobby, SteamId id)
    {
        if(server != null)
        {
            server.Close();
        }
        RoomEnter result = await lobby.Join();
        if(result != RoomEnter.Success)
        {
            Debug.Log($"Failed To Join Lobby from {(new Friend(id)).Name}");
        }
    }
}

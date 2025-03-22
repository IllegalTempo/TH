using Steamworks;
using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("<1> UI Element for Mission")]
    [SerializeField]
    private TMP_Text MissionTitle;
    [SerializeField]
    private TMP_Text MissionDescription;
    [SerializeField]
    private GameObject CrosshairObject;
    
    [Header("<2> UI Element for ClickTips")]
    [SerializeField]
    private TMP_Text key;
    [SerializeField]
    private TMP_Text keydesc;
    [SerializeField]
    private GameObject clicktips;

    [Header("<3> UI Element for PlaceIntro")]
    [SerializeField]
    private TMP_Text PlaceIntro;


    [Header("<4> UI Element for Loading Screen")]
    public GameObject LoadingScreenGameObject;
    public TMP_Text progressloading;

    [Header("<5> UI Element for InBattle UI ")]
    public TMP_Text Healthnumber;
    public TMP_Text PointNumber;
    public TMP_Text PowerNumber;
    public Slider HealthBar;

    [Header("<6> UI Element for Room Creation")]
    public TMP_Text PrefixText;
    public TMP_Text CoreText;
    public TMP_Text SuffixText;

    [Header("<7> UI Element for Boons(Spellcard)")]
    public GameObject BoonDisplayInstance;

    [Header("<8> UI Element for Character Upgrade")]
    public Slider EXP;
    public TMP_Text LevelText;
    public GameObject BoonsLayoutGroup;
    private GameObject[] BoonsObject = new GameObject[0];

    [Header("<9> UI Element for Multiplayer UI")]
    public TMP_Text RoomID;
    public Button RoomIDCopy;
    public Button JoinButton;
    public TMP_InputField enterWorldID;
    public TMP_Text StatusText;

    [Header("Message System")]
    public GameObject MessageInstance;
    public Transform MessageLayout;


    [Header("<;> Grouping Canvas")]
    public GameObject Mission;
    public GameObject Inventory;
    public GameObject InBattleUIObject;
    public GameObject RandomRoomDisplay;
    public GameObject BoonsDisplay;
    public GameObject NetworkUI;
    public Inventory invUI;
    public bool prefixsetted = true;
    public bool corefixsetted = true;
    public bool suffixsetted = true;

    public ulong UIRoomIDBuffer;
    public static GameUIManager instance;
    private System.Random r = new System.Random();
    private string rs = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890,./;'[]-=!@#$%^&*)";
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            OpenNetworkUI();
        }
    }
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }
    public void NewMessage(string message)
    {
        MessageObject msobj = Instantiate(MessageInstance, MessageLayout).GetComponent<MessageObject>();
        msobj.Init(message);
    }
    public void CopyRoomID()
    {
        GUIUtility.systemCopyBuffer = UIRoomIDBuffer.ToString();
    }
    public void OpenNetworkUI()
    {
        ulong roomID = GameInformation.instance.CurrentLobby.Id;
        Cursor.lockState = CursorLockMode.None;
        NetworkUI.SetActive(true);
        UIRoomIDBuffer = roomID;
        RoomID.text = roomID.ToString();
    }
    public async void OnClickJoinWorld()
    {
        ulong WorldID;
        bool success = ulong.TryParse(enterWorldID.text,out WorldID);
        StatusText.text = "Saving...";
        GameInformation.instance.currentsave.Saving();
        if(success)
        {
            
            Lobby lobby = new(WorldID);
            RoomEnter result;
            if (lobby.Id.IsValid)
            {
                StatusText.text = $"Enter Room Created by {lobby.Owner.Name}";

                result = await lobby.Join();
                StatusText.text = result.ToString();
                GameInformation.instance.MainNetwork.CreateGameLobby();

            }


        } else
        {
            StatusText.text = "Error not a valid World ID Format";
        }
    }
    public void StartRollRoom()
    {
        RandomRoomDisplay.SetActive(true);
        prefixsetted = false;
        suffixsetted = false;
        corefixsetted = false;
    }
    private string RandomString(int length)
    {
        string result = "";
        for(int i = 0; i < length;i++)
        {
            result += rs[r.Next(0,rs.Length)];
        }
        return result;
    }
    public void ConfirmClickNextRoom()
    {
        RandomRoomDisplay.SetActive(false);
    }
    public void SetXP(float xp_percentage)
    {
        EXP.value = xp_percentage;

    }
    public void StartBoonsChoosingMenu(int[] boonids)
    {
        for(int i = 0; i < BoonsObject.Length;i++)
        {
            Destroy(BoonsObject[i]);
        }
        BoonsObject = new GameObject[boonids.Length];
        BoonsDisplay.SetActive(true);
        for(int i = 0; i < boonids.Length;i++)
        {
            SpellCardDisplay d = Instantiate(BoonDisplayInstance, BoonsLayoutGroup.transform).GetComponent<SpellCardDisplay>();
            BoonsObject[i] = d.gameObject;
            d.InitSpellCardDisplay(GameInformation.instance.BoonInforms[boonids[i]],i);
        }
    }
    private void Start()
    {
        InBattleUIObject.SetActive(false);
        RandomRoomDisplay.SetActive(false);
        CrosshairObject.SetActive(false);
        PlaceIntro.gameObject.SetActive(false);
        Mission.gameObject.SetActive(false);
        Inventory.SetActive(false);
        NetworkUI.SetActive(false);
        clicktips.SetActive(false);
        LoadingScreenGameObject.SetActive(false);
        RandomRoomDisplay.SetActive(false);
        BoonsDisplay.SetActive(false);
        StartRepeatingMethod();

        DontDestroyOnLoad(gameObject);
    }
    private IEnumerator RepeatingMethodCoroutine()
    {
        // Create a reusable WaitForSeconds object
        // This is more efficient than creating a new one each iteration
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {

            if (!prefixsetted) { PrefixText.text = RandomString(6); }
            if (!corefixsetted) { CoreText.text = RandomString(8); }
            if (!suffixsetted) { SuffixText.text = RandomString(6); }
            // Wait for 0.1 seconds before the next call
            yield return wait;
        }
    }
    void OnDisable()
    {
        // Clean up when the component is disabled
        StopRepeatingMethod();
    }
    private Coroutine repeatingCoroutine;

    

    public void StartRepeatingMethod()
    {
        // Stop any existing coroutine first to prevent duplicates
        if (repeatingCoroutine != null)
        {
            StopCoroutine(repeatingCoroutine);
        }

        // Start a new coroutine and store its reference
        repeatingCoroutine = StartCoroutine(RepeatingMethodCoroutine());
    }

    public void StopRepeatingMethod()
    {
        // Stop the coroutine when no longer needed
        if (repeatingCoroutine != null)
        {
            StopCoroutine(repeatingCoroutine);
            repeatingCoroutine = null;
        }
    }
    public void SetPrefix(string prefix)
    {
        prefixsetted = true;
        PrefixText.text = LocalizationSettings.StringDatabase.GetLocalizedString("RoomArguments", "Prefix_"+prefix);
    }
    public void SetSuffix(string suffix)
    {
        suffixsetted = true;
        SuffixText.text = LocalizationSettings.StringDatabase.GetLocalizedString("RoomArguments", "Suffix_" + suffix);
    }
    public void SetCore(string core)
    {
        corefixsetted = true;
        CoreText.text = LocalizationSettings.StringDatabase.GetLocalizedString("RoomArguments", "Core_" + core);
    }
    public IEnumerator SetRoomArgument(string prefix,string core, string suffix)
    {
        Debug.Log($"Rolled Argument: {prefix} {core} {suffix}");
        yield return new WaitForSeconds(1);

        SetPrefix(prefix);
        yield return new WaitForSeconds(1);
        SetCore(core);
        yield return new WaitForSeconds(1);
        SetSuffix(suffix);

    }
    public void NewPlaceIntro(string title,bool translate)
    {
        clicktips.SetActive(false);

        CrosshairObject.SetActive(true);
        PlaceIntro.gameObject.SetActive(true);
        if(translate)
        {
            PlaceIntro.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", title);
        } else
        {
            PlaceIntro.text = title;
        }
    }
    public void EnterBattleState()
    {
        InBattleUIObject.SetActive(true);
        NetworkUI.SetActive(false);
    }
    private void SetMission(string title,string description)
    {
        CrosshairObject.SetActive(true);
        MissionTitle.text = title;
        MissionDescription.text = description;  
    }
    public void StartPlayerControl()
    {
        clicktips.SetActive(true);

    }
    public void EndPlaceIntro()
    {
        PlaceIntro.gameObject.SetActive(false);
    }

    
    public void StartInteraction()
    {
        StartKeyTips(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "interact"), "F");
    }
    public void StartKeyTips(string desc, string key)
    {
        keydesc.gameObject.SetActive(true);

        keydesc.text = desc;
        this.key.text = key;
    }
    public void EndInteractionTips()
    {
        keydesc.gameObject.SetActive(false);
    }
}

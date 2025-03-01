using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text MissionTitle;
    [SerializeField]
    private TMP_Text MissionDescription;
    [SerializeField]
    private GameObject CrosshairObject;
    [SerializeField]
    private TMP_Text keydesc;
    [SerializeField]
    private TMP_Text key;

    [SerializeField]
    private TMP_Text PlaceIntro;

    [SerializeField]
    private GameObject Mission;
    [SerializeField]
    private GameObject Inventory;
    [SerializeField]
    private GameObject clicktips;

    public GameObject LoadingScreenGameObject;
    public TMP_Text progressloading;
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

    private void Start()
    {
        GameInformation.instance.ui = this;


        CrosshairObject.SetActive(false);
        PlaceIntro.gameObject.SetActive(false);
        Mission.gameObject.SetActive(false);
        Inventory.SetActive(false);
        clicktips.SetActive(false);
        LoadingScreenGameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
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

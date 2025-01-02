using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("Speech Bubble")]
    public speechbubble SpeechBubble;
    public TMP_Text SpeechBubbleText;
    public Image SpeechBubbleBG;
    private float SpeechBubbleLife;
    private bool immortal;
    public void NewSpeechBubble(string text,float live,bool immortal,int type)
    {
        this.immortal = immortal;
        SpeechBubbleText.text = text;
        SpeechBubble.gameObject.SetActive(true);

    }
    public void NewTranslatedSpeechBubble(string text, float live, bool immortal, int type)
    {
        NewSpeechBubble(LocalizationSettings.StringDatabase.GetLocalizedString("NPC", text),live,immortal,type);

    }
    private void Start()
    {
        SpeechBubble.SpeechSender = this.gameObject;
    }
    private void Update()
    {
        if(SpeechBubbleLife <= 0 && !immortal)
        {
            SpeechBubble.gameObject.SetActive(false);
        } else
        {
            SpeechBubbleLife -= Time.deltaTime;
        }
    }
}

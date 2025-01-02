using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HakureiShrine : MonoBehaviour
{
    //References
    public NPC Marisa;
    delegate void SceneAction();
    private Dictionary<int,SceneAction> GetSceneAction;
    private const int SceneID = 0;
    //On Load
    void Start()
    {
        GetSceneAction = new Dictionary<int, SceneAction>()
        {
            { 0,Marisa_Ask_1}
        };
    }
    private void NextSceneAction()
    {
        GameInformation.instance.currentsave.GetBestPriorityAction(SceneID);
    }

    private void Marisa_Ask_1()
    {
        Marisa.NewTranslatedSpeechBubble("MARISA_ASK_1",3,false,0);
    }

}

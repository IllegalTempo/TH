using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellCardDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text SpellName;
    [SerializeField]
    private TMP_Text SpellDescription;
    [SerializeField]
    private Image SpellIcon;
    [SerializeField]
    private Image SpellCardTexture;
    [SerializeField]
    private TMP_Text SpellClickTip;
    private int DisplayPos;
    private PlayerMain owner;
    private int b;
    private string key;
    public void InitSpellCardDisplay(BoonInform inform ,int cardpositionnum)
    {
        this.b = inform.BoonID;
        SpellName.text = inform.Name;
        key = cardpositionnum + "";
        SpellDescription.text = inform.Description;
        SpellCardTexture.sprite = GameInformation.instance.SpellCardTexture[inform.Rarity];
        SpellIcon.sprite = Resources.Load<Sprite>($"boon/player/{inform.BoonID}");
        SpellClickTip.text = cardpositionnum + "";
        owner = GameInformation.instance.LocalPlayer.GetComponent<PlayerMain>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(key) && owner.CanChooseNewBoon)
        {
            owner.AddBoons(b);

        }
    }
}

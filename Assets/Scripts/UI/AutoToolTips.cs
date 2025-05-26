using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoToolTips : MonoBehaviour
{
    [SerializeField]
    private TMP_Text tooltips;
    private void Start()
    {
        tooltips.text = KeyMap.Interact2.ToString();
    }
}

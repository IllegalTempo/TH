using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class speechbubble : MonoBehaviour
{
    public GameObject SpeechSender;
    private Vector3 SpeechOffset = new Vector3(6,6,0);
    // Update is called once per frame
    
    void Update()
    {
        transform.position = SpeechSender.transform.position + Camera.main.transform.rotation * SpeechOffset;
        transform.rotation = Camera.main.transform.rotation;    
    }
}

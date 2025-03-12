using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageObject : MonoBehaviour
{
    public TMP_Text Message;
    public void Init(string text)
    {
        Message.text = text;

    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }    
}

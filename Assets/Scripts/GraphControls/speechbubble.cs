using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class speechbubble : MonoBehaviour
{
    public GameObject SpeechSender;
    private Vector3 SpeechOffset = new Vector3(4.5f, 4.5f, 0);
    public TMP_Text textobject;
    public Animator a;
    // Update is called once per frame
    public void DisableObject()
    {
        gameObject.SetActive(false);
        GameSystem.instance.NextSceneAction();

    }
    private void Start()
    {
        a.Play("FadeIn");
    }
    public void FadeoutObject()
    {
        a.Play("FadeOut");
    }
    void Update()
    {
        transform.position = SpeechSender.transform.position + Camera.main.transform.rotation * SpeechOffset;
        transform.rotation = Camera.main.transform.rotation;    
    }
}

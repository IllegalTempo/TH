using PathCreation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    //[Header("Speech Bubble Variables")]
    //public speechbubble SpeechBubble;
    private float SpeechBubbleLife;
    private bool immortal;
    //public Outline outlineclass;
    public PathCreator path;
    public Animator animator;
    public float speed = 5;
    public GameObject Head;
    float distanceTravelled;
    
    private void Start()
    {
        //SpeechBubble.SpeechSender = this.gameObject;
        //outlineclass.enabled = false;
    }
    public void MoveTo(PathCreator p)
    {
        path = p;
        animator.SetBool("moving",true);
        
    }
    private void OnStop()
    {
        path = null;
        animator.SetBool("moving", false);
    }
    private void Update()
    {
        Vector3 lookdir = GameInformation.instance.LocalPlayer.transform.position - transform.position;
        Head.transform.right = lookdir;
        lookdir.y = 0;
        transform.right = lookdir;

        //if (Input.GetKeyDown(KeyMap.Interact))
        //{
        //    outlineclass.interacted = true;
        //    outlineclass.OutlineColor = Color.red;

        //    outlineclass.UpdateMaterialProperties();

        //}
        //if (Input.GetKeyUp(KeyMap.Interact))
        //{
        //    outlineclass.interacted = false;
        //    outlineclass.OutlineColor = Color.white;

        //    outlineclass.UpdateMaterialProperties();

        //}
        if (path != null)
        {
            distanceTravelled += speed * Time.deltaTime;

            transform.position = path.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
            transform.forward = -path.path.GetDirection(distanceTravelled, EndOfPathInstruction.Stop);
            if (path.path.GetPointAtTime(1, EndOfPathInstruction.Stop) == transform.position)
            {
                OnStop();
            }
        }
        //if ((SpeechBubbleLife < 0 && !immortal) || outlineclass.interacted)
        //{
        //    if (!SpeechBubble.isActiveAndEnabled) return;
        //    SpeechBubbleLife = 0;
        //    SpeechBubble.FadeoutObject();
        //} else
        //{
        //    SpeechBubbleLife -= Time.deltaTime;
        //}
    }
}

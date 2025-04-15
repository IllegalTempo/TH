using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class Note
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public Note(string name, string description, Sprite icon)
    {
        Name = name;
        Description = description;
        Icon = icon;
    }
}
public class NoteSelection : MonoBehaviour
{
    public Image NoteIcon;
    public TMP_Text NoteName;
    public TMP_Text NoteDescription;
    public Note note;
    public void InitializeNote(string Name,string Desc, Sprite icon)
    {
        NoteIcon.sprite = icon;
        NoteName.text = Name;
        NoteDescription.text = Desc;
    }
    public void SelectNote()
    {
        GameSystem.instance.SelectNote(this);
    }
}

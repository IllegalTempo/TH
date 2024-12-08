using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiMain : MonoBehaviour
{
    public bool sex;
    public Color PrimaryColor;
    public Color SecondaryColor;
    public Color HairColor;
    public float speed;
    public GameObject[] MaleHairObjects;
    public GameObject[] MaleClothObjects;
    public Animator animator;
    public GameObject[] FeMaleHairObjects;
        
    public GameObject[] FeMaleClothObjects;
    private GameObject HairObject;
    private GameObject ClothObject;
    public SkinnedMeshRenderer Eye;

    public void Initialize(bool sex,Color a,Color b, Color c, float speed)
    {
        this.speed = speed;
        animator.speed = speed/5;
        this.sex = sex;
        Eye.material.SetColor("_BaseColor", c);
        Debug.Log($"NPC Initialized: {sex} , {a} , {b} , {c}");

        if (sex)
        {
            int Hairindex = Random.Range(0,MaleHairObjects.Length);
            int Clothindex = Random.Range(0, MaleClothObjects.Length);

            PrimaryColor = a; SecondaryColor = b; HairColor = c;
            HairObject = MaleHairObjects[Hairindex];
            HairObject.SetActive(true);
            MaleHairObjects[Hairindex] = null;
            ClothObject = MaleClothObjects[Clothindex];
            ClothObject.SetActive(true);
            MaleClothObjects[Clothindex] = null;
        }
        else
        {
            int Hairindex = Random.Range(0, FeMaleHairObjects.Length);
            int Clothindex = Random.Range(0, FeMaleClothObjects.Length);

            PrimaryColor = a; SecondaryColor = b; HairColor = c;
            HairObject = FeMaleHairObjects[Hairindex];
            HairObject.SetActive(true);
            FeMaleHairObjects[Hairindex] = null;
            ClothObject = FeMaleClothObjects[Clothindex];
            ClothObject.SetActive(true);
            FeMaleClothObjects[Clothindex] = null;
        }
        
        SkinnedMeshRenderer Clothren = ClothObject.GetComponent<SkinnedMeshRenderer>();

        Clothren.material.SetColor("_PatternColor", a);
        Clothren.material.SetColor("_PatternSecondColor",b);
        if (Clothren.materials.Length > 1)
        {
            Clothren.materials[1].SetColor("_BaseColor", c);

        }
        //HairObject.GetComponent<SkinnedMeshRenderer>().material.SetColor("_BaseColor",c);
        foreach (GameObject g in MaleHairObjects)
        {

            Destroy(g);

        }
        foreach (GameObject g in MaleClothObjects)
        {
            Destroy(g);

        }
        foreach (GameObject g in FeMaleHairObjects)
        {

            Destroy(g);

        }
        foreach (GameObject g in FeMaleClothObjects)
        {
            Destroy(g);

        }
        GetComponent<AiMovement>().speed = speed;
    }
}

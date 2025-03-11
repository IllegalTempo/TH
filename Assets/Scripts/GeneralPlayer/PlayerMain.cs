using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMain : MonoBehaviour
{
    public int NetworkID;
    public ulong PlayerID;
    public float health;
    public float maxHealth;
    public int power;
    public int points;
    public int level = 1;
    private int CurrentLevelXP = 0;
    private int CurrentLevelMaxXP = 4;
    public GeneralWeapon CurrentWeapon;
    public GameObject CurrentWeaponCollider;
    public bool IsLocal;
    public int CurrentWeaponID;
    public Animator animator;
    public Movement playermovement;
    public PlayerInventory inventory;
    public GameObject soul;
    public Transform hand;
    public Transform HighLightObject;
    private RaycastHit rch;
    public Rigidbody rb;
    public ParticleSystem OnHitEffect;
    public bool InBattle;
    public Dictionary<int, List<Baseboon>> BoonTypeIDMapGroup = new();
    public bool CanChooseNewBoon;
    public void Disconnect()
    {
        Destroy(gameObject);
    }
    public void SwitchScene(string newscenename,Vector3 spawnpoint)
    {

        bool IsCutScene = newscenename.Contains("CUTSCENE");
        bool IsBattle = newscenename == "InBattle";
        Debug.Log("In Battle: " + IsBattle);

        gameObject.SetActive(!IsCutScene);
        if (IsBattle)
        {

            OnEnterBattle(spawnpoint);
        }

        rb.velocity = Vector3.zero;
        transform.position = spawnpoint;
    }
    public void Localisation()
    {
        IsLocal = true;
        GameInformation.instance.LocalPlayer = gameObject;
        GameInformation.instance.ui.gameObject.SetActive(true);
        playermovement.cam.gameObject.SetActive(false);
    }
    public void DeLocalisation()
    {
        IsLocal = false;
        rb.useGravity = false;
        Destroy(playermovement.cam.gameObject);
    }
    public void LevelUP()
    {
        CanChooseNewBoon = true;
        CurrentLevelXP -= CurrentLevelMaxXP;

        CurrentLevelMaxXP = GameInformation.instance.GetMaxXP(level+1);
        level++;
        GameInformation.instance.ui.LevelText.text = "Level " + level;
        int[] boons = { UnityEngine.Random.Range(0, GameInformation.instance.BoonInforms.Length - 1) };
        GameInformation.instance.ui.StartBoonsChoosingMenu(boons);
    }
    public void AddXP()
    {
        CurrentLevelXP++;
        GameInformation.instance.ui.SetXP(((float)CurrentLevelXP / (float)CurrentLevelMaxXP));
        if (CurrentLevelXP >= CurrentLevelMaxXP)
        {
            LevelUP();
        }
    }
    private void Update()
    {
        if (HighLightObject != null)
        {
            HighLightObject.gameObject.GetComponent<Outline>().enabled = false;
            HighLightObject = null;
        }
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rch, Mathf.Infinity))
        {
            if (rch.transform.gameObject.GetComponent<Outline>() != null)
            {

                HighLightObject = rch.transform;
                HighLightObject.gameObject.GetComponent<Outline>().enabled = true;

            }





        }
    }
    public void AddBoons(int BoonID)
    {
        CanChooseNewBoon = false;
        GameInformation.instance.ui.BoonsDisplay.SetActive(false);
        BoonInform info;
        Baseboon b = (Baseboon)gameObject.AddComponent(GameInformation.instance.GetBoon(BoonID, out info));
        b.inform = info;
        Debug.Log($"Added Boon<{info.BoonID}>: " + info.Name);
        if (BoonTypeIDMapGroup.ContainsKey(info.BoonType))
        {
            BoonTypeIDMapGroup[info.BoonType].Add(b);
        }
        else
        {
            BoonTypeIDMapGroup.Add(info.BoonType, new List<Baseboon> { b });
        }
        if(CurrentLevelXP >= CurrentLevelMaxXP)
        {
            LevelUP();
        }
    }
    private void Start()
    {
        health = maxHealth;
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();
        playermovement = GetComponent<Movement>();
        ChooseWeapon((int)GameInformation.Weapon.HAKUREI_FLUTE);
    }
    private void ChangeInHealth()
    {
        GameInformation.instance.ui.Healthnumber.text = health.ToString();
        GameInformation.instance.ui.HealthBar.value = health / maxHealth;
    }


    public void Damage(float damage, Vector3 collidepos)
    {
        OnHitEffect.transform.position = collidepos;
        OnHitEffect.Play();
        animator.SetTrigger("Damaged");
        health -= damage;
        ChangeInHealth();
        OnDamage();
    }
    public void Heal(float health)
    {
        float result = this.health += health;
        if (result > maxHealth)
        {
            this.health = maxHealth;
        }

        ChangeInHealth();

        OnHeal();
    }
    public void OnEnterBattle(Vector3 SpawnPoint)
    {
        playermovement.cam.gameObject.SetActive(true);
        transform.position = SpawnPoint;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        InBattle = true;
    }
    public void OnExitBattle(Vector3 SpawnPoint)
    {
        transform.position = SpawnPoint;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        InBattle = false;
    }
    private int OnHurtBoonTypeID = (int)BoonInform.ContinuouseBoonTypes.PlayerOnHurtBoon;
    private int OnAttackBoonTypeID = (int)BoonInform.ContinuouseBoonTypes.PlayerAttackBoon;
    private void OnDamage()
    {
        if (BoonTypeIDMapGroup.ContainsKey(OnHurtBoonTypeID))
        {
            List<Baseboon> DamageBoons = BoonTypeIDMapGroup[OnHurtBoonTypeID];
            foreach (Baseboon b in DamageBoons)
            {
                b.OnDamaged();
            }
        }


    }
    private void OnHeal()
    {

    }
    public void OnAttack()
    {
        if (BoonTypeIDMapGroup.ContainsKey(OnAttackBoonTypeID))
        {
            List<Baseboon> AttackBoons = BoonTypeIDMapGroup[OnAttackBoonTypeID];
            foreach (Baseboon b in AttackBoons)
            {
                b.OnAttack();
            }
        }
    }
    private void ChangeInPower()
    {
        GameInformation.instance.ui.PowerNumber.text = power + "";
    }
    private void CHangeInPoint()
    {
        GameInformation.instance.ui.PointNumber.text = points + "";
    }
    public void OnPickUpPower()
    {
        power++;
        ChangeInPower();
        AddXP();
    }
    public void OnPickUpPoint()
    {
        points++;
        CHangeInPoint();
    }
    public void OnHit(string hitinform)
    {

    }
    public void ChooseWeapon(int WeaponID)
    {
        int type = GameInformation.instance.WeaponID2TypeID[WeaponID];
        if (CurrentWeapon != null) { Destroy(CurrentWeapon); Destroy(CurrentWeaponCollider); }
        CurrentWeapon = Instantiate(Resources.Load<GameObject>(GameInformation.instance.WeaponPrefabPath[WeaponID]), hand).GetComponent<GeneralWeapon>();
        CurrentWeaponCollider = Instantiate(Resources.Load<GameObject>(GameInformation.instance.WeaponPath[type] + "HitDetect"), transform);

        CurrentWeapon.name = CurrentWeapon.name.Replace("(Clone)", "").Trim();
        CurrentWeaponCollider.name = CurrentWeaponCollider.name.Replace("(Clone)", "").Trim();
        //Invidividually instantiate them is because sometimes the collider doesn't move with the weapon (Often)
        animator.Rebind();
        CurrentWeaponID = WeaponID;
    }
}

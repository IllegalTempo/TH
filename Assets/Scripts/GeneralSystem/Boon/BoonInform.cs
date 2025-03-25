using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoonInform
{
    public string Name;
    public string Description;
    public int Rarity;
    public int BoonID;
    public int BoonType;
    public bool WeaponBoon;
    public enum ContinuouseBoonTypes
    {
        PlayerAttackBoon = 0,
        PlayerOnHurtBoon = 1,
        PlayerKillBoon = 2,

    }
    public enum TriggerOnAttachBoonTypes
    {
        ProjectileCount = 100,
        ProjectileSize = 101,
        ProjectileVelocity = 102,
        MoveSpeed = 103,

    }
    public BoonInform(string name, string description, int rarity, int boonid, int boontype, bool weaponBoon)
    {
        Name = name;
        Description = description;
        Rarity = rarity;
        BoonID = boonid;
        BoonType = boontype;
        WeaponBoon = weaponBoon;
    }
}


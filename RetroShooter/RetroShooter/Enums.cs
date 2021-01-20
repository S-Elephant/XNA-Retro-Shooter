using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroShooter
{
    public enum eProjectile { Round, Boom1, Boom1Enemy, MG1, Missile, MissileM45, Missile45, MG2 }
    public enum eEnemyGunType { AutoAim, Boom1Enemy, MG1, Missile, DualMissile45, MG2}
    public enum ePickupType { HP, Boom1, Aim, Missile, Speed, Shield, DualMissile, ShieldRegen} // IMPORTANT: Also increase the max randomizer in Baseenemy.Die() when adding a new one or it will never drop.
    public enum eVisual { Explosion01 }
    public enum eDropRateMod { None = -1, Low = 7, Normal = 15, High = 20 }
    public enum eShopItem { HPHeal, ShieldHeal, ShieldUpg, ShieldRegen, SpeedUpg, Nuclear, UpgOffensiveTier, UpgDefensiveTier, Interest, HPRegen }
    public enum eControlType { Keyboard = 0, Mouse = 1 }
    public enum ePlaneType { Normal = 0, Dmg = 1, Regen = 2, Speed = 3, Tough = 4 }
}

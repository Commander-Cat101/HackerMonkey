using MelonLoader;
using BTD_Mod_Helper;
using HackerTower;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.TowerSets;
using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Unity;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Utils;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Mods;
using Assets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Unity.Bloons.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Simulation.Towers.Projectiles.Behaviors;
using Assets.Scripts.Simulation;
using HarmonyLib;
using Il2CppSystem;
using Assets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using Assets.Scripts.Simulation.Bloons.Behaviors;
using Assets.Scripts.Unity.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api;
using Assets.Scripts.Unity.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Displays2d;

[assembly: MelonInfo(typeof(HackerTower.HackerTower), ModHelperData.Name, ModHelperData.Version, "Commander__Cat")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace TowerDisplays
{
    public class Hacker000Display : ModTowerDisplay<Hacker>
    {
        // Copy the Boomerang Monkey display
        public override string BaseDisplay => Game.instance.model.GetTowerFromId("Benjamin 10").display.GUID;

        public override bool UseForTower(int[] tiers)
        {
            return tiers.Max() < 5;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
           SetMeshTexture(node, Name);
           SetMeshTexture(node, Name, 1);
        }
    }
    public class Hacker050Display : ModTowerDisplay<Hacker>
    {
        // Copy the Boomerang Monkey display
        public override string BaseDisplay => Game.instance.model.GetTowerFromId("Benjamin 10").display.GUID;

        public override bool UseForTower(int[] tiers)
        {
            return tiers[1] == 5;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
            SetMeshTexture(node, Name, 1);
        }
    }
    public class Hacker005Display : ModTowerDisplay<Hacker>
    {
        // Copy the Boomerang Monkey display
        public override string BaseDisplay => Game.instance.model.GetTowerFromId("Alchemist-005").display.GUID;

        public override bool UseForTower(int[] tiers)
        {
            return tiers[2] == 5;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            node.RemoveBone("AlchemistRig:Propjectile_R");
        }
    }
    public class Hacker500Display : ModTowerDisplay<Hacker>
    {
        // Copy the Boomerang Monkey display
        public override string BaseDisplay => Game.instance.model.GetTowerFromId("GlueGunner-400").display.GUID;

        public override bool UseForTower(int[] tiers)
        {
            return tiers[0] == 5;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
        }
    }
}
namespace Displays2d
{
    public class BulletDisplay : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "HackerBullet");
        }
    }
}
namespace HackerTower
{

    public class HackerTower : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            ModHelper.Msg<HackerTower>("HackerTowerLoaded!");
        }
    }
    public class Hacker : ModTower
    {

        public override string TowerSet => TowerSetType.Magic;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 550;
        public override int TopPathUpgrades => 5;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 5;
        public override string Description => "The Hacker joined the game";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.range = 50;
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            attackModel.range = 50;
            attackModel.weapons[0].projectile.ApplyDisplay<BulletDisplay>();
            projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead;
        }
    }
    public class FasterKeyboard : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => MIDDLE;
        public override int Tier => 1;
        public override int Cost => 150;
        public override string Description => "Faster keyboard allows faster attacks";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            attackModel.weapons[0].Rate *= .8f;
        }
    }
    public class Firewall : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => MIDDLE;
        public override int Tier => 2;
        public override int Cost => 1200;
        public override string Description => "Codes a firewall to stop the bloons";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            var Firewall = Game.instance.model.GetTowerFromId("WizardMonkey-020").behaviors.First(a => a.name.Contains("Wall")).Cast<AttackModel>().Duplicate();
            Firewall.name = "Firewall.weapon";
            Firewall.weapons[0].animateOnMainAttack = false;
            towerModel.AddBehavior(Firewall);
        }
    }
    public class HackerBlast : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => MIDDLE;
        public override int Tier => 3;
        public override int Cost => 1800;
        public override string Description => "Now shoots deadly bombs at the bloons";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            projectile.GetDamageModel().damage += 1;
            attackModel.weapons[0].Rate *= .8f;
            projectile.pierce += 1;
            var Bomb = Game.instance.model.GetTowerFromId("BombShooter-202").GetAttackModel().Duplicate();
            Bomb.name = "Bomb.weapon";
            towerModel.AddBehavior(Bomb);
        }
    }
    public class BoostedWeaponary : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => MIDDLE;
        public override int Tier => 4;
        public override int Cost => 16000;
        public override string Description => "Ability: Hacks nearby monkeys weapons making them attack faster";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            var Ability = Game.instance.model.GetTowerFromId("MonkeyVillage-040").GetBehavior<AbilityModel>().Duplicate();
            Ability.AddBehavior(Game.instance.model.GetTowerFromId("Benjamin 3").GetBehavior<AbilityModel>().GetBehavior<CreateEffectOnAbilityModel>());
            var arms = Ability.GetBehavior<CallToArmsModel>();
            arms.GetBuffIndicatorModel().iconName = "BoostedWeaponary-Icon";
            arms.useRadius = false;
            towerModel.AddBehavior(Ability);
            towerModel.GetBehavior<AbilityModel>().icon = GetSpriteReference(mod, "BoostedWeaponary-Icon");
            attackModel.weapons[0].Rate *= .5f;
            projectile.GetDamageModel().damage *= 3;
        }
    }
    public class WhiteHatHacker : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => MIDDLE;
        public override int Tier => 5;
        public override int Cost => 36000;
        public override string Description => "Ability: Hacks nearby monkeys weapons making them attack at high speeds for a long period of time";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            var ability = Game.instance.model.GetTowerFromId("MonkeyVillage-050").GetBehavior<AbilityModel>().Duplicate();
            ability.AddBehavior(Game.instance.model.GetTowerFromId("Benjamin 3").GetBehavior<AbilityModel>().GetBehavior<CreateEffectOnAbilityModel>());
            ability.GetBehavior<CallToArmsModel>().Lifespan = ability.cooldown *= .9f;
            towerModel.RemoveBehavior<AbilityModel>();
            var arms = ability.GetBehavior<CallToArmsModel>();
            towerModel.AddBehavior(ability);
            towerModel.GetBehavior<AbilityModel>().icon = GetSpriteReference(mod, "WhiteHatHacker-Icon");
            attackModel.weapons[0].Rate *= .5f;
            projectile.GetDamageModel().damage *= 3;
        }
    }
    public class BetterMouse : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => TOP;
        public override int Tier => 1;
        public override int Cost => 500;
        public override string Description => "Better mouse allows better hacking and can also see camo";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            projectile.GetDamageModel().damage += 1;
            towerModel.AddBehavior(new OverrideCamoDetectionModel("camo", true));
        }
    }
    public class RegrowFirewall : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => TOP;
        public override int Tier => 2;
        public override int Cost => 600;
        public override string Description => "Creates a firewall stopping all regrow bloons";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            var grow1 = Game.instance.model.GetTowerFromId("MonkeyVillage-010").GetBehavior<AddBehaviorToBloonInZoneModel>().Duplicate();
            towerModel.AddBehavior(grow1);
        }
    }
    public class GlitchPaste : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => TOP;
        public override int Tier => 3;
        public override int Cost => 3600;
        public override string Description => "Shoots glitch paste at the bloons damaging and slowing them";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            var paste = Game.instance.model.GetTowerFromId("GlueGunner-203").GetAttackModel().Duplicate();
            paste.name = "Paste_Weapon";
            paste.range = towerModel.range;
            towerModel.AddBehavior(paste);
        }
    }
    public class CorrosivePaste : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => TOP;
        public override int Tier => 4;
        public override int Cost => 3800;
        public override string Description => "Corrosive Paste now melts faster into the bloons";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            foreach (var attacks in towerModel.GetAttackModels())
            {
                if (attacks.name.Contains("Paste"))
                {
                    attacks.weapons[0].projectile.pierce += 5;
                    attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = .03f;
                    attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 1f;
                }
            }
        }
    }
    public class Disintegrater : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => TOP;
        public override int Tier => 5;
        public override int Cost => 59000;
        public override string Description => "Disintegrates all but the biggest blimps";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            foreach (var attacks in towerModel.GetAttackModels())
            {
                if (attacks.name.Contains("Paste"))
                {             
                    attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = .001f;
                    attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 3f;
                    attacks.weapons[0].projectile.GetBehavior<SlowModel>().Multiplier = .003f;
                }
            }
        }
    }
    public class PenetratingHacks : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => BOTTOM;
        public override int Tier => 1;
        public override int Cost => 250;
        public override string Description => "Hacks can pass through more bloons";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            projectile.pierce += 1;
        }
    }
    public class HackShotgun : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => BOTTOM;
        public override int Tier => 2;
        public override int Cost => 750;
        public override string Description => "Now shoots 3 hack rays";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            projectile.pierce -= 1;
            attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0, 35, null, false);
        }
    }
    public class GoldenBananas : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => BOTTOM;
        public override int Tier => 3;
        public override int Cost => 2400;
        public override string Description => "Nearby bananas are worth a bit more";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            towerModel.AddBehavior(new MonkeyCityIncomeSupportModel("MonkeyCityIncomeSupportModel_", true, 1.5f, null, "MonkeyCityBuff", "BuffIconVillagexx4"));
        }
    }
    public class HackedCash : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => BOTTOM;
        public override int Tier => 4;
        public override int Cost => 8700;
        public override string Description => "Nearby monkey generate much more money";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            towerModel.GetBehavior<MonkeyCityIncomeSupportModel>().incomeModifier = 2.5f;
        }
    }
    public class GlitchedRiches : ModUpgrade<Hacker>
    {
        public override string Portrait => "Hacker-Portrait.png";
        public override int Path => BOTTOM;
        public override int Tier => 5;
        public override int Cost => 67000;
        public override string Description => "Nearby monkey make so much money that you will never run out again";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            towerModel.GetBehavior<MonkeyCityIncomeSupportModel>().incomeModifier = 10;
        }
    }
}

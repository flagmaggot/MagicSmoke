﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace MagicSmoke
{
    public class MagicSmoke : ETGModule
    {
        private ConsoleCommandGroup _MsGroup;
        private ConsoleCommandGroup _SetGroup;
        private ConsoleCommandGroup _GetGroup;
        protected static AutocompletionSettings _AutocompletionSettings;

        private Dictionary<string, PlayerStats.StatType> _Stats = new Dictionary<string, PlayerStats.StatType>
        {
            ["movespeed"] = PlayerStats.StatType.MovementSpeed,
            ["firerate"] = PlayerStats.StatType.RateOfFire,
            ["spread"] = PlayerStats.StatType.Accuracy,
            ["health"] = PlayerStats.StatType.Health,
            ["coolness"] = PlayerStats.StatType.Coolness,
            ["damage"] = PlayerStats.StatType.Damage,
            ["projspeed"] = PlayerStats.StatType.ProjectileSpeed,
            ["guncapacity"] = PlayerStats.StatType.AdditionalGunCapacity,
            ["itemcapacity"] = PlayerStats.StatType.AdditionalItemCapacity,
            ["ammomult"] = PlayerStats.StatType.AmmoCapacityMultiplier,
            ["reloadspeed"] = PlayerStats.StatType.ReloadSpeed,
            ["shotpiercing"] = PlayerStats.StatType.AdditionalShotPiercing,
            ["knockbackmult"] = PlayerStats.StatType.KnockbackMultiplier,
            ["globalpricemult"] = PlayerStats.StatType.GlobalPriceMultiplier,
            ["curse"] = PlayerStats.StatType.Curse,
            ["bulletscale"] = PlayerStats.StatType.PlayerBulletScale,
            ["clipmult"] = PlayerStats.StatType.AdditionalClipCapacityMultiplier,
            ["shotbounces"] = PlayerStats.StatType.AdditionalShotBounces,
            ["blanksperfloor"] = PlayerStats.StatType.AdditionalBlanksPerFloor,
            ["shadowbulletchance"] = PlayerStats.StatType.ShadowBulletChance,
            ["throwngundamage"] = PlayerStats.StatType.ThrownGunDamage,
            ["dodgedamage"] = PlayerStats.StatType.DodgeRollDamage,
            ["damagetobosses"] = PlayerStats.StatType.DamageToBosses,
            ["enemyprojspeed"] = PlayerStats.StatType.EnemyProjectileSpeedMultiplier,
            ["extremeshadowchance"] = PlayerStats.StatType.ExtremeShadowBulletChance,
            ["chargemult"] = PlayerStats.StatType.ChargeAmountMultiplier,
            ["rangemult"] = PlayerStats.StatType.RangeMultiplier,
            ["dodgedistance"] = PlayerStats.StatType.DodgeRollDistanceMultiplier,
            ["dodgespeed"] = PlayerStats.StatType.DodgeRollSpeedMultiplier,
            ["tarnishclipmult"] = PlayerStats.StatType.TarnisherClipCapacityMultiplier,
            ["moneymult"] = PlayerStats.StatType.MoneyMultiplierFromEnemies
        };

        private static Action<string[]> _StatSet;
        private static Action<string[]> _StatGet;
        private static Action<string[]> _StatGetAll;


        public void SetStat(PlayerController player, PlayerStats.StatType type, float val)
        {
            GameManager.Instance.PrimaryPlayer.stats.SetBaseStatValue(type, val, player);
        }

        public float GetStat(PlayerController player, PlayerStats.StatType type)
        {
            //return stats.GetStatValue(type);
            return GameManager.Instance.PrimaryPlayer.stats.GetStatValue(type);
        }

        public void AddStatCommands(PlayerController player, string name, PlayerStats.StatType type)
        {
            _SetGroup.AddUnit(name, (args) => SetStat(player, type, float.Parse(args[0])));
            _GetGroup.AddUnit(name, (args) => ETGModConsole.Log(GetStat(player, type).ToString()));
        }

        public override void Init()
        {
            _AutocompletionSettings = new AutocompletionSettings((input) => {
                List<string> ret = new List<string>();
                foreach (var stat in _Stats)
                {
                    if (stat.Key.AutocompletionMatch(input.ToLower()))
                    {
                        ret.Add(stat.Key);
                    }
                }
                return ret.ToArray();
            });

            _StatGet = (args) => {
                if (_Stats.ContainsKey(args[0]))
                {
                    ETGModConsole.Log(_Stats[args[0]] + " value is: <color=#ff0000ff>" + GameManager.Instance.PrimaryPlayer.stats.GetStatValue(_Stats[args[0]]) + "</color>");
                }
                else
                    ETGModConsole.Log($"Please check your command and try again");
            };

            _StatSet = (args) => {
                
                float value;
                if (_Stats.ContainsKey(args[0]) && float.TryParse(args[1], out value))
                {
                    GameManager.Instance.PrimaryPlayer.stats.SetBaseStatValue(_Stats[args[0]], value, GameManager.Instance.PrimaryPlayer);
                    ETGModConsole.Log(_Stats[args[0]] + " set to: <color=#ff0000ff>" + value + "</color>");
                }
                else
                    ETGModConsole.Log($"Please check your command and try again");
            };

            _StatGetAll = (args) => {
                ETGModConsole.Log("Available stats:");
                foreach (var stat in _Stats)
                {
                    ETGModConsole.Log($"- {stat.Key} <color=#ff0000ff>" + GameManager.Instance.PrimaryPlayer.stats.GetStatValue(stat.Value) + "</color>");
                }
            };

            ETGModConsole.Commands.AddGroup("ms", (string[] args) =>
            {
                ETGModConsole.Log("<size=100><color=#ff0000ff>MagicSmoke v1 by flagmaggot</color></size>", false);
                ETGModConsole.Log("<size=100><color=#ff0000ff>DOES NOT WORK WHILE IN THE BREACH</color></size>", false);
                ETGModConsole.Log("<size=100><color=#ff0000ff>--------------------------------</color></size>", false);
                ETGModConsole.Log("Use \"ms help\" for help!", false);

            });
            


            ETGModConsole.Commands.GetGroup("ms").AddUnit("help", (string[] args) =>
            {
                ETGModConsole.Log("<size=100><color=#ff0000ff>MagicSmoke v1 by flagmaggot</color></size>", false);
                ETGModConsole.Log("<size=100><color=#ff0000ff>DOES NOT WORK WHILE IN THE BREACH</color></size>", false);
                ETGModConsole.Log("<size=100><color=#ff0000ff>--------------------------------</color></size>", false);
                ETGModConsole.Log("Magic Smoke Command Reference:", false);
                ETGModConsole.Log("", false);
                ETGModConsole.Log("ms help - Displays this help", false);
                ETGModConsole.Log("ms get <stat name> - gets the state value", false);
                ETGModConsole.Log("ms set <stat name> [arg] - sets the player speed (decimal values)", false);
                ETGModConsole.Log("ms getallstats - returns all the stats and their values", false);
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("forcedualwield", (string[] args) =>
            {
                if (args.Length < 1) throw new Exception("At least 1 argument required.");
                var partner_id = int.Parse(args[0]);
                var player = GameManager.Instance.PrimaryPlayer;
                var gun = player.inventory.CurrentGun;
                var partner_gun = PickupObjectDatabase.GetById(partner_id) as Gun;
                player.inventory.AddGunToInventory(partner_gun);
                var forcer = gun.gameObject.AddComponent<DualWieldForcer>();
                forcer.Gun = gun;
                forcer.PartnerGunID = partner_gun.PickupObjectId;
                forcer.TargetPlayer = player;
                
            });

            _MsGroup = ETGModConsole.Commands.GetGroup("ms");
            _SetGroup = _MsGroup.AddUnit("set", _StatSet, _AutocompletionSettings);
            _SetGroup = _MsGroup.AddUnit("get", _StatGet, _AutocompletionSettings);
            _GetGroup = _MsGroup.AddUnit("getallstats", _StatGetAll, _AutocompletionSettings);
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }
    }
}

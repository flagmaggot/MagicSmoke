using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft;
using System.IO;
using Newtonsoft.Json;
using Gungeon;

namespace MagicSmoke
{
    public class MagicSmoke : ETGModule
    {
        private ConsoleCommandGroup _MsGroup;
        private ConsoleCommandGroup _SetGroup;
        private ConsoleCommandGroup _GetGroup;
        private ConsoleCommandGroup _SynergyChestGroup;
        protected static AutocompletionSettings _AutocompletionSettings;
        protected static AutocompletionSettings _AutocompletionChests;

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

        private Dictionary<string, Chest> _Chests = new Dictionary<string, Chest>
        {
            ["a"] = GameManager.Instance.RewardManager.A_Chest,
            ["b"] = GameManager.Instance.RewardManager.B_Chest,
            ["c"] = GameManager.Instance.RewardManager.C_Chest,
            ["d"] = GameManager.Instance.RewardManager.D_Chest,
            ["s"] = GameManager.Instance.RewardManager.S_Chest,
            ["red"] = GameManager.Instance.RewardManager.A_Chest,
            ["green"] = GameManager.Instance.RewardManager.B_Chest,
            ["blue"] = GameManager.Instance.RewardManager.C_Chest,
            ["brown"] = GameManager.Instance.RewardManager.D_Chest,
            ["black"] = GameManager.Instance.RewardManager.S_Chest
        };

        private static Action<string[]> _StatSet;
        private static Action<string[]> _StatGet;
        private static Action<string[]> _StatGetAll;
        private static Action<string[]> _GlitchSpawn;
        private static Action<string[]> _MagnificenceSet;
        private static Action<string[]> _MagnificenceGet;

        public void SetStat(PlayerController player, PlayerStats.StatType type, float val)
        {
            GameManager.Instance.PrimaryPlayer.stats.SetBaseStatValue(type, val, player);
        }

        public float GetStat(PlayerController player, PlayerStats.StatType type)
        {
            return GameManager.Instance.PrimaryPlayer.stats.GetStatValue(type);
        }

        public void AddStatCommands(PlayerController player, string name, PlayerStats.StatType type)
        {
            _SetGroup.AddUnit(name, (args) => SetStat(player, type, float.Parse(args[0])));
            _GetGroup.AddUnit(name, (args) => ETGModConsole.Log(GetStat(player, type).ToString()));
        }

        public override void Init()
        {
            System.IO.Directory.CreateDirectory("MagicSmokeSaves");

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

            _AutocompletionChests = new AutocompletionSettings((input) => {
                List<string> ret = new List<string>();
                foreach (var stat in _Chests)
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

            _GlitchSpawn = (args) => {
                if (_Chests.ContainsKey(args[0]))
                {
                    Chest glitchedchest = _Chests[args[0]];
                    glitchedchest.ForceGlitchChest = true;
                    glitchedchest.BecomeGlitchChest();
                    IntVector2 basePosition = new IntVector2((int)GameManager.Instance.PrimaryPlayer.transform.position.x, (int)GameManager.Instance.PrimaryPlayer.transform.position.y);
                    Chest.Spawn(glitchedchest, basePosition);
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
                ETGModConsole.Log("ms spawnrainbowsynergy - spawns rainbow synergy chest", false);
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("forcedualwield", (string[] args) =>
            {
                if (_Chests.ContainsKey(args[0]))
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
                }
            });

            _MagnificenceSet = (args) => {
                float value;
                if (float.TryParse(args[0], out value))
                {
                    GameManager.Instance.PrimaryPlayer.stats.AddFloorMagnificence(value);
                    ETGModConsole.Log("Magnificence set to: <color=#ff0000ff>" + value + "</color>");
                }
                else
                    ETGModConsole.Log($"Please check your command and try again");
            };


            _MagnificenceGet = (args) => {
                ETGModConsole.Log("Magnificence set to: <color=#ff0000ff>" + GameManager.Instance.PrimaryPlayer.stats.Magnificence + "</color>");
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

            ETGModConsole.Commands.GetGroup("ms").AddUnit("spawnsynergy", (string[] args) =>
            {
                Chest synergy_Chest = GameManager.Instance.RewardManager.Synergy_Chest;
                if(args.Length != 0)
                {

                     synergy_Chest.IsRainbowChest = true;
                }
                IntVector2 basePosition = new IntVector2((int)GameManager.Instance.PrimaryPlayer.transform.position.x, (int)GameManager.Instance.PrimaryPlayer.transform.position.y);
                Chest.Spawn(synergy_Chest, basePosition);
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("spawnrainbowsynergy", (string[] args) =>
            {
                Chest synergy_Chest = GameManager.Instance.RewardManager.Synergy_Chest;
                synergy_Chest.IsRainbowChest = true;
                IntVector2 basePosition = new IntVector2((int)GameManager.Instance.PrimaryPlayer.transform.position.x, (int)GameManager.Instance.PrimaryPlayer.transform.position.y);
                Chest.Spawn(synergy_Chest, basePosition);
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("savesettings", (string[] args) =>
            {
                SaveSettings(args[0]);
                ETGModConsole.Log("Settings file saved as: " + args[0] + ".json");
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("loadsettings", (string[] args) =>
            {
                LoadSettings(args[0]);
            });

            _MsGroup = ETGModConsole.Commands.GetGroup("ms");
            _SetGroup = _MsGroup.AddUnit("set", _StatSet, _AutocompletionSettings);
            _SetGroup = _MsGroup.AddUnit("get", _StatGet, _AutocompletionSettings);
            _SynergyChestGroup = _MsGroup.AddUnit("spawnglitched", _GlitchSpawn, _AutocompletionChests);
            _GetGroup = _MsGroup.AddUnit("getallstats", _StatGetAll, _AutocompletionSettings);

            _SetGroup = _MsGroup.AddUnit("getmagnificence", _MagnificenceGet, _AutocompletionSettings);
            _SetGroup = _MsGroup.AddUnit("setmagnificence", _MagnificenceSet, _AutocompletionSettings);
        }



        private void SaveSettings(string filename)
        {
            //JObject characterObject = new JObject();
            //if (Enum.IsDefined(typeof(PlayableCharacters), GameManager.Instance.PrimaryPlayer.characterIdentity))
            //    characterObject["character"] = GameManager.Instance.PrimaryPlayer.characterIdentity.ToString();

            JObject statsObject = new JObject();
            foreach (var stat in _Stats)
            {
                statsObject[stat.Key] = GameManager.Instance.PrimaryPlayer.stats.GetStatValue(stat.Value);
            }

            JObject gunObject = new JObject();
            foreach (Gun i in GameManager.Instance.PrimaryPlayer.inventory.AllGuns)
            {
                gunObject[i.name] = i.PickupObjectId;
            }

            JObject passiveObject = new JObject();
            foreach (PassiveItem i in GameManager.Instance.PrimaryPlayer.passiveItems)
            {
                passiveObject[i.name] = i.PickupObjectId;
            }

            JObject activeObject = new JObject();
            foreach (PlayerItem i in GameManager.Instance.PrimaryPlayer.activeItems)
            {
                activeObject[i.name] = i.PickupObjectId;
            }

            var json = JsonConvert.SerializeObject(new { gunObject, statsObject, activeObject, passiveObject },Newtonsoft.Json.Formatting.Indented);
            //var json = JsonConvert.SerializeObject(new { characterObject, gunObject, statsObject, activeObject, passiveObject }, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("MagicSmokeSaves/" + filename + ".json", Base64Encode(json));
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void LoadSettings(string filename)
        {
            if (System.IO.File.Exists("MagicSmokeSaves/" + filename + ".json"))
            {
                var jsonFile = Base64Decode(File.ReadAllText("MagicSmokeSaves/" + filename + ".json"));
                
                var deserialized = JsonConvert.DeserializeObject<JObject>(jsonFile);
                //foreach (JProperty character in deserialized["characterObject"])
                //{
                //    if (Enum.IsDefined(typeof(PlayableCharacters), character.Value.ToString()))
                //    {
                //        string[] characterName = new string[] { character.Value.ToString() };
                //        SwitchCharacter(characterName);
                //    }
                //}

                foreach (JProperty gun in deserialized["gunObject"])
                {
                    int id = 0;
                    if (Int32.TryParse(gun.Value.ToString(), out id))
                    {
                        if (!GameManager.Instance.PrimaryPlayer.inventory.ContainsGun(id))
                        {
                            ETGModConsole.Log($"Loading gun: {gun.Name} with id: {gun.Value.ToString()}");
                            GameManager.Instance.PrimaryPlayer.inventory.AddGunToInventory(PickupObjectDatabase.GetById(id) as Gun, true);
                        }
                    }
                    else
                    {
                        ETGModConsole.Log($"Gun not in list!");
                    }
                }
                
                foreach (JProperty stat in deserialized["statsObject"])
                {
                    if (_Stats.ContainsKey(stat.Name))
                    {
                        _Stats.TryGetValue(stat.Name, out PlayerStats.StatType specificstat);
                        GameManager.Instance.PrimaryPlayer.stats.SetBaseStatValue(specificstat, (float)stat.Value, GameManager.Instance.PrimaryPlayer);
                    }
                }

                foreach (JProperty active in deserialized["activeObject"])
                {
                    int id = 0;
                    GameManager.Instance.PrimaryPlayer.RemoveAllActiveItems();
                    if (Int32.TryParse(active.Value.ToString(), out id))
                    {
                        
                        LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(id).gameObject, GameManager.Instance.PrimaryPlayer, false);
                    }
                    else
                    {
                        ETGModConsole.Log($"Active item not in list!");
                    }
                }

                foreach (JProperty passive in deserialized["passiveObject"])
                {
                    int id = 0;
                    GameManager.Instance.PrimaryPlayer.RemoveAllPassiveItems();
                    if (Int32.TryParse(passive.Value.ToString(), out id))
                    {
                        LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(id).gameObject, GameManager.Instance.PrimaryPlayer, false);
                    }
                    else
                    {
                        ETGModConsole.Log($"Passive item not in list!");
                    }
                }
            }
            else
                ETGModConsole.Log("File <color=#ff0000ff>" + filename + ".json </color>in the MagicSmokeSaves directory not found!");
        }

        public override void Start()
        {
            //throw new NotImplementedException();
        }

        public override void Exit()
        {
            //SaveSettings();
        }

        void SwitchCharacter(string[] args)
        {
            if (!ETGModConsole.ArgCount(args, 1, 2)) return;
            var prefab = (GameObject)BraveResources.Load("Player" + args[0], ".prefab");//Resources.Load("Assets/Resources/CHARACTERDB:" + args[0] + ".prefab");
            if (prefab == null)
            {
                Debug.Log(args[0] + " is not a mod character, checking if it's one of the standard characters");
                prefab = (GameObject)Resources.Load("Player" + args[0]);
            }
            if (prefab == null)
            {
                //Log("Failed getting prefab for " + args[0]);
                return;
            }

            //Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
            bool wasInGunGame = false;
            if (GameManager.Instance.PrimaryPlayer)
            {
                wasInGunGame = GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns;
            }
            GameManager.Instance.PrimaryPlayer.SetInputOverride("getting deleted");

            PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
            Vector3 position = primaryPlayer.transform.position;
            UnityEngine.Object.Destroy(primaryPlayer.gameObject);
            GameManager.Instance.ClearPrimaryPlayer();
            GameManager.PlayerPrefabForNewGame = prefab;
            PlayerController component = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>();
            GameStatsManager.Instance.BeginNewSession(component);
            PlayerController playerController;
            GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(GameManager.PlayerPrefabForNewGame, position, Quaternion.identity);
            GameManager.PlayerPrefabForNewGame = null;
            gameObject.SetActive(true);
            playerController = gameObject.GetComponent<PlayerController>();
            if (args.Length == 2)
            {
                component.SwapToAlternateCostume(null);
            }
            GameManager.Instance.PrimaryPlayer = playerController;
            playerController.PlayerIDX = 0;

            GameManager.Instance.MainCameraController.ClearPlayerCache();
            GameManager.Instance.MainCameraController.SetManualControl(false, true);
            Foyer.Instance.PlayerCharacterChanged(playerController);

            //Pixelator.Instance.FadeToBlack(0.5f, true, 0f);

            if (wasInGunGame)
            {
                GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns = true;
                while (GameManager.Instance.PrimaryPlayer.inventory.AllGuns.Count > 1)
                {
                    var gun = GameManager.Instance.PrimaryPlayer.inventory.AllGuns[1];
                    GameManager.Instance.PrimaryPlayer.inventory.RemoveGunFromInventory(gun);
                    UnityEngine.Object.Destroy(gun.gameObject);
                }
            }
        }
    }
}
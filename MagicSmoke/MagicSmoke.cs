using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft;
using System.IO;
using Newtonsoft.Json;
using Gungeon;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using Dungeonator;
using System.Text;

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
                ETGModConsole.Log("<color=#FFFF33>ms help</color> - Displays this help", false);
                ETGModConsole.Log("<color=#FFFF33>ms get <stat name></color> - gets the state value", false);
                ETGModConsole.Log("<color=#FFFF33>ms set <stat name> [arg]</color> - sets the player speed (decimal values)", false);
                ETGModConsole.Log("<color=#FFFF33>ms getallstats</color> - returns all the stats and their values", false);
                ETGModConsole.Log("<color=#FFFF33>ms forcedualwield [gunid]</color> - forces the player to dual wield, use gun names, it will autocomplete to help.", false);
                ETGModConsole.Log("<color=#FFFF33>ms spawnsynergy</color> - spawns synergy chest", false);
                ETGModConsole.Log("<color=#FFFF33>ms spawnrainbowsynergy</color> - spawns rainbow synergy chest", false);
                ETGModConsole.Log("<color=#FFFF33>ms savesettings [name</color> - saves player settings to a file [name].json", false);
                ETGModConsole.Log("<color=#FFFF33>ms loadsettings [name]</color> - loads player settings from a file [name].json", false);
                ETGModConsole.Log("<color=#FFFF33>ms spawnglitched [arg] [true]</color> - spawns a glitch chest, [arg] can be a,b,c,d,s,red,green,blue,brown,black [use true after if you want to warp to glitched level]", false);
                ETGModConsole.Log("<color=#FFFF33>ms setmagnificense [arg]</color> - sets the magnificence value", false);
                ETGModConsole.Log("<color=#FFFF33>ms getmagnificense [arg]</color> - gets the current magnificence value", false);
                ETGModConsole.Log("<color=#FFFF33>ms saveclipboard</color>  - copies the encoded settings to the clipboard", false);
                ETGModConsole.Log("<color=#FFFF33>ms loadclipboard</color> - loads the settings directly from clipboard", false);
                ETGModConsole.Log("<color=#FFFF33>ms loadclipboard [arg</color>] - loads the settings based off the encoded pasted data [arg]", false);
                ETGModConsole.Log("<color=#FFFF33>ms pollthegungeon</color> - Request from YouTuber Hutts, finds all chests on current floor and spawns 3 additional random chests.", false);
                ETGModConsole.Log("<color=#FFFF33>ms findchests</color> - gives coordinates for all chests currently on the floor (you can teleport to them by using tp <first number> <second number>", false);
                ETGModConsole.Log("<color=#FFFF33>ms addteleporters</color> - adds teleporters to almost all the rooms on the current dungeon", false);
                ETGModConsole.Log("<color=#FFFF33>ms visitallrooms</color> - marks all rooms as visited, makes all visible on minimap", false);
                ETGModConsole.Log("<color=#FFFF33>ms revealrooms</color> - makes all rooms visible (similar to Floor revealed item)", false);
                ETGModConsole.Log("<color=#FFFF33>ms dropactives</color> - drops all active items", false);
                ETGModConsole.Log("<color=#FFFF33>ms droppassives</color> - drops all passive items", false);
                ETGModConsole.Log("<color=#FFFF33>ms dropall</color> - drop all items and guns (except defauilt gun)", false);
                ETGModConsole.Log("<color=#FFFF33>ms clearall</color> - clears all active and passive items.", false);
                ETGModConsole.Log("<color=#FFFF33>ms clearpassives</color> - clears passive items", false);
                ETGModConsole.Log("<color=#FFFF33>ms clearactives</color> - clears active items", false);
                ETGModConsole.Log("<color=#FFFF33>ms dropguns</color> - drops all guns (except default gun)", false);
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("forcedualwield", DualWieldItem, _GiveAutocompletionSettings);

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
                //if(args.Length != 0)
                //{

                //     synergy_Chest.IsRainbowChest = true;
                //}
                synergy_Chest.IsRainbowChest = false;
                synergy_Chest.ForceUnlock();
                IntVector2 basePosition = new IntVector2((int)GameManager.Instance.PrimaryPlayer.transform.position.x, (int)GameManager.Instance.PrimaryPlayer.transform.position.y);
                Chest.Spawn(synergy_Chest, basePosition);
                synergy_Chest.ForceUnlock();
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("spawnrainbowsynergy", (string[] args) =>
            {
                Chest synergy_Chest = GameManager.Instance.RewardManager.Synergy_Chest;
                synergy_Chest.IsRainbowChest = true;
                IntVector2 basePosition = new IntVector2((int)GameManager.Instance.PrimaryPlayer.transform.position.x, (int)GameManager.Instance.PrimaryPlayer.transform.position.y);
                Chest.Spawn(synergy_Chest, basePosition);
                synergy_Chest.ForceUnlock();
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("savesettings", (string[] args) =>
            {
                SaveSettings(args[0]);
                ETGModConsole.Log("Settings file saved as: " + args[0] + ".json");
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("saveclipboard", (string[] args) =>
            {
                SaveSettingsToClipboard();
                ETGModConsole.Log("Settings copied to clipboard");
            });

            //ETGModConsole.Commands.GetGroup("ms").AddUnit("summon", (string[] args) =>
            //{
            //    //ResourceManager.LoadAssetBundle("./shared_auto_001/Shrine_Mirror55");
            //    AdvancedShrineController ads = new AdvancedShrineController();
            //    ads.IsBlankShrine = true;

            //    ads.Start();
            //    ETGModConsole.Log("No");
            //});

            ETGModConsole.Commands.GetGroup("ms").AddUnit("zoom", (string[] args) =>
            {
                if (args.Count() > 0)
                {
                    float zoomScale;
                    float.TryParse(args[0], out zoomScale);
                    GameManager.Instance.MainCameraController.OverrideZoomScale = zoomScale;
                    ETGModConsole.Log($"Camera zoom set to {zoomScale}");
                }
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("pollthegungeon", (string[] args) =>
            {
                List<Chest> chests = new List<Chest>();
                chests = StaticReferenceManager.AllChests;

                foreach (Chest c in chests.ToList())
                {
                    RoomHandler room = c.GetAbsoluteParentRoom();
                    while (room.GetComponentsAbsoluteInRoom<Chest>().Count() < 4)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            SpawnChests(room);
                        }
                    }
                }
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("findchests", (string[] args) =>
            {
                List<Chest> chests = StaticReferenceManager.AllChests;

                foreach (Chest c in chests)
                {
                    ETGModConsole.Log($"Chest location: <color=#ff0000ff>{c.transform.position}</color>");
                }
            });

            //ETGModConsole.Commands.GetGroup("ms").AddUnit("open", (string[] args) =>
            //{
            //    List<Chest> chests = StaticReferenceManager.AllChests;

            //    foreach (Chest c in chests)
            //    {
            //        ETGModConsole.Log($"Chest location: <color=#ff0000ff>{c.transform.position}</color>");
            //    }
            //});

            ETGModConsole.Commands.GetGroup("ms").AddUnit("addteleporters", (string[] args) =>
            {
                List<RoomHandler> rooms = GameManager.Instance.Dungeon.data.rooms;
                foreach (RoomHandler room in rooms)
                {
                    try
                    {

                        room.AddProceduralTeleporterToRoom();
                    }
                    catch (Exception)
                    {
                        //do nothing
                    }
                }
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("visitallrooms", (string[] args) =>
            {
                List<RoomHandler> rooms = GameManager.Instance.Dungeon.data.rooms;
                foreach (RoomHandler room in rooms)
                {
                    room.visibility = RoomHandler.VisibilityStatus.VISITED;
                }
                Minimap.Instance.RevealAllRooms(true);
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("revealrooms", (string[] args) =>
            {
                List<RoomHandler> rooms = GameManager.Instance.Dungeon.data.rooms;

                foreach (RoomHandler room in rooms)
                {
                    Minimap.Instance.RevealAllRooms(true);
                }
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("getroomname", (string[] args) =>
            {

                ETGModConsole.Log($"{GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom().GetRoomName()}");

            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("loadsettings", (string[] args) =>
            {
                LoadSettings(args[0]);
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("dropactives", (string[] args) =>
            {
                foreach (PlayerItem i in GameManager.Instance.PrimaryPlayer.activeItems.ToList())
                {
                    GameManager.Instance.PrimaryPlayer.DropActiveItem(i);
                }
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("droppassives", (string[] args) =>
            {
                foreach (PassiveItem i in GameManager.Instance.PrimaryPlayer.passiveItems.ToList())
                {
                    GameManager.Instance.PrimaryPlayer.DropPassiveItem(i);
                }
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("dropall", (string[] args) =>
            {
                foreach (PassiveItem i in GameManager.Instance.PrimaryPlayer.passiveItems.ToList())
                {
                    GameManager.Instance.PrimaryPlayer.DropPassiveItem(i);
                }
                foreach (PlayerItem i in GameManager.Instance.PrimaryPlayer.activeItems.ToList())
                {
                    GameManager.Instance.PrimaryPlayer.DropActiveItem(i);
                }
                foreach (Gun i in GameManager.Instance.PrimaryPlayer.inventory.AllGuns.ToList())
                {
                    GameManager.Instance.PrimaryPlayer.ForceDropGun(i);
                }
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("clearall", (string[] args) =>
            {
                GameManager.Instance.PrimaryPlayer.RemoveAllActiveItems();
                GameManager.Instance.PrimaryPlayer.RemoveAllPassiveItems();
                foreach (Gun i in GameManager.Instance.PrimaryPlayer.inventory.AllGuns.ToList())
                {
                    GameManager.Instance.PrimaryPlayer.ForceDropGun(i);
                }
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("clearactives", (string[] args) =>
            {
                GameManager.Instance.PrimaryPlayer.RemoveAllActiveItems();
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("clearpassives", (string[] args) =>
            {
                GameManager.Instance.PrimaryPlayer.RemoveAllPassiveItems();
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("dropguns", (string[] args) =>
            {
                foreach (Gun i in GameManager.Instance.PrimaryPlayer.inventory.AllGuns.ToList())
                {
                    GameManager.Instance.PrimaryPlayer.ForceDropGun(i);
                }
            });

            ETGModConsole.Commands.GetGroup("ms").AddUnit("charmall", (string[] args) =>
            {
                if (args.Count() == 0 || args[0] == "true")
                {
                    foreach (AIActor actor in StaticReferenceManager.AllEnemies)
                    {
                        //AkSoundEngine.PostEvent("Play_OBJ_enemy_charmed_01", GameManager.Instance.gameObject);
                        AIActor aiactor = actor as AIActor;
                        aiactor.CanTargetEnemies = true;
                        aiactor.CanTargetPlayers = false;
                    }
                }
                else if (args[0] == "false")
                {
                    foreach (AIActor actor in StaticReferenceManager.AllEnemies)
                    {
                        //AkSoundEngine.PostEvent("Play_OBJ_enemy_charmed_01", GameManager.Instance.gameObject);
                        AIActor aiactor = actor as AIActor;
                        aiactor.CanTargetEnemies = false;
                        aiactor.CanTargetPlayers = true;
                    }
                }
            });

            //ETGModConsole.Commands.GetGroup("ms").AddUnit("removeallcompanions", (string[] args) =>
            //{
            //    //GameManager.Instance.PrimaryPlayer.companions.RemoveAll(AIActor);
            //    GameManager.Instance.PrimaryPlayer.companions.Clear();
            //});

            ETGModConsole.Commands.GetGroup("ms").AddUnit("loadclipboard", (string[] args) =>
            {
                if (args.Count() == 0)
                    LoadSettingsFromClipBoard();
                else
                    LoadSettingsFromClipBoard(args[0]);
                ETGModConsole.Log("Loading settings from clipboard");
            });

            _GlitchSpawn = (args) => {
                if (_Chests.ContainsKey(args[0]))
                {
                    Chest glitchedchest = _Chests[args[0]];
                    glitchedchest.ForceGlitchChest = true;
                    glitchedchest.ForceUnlock();
                    glitchedchest.BecomeGlitchChest();
                    IntVector2 basePosition = new IntVector2((int)GameManager.Instance.PrimaryPlayer.transform.position.x, (int)GameManager.Instance.PrimaryPlayer.transform.position.y);
                    Chest.Spawn(glitchedchest, basePosition);
                    if (args.Count() == 2)
                        if (args[1] == "true")
                            ETGMod.Chest.OnPostOpen += glitchopen;
                }
                else
                    ETGModConsole.Log($"Please check your command and try again");
            };


            _MsGroup = ETGModConsole.Commands.GetGroup("ms");
            _SetGroup = _MsGroup.AddUnit("set", _StatSet, _AutocompletionSettings);
            _SetGroup = _MsGroup.AddUnit("get", _StatGet, _AutocompletionSettings);
            _SynergyChestGroup = _MsGroup.AddUnit("spawnglitched", _GlitchSpawn, _AutocompletionChests);
            _GetGroup = _MsGroup.AddUnit("getallstats", _StatGetAll, _AutocompletionSettings);
            _SetGroup = _MsGroup.AddUnit("getmagnificence", _MagnificenceGet, _AutocompletionSettings);
            _SetGroup = _MsGroup.AddUnit("setmagnificence", _MagnificenceSet, _AutocompletionSettings);
        }



        private void glitchopen(Chest c, PlayerController pc)
        {
            if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
            {
                PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(pc);
                if (otherPlayer && otherPlayer.IsGhost)
                {
                    //pc.StartCoroutine(GiveCoopPartnerBack2(false));
                    AkSoundEngine.PostEvent("play_obj_chest_open_01", c.gameObject);
                    AkSoundEngine.PostEvent("stop_obj_fuse_loop_01", c.gameObject);
                    PlayerController deadPlayer = (!GameManager.Instance.PrimaryPlayer.healthHaver.IsDead) ? GameManager.Instance.SecondaryPlayer : GameManager.Instance.PrimaryPlayer;
                    deadPlayer.specRigidbody.enabled = true;
                    deadPlayer.gameObject.SetActive(true);
                    deadPlayer.sprite.renderer.enabled = true;
                    deadPlayer.ResurrectFromChest(c.sprite.WorldBottomCenter);
                }
            }
            GameManager.Instance.InjectedFlowPath = "Core Game Flows/Secret_DoubleBeholster_Flow";
            Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
            GameManager.Instance.DelayedLoadNextLevel(0.5f);
            ETGMod.Chest.OnPostOpen -= glitchopen;
        }


        private static void SpawnChests(RoomHandler r)//, Chest chest)
        {
            Chest chest = new Chest();
            chest = GameManager.Instance.RewardManager.A_Chest;
            WeightedGameObject wGameObject = new WeightedGameObject();
            wGameObject.rawGameObject = chest.gameObject;
            WeightedGameObjectCollection wGameObjectCollection = new WeightedGameObjectCollection();
            wGameObjectCollection.Add(wGameObject);

            Chest spawnedChest = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(r.GetBestRewardLocation(new IntVector2(2, 1), Dungeonator.RoomHandler.RewardLocationStyle.PlayerCenter, true));
            spawnedChest.ForceUnlock();
            spawnedChest.overrideMimicChance = 0f;
        }

        void DualWieldItem(string[] args)
        {
            if (!ArgCount(args, 1, 2)) return;

            if (!GameManager.Instance.PrimaryPlayer)
            {
                ETGModConsole.Log("Couldn't access Player Controller");
                return;
            }

            string id = args[0];
            if (!Game.Items.ContainsID(id))
            {
                ETGModConsole.Log($"Invalid item ID {id}!");
                return;
            }
             
            ETGModConsole.Log("Attempting to force wield " + args[0] + " (numeric " + id + ")" + ", class " + Game.Items.Get(id).GetType());



            if (args.Length < 1) throw new Exception("At least 1 argument required.");

            var player = GameManager.Instance.PrimaryPlayer;
            var gun = player.inventory.CurrentGun;
            int idNumber;
            int.TryParse(id, out idNumber);
            var test = Game.Items[id];
            var partner_gun = PickupObjectDatabase.GetByName(test.name) as Gun;

            if (GameManager.Instance.PrimaryPlayer.inventory.AllGuns.Contains(partner_gun))
            {
                //GameManager.Instance.PrimaryPlayer.ForceDropGun(i);
                GameManager.Instance.PrimaryPlayer.ForceDropGun(partner_gun);
            }

            player.inventory.AddGunToInventory(partner_gun);
            var forcer = gun.gameObject.AddComponent<DualWieldForcer>();
            forcer.Gun = gun;
            forcer.PartnerGunID = partner_gun.PickupObjectId;
            forcer.TargetPlayer = player;
        }


        public static T GetFieldValue<T>(object obj, string fieldName)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var field = obj.GetType().GetField(fieldName, BindingFlags.Public |
                                                          BindingFlags.NonPublic |
                                                          BindingFlags.Instance);

            if (field == null)
                throw new ArgumentException("fieldName", "No such field was found.");

            if (!typeof(T).IsAssignableFrom(field.FieldType))
                throw new InvalidOperationException("Field type and requested type are not compatible.");

            return (T)field.GetValue(obj);
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

            var json = JsonConvert.SerializeObject(new { gunObject, statsObject, activeObject, passiveObject }, Newtonsoft.Json.Formatting.Indented);

            //var json = JsonConvert.SerializeObject(new { characterObject, gunObject, statsObject, activeObject, passiveObject }, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("MagicSmokeSaves/" + filename + ".json", Base64Encode(json));
        }

        private void SaveSettingsToClipboard()
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

            var json = JsonConvert.SerializeObject(new { gunObject, statsObject, activeObject, passiveObject }, Newtonsoft.Json.Formatting.Indented);
            GUIUtility.systemCopyBuffer = Base64Encode(json);
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
                //        LoadCharacter(characterName);

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
                            ETGModConsole.Log($"Gun not in list!");
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

                GameManager.Instance.PrimaryPlayer.RemoveAllActiveItems();
                foreach (JProperty active in deserialized["activeObject"])
                {
                    int id = 0;

                    if (Int32.TryParse(active.Value.ToString(), out id))
                    {
                        LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(id).gameObject, GameManager.Instance.PrimaryPlayer, false);
                    }
                    else
                    {
                        ETGModConsole.Log($"Active item not in list!");
                    }
                }

                GameManager.Instance.PrimaryPlayer.RemoveAllPassiveItems();
                foreach (JProperty passive in deserialized["passiveObject"])
                {
                    int id = 0;

                    if (Int32.TryParse(passive.Value.ToString(), out id))
                    {
                        LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(id).gameObject, GameManager.Instance.PrimaryPlayer, false);
                        ETGModConsole.Log($"Passive item {id}!");
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

        private void LoadSettingsFromClipBoard(string clipboard = "")
        {
            string encodedJson = "";
            if (clipboard == "")
            {
                ETGModConsole.Log($"{GUIUtility.systemCopyBuffer}");
                encodedJson = Base64Decode(GUIUtility.systemCopyBuffer);
            }
            else
                encodedJson = Base64Decode(clipboard);
            var deserialized = JsonConvert.DeserializeObject<JObject>(encodedJson);

            foreach (JProperty gun in deserialized["gunObject"])
            {
                int id = 0;
                if (Int32.TryParse(gun.Value.ToString(), out id))
                {
                    if (!GameManager.Instance.PrimaryPlayer.inventory.ContainsGun(id))
                    {
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

            GameManager.Instance.PrimaryPlayer.RemoveAllActiveItems();
            foreach (JProperty active in deserialized["activeObject"])
            {
                int id = 0;
                if (Int32.TryParse(active.Value.ToString(), out id))
                {
                    LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(id).gameObject, GameManager.Instance.PrimaryPlayer, false);
                }
                else
                {
                    ETGModConsole.Log($"Active item not in list!");
                }
            }

            GameManager.Instance.PrimaryPlayer.RemoveAllPassiveItems();
            foreach (JProperty passive in deserialized["passiveObject"])
            {
                int id = 0;
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

        public override void Start()
        {
            //throw new NotImplementedException();
        }

        public override void Exit()
        {
            //SaveSettings();
        }

        //[Obsolete]
        //public override void Update()
        //{
        //    ETGModConsole.Log($"Yay2");
        //    base.Update();
        //}

        protected static AutocompletionSettings _GiveAutocompletionSettings = new AutocompletionSettings(delegate (string input) {
            List<string> ret = new List<string>();
            foreach (string key in Game.Items.IDs)
            {
                if (key.AutocompletionMatch(input.ToLower()))
                {
                    Console.WriteLine($"INPUT {input} KEY {key} MATCH!");
                    ret.Add(key.Replace("gungeon:", ""));
                }
                else
                {
                    Console.WriteLine($"INPUT {input} KEY {key} NO MATCH!");
                }
            }
            return ret.ToArray();
        });


        void LoadCharacter(string[] args)
        {
            //Log("Trying to switch costume");

            if (!ArgCount(args, 1, 2)) return;
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

            Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
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

            try
            {
                GameManager.Instance.PrimaryPlayer = playerController;
                playerController.PlayerIDX = 0;

                GameManager.Instance.MainCameraController.ClearPlayerCache();
                GameManager.Instance.MainCameraController.SetManualControl(false, true);
                Foyer.Instance.PlayerCharacterChanged(playerController);

                Pixelator.Instance.FadeToBlack(0.5f, true, 0f);

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
            catch
            {
                ETGModConsole.Log($"Catching silly switch exception!");
            }
        }

        public static bool ArgCount(string[] args, int min)
        {
            if (args.Length >= min) return true;
            //Log("Error: need at least " + min + " argument(s)");
            return false;
        }

        public static bool ArgCount(string[] args, int min, int max)
        {
            if (args.Length >= min && args.Length <= max) return true;
            if (min == max)
            {
                //Log("Error: need exactly " + min + " argument(s)");
            }
            else
            {
                //Log("Error: need between " + min + " and " + max + " argument(s)");
            }
            return false;
        }
    }
}
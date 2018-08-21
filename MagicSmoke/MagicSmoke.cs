using System;
using UnityEngine;

namespace MagicSmoke
{
    public class MagicSmoke : ETGModule
    {
        public override void Exit()
        {
            throw new NotImplementedException();
        }

        public static int speed;
        public static bool speedChanged;

        static MagicSmoke()
        {
            MagicSmoke.speed = 7;
            speedChanged = false;
        }

        public MagicSmoke()
        { }

        
        public override void Init()
        {
            ETGMod.Objects.AddHook<PlayerStats>(new ComponentHook(this.NewPlayerStats));
            ETGModConsole.Commands.AddGroup("ms", (string[] args) => {
                ETGModConsole.Log("<size=100><color=#ff0000ff>MagicSmoke v1 by flagmaggot</color></size>", false);
                ETGModConsole.Log("<size=100><color=#ff0000ff>DOES NOT WORK WHILE IN THE BREACH</color></size>", false);
                ETGModConsole.Log("<size=100><color=#ff0000ff>--------------------------------</color></size>", false);
                ETGModConsole.Log("Use \"ms help\" for help!", false);
            });
            //ETGModConsole.Commands.AddGroup("ms");
            ETGModConsole.Commands.GetGroup("ms").AddUnit("help", (string[] args) =>
            {
                ETGModConsole.Log("<size=100><color=#ff0000ff>MagicSmoke v1 by flagmaggot</color></size>", false);
                ETGModConsole.Log("<size=100><color=#ff0000ff>DOES NOT WORK WHILE IN THE BREACH</color></size>", false);
                ETGModConsole.Log("<size=100><color=#ff0000ff>--------------------------------</color></size>", false);
                ETGModConsole.Log("Magic Smoke Command Reference:", false);
                ETGModConsole.Log("", false);
                ETGModConsole.Log("ms help - Displays this help", false);
                ETGModConsole.Log("ms get_player_speed - gets the player speed", false);
                ETGModConsole.Log("ms set_player_speed [arg] - sets the player speed (integers only)", false);
            });
            ETGModConsole.Commands.GetGroup("ms").AddUnit("set_player_speed", new Action<string[]>(this.setSpeedCommand));
            ETGModConsole.Commands.GetGroup("ms").AddUnit("get_player_speed", new Action<string[]>(this.getSpeedCommand));
        }

        public override void Start()
        {
            
        }

        //private PlayerStats m_playerStats;
        private void setSpeedCommand(string[] args)
        {
            if (args.Length != 0)
            {
                int value;
                if (int.TryParse(args[0], out value))
                {
                    ETGModConsole.Log(string.Concat("Player speed set to: <color=#ff0000ff>", value, "</color>"));
                    speed = value;
                    speedChanged = true;
                }
                else
                    ETGModConsole.Log(string.Concat(args[0], " is not a valid number, try again"));

            }
        }

        private void getSpeedCommand(string[] args)
        {
            ETGModConsole.Log(string.Concat("Player speed is: <color=#ff0000ff>", stats.MovementSpeed, "</color>"));
        }

        private PlayerStats stats;
        private void NewPlayerStats(Component component)
        {
            stats = component as PlayerStats;
            if(stats.GetComponent<SpeedBehavior>() == null)
                stats.gameObject.AddComponent<SpeedBehavior>();
        }

    }
}

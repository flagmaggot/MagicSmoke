using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace MagicSmoke
{
    class SpeedBehavior : MonoBehaviour
    {
        private PlayerStats m_playerStats;
        private PlayerController m_playerController;

        public SpeedBehavior()
        {
        }

        public void Start()
        {
        }

        public void Update()
        {
            if (MagicSmoke.speedChanged)
            {
                m_playerStats = base.GetComponent<PlayerStats>();
                m_playerController = base.GetComponent<PlayerController>();
                m_playerStats.SetBaseStatValue(PlayerStats.StatType.MovementSpeed, (float)MagicSmoke.speed, m_playerController);
                //Debug.Log("updating the speed: " + m_playerStats.MovementSpeed);
                MagicSmoke.speedChanged = false;
            }
        }

        

    }
}

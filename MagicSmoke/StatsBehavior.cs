//using UnityEngine;

//#pragma warning disable 0246

//namespace MagicSmoke
//{
//    class StatsBehavior : MonoBehaviour
//    {
//        private PlayerStats m_playerStats;
//        private PlayerController m_playerController;

//        public StatsBehavior()
//        {
//        }

//        public void Start()
//        {
//        }

//        public void updateStat(bool nameStatIsChanged, PlayerStats.StatType stat, float statValue)
//        {
//            if (nameStatIsChanged)
//            {
//                m_playerStats = base.GetComponent<PlayerStats>();
//                m_playerController = base.GetComponent<PlayerController>();
//                m_playerStats.SetBaseStatValue(stat, (float)statValue, m_playerController);
//                nameStatIsChanged = false;
//            }
//        }


//        public void Update()
//        {
//            updateStat(MagicSmoke.MovementSpeedChanged, PlayerStats.StatType.MovementSpeed, (float)MagicSmoke.MovementSpeed);
//            updateStat(MagicSmoke.RateOfFireChanged, PlayerStats.StatType.RateOfFire, (float)MagicSmoke.RateOfFire);
//            updateStat(MagicSmoke.AccuracyChanged, PlayerStats.StatType.Accuracy, (float)MagicSmoke.Accuracy);
//            updateStat(MagicSmoke.HealthChanged, PlayerStats.StatType.Health, (float)MagicSmoke.Health);
//            updateStat(MagicSmoke.CoolnessChanged, PlayerStats.StatType.Coolness, (float)MagicSmoke.Coolness);
//            updateStat(MagicSmoke.DamageChanged, PlayerStats.StatType.Damage, (float)MagicSmoke.Damage);
//            updateStat(MagicSmoke.ProjectileSpeedChanged, PlayerStats.StatType.ProjectileSpeed, (float)MagicSmoke.ProjectileSpeed);
//            updateStat(MagicSmoke.AdditionalGunCapacityChanged, PlayerStats.StatType.AdditionalGunCapacity, (float)MagicSmoke.AdditionalGunCapacity);
//            updateStat(MagicSmoke.AdditionalItemCapacityChanged, PlayerStats.StatType.AdditionalItemCapacity, (float)MagicSmoke.AdditionalItemCapacity);
//            updateStat(MagicSmoke.AmmoCapacityMultiplierChanged, PlayerStats.StatType.AmmoCapacityMultiplier, (float)MagicSmoke.AmmoCapacityMultiplier);
//            updateStat(MagicSmoke.AmmoCapacityMultiplierChanged, PlayerStats.StatType.AmmoCapacityMultiplier, (float)MagicSmoke.AmmoCapacityMultiplier);
//            updateStat(MagicSmoke.ReloadSpeedChanged, PlayerStats.StatType.ReloadSpeed, (float)MagicSmoke.ReloadSpeed);
//            updateStat(MagicSmoke.AdditionalShotPiercingChanged, PlayerStats.StatType.AdditionalShotPiercing, (float)MagicSmoke.AdditionalShotPiercing);
//            updateStat(MagicSmoke.KnockbackMultiplierChanged, PlayerStats.StatType.KnockbackMultiplier, (float)MagicSmoke.KnockbackMultiplier);
//            updateStat(MagicSmoke.GlobalPriceMultiplierChanged, PlayerStats.StatType.GlobalPriceMultiplier, (float)MagicSmoke.GlobalPriceMultiplier);
//            updateStat(MagicSmoke.CurseChanged, PlayerStats.StatType.Curse, (float)MagicSmoke.Curse);
//            updateStat(MagicSmoke.PlayerBulletScaleChanged, PlayerStats.StatType.PlayerBulletScale, (float)MagicSmoke.PlayerBulletScale);
//            updateStat(MagicSmoke.MovementSpeedChanged, PlayerStats.StatType.MovementSpeed, (float)MagicSmoke.MovementSpeed);
//            updateStat(MagicSmoke.MovementSpeedChanged, PlayerStats.StatType.MovementSpeed, (float)MagicSmoke.MovementSpeed);
//            updateStat(MagicSmoke.MovementSpeedChanged, PlayerStats.StatType.MovementSpeed, (float)MagicSmoke.MovementSpeed);
//            updateStat(MagicSmoke.MovementSpeedChanged, PlayerStats.StatType.MovementSpeed, (float)MagicSmoke.MovementSpeed);




//            //if (MagicSmoke.AdditionalClipCapacityMultiplierChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.AdditionalClipCapacityMultiplier, (float)MagicSmoke.AdditionalClipCapacityMultiplier, m_playerController);
//            //    MagicSmoke.AdditionalClipCapacityMultiplierChanged = false;
//            //}

//            //if (MagicSmoke.AdditionalShotBouncesChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.AdditionalShotBounces, (float)MagicSmoke.AdditionalShotBounces, m_playerController);
//            //    MagicSmoke.AdditionalShotBouncesChanged = false;
//            //}

//            //if (MagicSmoke.AdditionalBlanksPerFloorChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.AdditionalBlanksPerFloor, (float)MagicSmoke.AdditionalBlanksPerFloor, m_playerController);
//            //    MagicSmoke.AdditionalBlanksPerFloorChanged = false;
//            //}

//            //if (MagicSmoke.ShadowBulletChanceChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.ShadowBulletChance, (float)MagicSmoke.ShadowBulletChance, m_playerController);
//            //    MagicSmoke.ShadowBulletChanceChanged = false;
//            //}

//            //if (MagicSmoke.ThrownGunDamageChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.ThrownGunDamage, (float)MagicSmoke.ThrownGunDamage, m_playerController);
//            //    MagicSmoke.ThrownGunDamageChanged = false;
//            //}

//            //if (MagicSmoke.DodgeRollDamageChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.DodgeRollDamage, (float)MagicSmoke.DodgeRollDamage, m_playerController);
//            //    MagicSmoke.DodgeRollDamageChanged = false;
//            //}

//            //if (MagicSmoke.DamageToBossesChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.DamageToBosses, (float)MagicSmoke.DamageToBosses, m_playerController);
//            //    MagicSmoke.DamageToBossesChanged = false;
//            //}

//            //if (MagicSmoke.EnemyProjectileSpeedMultiplierChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.EnemyProjectileSpeedMultiplier, (float)MagicSmoke.EnemyProjectileSpeedMultiplier, m_playerController);
//            //    MagicSmoke.EnemyProjectileSpeedMultiplierChanged = false;
//            //}

//            //if (MagicSmoke.ExtremeShadowBulletChanceChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.ExtremeShadowBulletChance, (float)MagicSmoke.ExtremeShadowBulletChance, m_playerController);
//            //    MagicSmoke.ExtremeShadowBulletChanceChanged = false;
//            //}

//            //if (MagicSmoke.ChargeAmountMultiplierChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.ChargeAmountMultiplier, (float)MagicSmoke.ChargeAmountMultiplier, m_playerController);
//            //    MagicSmoke.ChargeAmountMultiplierChanged = false;
//            //}

//            //if (MagicSmoke.RangeMultiplierChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.RangeMultiplier, (float)MagicSmoke.RangeMultiplier, m_playerController);
//            //    MagicSmoke.RangeMultiplierChanged = false;
//            //}

//            //if (MagicSmoke.DodgeRollDistanceMultiplierChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.DodgeRollDistanceMultiplier, (float)MagicSmoke.DodgeRollDistanceMultiplier, m_playerController);
//            //    MagicSmoke.DodgeRollDistanceMultiplierChanged = false;
//            //}

//            //if (MagicSmoke.DodgeRollSpeedMultiplierChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();

//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.DodgeRollSpeedMultiplier, (float)MagicSmoke.DodgeRollSpeedMultiplier, m_playerController);
//            //    MagicSmoke.DodgeRollSpeedMultiplierChanged = false;
//            //}

//            //if (MagicSmoke.TarnisherClipCapacityMultiplierChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.TarnisherClipCapacityMultiplier, (float)MagicSmoke.TarnisherClipCapacityMultiplier, m_playerController);
//            //    MagicSmoke.TarnisherClipCapacityMultiplierChanged = false;
//            //}

//            //if (MagicSmoke.MoneyMultiplierFromEnemiesChanged)
//            //{
//            //    m_playerStats = base.GetComponent<PlayerStats>();
//            //    m_playerController = base.GetComponent<PlayerController>();
//            //    m_playerStats.SetBaseStatValue(PlayerStats.StatType.MoneyMultiplierFromEnemies, (float)MagicSmoke.MoneyMultiplierFromEnemies, m_playerController);
//            //    MagicSmoke.MoneyMultiplierFromEnemiesChanged = false;
//            //}


//        }

        

//    }
//}

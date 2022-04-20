using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ItemManager;
using ServerSync;
using UnityEngine;

namespace ShrinkMe
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class ShrinkMe : BaseUnityPlugin
    {
        private const string ModName = "ShrinkMe";
        private const string ModVersion = "0.0.0.1";
        private const string ModGUID = "com.zarboz.ShrinkMe";
        private static Harmony harmony = null!;

        internal static SE_Stats? ShrinkStat;
        internal static Item? HaldorPipe;
        
        ConfigSync configSync = new(ModGUID) 
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion};
        internal static ConfigEntry<bool> ServerConfigLocked = null!;
        ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
        ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        public void Awake()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            harmony = new(ModGUID);
            harmony.PatchAll(assembly);
            ServerConfigLocked = config("1 - General", "Lock Configuration", true, "If on, the configuration is locked and can be changed by server admins only.");
            configSync.AddLockingConfigEntry(ServerConfigLocked);
            ShrinkStat = ScriptableObject.CreateInstance<SE_Shrink>();
            HaldorPipe = new("odinspipe", "HaldorsMagicPipe");           //add item
            HaldorPipe.Crafting.Add(CraftingTable.Forge, 2);
            HaldorPipe.Name.English("HaldorsMagicPipe");
            HaldorPipe.Description.English("Haldor must have dropped this, wonder what it does.");
            HaldorPipe.RequiredItems.Add("Wood", 1);
            HaldorPipe.RequiredItems.Add("BlackMetal", 8);
            HaldorPipe.RequiredUpgradeItems.Add("Wood", 6);
            HaldorPipe.RequiredUpgradeItems.Add("BlackMetal", 6);
            HaldorPipe.CraftAmount = 1;
        }

        [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.Awake))]
        public class DBPatch
        {
            public static void Prefix(ObjectDB __instance)
            {
                if (__instance.m_StatusEffects.Count <= 0) return;
                __instance.m_StatusEffects.Add(ShrinkStat);
                
            }

            public static void Postfix()
            {
                var itemtoadd = HaldorPipe?.Prefab;
                itemtoadd!.GetComponent<ItemDrop>().m_itemData.m_shared.m_equipStatusEffect = ShrinkStat;
            }
        }
    }
}

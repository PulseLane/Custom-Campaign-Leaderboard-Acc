using HarmonyLib;
using IPA;
using System;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;

namespace CustomCampaignLeaderboardAcc
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public const string HarmonyId = "com.PulseLane.BeatSaber.CustomCampaignLeaderboardAcc";
        internal static Harmony harmony => new Harmony(HarmonyId);

        internal static Plugin instance { get; private set; }
        internal static string Name => "CustomCampaignLeaderboardAcc";

        [Init]
        public Plugin(IPALogger logger)
        {
            instance = this;
            Logger.log = logger;
            Logger.log.Debug("Logger initialized.");
        }

        [OnEnable]
        public void OnEnable()
        {
            ApplyHarmonyPatches();
        }

        [OnDisable]
        public void OnDisable()
        {
            RemoveHarmonyPatches();
        }

        public static void ApplyHarmonyPatches()
        {
            try
            {
                Logger.log.Debug("Applying Harmony patches.");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Logger.log.Critical("Error applying Harmony patches: " + ex.Message);
                Logger.log.Debug(ex);
            }
        }

        public static void RemoveHarmonyPatches()
        {
            try
            {
                // Removes all patches with this HarmonyId
                harmony.UnpatchAll(HarmonyId);
            }
            catch (Exception ex)
            {
                Logger.log.Critical("Error removing Harmony patches: " + ex.Message);
                Logger.log.Debug(ex);
            }
        }
    }
}

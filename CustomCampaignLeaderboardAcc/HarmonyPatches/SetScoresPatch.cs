using HarmonyLib;

namespace CustomCampaignLeaderboardAcc.HarmonyPatches
{
    [HarmonyPatch(typeof(LeaderboardTableView), "SetScores")]
    class SetScoresPatch
    {
        static void Postfix()
        {
            LeaderboardAcc.FinishLoad();
        }
    }
}

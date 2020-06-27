using BeatSaberCustomCampaigns;
using HarmonyLib;
using SongCore;
using System.Linq;
using UnityEngine;

namespace CustomCampaignLeaderboardAcc.HarmonyPatches
{

    [HarmonyPatch(typeof(CampaignChallengeLeaderboardViewController), "UpdateLeaderboards")]
    public class LeaderboardPatch
    {
        static void Postfix(CampaignChallengeLeaderboardViewController __instance, ref LeaderboardTableView ___table, ref Challenge ___lastClicked)
        {
            // Can't get data, skip execution
            if (!SongDataCore.Plugin.Songs.IsDataAvailable())
            {
                Logger.log.Debug("SongDataCore data not available");
                return;
            }

            int maxScore = 0;
            var id = ___lastClicked.FindSong().levelID;
            var beatmapLevel = Loader.BeatmapLevelsModelSO.GetBeatmapLevelIfLoaded(id);
            if (beatmapLevel == null)
            {
                Logger.log.Debug("beatmap level null");
                return;
            }
            var levelDifficultyBeatmapSets = beatmapLevel.beatmapLevelData.difficultyBeatmapSets;
            var levelDifficultyBeatmaps = levelDifficultyBeatmapSets[0].difficultyBeatmaps;
            foreach (var diff in levelDifficultyBeatmaps)
            {
                if (diff.difficulty.Equals(___lastClicked.difficulty))
                {
                    Logger.log.Debug("Found note count");
                    var noteCount = diff.beatmapData.notesCount;

                    var modifiers = ___lastClicked.modifiers.GetGameplayModifiers();
                    var gameplayModifiersModelSO = Resources.FindObjectsOfTypeAll<GameplayModifiersModelSO>().First();
                    var multiplier = gameplayModifiersModelSO.GetTotalMultiplier(modifiers);
                    maxScore = (int)(ScoreModel.MaxRawScoreForNumberOfNotes(noteCount) * multiplier);
                }
            }

            LeaderboardAcc.GiveParams(ref ___table, maxScore);
            Logger.log.Debug($"Max Score: {maxScore}");
        }
    }
}
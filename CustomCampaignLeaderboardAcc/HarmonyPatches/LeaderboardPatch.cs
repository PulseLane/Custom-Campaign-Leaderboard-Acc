using BeatSaberCustomCampaigns;
using BeatSaberCustomCampaigns.campaign;
using HarmonyLib;
using SongCore;
using System;
using System.Linq;
using UnityEngine;

namespace CustomCampaignLeaderboardAcc.HarmonyPatches
{

    [HarmonyPatch(typeof(CampaignChallengeLeaderboardViewController), "UpdateLeaderboards")]
    public class LeaderboardPatch
    {
        static void Postfix(CampaignChallengeLeaderboardViewController __instance, ref LeaderboardTableView ___table, ref Challenge ___lastClicked)
        {
            int maxScore = 0;
            try
            {
                var id = ___lastClicked.FindSong().levelID;
                var beatmapLevel = Loader.BeatmapLevelsModelSO.GetBeatmapLevelIfLoaded(id);
                if (beatmapLevel == null)
                {
                    Logger.log.Debug("beatmap level null");
                    return;
                }
                var levelDifficultyBeatmapSets = beatmapLevel.beatmapLevelData.difficultyBeatmapSets;
                var missionData = ___lastClicked.GetMissionData(new Campaign()); // campaign doesn't matter here
                var levelDifficultyBeatmaps = levelDifficultyBeatmapSets.First(x => x.beatmapCharacteristic.Equals(missionData.beatmapCharacteristic)).difficultyBeatmaps;
                foreach (var diff in levelDifficultyBeatmaps)
                {
                    if (diff.difficulty.Equals(___lastClicked.difficulty))
                    {
                        Logger.log.Debug("Found note count");
                        var noteCount = diff.beatmapData.notesCount;
                        maxScore = ScoreModel.MaxRawScoreForNumberOfNotes(noteCount);
                    }
                }

                LeaderboardAcc.GiveParams(ref ___table, maxScore);
                Logger.log.Debug($"Max Score: {maxScore}");
            }
            catch (Exception e)
            {
                Logger.log.Debug($"Could not load level data: {e}");
                return;
            }
        }
    }
}
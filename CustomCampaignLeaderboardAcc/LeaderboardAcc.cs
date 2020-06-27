using BS_Utils.Utilities;
using System;
using TMPro;
using UnityEngine;

namespace CustomCampaignLeaderboardAcc
{
    public static class LeaderboardAcc
    {
        private static LeaderboardTableView _table;
        private static int _maxScore;

        internal static void FinishLoad()
        {
            if (_table == null)
            {
                return;
            }

            foreach (var cell in Resources.FindObjectsOfTypeAll<LeaderboardTableCell>())
            {
                var name = cell.GetPrivateField<TextMeshProUGUI>("_playerNameText").text;
                var score = cell.GetPrivateField<TextMeshProUGUI>("_scoreText");
                var scoreText = score.text;
                try
                {
                    var scoreInt = scoreToInt(scoreText);
                    if (cell.name.Equals("LeaderboardTableCell(Clone)"))
                    {
                        var acc = Math.Round((double)scoreInt / (double)_maxScore * 100, 2);
                        if (acc <= 100.1)
                        {
                            score.text += $" ({acc}%)";
                            //Logger.log.Debug($"{cell.name}, {score.text}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.log.Error($"Could not parse score: {scoreText}");
                }

            }

            _table = null;
        }

        internal static void GiveParams(ref LeaderboardTableView table, int maxScore)
        {
            _table = table;
            _maxScore = maxScore;
        }

        private static int scoreToInt(string score)
        {
            var noSpace = score.Replace(" ", String.Empty);
            return Int32.Parse(noSpace);
        }
    }
}

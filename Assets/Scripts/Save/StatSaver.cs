using UnityEditor;
using UnityEngine;

namespace TestProject.Save
{
    public static class StatSaver
    {
        private const string StatInfoKey = nameof(StatInfoKey);

        public static void Save(StatInfo info)
        {
            PlayerPrefs.SetInt(StatInfoKey, info.HighScore);
        }

        public static StatInfo Load()
        {
            return new StatInfo
            {
                HighScore = PlayerPrefs.GetInt(StatInfoKey)
            };
        }

        [MenuItem("Game/Reset High Score")]
        public static void ResetStats()
        {
            PlayerPrefs.SetInt(StatInfoKey, 0);
        }
    }

    public sealed class StatInfo
    {
        public int HighScore;
    }
}
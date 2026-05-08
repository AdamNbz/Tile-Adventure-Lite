using UnityEngine;

public static class GameProgress
{
    private const string LEVEL_KEY = "CurrentLevel";

    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }

    public static void SetLevelProgress(int level)
    {
        int current = GetCurrentLevel();
        if (level > current)
        {
            PlayerPrefs.SetInt(LEVEL_KEY, level);
            PlayerPrefs.Save();
        }
    }
}

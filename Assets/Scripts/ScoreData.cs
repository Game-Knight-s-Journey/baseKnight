// ScoreData.cs
using System.Collections.Generic;
using System;

[System.Serializable]
public class ScoreEntry
{
    public int score;
    public string date;
}

[System.Serializable]
public class HighScoreData
{
    public List<ScoreEntry> allScores = new List<ScoreEntry>();
}
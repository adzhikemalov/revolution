using System;
using UnityEngine;

public class TimeManager
{ 
    public float GameTimeMultiplier;

    public int GameTime { get; private set; }
    public float RealTime { get; private set; }
    public string CurrentWorldTime { get; private set; }


    public void IncrementTime(float time)
    {
        RealTime += time;
        GameTime = (int)(RealTime*GameTimeMultiplier);
        CurrentWorldTime = GameTimeToWorldTime(GameTime);
    }

    public string GameTimeToWorldTime(int gameTime)
    {
        var time = TimeSpan.FromSeconds(gameTime);

        return string.Format(
                "{0:D2}d:{1:D2}h:{2:D2}m",
                time.Days,
                time.Hours,
                time.Minutes
                );
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerLevelingDetails
{
    public int level;
    public int xp; //xp user has acquired since start of game (allows to dynamically adjust range of xp per level later on in game's design)

    public event EventHandler OnXpChanged;
    public event EventHandler OnLevelChanged;

    private static readonly int[] xpPerLevel = new[] { 100, 120, 140 };

    //constructor (should not be used in Monobehaviours)
    public PlayerLevelingDetails()
    {
        level = 0;
        xp = 0;
    }

    public void AddXp(int amount)
    {
        if (!IsMaxLevel())
        {
            xp += amount;
            while (!IsMaxLevel() && xp >= GetXpToNextLevel(level))
            {
                // Enough xp to level up
                xp -= GetXpToNextLevel(level);
                level++;
                if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            }
            if (OnXpChanged != null) OnXpChanged(this, EventArgs.Empty);
        }
    }

    public int GetLevelNumber()
    {
        return level;
    }

    public float GetXpNormalized()
    {
        if (IsMaxLevel())
        {
            return 1f;
        }
        else
        {
            return (float)xp / GetXpToNextLevel(level);
        }
    }

    public int GetXp()
    {
        return xp;
    }

    public int GetXpToNextLevel(int level)
    {
        if (level < xpPerLevel.Length)
        {
            return xpPerLevel[level];
        }
        else
        {
            // Level Invalid
            Debug.LogError("Level invalid: " + level);
            return 100;
        }
    }

    public bool IsMaxLevel()
    {
        return IsMaxLevel(level);
    }

    public bool IsMaxLevel(int level)
    {
        return level == xpPerLevel.Length - 1;
    }
}

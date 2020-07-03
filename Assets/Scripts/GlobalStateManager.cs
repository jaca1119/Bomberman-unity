using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStateManager : MonoBehaviour
{
    private Dictionary<string, int> playerFlameLength = new Dictionary<string, int>();

    public void Register(string playerName, int initialFlameLength)
    {
        playerFlameLength[playerName] = initialFlameLength;
    }

    public void IncrementFlameLength(string playerName)
    {
        int flame = playerFlameLength[playerName];
        
        if (flame <= 10)
        {
            flame++;

            playerFlameLength[playerName] = flame;
        }
    }

    public int GetPlayerFlameLength(string playerName)
    {
        return playerFlameLength[playerName];
    }
}

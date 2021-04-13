using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderDataEnemy : MonoBehaviour
{
    public int IndexEnemy;
    public CharacterAi Ai;
    private void Awake()
    {
        var enemy = StaticValues.EnemiesData.enemies[IndexEnemy];
        Ai = new CharacterAi(enemy);
        Ai.UpdateStats();
    }
}

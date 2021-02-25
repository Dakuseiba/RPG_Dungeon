using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Data/Character")]
public class DataCharacters : ScriptableObject
{
    public Characters character;

    DataCharacters()
    {
        character = new Characters();
        character.currentStats = new CharacterStats();
        character.currentStats.Base.agility = 10;
        character.currentStats.Base.charisma = 10;
        character.currentStats.Base.intelligence = 10;
        character.currentStats.Base.perception = 10;
        character.currentStats.Base.strength = 10;
        character.currentStats.Base.willpower = 10;

        character.currentStats.Battle.accuracy = 10;
        character.currentStats.Battle.actionPoint = 10;
        character.currentStats.Battle.armor_magicial = 10;
        character.currentStats.Battle.armor_phisical = 10;
        character.currentStats.Battle.calm = 10;
        character.currentStats.Battle.contrattack = 10;
        character.currentStats.Battle.crit_chance = 10;
        character.currentStats.Battle.crit_multiply = 10;
        character.currentStats.Battle.dmg = 10;
        character.currentStats.Battle.dmg_dice = 10;
        character.currentStats.Battle.evade = 10;
        character.currentStats.Battle.iniciative = 10;
        character.currentStats.Battle.move = 10;
        character.currentStats.Battle.parry = 10;
        character.currentStats.Battle.range = 10;
        character.currentStats.Battle.stressReduce = 10;
    }
}

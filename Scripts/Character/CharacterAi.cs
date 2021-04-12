using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAi
{
    public string Name;
    public Stats Stats;
    public CharEquip Equipment;
    public int Level;
    public int Exp;
    public List<int> Traits;
    
    public CharacterStats currentStats;
    public Precent_Stats Precent_Stats;
    public List<Effect> Effects;
    public CharacterAi()
    {
    }
    public CharacterAi(CharacterAi character)
    {
        Name = character.Name;
       // stats = new Stats();
        //stats.AddStats(character.stats);
        Equipment = character.Equipment;
        Level = character.Level;
        Exp = character.Exp;
        Traits = new List<int>(character.Traits);

        currentStats = new CharacterStats();
        Precent_Stats = new Precent_Stats();
        Effects = new List<Effect>();
    }
}

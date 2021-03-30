using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MissionController : MonoBehaviour
{
    public static List<GameObject> Characters;
    public static List<GameObject> SecondTurn;
    public static int Index;
    MissionMachine missionMachine;
    public static float multiplyDistance=0.5f;

    public List<LineRenderer> LineRenders;
    public static List<LineRenderer> Lines;

    public GameObject ColliderSphere;
    public static GameObject SphereCollider;
    public GameObject ColliderBox;
    public static GameObject BoxCollider;

    private void Start()
    {
        SphereCollider = ColliderSphere;
        SphereCollider.SetActive(false);
        BoxCollider = ColliderBox;
        BoxCollider.SetActive(false);

        missionMachine = new MissionMachine();
        missionMachine.ChangeState(new IMS_SetData());
        Lines = LineRenders;
        foreach (var line in Lines) line.enabled = false;
    }

    private void Update()
    {
        missionMachine.ExecuteStateLogic();
    }

    public static List<GameObject> FindCharacters()
    {
        List<GameObject> characters = new List<GameObject>();
        var players = GameObject.FindGameObjectsWithTag("Player");
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int i = 0;
        foreach (var player in players)
        {
            player.GetComponent<HolderDataCharacter>().character = StaticValues.Team[i];
            i++;
            characters.Add(player);
        }
        foreach (var enemy in enemies)
        {
            characters.Add(enemy);
        }

        foreach (var character in characters)
        {
            character.GetComponent<NavMeshAgent>().enabled = false;
            character.GetComponent<NavMeshObstacle>().enabled = true;
        }
        characters = SetOrder(characters);
        return characters;
    }

    public static List<GameObject> SetOrder(List<GameObject> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            int ini1 = GetIniciative(characters[i]);
            for (int j = i; j < characters.Count; j++)
            {
                int ini2 = GetIniciative(characters[j]);
                if (ini2 > ini1)
                {
                    var temp = characters[i];
                    characters[i] = characters[j];
                    characters[j] = temp;
                }
            }
        }
        return characters;
    }

    static int GetIniciative(GameObject obj)
    {
        if (obj.GetComponent<HolderDataCharacter>())
        {
            return obj.GetComponent<HolderDataCharacter>().GetCharacter().currentStats.Battle.iniciative;
        }
        if (obj.GetComponent<HolderDataEnemy>())
        {
            return obj.GetComponent<HolderDataEnemy>().stats.Battle.iniciative;
        }
        return 0;
    }

    public static void AddChara(GameObject chara)
    {
        Characters.Add(chara);
        SecondTurn.Add(chara);
        SecondTurn = SetOrder(SecondTurn);
    }
    public static void RemoveChara(GameObject chara)
    {
        Characters.Remove(chara);
        SecondTurn.Remove(chara);
        Destroy(chara);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MissionController : MonoBehaviour
{
    public List<GameObject> Characters;
    public LineRenderer lineRender;
    Vector3 target = new Vector3();
    NavMeshAgent agent;

    private void Start()
    {
        Characters = new List<GameObject>();
        var players = GameObject.FindGameObjectsWithTag("Player");
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(players.Length);
        foreach(var player in players)
        {
            Characters.Add(player);
        }
        foreach(var enemy in enemies)
        {
            Characters.Add(enemy);
        }
        agent = Characters[0].GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateTarget();
        CharacterMove();
    }
    void PathRender(bool hasPath)
    {
        if(hasPath)
        {
            lineRender.positionCount = agent.path.corners.Length;
            lineRender.SetPositions(agent.path.corners);
            lineRender.enabled = true;
        }
        else
        {
            lineRender.enabled = false;
        }
    }

    void UpdateTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hit))
        {
            target= hit.point;
        }
    }

    void CharacterMove()
    {
        agent.SetDestination(target);
        PathRender(agent.hasPath);
    }
}

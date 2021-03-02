using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMachine
{
    IPlayerState currentState;
    IPlayerState previousState;
    public Data playerData;
    public void ChangeState(IPlayerState state)
    {
        if (currentState != null)
        {
            currentState.Exit();
            previousState = currentState;
        }
        currentState = state;
        currentState.Enter(playerData);
    }
    public void SwitchToPreviusState()
    {
        ChangeState(previousState);
    }

    public void ExecuteStateLogic()
    {
        var newState = currentState.Execute();
        if (newState != null)
        {
            currentState.Exit();
            newState.Enter(playerData);
            previousState = currentState;
            currentState = newState;
        }
    }
    public void ExecuteAction()
    {
        currentState.Action();
    }

    public class Data
    {
        public LineRenderer lineRender;
        public NavMeshAgent agent;
        public Vector3 target;
        public Characters character;

        public int points;
        public int cost;
        public float freeMove;
        public float distance;

        public bool isEndTurn;

        public Color colorPositive;
        public Color colorNegative;

        public List<GameObject> targets;

        public Data()
        {
            colorPositive = new Color(0f, 0.15f, 1f);
            colorNegative = new Color(1f, 0f, 0f);
            isEndTurn = false;
            target = new Vector3();
            lineRender = GameObject.FindObjectOfType<LineRenderer>();
            agent = MissionController.Characters[MissionController.Index].GetComponent<NavMeshAgent>();

            character = agent.GetComponent<HolderDataCharacter>().GetCharacter();
            points = character.currentStats.Battle.actionPoint;

            targets = new List<GameObject>();

            agent.enabled = true;
            agent.isStopped = true;
        }
    }
}

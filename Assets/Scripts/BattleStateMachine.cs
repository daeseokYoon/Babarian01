using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        Wait,
        TakeAction,
        PerformAction,
    }

    public PerformAction battleStaes;

    public List<HandleTurn> PerformList = new List<HandleTurn>();
    public List<GameObject> PlayerInBattle = new List<GameObject>();
    public List<GameObject> NpcInBattle = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        battleStaes = PerformAction.Wait;
        NpcInBattle.AddRange(GameObject.FindGameObjectsWithTag("NPC"));
        PlayerInBattle.Add(GameObject.FindGameObjectWithTag("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        switch(battleStaes)
        {
            case(PerformAction.Wait):
                if(PerformList.Count > 0)
                {
                    battleStaes = PerformAction.TakeAction;
                }
                break;
            case(PerformAction.TakeAction):
                //GameObject performer = gameObject
                break;
            case(PerformAction.PerformAction):
                break;
        }
    }

    public void CollectAction(HandleTurn input)
    {
        PerformList.Add(input);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    BattleStateMachine BSM;
    public BaseNPCStatus npc;

    public enum TurnState
    {
        Processing,
        ChooseAction,
        Waiting,
        Selecting,
        Action,
        Dead,
    }
    public TurnState currentState; // for the progressBar

    float cur_cooldown = 0f;
    float max_cooldown = 5f;

    Vector3 startposition;
    bool actionStarted = false;
    GameObject PlayerToAttack;
    float animSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        currentState = TurnState.Processing;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case TurnState.Processing:
                //UpgradeProgressBar();
                break;
            case TurnState.ChooseAction:
                ChooseAction();
                currentState = TurnState.Waiting;
                break;
            case TurnState.Waiting:
                // idle State
                break;
            case TurnState.Selecting:
                break;
            case TurnState.Action:
                StartCoroutine(TimeForAction());
                break;
            case TurnState.Dead:
                break;
        }
    }

    private void UpgradeProgressBar() // 7년전 방식이라 안좋음 업데이트된 기능인 슬라이드 기능을 이용하는게 더 좋다. 지금은 일단 썼지만 지우거나 바꿀것임.
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
       
        if(cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.ChooseAction;
        }
    }

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = npc.name;
        myAttack.AttacksGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.PlayerInBattle[Random.Range(0, BSM.PlayerInBattle.Count)];
        BSM.CollectAction(myAttack);
    }

    IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        // 적이 공격하는 모션
        Vector3 playerPosition = PlayerToAttack.transform.position;
        while (MoveTowardsEnemy(playerPosition))
        {
            yield return null;
        }
        // wait
        // do damage

        // 적이 원래 자리로 돌아가는 모션
        // 방금 움직임을 배틀머신의 리스트에서 제거

        // 배틀머신 리셋

        actionStarted = false;
        // npc 스테이트 리셋
        cur_cooldown = 0f;
        currentState = TurnState.Processing;
    }

    bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
}

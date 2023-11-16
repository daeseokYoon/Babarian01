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

    private void UpgradeProgressBar() // 7���� ����̶� ������ ������Ʈ�� ����� �����̵� ����� �̿��ϴ°� �� ����. ������ �ϴ� ������ ����ų� �ٲܰ���.
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

        // ���� �����ϴ� ���
        Vector3 playerPosition = PlayerToAttack.transform.position;
        while (MoveTowardsEnemy(playerPosition))
        {
            yield return null;
        }
        // wait
        // do damage

        // ���� ���� �ڸ��� ���ư��� ���
        // ��� �������� ��Ʋ�ӽ��� ����Ʈ���� ����

        // ��Ʋ�ӽ� ����

        actionStarted = false;
        // npc ������Ʈ ����
        cur_cooldown = 0f;
        currentState = TurnState.Processing;
    }

    bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
}

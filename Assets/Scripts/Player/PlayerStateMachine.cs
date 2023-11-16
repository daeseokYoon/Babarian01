using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStateMachine : MonoBehaviour
{
    public BasePlayerStatus player;

    public enum TurnState
    {
        Processing,
        AddToList,
        Waiting,
        Selecting,
        Action,
        Dead,
    }

    public TurnState currentState; // for the progressBar

    float cur_cooldown = 0f;
    float max_cooldown = 5f;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case TurnState.Processing:
                break;
            case TurnState.AddToList:
                break;
            case TurnState.Waiting:
                break;
            case TurnState.Selecting:
                break;
            case TurnState.Action:
                break;
            case TurnState.Dead:
                break;
        }
    }

    private void UpgradeProgressBar() // 7���� ����̶� ������ ������Ʈ�� ����� �����̵� ����� �̿��ϴ°� �� ����. ������ �ϴ� ������ ����ų� �ٲܰ���.
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float cacl_cooldown = cur_cooldown / max_cooldown;
        //ProgressBar.transform.localScale = new Vector3()
    }
}

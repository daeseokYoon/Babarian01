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

    private void UpgradeProgressBar() // 7년전 방식이라 안좋음 업데이트된 기능인 슬라이드 기능을 이용하는게 더 좋다. 지금은 일단 썼지만 지우거나 바꿀것임.
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float cacl_cooldown = cur_cooldown / max_cooldown;
        //ProgressBar.transform.localScale = new Vector3()
    }
}

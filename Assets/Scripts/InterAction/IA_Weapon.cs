using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Weapon : MonoBehaviour, IInteractable
{
    [SerializeField] string _prompt;
    public string InteractionPrompt => _prompt;

    public bool Interact(ObjectInteractor interactor) // 어느 조건이 충족되면 무기를 얻을 수 있게 코드를 작성하고 true를 반환할 수도 있다.
    {
       Debug.Log("무기 줍기");
       return true;
    }

    public void Interact_Dialog(DialogueSO log)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

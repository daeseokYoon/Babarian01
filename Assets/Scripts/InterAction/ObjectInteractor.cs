using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface IInteractable
{
    public string InteractionPrompt { get; }
    public bool Interact(ObjectInteractor interactor);

    public void Interact_Dialog(DialogueSO log);
}
public class ObjectInteractor : MonoBehaviour // 약간 컨트롤러 느낌 + 다이어로그의 컨트롤러???
{
    [SerializeField] Transform _interActionPoint;
    [SerializeField] float _interActionPointRadius;
    [SerializeField] LayerMask _interactableMask;
    [SerializeField] InteractionPromptUI _interactionPromptUI;

    readonly Collider[] _colliders = new Collider[3];                                                        // 속도냐 메모리효율이냐 선택의 문제
    [SerializeField] int _numFound;                                                                         // 충돌체의 수를 확인만 하는 정도임

    IInteractable _interactable;                                                                                // 상호작용할 오브젝트의 스크립트에 인터페이스로 들어가있는 인터엑트 함수에 원하는 기능을 코드로 작성해놓은 것을
                                                                                                                // 가져와서 아래 레이어가 true가 되면 원하는 코드가 실행되는 형식

    private void Update()
    {
        // (int) _interactableMask = 실제 발견된 레이어의 숫자가 int로 변환된 수 두개가 동시에 잡히면 numFound가 2로 나옴
        _numFound = Physics.OverlapSphereNonAlloc(_interActionPoint.position, _interActionPointRadius, _colliders, (int)_interactableMask);

        if(_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>(); // 충돌한 오브젝트의 인터페이스 정보를 가져온다.

            if (_interactable != null)// && Input.GetKeyDown(KeyCode.E)) // = Keyboard.current.eKey.wasPressedThisFrame < using unityengine.inputsystem있어야함
            {
                if (!_interactionPromptUI.IsDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);

                if (Input.GetKeyDown(KeyCode.E) && DialogManager.instance.IsDialogActive() == false)  // 현재 움직이면서 E버튼을 연타시 중복해서 다이어로그가 나오면서 리스폰스가 중복해서 나오는 버그 있음
                {
                    _interactable.Interact(this); // 플레이어가 가진 이 스크립트를 상호작용할 오브젝트의 interact에 넣어서 원하는 코드를 실행
                }
            }
            // _interactable.Interact(this);
        }
        else
        {
            if (_interactable != null) _interactable = null;
            if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
            if (DialogManager.instance.IsDialogActive() == true) DialogManager.instance.CloseDialogBox();
            CameraManager.Instance.SetCameraPriority(0);
            CameraManager.Instance.InvisiblePlayer(true);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interActionPoint.position, _interActionPointRadius);
    }
}

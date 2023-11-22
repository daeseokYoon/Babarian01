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
public class ObjectInteractor : MonoBehaviour // �ణ ��Ʈ�ѷ� ���� + ���̾�α��� ��Ʈ�ѷ�???
{
    [SerializeField] Transform _interActionPoint;
    [SerializeField] float _interActionPointRadius;
    [SerializeField] LayerMask _interactableMask;
    [SerializeField] InteractionPromptUI _interactionPromptUI;

    readonly Collider[] _colliders = new Collider[3];                                                        // �ӵ��� �޸�ȿ���̳� ������ ����
    [SerializeField] int _numFound;                                                                         // �浹ü�� ���� Ȯ�θ� �ϴ� ������

    IInteractable _interactable;                                                                                // ��ȣ�ۿ��� ������Ʈ�� ��ũ��Ʈ�� �������̽��� ���ִ� ���Ϳ�Ʈ �Լ��� ���ϴ� ����� �ڵ�� �ۼ��س��� ����
                                                                                                                // �����ͼ� �Ʒ� ���̾ true�� �Ǹ� ���ϴ� �ڵ尡 ����Ǵ� ����

    private void Update()
    {
        // (int) _interactableMask = ���� �߰ߵ� ���̾��� ���ڰ� int�� ��ȯ�� �� �ΰ��� ���ÿ� ������ numFound�� 2�� ����
        _numFound = Physics.OverlapSphereNonAlloc(_interActionPoint.position, _interActionPointRadius, _colliders, (int)_interactableMask);

        if(_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>(); // �浹�� ������Ʈ�� �������̽� ������ �����´�.

            if (_interactable != null)// && Input.GetKeyDown(KeyCode.E)) // = Keyboard.current.eKey.wasPressedThisFrame < using unityengine.inputsystem�־����
            {
                if (!_interactionPromptUI.IsDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);

                if (Input.GetKeyDown(KeyCode.E) && DialogManager.instance.IsDialogActive() == false)  // ���� �����̸鼭 E��ư�� ��Ÿ�� �ߺ��ؼ� ���̾�αװ� �����鼭 ���������� �ߺ��ؼ� ������ ���� ����
                {
                    _interactable.Interact(this); // �÷��̾ ���� �� ��ũ��Ʈ�� ��ȣ�ۿ��� ������Ʈ�� interact�� �־ ���ϴ� �ڵ带 ����
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

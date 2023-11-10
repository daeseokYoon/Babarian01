using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueSO : ScriptableObject
{
    [SerializeField][TextArea] string[] dialogue;
    [SerializeField] Response[] responses; //[System.Serializable] �������� ��ũ��Ʈ�� ���� ���� �ڵ尡 �־�� �ν����Ϳ� SerializeField ��������


    public string[] Dialogue => dialogue;

    public bool HasResponses => Responses != null && Responses.Length > 0; // ���䳻�� �ִ���, ���� ������ 1�� �̻� �־����

    public Response[] Responses => responses;
}

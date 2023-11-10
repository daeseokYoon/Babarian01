using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueSO : ScriptableObject
{
    [SerializeField][TextArea] string[] dialogue;
    [SerializeField] Response[] responses; //[System.Serializable] 리스폰스 스크립트에 옆과 같은 코드가 있어야 인스펙터에 SerializeField 생성가능


    public string[] Dialogue => dialogue;

    public bool HasResponses => Responses != null && Responses.Length > 0; // 응답내용 있는지, 응답 내용이 1개 이상 있어야함

    public Response[] Responses => responses;
}

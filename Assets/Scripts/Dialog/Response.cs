using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Response
{
    [SerializeField] string responseText;
    [SerializeField] DialogueSO dialogueSO;

    public string ResponseText => responseText;
    public DialogueSO DialogueSO => dialogueSO; 
}

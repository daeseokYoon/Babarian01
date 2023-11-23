using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTextBox : MonoBehaviour
{
    [SerializeField] Text finalResults;
    //[SerializeField] RectTransform result;
    
    private void Awake()
    {
        ResetResult();
    }

    public void SetResultText(string lists, int score)
    {
        finalResults.text = lists +" : "+ score.ToString();
    }

    public void ResetResult()
    {
        finalResults.text = "ฐ๘ น้";
    }


}

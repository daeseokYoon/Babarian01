using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPage : MonoBehaviour
{
    [SerializeField] ResultTextBox boxPrefab;
    [SerializeField] RectTransform listPanel;
    [SerializeField] Text resultText;

    List<ResultTextBox> results = new List<ResultTextBox>();
    
    Dictionary<string, string> dict = new Dictionary<string, string>(); // 고정으로 사용예정 추가없음
    public void Dict()
    {
        dict.Add("항목0", "너는 0이다.");
        dict.Add("항목1", "너는 1이다.");
        dict.Add("항목2", "너는 2이다.");
        dict.Add("항목3", "너는 3이다.");
    }

    private void Start()
    {
        ShowResultList();
    }

    public void ShowResultList() // 싱글톤으로 참조하지말고 의존성을 최소화한 개별 클래스로 만들었으면 어떨까 // 싱글톤으로 쓸 이유가 없어진듯함
    {
        Dict();
        int maxValue = int.MinValue; // 0보다 작은 값이 더 큰값으로 인식되는 것을 방지하기 위해서 0이 아니라 int.MinValue를 쓴다. 가장 안전한 초깃값
        int maxIndex = -1;
        //int? maxValue = null; // 아닛? HasValue, Value가 숨어있네 // MinValue를 이해 못해서 바꿔쓴거였음 지워도 무관
        string dictValue = "";

        List<string> traitNames = GameManager.Instance.EachKeyName();

        for (int i = 0; i < 4; i++)
        {
            ResultTextBox resultTextBox = Instantiate(boxPrefab, Vector3.zero, Quaternion.identity);
            resultTextBox.transform.SetParent(listPanel);
            results.Add(resultTextBox);
                       
            if (i < traitNames.Count)
            {
                string traitName = traitNames[i];
                int value = GameManager.Instance.EachTotalValue(traitName);
                resultTextBox.SetResultText(traitName, value);
                if (value > maxValue) // for문 한번씩 돌아가는 값 하나하나씩 넣어가면서 확인하기 때문!
                {
                    maxValue = value;
                    maxIndex = i; // 최대값이 걸린 value와 맞춘 i
                }
                else 
                {
                    Debug.Log("얼마나 들어왔냐?" + i);
                }
            }
            //string traitName = GameManager.Instance.PlayerTrait().[i].traitName[i];            
            if (maxIndex != -1)
            {
                List<string> maxTraitNames = GameManager.Instance.EachKeyName();
                if(maxIndex < maxTraitNames.Count)
                {
                    string maxTraitName = maxTraitNames[maxIndex];

                    if (dict.ContainsKey(maxTraitName))
                    {
                        dictValue = dict[maxTraitName];
                    }
                }
                else
                {
                    Debug.Log("최대 인덱스는? " +  maxIndex);
                }
                
            }
        }
        
        resultText.text = dictValue;

       // int maxValue = FindMaxValue(randomValue);
    }

    //public int FindMaxValue(List<int> list)
    //{
    //    int max = int.MinValue; // 0보다 작은 값이 더 큰값으로 인식되는 것을 방지하기 위해서 0이 아니라 int.MinValue를 쓴다. 가장 안전한 초깃값

    //    foreach (int number in list) // for문 한번씩 돌아가는 값 하나하나씩 넣어가면서 확인하기 때문!
    //    {
    //        if(number > max)
    //        {
    //            max = number;
    //        }
    //    }
    //    return max;
    //}

    public void ResetResult() // 버튼에 따로 추가하자
    {
        foreach (ResultTextBox resultTextBox in results)
        {
            Destroy(resultTextBox);
        }
        results.Clear();
        resultText.text = "";

        GameManager.Instance.PlayerTrait().Clear();
    }


}

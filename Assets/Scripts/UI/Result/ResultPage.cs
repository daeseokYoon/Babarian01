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
    
    Dictionary<string, string> dict = new Dictionary<string, string>(); // �������� ��뿹�� �߰�����
    public void Dict()
    {
        dict.Add("�׸�0", "�ʴ� 0�̴�.");
        dict.Add("�׸�1", "�ʴ� 1�̴�.");
        dict.Add("�׸�2", "�ʴ� 2�̴�.");
        dict.Add("�׸�3", "�ʴ� 3�̴�.");
    }

    private void Start()
    {
        ShowResultList();
    }

    public void ShowResultList() // �̱������� ������������ �������� �ּ�ȭ�� ���� Ŭ������ ��������� ��� // �̱������� �� ������ ����������
    {
        Dict();
        int maxValue = int.MinValue; // 0���� ���� ���� �� ū������ �νĵǴ� ���� �����ϱ� ���ؼ� 0�� �ƴ϶� int.MinValue�� ����. ���� ������ �ʱ갪
        int maxIndex = -1;
        //int? maxValue = null; // �ƴ�? HasValue, Value�� �����ֳ� // MinValue�� ���� ���ؼ� �ٲ㾴�ſ��� ������ ����
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
                if (value > maxValue) // for�� �ѹ��� ���ư��� �� �ϳ��ϳ��� �־�鼭 Ȯ���ϱ� ����!
                {
                    maxValue = value;
                    maxIndex = i; // �ִ밪�� �ɸ� value�� ���� i
                }
                else 
                {
                    Debug.Log("�󸶳� ���Գ�?" + i);
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
                    Debug.Log("�ִ� �ε�����? " +  maxIndex);
                }
                
            }
        }
        
        resultText.text = dictValue;

       // int maxValue = FindMaxValue(randomValue);
    }

    //public int FindMaxValue(List<int> list)
    //{
    //    int max = int.MinValue; // 0���� ���� ���� �� ū������ �νĵǴ� ���� �����ϱ� ���ؼ� 0�� �ƴ϶� int.MinValue�� ����. ���� ������ �ʱ갪

    //    foreach (int number in list) // for�� �ѹ��� ���ư��� �� �ϳ��ϳ��� �־�鼭 Ȯ���ϱ� ����!
    //    {
    //        if(number > max)
    //        {
    //            max = number;
    //        }
    //    }
    //    return max;
    //}

    public void ResetResult() // ��ư�� ���� �߰�����
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

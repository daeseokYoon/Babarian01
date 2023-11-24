using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour // ������ ���� ���� ���� ���� ���� ���� ���� ���� ���� ���� ���� ���� ���� ���� ���� ����
{
    static GameManager instance;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // GameManager�� �� �ϳ�, ������ ��ü�� �����
        if(instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       
    }

    // ���� �޼ҵ���� ���� �̱��濡 ���� �ʿ䰡 �־��°�? ���� �̵��ϸ鼭 ����� �����͸� ���ӸŴ��� ������Ʈ�� ����� ���ӸŴ��� ��ũ��Ʈ�� ������ �����ϱ� ���� �̱��ѵ� �����̳� �ڳ� ��Ʈ��ũ�� 
    // ����� �� �ȴٸ�,...
    
    //Dictionary<string, int> traitValue = new Dictionary<string, int>(); // �̸��� ���� �ִ� ����
    //List<PersonalityTrait> playerTraits = new List<PersonalityTrait>();
    List<KeyValuePair<string, int>> traitList = new List<KeyValuePair<string, int>>();

    public List<KeyValuePair<string, int>> PlayerTrait() => traitList; // ���â ���� ���

    public void AddTraitValue(string traitName) // ���� Ư���� ���� ��
    {
        int index = traitList.FindIndex(item => item.Key == traitName);

        if(index != -1)
        {
            traitList[index] = new KeyValuePair<string, int>(traitName, traitList[index].Value + 1);
        }
        else
        {
            traitList.Add(new KeyValuePair<string, int>(traitName, 1));
        }


        //if (traitValue.ContainsKey(traitName))
        //{
        //    traitValue[traitName] += 1;
        //}
        //else
        //{
        //    traitValue.Add(traitName, 1);
        //}
    }

    public int EachTotalValue(string traitName) // ����Ʈ���� ������ 
    {
        int index = traitList.FindIndex(item => item.Key == traitName);

        if(index != -1)
        {
            return traitList[index].Value;
        }
        return 0;
        //if (traitValue.ContainsKey(traitName))
        //{
        //    return traitValue[traitName];
        //}
        //return 0;
    }

    public List<string> EachKeyName()
    {
        List<string> keys = traitList.Select(item => item.Key).ToList();
        return keys;
    }


    //public int CalculateEachValue()
    //{
    //    int eachValue = 0; // ��ü ��
    //    foreach(var trait in playerTraits) // ����Ʈ�� �ִ� ���� �ҷ��� (��ųʸ�ó�� ���� �ǰ�? ���� �ٸ��� �ֳĸ� �� ����Ʈ�� �� Ŭ������ �����ͻ����̱� ����)
    //    {
    //        eachValue += trait.value; // 
    //    }
    //    return eachValue;
    //}


    // int ���� 12345 ��� ����Ʈ ���빰�� ���� �� x> 2 �� find�� int�� �O���� 3�� ���´�. 345 3�� �̱� ���� x> 5�� ���� ��� 0�� ��ȯ // ��ť any�� Ȱ���� ã��, Array.IndexOf�� ã��
    //PersonalityTrait trait = playerTraits.Find(t => t.traitName.Contains(traitName)); 
    //if (trait != null ) // �̸��� ������ �ش� �̸��� ���� �� �߰�
    //{
    //if(trait.value == null) // trait���� ���� �ʱ�ȭ �߱⶧���� ������. // ������ �ʱ�ȭ ���� ������ ������ null�� ��� ���ο� ���� �߰� �������� ����.
    //{
    //    trait.value = new int[trait.traitName.Length];
    //}

    //int index = Array.IndexOf(trait.traitName, traitName); // (����, ã�� value)

    //if(index !=  -1) // IndexOf�� ã�� ���� ������ -1�� ��ȯ��.
    //{
    //trait.value[index] += 1; // �Ķ���� �ΰ��� ���� ��ư�� �۵��� ���ϳ�?
    //}
    //}
    //else // ������ ���Ӱ� �̸��� �ְ� ���� �߰�
    //{
    //    playerTraits.Add(new PersonalityTrait { traitName = traitname, value = value }); // ��ųʸ����� �ִ� ���� // FindIndex, Find �Լ� ���� �˻� // �̰� �̸� ��Ʈ�� �������� �ٲ������
    //}

    //PersonalityTrait trait = playerTraits.Find(t => t.traitValues.ContainsKey(traitName)); // Dictionary�� ������� �� �ڵ�

    //if (trait != null)
    //{
    //    trait.traitValues[traitName] += valueToAdd;
    //}

    //PersonalityTrait trait = playerTraits[index];

    //if (index >= 0 && index < playerTraits.Count)
    //{
    //    if (trait.value.Length > index)
    //    {
    //        return trait.value[index]; //return playerTraits[index].value[index]; // ��
    //    }
    //}
    //return 0;
}

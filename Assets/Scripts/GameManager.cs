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

    

    List<PersonalityTrait> playerTraits = new List<PersonalityTrait>();

    public List<PersonalityTrait> PlayerTrait() => playerTraits; // ���â ���� ���

    public void AddTraitValue(string traitName) // ���� Ư���� ���� ��
    {
        // int ���� 12345 ��� ����Ʈ ���빰�� ���� �� x> 2 �� find�� int�� �O���� 3�� ���´�. 345 3�� �̱� ���� x> 5�� ���� ��� 0�� ��ȯ // ��ť any�� Ȱ���� ã��, Array.IndexOf�� ã��
        PersonalityTrait trait = playerTraits.Find(t => t.traitName.Contains(traitName)); 
        if (trait != null ) // �̸��� ������ �ش� �̸��� ���� �� �߰�
        {
            //if(trait.value == null) // trait���� ���� �ʱ�ȭ �߱⶧���� ������. // ������ �ʱ�ȭ ���� ������ ������ null�� ��� ���ο� ���� �߰� �������� ����.
            //{
            //    trait.value = new int[trait.traitName.Length];
            //}
            
            int index = Array.IndexOf(trait.traitName, traitName); // (����, ã�� value)

            if(index !=  -1) // IndexOf�� ã�� ���� ������ -1�� ��ȯ��.
            {
                trait.value[index] += 1; // �Ķ���� �ΰ��� ���� ��ư�� �۵��� ���ϳ�?
            }
        }
        //else // ������ ���Ӱ� �̸��� �ְ� ���� �߰�
        //{
        //    playerTraits.Add(new PersonalityTrait { traitName = traitname, value = value }); // ��ųʸ����� �ִ� ���� // FindIndex, Find �Լ� ���� �˻� // �̰� �̸� ��Ʈ�� �������� �ٲ������
        //}

        //PersonalityTrait trait = playerTraits.Find(t => t.traitValues.ContainsKey(traitName)); // Dictionary�� ������� �� �ڵ�

        //if (trait != null)
        //{
        //    trait.traitValues[traitName] += valueToAdd;
        //}
    }

    public int EachTotalValue(int index)
    {
        return playerTraits[index].value[index];
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


}

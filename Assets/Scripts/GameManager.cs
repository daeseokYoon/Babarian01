using System.Collections;
using System.Collections.Generic;
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

    public void AddTraitValue(string traitname, int value)
    {
        PersonalityTrait trait = playerTraits.Find(t => t.traitName == traitname); // int ���� 12345 ��� ����Ʈ ���빰�� ���� �� x>2 �� find�� int�� �O���� 3�� ���´�. 345 3�� �̱� ���� x>5�� ���� ��� 0�� ��ȯ
        if(trait != null ) // �̸��� ������ �ش� �̸��� ���� �� �߰�
        {
            trait.value += value;
        }
        else // ������ ���Ӱ� �̸��� �ְ� ���� �߰�
        {
            playerTraits.Add(new PersonalityTrait { traitName = traitname, value = value }); // ��ųʸ����� �ִ� ���� // FindIndex, Find �Լ� ���� �˻�
        }
    }

    public void ResetTrait()
    {
        playerTraits.Clear();
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

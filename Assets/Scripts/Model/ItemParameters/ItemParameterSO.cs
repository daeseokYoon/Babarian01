using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class ItemParameterSO : ScriptableObject
    {
        [field: SerializeField]
        public string ParameterName {  get; private set; } 
        // ���� �Ķ���ʹ� SO ��ü ���� �̸��� �����ϸ� Ȯ���� �� �ִ� �̸��� �ϳ����̰� ��ü ������ �ϳ��� �ִ� ���
        // �� ������ �Ķ���͸� �ְ� descriptible ��ü(������Ʈ) ������ �ָ� �Ű������� ���� ��ġ�ϴ� �� Ȯ�ΰ���
        // string � ������ �Ű��������� ��Ȯ���� �����ϴ�!
        // ������ enum���� �з��� ���ϰ� SO�� ������
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersonalityTrait // �Ⱦ��� �Ǿ����� �ڷᵢ��� ����������
{
    public string[] traitName = {"�׸�0", "�׸�1", "�׸�2", "�׸�3"};
    public int[] value; // = new int[4]; // �迭�� ���� �ʱ�ȭ. ���� ���ҰŶ� ������ �迭�� �����
    // �迭, ����Ʈ ������� ���� ���� // Dictionary�� ��ٸ� �� �ڵ尡 ���������� ���̴�.
    // public Dictionary<string, int> traitValues = new Dictionary<string, int>();
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialog : MonoBehaviour // �� Ŭ������ ���� : ��ȭ �� player���� �������� �ְų� ���丮������, ����Ʈ�� Ʈ�����ϱ� ���� // SO �������� �����ϴ� �� ��ü�Ϸ���
{
    [SerializeField] List<string> lines; // ��簡 ��µǴ� �ν��Ͻ��ε� List�� �̾Ƴ��� ���� �����ΰ�? Queue�� ���� ��縦 ����ϰ�... �׷��� �̰� �� Npc�� �δ°� �³�? �Ȱ��� ��縸 ���� �Ÿ� ������µ�

    public List<string> Lines => lines;
}

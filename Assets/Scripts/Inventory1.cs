using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory1 : MonoBehaviour
{
    // �ӽ÷� �÷��̾ �޾Ƶ�
    public bool HasGun = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) HasGun = !HasGun; // �ӽ÷� e��ư�� ������ �κ��丮�� ���� ����ٶ�� bool���� �ο�
    }
}

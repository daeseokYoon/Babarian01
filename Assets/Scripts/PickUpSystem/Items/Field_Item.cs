using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field_Item : MonoBehaviour
{
    public enum Type { Ammo, Weapon, Accessory, Heart };
    public Type type; // ������ ����
    public int value; // ������ ��ȣ��

    private void Update()
    {
        Vector3 curRotation = transform.rotation.eulerAngles;
        // rotation.eulerAngles�� x�� z�� ������Ų��. eulerAngles �˻�
        curRotation.x = transform.rotation.eulerAngles.x; // ����� x������ ������.
        curRotation.z = 0;
        curRotation.y += 20 * Time.deltaTime;

        transform.rotation = Quaternion.Euler(curRotation);
        //transform.Rotate(new Vector3(transform.rotation.x, 1, 0) * 20 * Time.deltaTime);
        // �ݶ��̴��� �־���� ���̾ �ν�
    }
}

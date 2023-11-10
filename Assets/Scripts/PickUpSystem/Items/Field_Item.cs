using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field_Item : MonoBehaviour
{
    public enum Type { Ammo, Weapon, Accessory, Heart };
    public Type type; // 아이템 종류
    public int value; // 아이템 번호값

    private void Update()
    {
        Vector3 curRotation = transform.rotation.eulerAngles;
        // rotation.eulerAngles로 x와 z을 고정시킨다. eulerAngles 검색
        curRotation.x = transform.rotation.eulerAngles.x; // 저장된 x값으로 움직임.
        curRotation.z = 0;
        curRotation.y += 20 * Time.deltaTime;

        transform.rotation = Quaternion.Euler(curRotation);
        //transform.Rotate(new Vector3(transform.rotation.x, 1, 0) * 20 * Time.deltaTime);
        // 콜라이더가 있어야지 레이어가 인식
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory1 : MonoBehaviour
{
    // 임시로 플레이어에 달아둠
    public bool HasGun = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) HasGun = !HasGun; // 임시로 e버튼을 누르면 인벤토리에 총이 생겼다라고 bool값을 부여
    }
}

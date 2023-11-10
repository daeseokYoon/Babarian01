using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCheck : MonoBehaviour
{
    public Vector3 sphereOffset = new Vector3(0, 0.2f, 0);
    public float sphereRadius = 0.9f;
    public LayerMask itemLayer;

    ItemInfo hitData;

    [Header("Weapon Bat Check")]
    [SerializeField] float batRangeLength = 3;

    [Header("Weapon Pistol Check")]
    [SerializeField] float pistolRangeLength = 10;
    [SerializeField] float reactRangeLength = 5;

    public ItemInfo CheckItem() // Ray로 콜라이더 체크를 하려고 했는데... OnTrigger 쓰기로 함
    {
        hitData = new ItemInfo();
        Quaternion rotation = Quaternion.Euler(0f, 360f, 0f);
        Vector3 direction = rotation * Vector3.forward;
        var sphereOrigin = transform.position + sphereOffset;

        hitData.hitFound = Physics.SphereCast(sphereOrigin, sphereRadius, direction, out hitData.hitInfo, sphereRadius, itemLayer); // 발사위치, 방향, 거리, 레이어 out
        

        return hitData;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = (hitData.hitFound) ? Color.green : Color.red;
    //    Gizmos.DrawSphere(transform.position, sphereRadius);
    //}

    public struct ItemInfo // 구조체!
    {
        public bool hitFound;
        public RaycastHit hitInfo;
    }

}

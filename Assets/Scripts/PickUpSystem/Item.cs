using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField] public ItemSO InventoryItem { get; private set; }               // public이면서 프로퍼티인 변수를 관리하려고 field를 쓴다? // 프로퍼티는 field를 써줘야지 인스펙터 창이 나온다. 그냥 퍼플릭으로 하면 안나옴.
    [field: SerializeField] public int Quantity { get; set; } = 1;                                                                      // 필터링된 필드 공개 수량? // 프로퍼티와 변수의 차이는? 변수를 프로퍼티라 부르면 화냄
    [SerializeField] AudioSource audioSource;
    [SerializeField] float duration = 0.3f;                                                                                             // 재생 후 파괴하려고

    private void Start()
    {
       // transform.GetComponentInParent<SpriteRenderer>().sprite = InventoryItem.ItemImage; // 상위 오브젝트 이미지 참조 없어도 돼!
    }

    public void DestroyItem()
    {
        GetComponent<SphereCollider>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }

    IEnumerator AnimateItemPickup()
    {
        audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null; // 0보다 작아지면 null을 리턴
        }
        transform.localScale = endScale;
        Destroy(gameObject);
    }
}

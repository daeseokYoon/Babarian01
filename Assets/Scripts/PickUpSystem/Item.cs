using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField] public ItemSO InventoryItem { get; private set; }               // public�̸鼭 ������Ƽ�� ������ �����Ϸ��� field�� ����? // ������Ƽ�� field�� ������� �ν����� â�� ���´�. �׳� ���ø����� �ϸ� �ȳ���.
    [field: SerializeField] public int Quantity { get; set; } = 1;                                                                      // ���͸��� �ʵ� ���� ����? // ������Ƽ�� ������ ���̴�? ������ ������Ƽ�� �θ��� ȭ��
    [SerializeField] AudioSource audioSource;
    [SerializeField] float duration = 0.3f;                                                                                             // ��� �� �ı��Ϸ���

    private void Start()
    {
       // transform.GetComponentInParent<SpriteRenderer>().sprite = InventoryItem.ItemImage; // ���� ������Ʈ �̹��� ���� ��� ��!
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
            yield return null; // 0���� �۾����� null�� ����
        }
        transform.localScale = endScale;
        Destroy(gameObject);
    }
}

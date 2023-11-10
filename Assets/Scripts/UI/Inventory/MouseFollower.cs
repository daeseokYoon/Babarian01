using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] Canvas canvas;

                                                                                                                                                                 // �巡�� �� �� ���콺 �ȷξ�� �Ѿ�� item ����
    [SerializeField] UIInventoryItem item; 

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();                                                                                                                     // root �ֻ���
        item = GetComponentInChildren<UIInventoryItem>();
    }

    public void SetData(Sprite sprite, int quantity)                                                                                                 // �巡�� �ɶ� �޾ƿ� ������ �ȷο쿡 ����
    {
        item.SetData(sprite, quantity);
    }

    private void Update()
    {
        Vector2 position;                                                                                                                               // �巡���Ҷ� position�� Update�� ���̱� ������ ��ġ�� �������.
                                                                                                                        // RectTransform > ���� �ٸ� ��ǥ�迡 �ֱ� ������ Ʈ����ȯ ��ƿ��Ƽ�� ��ȯ�Ǵ� ���� ���������� ��ġ�� ��ȯ
                                                                                                                        // �Ʒ��� ĵ������ ��ġ�� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
                                                                                                                                                             //out���� ������ position�� ��ȯ ������ �Ʒ� �ڵ�� ����
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val)
    {
        Debug.Log($"Item toggled {val}");
        gameObject.SetActive(val);
    }
    
}

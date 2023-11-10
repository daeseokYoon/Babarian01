using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] Canvas canvas;

                                                                                                                                                                 // 드래그 할 때 마우스 팔로어로 넘어올 item 정보
    [SerializeField] UIInventoryItem item; 

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();                                                                                                                     // root 최상위
        item = GetComponentInChildren<UIInventoryItem>();
    }

    public void SetData(Sprite sprite, int quantity)                                                                                                 // 드래그 될때 받아올 정보를 팔로우에 세팅
    {
        item.SetData(sprite, quantity);
    }

    private void Update()
    {
        Vector2 position;                                                                                                                               // 드래그할때 position이 Update될 것이기 때문에 위치를 잡아주자.
                                                                                                                        // RectTransform > 서로 다른 좌표계에 있기 때문에 트랙변환 유틸리티로 변환되는 로컬 포지션으로 위치를 변환
                                                                                                                        // 아래는 캔버스의 위치로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
                                                                                                                                                             //out으로 나오는 position의 변환 지점을 아래 코드로 지정
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val)
    {
        Debug.Log($"Item toggled {val}");
        gameObject.SetActive(val);
    }
    
}

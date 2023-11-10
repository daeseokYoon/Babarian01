using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryDescription : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text description;

    public void Awake()
    {
        ResetDescription(); // start전에 스크립션 초기화
    }
    public void ResetDescription()
    {
        itemImage.gameObject.SetActive(false); // 비활성화
        title.text = "";
        description.text = "";
    }

    public void SetDescription(Sprite sprite, string itemName, string itemDescription)
    {
        itemImage.gameObject.SetActive (true);
        itemImage.sprite = sprite;
        title.text = itemName;
        description.text = itemDescription;
    }
}

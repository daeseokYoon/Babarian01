using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    Camera _mainCam;
    [SerializeField] GameObject _uiPanel; 
    [SerializeField] TextMeshProUGUI _promptText;

    private void Start()
    {
        _mainCam = Camera.main;
        _uiPanel.SetActive(false);

    }

    private void LateUpdate()
    {
        Quaternion rotation = _mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public bool IsDisplayed = false;

    public void SetUp(string promptText)
    {
        IsDisplayed = true;
        _uiPanel.SetActive(true);
        _promptText.text = promptText;
      
    }

    public void Close()
    {
        IsDisplayed = false;
        _uiPanel.SetActive(false);
        
    }
}

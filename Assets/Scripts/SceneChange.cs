using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ChanegeLogo()
    {
        SceneManager.LoadScene("Logo"); 
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Demo11");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {


    public void GotoMainGame()
    {
        SceneManager.LoadScene("mainGame");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

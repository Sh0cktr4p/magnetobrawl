using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    //Scene 0 = MainMenu
    //Scene 1 = Level
    public Material effectMaterial;
    public float fadeOutTime;

    public void StartGame()
    {
        StartCoroutine(fadeOut());
    }

    IEnumerator fadeOut() {
        float timer = 0;

        while (timer < fadeOutTime) {
            timer += Time.deltaTime;
            effectMaterial.SetFloat("_Cutoff", timer / fadeOutTime);
            yield return 0;
        }
        SceneManager.LoadScene(1);
    }
    
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
}

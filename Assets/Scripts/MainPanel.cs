using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainPanel : MonoBehaviour{
    public AudioSource fxSource;
    public AudioClip clickSound; // clip del audio cuando se presiona un boton

    //Diferentes Paneles
    [Header("Panels")]
    public GameObject mainPanel;

    public void PlayLevel(string levelName){
        SceneManager.LoadScene(levelName);
    }

    //Metodo para salir del juego cuando se presiona el boton salir
    public void ExitGame(){
        Application.Quit();
    }

    //Todos los paneles comienzan en falso, si se presiona algun boton se activa el panel
    public void OpenPanel(GameObject panel){
        mainPanel.SetActive(false);
        panel.SetActive(true);
        PlaySoundButton();
    }
    
    //Metodo del sonido al clickear, lo reproduce al momento
    public void PlaySoundButton(){
        fxSource.PlayOneShot(clickSound);
    }
}
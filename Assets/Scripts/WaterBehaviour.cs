using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterBehaviour : MonoBehaviour{
    public AudioSource controlSonido;
    public AudioClip sonidoAgua;
    private PlayerBehaviour player;

    void Start(){
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){ //cuando el jugador sale de la colisión, termina el juego
            player.health--;
            player.transform.position=new Vector3(1,8,0);
            controlSonido.PlayOneShot(sonidoAgua);
        }
    }
}

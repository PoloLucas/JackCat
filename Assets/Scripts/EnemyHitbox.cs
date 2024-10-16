using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHitbox : MonoBehaviour{
    public AudioSource controlSonido;
    public AudioClip sonidoDanyo;
    private PlayerBehaviour player;
    private bool yaAtaco=false;

    void Start(){
        gameObject.SetActive(false);
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void OnTriggerEnter(Collider other){
        if(!yaAtaco && other.gameObject.CompareTag("Player")){
            player.health--;
            controlSonido.PlayOneShot(sonidoDanyo);
        }
    }
}
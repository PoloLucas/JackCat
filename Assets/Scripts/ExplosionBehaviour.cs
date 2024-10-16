using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour{
    public AudioSource controlSonido;
    [SerializeField]public AudioClip sonidoExplosion;

    void Start(){
        controlSonido.PlayOneShot(sonidoExplosion);
        Destroy(gameObject,4); //destruye el enemigo automáticamente, coincidiendo con el final de la animación
    }
}
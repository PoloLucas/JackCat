using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour{
    [SerializeField]private GameObject explosion;
    private PlayerBehaviour player;
    private Rigidbody body;
    private Vector3 caida;
    private float velocidad = 0.00001f;
    private float tiempo = 0f;
    private float tiempoFinal = 0.8f;

    void Start(){
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        body=GetComponent<Rigidbody>();
        caida=new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }

    void Update(){
        if(tiempo<tiempoFinal){
            caida.y-=velocidad*tiempo+(0.5f * 9.8f * Mathf.Pow(tiempo,2));
            tiempo+=Time.deltaTime;
        }
        transform.position=caida;
    }

    void OnCollisionEnter(Collision other){ //al tocar al jugador, muestra la pantalla de Fín del Juego
        velocidad=0f;
        Instantiate(explosion, transform.position, transform.rotation);
        if(other.gameObject.CompareTag("Player")){
            player.health-=2;
        }
        Destroy(gameObject,0.15f);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyBehaviour : MonoBehaviour{
    public AudioSource controlSonido;
    public AudioClip sonidoDanyo;
    public AudioClip sonidoDerrota;
    public AudioClip sonidoDisparo;
    private GameObject player;
    private NavMeshAgent agente;
    private Transform objetivo;
    private Rigidbody body;
    private Animator anim;
    private float jumpForce = 125f;
    private float pushForce = 125f;
    private float waitRate = 0.35f;
    private float waitTime = 0;
    private int health=5;
    public bool canMove = true;
    private bool esDerrotado = false;

    void Start(){
        player=GameObject.FindGameObjectWithTag("Player");
        objetivo=player.GetComponent<Transform>();
        agente=GetComponent<NavMeshAgent>(); //obtiene los valores del NavMesh (Enemigo)
        anim=GetComponent<Animator>();
        body=GetComponent<Rigidbody>();
    }

    void Update(){
        if(health>0){
            MoverEnemigo();
        }
        ComprobarSalud();
    }

    void MoverEnemigo(){
        Vector3 direction=objetivo.position-transform.position;
        Quaternion playerDirection=Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        if(canMove){
            agente.destination=objetivo.position; //dirige al enemigo hacia el jugador a través de NavMesh
            agente.isStopped=false;
            transform.rotation=Quaternion.LookRotation(new Vector3(agente.velocity.x,0,agente.velocity.z).normalized);
            //transform.rotation=playerDirection;
            anim.SetBool("Walk",true);
        }else{
            agente.isStopped=true;
            anim.SetBool("Walk",false);
        }
    }

    void ComprobarSalud(){
        if(health<=0 && !esDerrotado){
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled=false;
            body.AddRelativeForce(Vector3.up*jumpForce, ForceMode.Impulse);
            body.AddRelativeForce(Vector3.forward*-pushForce, ForceMode.Impulse);
            Destroy(gameObject,2);
            controlSonido.PlayOneShot(sonidoDisparo);
            controlSonido.PlayOneShot(sonidoDerrota);
            esDerrotado=true;
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("PlayerSword") && Time.time>waitTime){ //si entra en contacto con una bala o el campo de fuerza del NPC, se destruye
            health--;
            waitTime = Time.time + waitRate; //limita la cadencia de disparo
            controlSonido.PlayOneShot(sonidoDanyo);
        }
    }
}
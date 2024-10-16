using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy2Behaviour : MonoBehaviour{
    public AudioSource controlSonido;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoDanyo;
    public AudioClip sonidoDerrota;
    public AudioClip sonidoDisparo;
    [SerializeField]private GameObject hitbox;
    [SerializeField]private GameObject collision;
    private GameObject activeHitbox;
    private GameObject player;
    private NavMeshAgent agente;
    private Transform objetivo;
    private Rigidbody body;
    private Animator anim;
    private float distancia;
    private float rango = 10;
    private float attackForce = 20f;
    private float jumpForce = 60f;
    private float pushForce = 50f;
    private float waitRate = 0.35f;
    private float waitTime = 0;
    private int health = 5;
    public bool canMove = true;
    private bool esDerrotado = false;
    private bool yaAtaco = false;
    /*private float posX=0;
    private float posZ=0;
    private float pendiente=0;
    private float origen=0;*/

    void Start(){
        player=GameObject.FindGameObjectWithTag("Player");
        objetivo=player.GetComponent<Transform>();
        agente=GetComponent<NavMeshAgent>(); //obtiene los valores del NavMesh (Enemigo)
        anim=GetComponent<Animator>();
        body=GetComponent<Rigidbody>();
    }

    void Update(){
        Interactuar();
        ComprobarSalud();
        if(health>0){
            MoverEnemigo();
        }
    }


    void Interactuar(){
        RaycastHit hit;
        distancia=CalcularDistancia(objetivo);
        //Debug.Log(distancia);
        if(distancia<=rango){
            canMove=true;
        }else{
            canMove=false;
        }
        if(Physics.Raycast(transform.position,transform.forward,out hit,2) && hit.transform.tag=="Player"){ //detecta si el rayo tocaa al jugador
            StartCoroutine(Atacar());
        }
    }

    
    /*public void Mover(){
        pendiente=(objetivo.position.z-transform.position.z)/(objetivo.position.x-transform.position.x);
        origen=transform.position.z-pendiente*transform.position.x;
        if(transform.position.x<objetivo.position.x){
            posX+=0.2f;
        }else{
            posX-=0.2f;
        }
        posZ=pendiente*transform.position.x+origen;
        Debug.Log(posZ);
        transform.position=new Vector3(posX,0,posZ);
    }*/

    void MoverEnemigo(){
        Vector3 direction=objetivo.position-transform.position;
        Quaternion playerDirection=Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        if(canMove && !yaAtaco){
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
            Vector3 finalScale=new Vector3(0,0,0);
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled=false;
            collision.SetActive(false);
            body.AddRelativeForce(Vector3.up*jumpForce, ForceMode.Impulse);
            body.AddRelativeForce(Vector3.forward*-pushForce, ForceMode.Impulse);
            StartCoroutine(LerpScale(new Vector3(0,0,0), 0.75f));
            Destroy(gameObject,1.2f);
            controlSonido.PlayOneShot(sonidoDisparo);
            controlSonido.PlayOneShot(sonidoDerrota);
            esDerrotado=true;
        }
    }

    IEnumerator Atacar(){
        if(!yaAtaco){
            yaAtaco=true;
            yield return new WaitForSeconds(0.8f);
            body.AddRelativeForce(Vector3.forward*attackForce, ForceMode.Impulse);
            hitbox.SetActive(true);
            controlSonido.PlayOneShot(sonidoAtaque);
            yield return new WaitForSeconds(0.25f);
            hitbox.SetActive(false);
            yield return new WaitForSeconds(0.75f);
            yaAtaco=false;
        }
    }

    IEnumerator LerpScale(Vector3 finish, float time){
        float timePassed=0f;
        while(timePassed<time){
            transform.localScale=Vector3.Lerp(transform.localScale, finish, timePassed/time);
            timePassed+=Time.deltaTime;
            yield return null;
        }
        transform.localScale=finish;
    }

    float CalcularDistancia(Transform objetivo){
        return Mathf.Sqrt(Mathf.Pow(objetivo.position.x-transform.position.x,2) + Mathf.Pow(objetivo.position.z-transform.position.z,2));
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("PlayerSword") && Time.time>waitTime){ //si entra en contacto con una bala o el campo de fuerza del NPC, se destruye
            health--;
            waitTime = Time.time + waitRate; //limita la cadencia de disparo
            controlSonido.PlayOneShot(sonidoDanyo);
        }
    }
}
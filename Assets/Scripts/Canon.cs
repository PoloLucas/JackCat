using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Canon : MonoBehaviour{
    public AudioSource controlSonido;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoDanyo;
    public AudioClip sonidoDisparo;
    [SerializeField]private GameObject collision;
    [SerializeField]private GameObject balaInicial;
    [SerializeField]private GameObject bala;
    private GameObject activeHitbox;
    private GameObject player;
    private Transform objetivo;
    private Rigidbody body;
    private Vector3 invisible = new Vector3(0,0,0);
    private Vector3 alturaMaxima;
    private Vector3 originalPos;
    private Vector3 spawnBala;
    private float distancia;
    private float rango = 25;
    private float jumpForce = 10560f;
    private float pushForce = 10550f;
    private float waitRate = 0.35f;
    private float waitTime = 0;
    private int health = 10;
    private bool esDerrotado = false;
    private bool yaAtaco = false;

    void Start(){
        player=GameObject.FindGameObjectWithTag("Player");
        objetivo=player.GetComponent<Transform>();
        body=GetComponent<Rigidbody>();
        alturaMaxima=new Vector3(balaInicial.transform.position.x, balaInicial.transform.position.y+15, balaInicial.transform.position.z);
        originalPos=balaInicial.transform.position;
        balaInicial.SetActive(false);
    }

    void Update(){
        Interactuar();
        ComprobarSalud();
    }


    void Interactuar(){
        distancia=CalcularDistancia(objetivo);
        if(distancia<=rango){
            StartCoroutine(Disparar());
        }
    }


    void ComprobarSalud(){
        if(health<=0 && !esDerrotado){
            collision.SetActive(false);
            body.AddRelativeForce(Vector3.up*jumpForce, ForceMode.Impulse);
            body.AddRelativeForce(Vector3.forward*-pushForce, ForceMode.Impulse);
            StartCoroutine(LerpScale(invisible, 0.75f));
            Destroy(gameObject,1.2f);
            controlSonido.PlayOneShot(sonidoDisparo);
            esDerrotado=true;
        }
    }


    IEnumerator Disparar(){
        if(!yaAtaco){
            yaAtaco=true;
            yield return new WaitForSeconds(1.2f);
            balaInicial.SetActive(true);
            StartCoroutine(LerpBala(alturaMaxima,2f));
            controlSonido.PlayOneShot(sonidoAtaque);
            yield return new WaitForSeconds(2f);
            balaInicial.SetActive(false);
            spawnBala=new Vector3(objetivo.position.x, alturaMaxima.y+15, objetivo.position.z);
            Instantiate(bala,spawnBala,objetivo.rotation);
            yield return new WaitForSeconds(1f);
            yaAtaco=false;
        }
    }


    IEnumerator LerpBala(Vector3 finish, float lerpTime){
        float timePassed=0f;
        while(timePassed<lerpTime){
            balaInicial.transform.position=Vector3.Lerp(balaInicial.transform.position, finish, timePassed/lerpTime);
            timePassed+=Time.deltaTime;
            yield return null;
        }
        balaInicial.transform.position=originalPos;
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
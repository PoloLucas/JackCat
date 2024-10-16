using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour{
    public AudioSource controlSonido;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoDerrota;
    [SerializeField]private GameObject hitbox;
    private Rigidbody physiscsBody;
    private Animator cat;
    public int health = 20;
    private float speed = 6;
    private float rotationSpeed = 200;
    private float jumpForce = 16f;
    private float attackForce = 13f;
    private bool floorDetected = true;
    private bool isAttacking = false;
    private bool estaVivo = true;
    private bool isShowing = false;
    public bool isLive = true;


    void Start(){
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
        hitbox.SetActive(false);
        cat=GameObject.FindGameObjectWithTag("Cat").GetComponent<Animator>();
        physiscsBody = GetComponent<Rigidbody>();
    }

    void Update(){
        if(estaVivo){
            MoverGato();
            SaltarGato();
            AtacarGato();
            AnimarGato();
            if(Input.GetButtonDown("Fire1") && !isAttacking){ //detecta la presión del Clic Izquierdo
                isAttacking=true;
                StartCoroutine(AtacarGato());
            }
        }
        ComprobarSalud();
    }

    public void MoverGato(){
        if(!isAttacking){
            float moveX=Input.GetAxis("Horizontal"); //mueve hacia los lados con A y D
            float moveZ=Input.GetAxis("Vertical"); //mueve hacia adelante y atrás con W y S
            float rotateY=Input.GetAxis("Mouse X"); //mueve la cámara con el Eje Horzontal del Mouse
            Vector3 movement=new Vector3(moveX,0,moveZ).normalized;
            transform.Translate(movement*speed*Time.deltaTime);
            transform.Rotate(new Vector3(0,rotateY,0)*rotationSpeed*Time.deltaTime);
        }
    }

    public void SaltarGato(){
        Vector3 floor = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, floor, 1.08f)){ //calcula la distancia al suelo con Raycast
            floorDetected = true;
        }else{
            floorDetected = false;
        }
        //si se presiona Espacio y el personaje esta en el suelo podra realizar el salto
        if (Input.GetKeyDown(KeyCode.Space) && floorDetected == true){
            physiscsBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    IEnumerator AtacarGato(){
        hitbox.SetActive(true);
        cat.SetBool("Walk",false);
        physiscsBody.AddForce(transform.forward*attackForce, ForceMode.Impulse);
        controlSonido.PlayOneShot(sonidoAtaque);
        yield return new WaitForSeconds(0.3f);
        hitbox.SetActive(false);
        isAttacking=false;
        cat.SetBool("Walk",true);
    }

    public void AnimarGato(){ //si detecta las teclas de movimiento, realizará la animación de caminar
        if(Input.GetAxis("Vertical")!=0 || Input.GetAxis("Horizontal")!=0){
            cat.SetBool("Walk",true);
        }else{
            cat.SetBool("Walk",false);
        }
    }

    public void ComprobarSalud(){
        if(health<=0){
            health=0;
            estaVivo=false;
            cat.SetBool("Walk",false);
            if(!isShowing){
                isShowing=true;
                controlSonido.PlayOneShot(sonidoDerrota);
                Cursor.lockState=CursorLockMode.None; //desbloquea la posición del cursor
                Cursor.visible=true; //vuelve a mostrar el cursor
                SceneManager.LoadScene(2); //carga escena de derrota
            }
        }
    }

    public int SetHealth {get => health; set => health=value;}
}
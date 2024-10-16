using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour{
    public AudioSource controlSonido;
    public AudioClip sonidoAtaque;
    [SerializeField]private GameObject hitbox;
    private GameObject activeHitbox;
    private GameObject enemy;
    private EnemyBehaviour script;
    private Rigidbody body;
    private Vector3 enemyPosF;
    private float attackForce = 20f;
    private bool yaAtaco = false;

    void Start(){
        enemy=transform.parent.gameObject;
        script=enemy.GetComponent<EnemyBehaviour>();
        body=enemy.GetComponent<Rigidbody>();
    }

    IEnumerator Atacar(){
        if(!yaAtaco){
            yaAtaco=true;
            yield return new WaitForSeconds(0.8f);
            body.AddRelativeForce(Vector3.forward*attackForce, ForceMode.Impulse);
            activeHitbox=Instantiate(hitbox,transform.position,transform.rotation);
            activeHitbox.transform.parent=transform;
            controlSonido.PlayOneShot(sonidoAtaque);
            yield return new WaitForSeconds(1);
        }
        script.canMove=true;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){ //si toca al jugador, carga la pantalla de Fín del Juego
            script.canMove=false;
            StartCoroutine(Atacar());
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){ //si toca al jugador, carga la pantalla de Fín del Juego
            yaAtaco=false;
        }
    }
}
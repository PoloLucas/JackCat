using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyGenerator : MonoBehaviour{
    [SerializeField] private GameObject enemy;

    void Start(){
        InvokeRepeating("GenerarEnemigo",5,7.5f);
    }

    public void GenerarEnemigo(){
        Instantiate(enemy,transform.position,transform.rotation);
    }
}
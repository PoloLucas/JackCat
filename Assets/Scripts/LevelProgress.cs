using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelProgress : MonoBehaviour{
    public GameObject[][] mat=new GameObject[2][];
    public bool[][] matBool=new bool[2][];
    [SerializeField]GameObject[] enemy;
    [SerializeField]GameObject enemyIcon;
    [SerializeField]GameObject cannonIcon;
    [SerializeField]GameObject defeatIcon;
    private RectTransform grid;
    private Vector3 celdaPos;
    private int enemyCounter = 0;
    private float celda = 48;

    public void Start(){
        mat[0]=new GameObject[10];
        mat[1]=new GameObject[3];
        matBool[0]=new bool[10];
        matBool[1]=new bool[3];
        grid=GetComponent<RectTransform>();
        celdaPos=new Vector3(84,grid.rect.height-64,0);
        LlenarMatriz();
    }

    public void Update(){
        AnalizarMatriz();
    }

    public void LlenarMatriz(){
        int k=0;
        for(int i=0; i<mat.Length; i++){
            for(int j=0; j<mat[i].Length; j++){
                GameObject iconTag;
                mat[i][j]=enemy[k];
                matBool[i][j]=false;
                if(mat[i][j].transform.tag=="Enemy"){
                    iconTag=enemyIcon;
                }else{
                    iconTag=cannonIcon;
                }
                GameObject newIcon=Instantiate(iconTag, new Vector3(celdaPos.x+celda*j, celdaPos.y-celda*i,0), grid.rotation);
                newIcon.transform.SetParent(transform);
                k++;
            }
        }
    }

    public void AnalizarMatriz(){
        for(int i=0; i<mat.Length; i++){
            for(int j=0; j<mat[i].Length; j++){
                if(mat[i][j]==null && !matBool[i][j]){
                    GameObject newIcon=Instantiate(defeatIcon, new Vector3(celdaPos.x+celda*j, celdaPos.y-celda*i,0), grid.rotation);
                    newIcon.transform.SetParent(transform);
                    enemyCounter++;
                    matBool[i][j]=true;
                    FinalizarPartida();
                }
            }
        }
    }

    public void FinalizarPartida(){
        if(enemyCounter>=enemy.Length){
            Cursor.lockState=CursorLockMode.None; //desbloquea la posición del cursor
            Cursor.visible=true; //vuelve a mostrar el cursor
            SceneManager.LoadScene(3); //carga escena de derrota
        }
    }
}
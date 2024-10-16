using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private PlayerBehaviour player;

    public void Start(){
        scoreText.GetComponent<TextMeshProUGUI>(); //obtiene el Texto del objeto
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    public void Update(){
        scoreText.text=player.health.ToString(); //actualiza la puntuación del Jugador
    }
}
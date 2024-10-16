using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHitbox : MonoBehaviour{
    void Start(){
        Destroy(gameObject,0.4f);
    }
}
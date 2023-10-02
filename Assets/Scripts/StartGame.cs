using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    [SerializeField] GameObject EnterUi;
    void Start() {
        EnterUi.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        
    }
}

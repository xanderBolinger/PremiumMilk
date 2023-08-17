using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {

    [SerializeField] public GameObject PopUp;
    [SerializeField] public TextAsset inkJSON;

    private bool inRange;

    private void Awake() {
        inRange = false;
        PopUp.SetActive(false);
    }

    private void Update() {
        if (inRange) {
            PopUp.SetActive(true);
            /*if (InputManager.GetInstance().GetInteractPressed()) {
                Debug.log(inkJSON.text);
            }*/
        } else {
            PopUp.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Player") {
            inRange = false;
        }
    }
}

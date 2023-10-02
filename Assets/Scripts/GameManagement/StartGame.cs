using UnityEngine;

public class StartGame : MonoBehaviour {

    [SerializeField] GameObject EnterUi;

    void Start() {
        EnterUi.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter(Collision collision) { 
        EnterUi.SetActive(true);
    }
}

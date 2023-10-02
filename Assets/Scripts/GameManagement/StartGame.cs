using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    [SerializeField] GameObject EnterUi;

    void Start() {
        EnterUi.SetActive(false);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.gameObject.tag != "Character")
            return;

        EnterUi.SetActive(true);
    }

    private void OnCollisionExit(Collision collision) {
        EnterUi.SetActive(false);
    }


    public void LoadLabrynth() {
        SceneManager.LoadScene("Labrynth");
    }

}

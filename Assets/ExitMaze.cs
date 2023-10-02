using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMaze : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject;
        if (obj.tag != "Character" && obj.GetComponent<CharacterController>().player) {

            SceneManager.LoadScene("End");

        }


    }

}

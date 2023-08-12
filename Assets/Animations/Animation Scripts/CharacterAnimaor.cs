using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimaor : MonoBehaviour
{

    public GameObject idle_0;
    public GameObject idle_1;


    List<GameObject> states;

    private void Start()
    {
        states = new List<GameObject>
        {
            idle_0,
            idle_1
        };
    }

    public void Deactivate() { 
        foreach(var state in states)
        {
            state.SetActive(false); 
        }
    }

    public void IdleZero() { 
        Deactivate();
        idle_0.SetActive(true);
    }

    public void IdleOne() {
        Deactivate();
        idle_1.SetActive(true);
    }

}

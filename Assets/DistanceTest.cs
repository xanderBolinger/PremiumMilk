using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTest : MonoBehaviour
{
    [SerializeField] GameObject tile1;
    [SerializeField] GameObject tile2;

    private void Start()
    {
       Debug.Log(Vector3.Distance(tile1.transform.position, tile2.transform.position));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodController : MonoBehaviour
{
    public ParticleSystem levelOneBloodPrefab;
    public ParticleSystem levelTwoBloodPrefab;
    public ParticleSystem levelThreeBloodPrefab;
    public ParticleSystem levelFourBloodPrefab;
    public ParticleSystem levelFiveBloodPrefab;

    //public List<GameObject> frontLegPositions;
    public List<GameObject> frontBodyPositions;
    //public List<GameObject> frontHeadPositions;


    public void Hit(GameObject attacker, int level)
    {
        var front = FrontFacing(attacker);

        var prefab = levelOneBloodPrefab;

        switch (level) {
            case 1:
                prefab = levelOneBloodPrefab;
                break;
            case 2:
                prefab = levelTwoBloodPrefab;
                break;
            case 3:
                prefab = levelThreeBloodPrefab;
                break;
            case 4:
                prefab = levelFourBloodPrefab;
                break;
            case 5:
                prefab = levelFiveBloodPrefab;
                break;
        }

        


    }

    private bool FrontFacing(GameObject attacker) {
        float dot = Vector3.Dot(attacker.transform.forward, (transform.position - attacker.transform.position).normalized);
        if (dot > 0.5f) // Front facing 
        {
            return true;
        }
        else
        { // Rear facing
            return false;
        }
    }

}

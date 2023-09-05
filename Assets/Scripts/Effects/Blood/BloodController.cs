using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class BloodController : NetworkBehaviour
{
    public ParticleSystem levelOneBloodPrefab;
    public ParticleSystem levelTwoBloodPrefab;
    public ParticleSystem levelThreeBloodPrefab;
    public ParticleSystem levelFourBloodPrefab;
    public ParticleSystem levelFiveBloodPrefab;

    //public List<GameObject> frontLegPositions;
    public List<GameObject> frontBodyPositions;
    public List<GameObject> rearBodyPositions;
    //public List<GameObject> frontHeadPositions;

    [ClientRpc]
    public void RpcHit(string attackerName, int level) {
        var attacker = CharacterController.GetCharacterObject(attackerName);
        Hit(attacker, level);
    }

    public void Hit(GameObject attacker, int level)
    {
        var front = FrontFacing(attacker);

        ParticleSystem prefab = levelOneBloodPrefab;

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

        var list = front ? frontBodyPositions : rearBodyPositions;
        var pos = RandomElem.GetElem(list);
        ParticleSystem blood = Instantiate(prefab/*, Vector3.zero, Quaternion.identity, pos.transform*/);
        blood.transform.SetParent(pos.transform, false);
        /*blood.transform.parent = pos.transform;
        blood.transform.position = Vector3.zero;
        blood.transform.localPosition = Vector3.zero;
        blood.transform.rotation = new Quaternion(0,0,0,0);
        blood.transform.localRotation = new Quaternion(0, 0, 0, 0);*/
        blood.Play();
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

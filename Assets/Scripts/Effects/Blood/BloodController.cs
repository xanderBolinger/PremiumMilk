using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor;

public class BloodController : NetworkBehaviour
{
    private ParticleSystem levelOneBloodPrefab;
    private ParticleSystem levelTwoBloodPrefab;
    private ParticleSystem levelThreeBloodPrefab;
    private ParticleSystem levelFourBloodPrefab;
    private ParticleSystem levelFiveBloodPrefab;

    //public List<GameObject> frontLegPositions;
    private List<GameObject> frontBodyPositions;
    private List<GameObject> rearBodyPositions;
    private List<GameObject> frontHeadPositions;
    private List<GameObject> rearHeadPositions;
    private List<GameObject> frontLegsPositions;
    private List<GameObject> rearLegsPositions;
    //public List<GameObject> frontHeadPositions;

    private void Start()
    {

        var body = gameObject.transform.Find("Body Zones");
        var leg = gameObject.transform.Find("Head Zones");
        var head = gameObject.transform.Find("Leg Zones");


    }

    private GameObject LoadPrefab(string name, string itemType)
    {
        GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/" + itemType + "/" + name + ".prefab");

        return asset;
    }

    [ClientRpc]
    public void RpcHit(string attackerName, int level, string location) {
        var attacker = CharacterController.GetCharacterObject(attackerName);
        Hit(attacker, level, location);
    }

    private ParticleSystem GetPrefab(int level) {

        switch (level)
        {
            case 1:
                return levelOneBloodPrefab;
            case 2:
                return levelTwoBloodPrefab;
            case 3:
                return levelThreeBloodPrefab;
            case 4:
                return levelFourBloodPrefab;
            case 5:
                return levelFiveBloodPrefab;
        }

        throw new System.Exception("Level not 1-5, blood prefab: "+level);
    }

    private List<GameObject> GetList(MeleeHitLocationData.HitLocationZone zone, bool front) {
        if (zone == MeleeHitLocationData.HitLocationZone.Head) {
            return front ? frontHeadPositions : rearHeadPositions;
        } else if (zone == MeleeHitLocationData.HitLocationZone.Legs) {
            return front ? frontLegsPositions : rearLegsPositions;
        } else if (zone == MeleeHitLocationData.HitLocationZone.Body) {
            return front ? frontBodyPositions : rearBodyPositions;
        } else
            throw new System.Exception("Zone not found for zone: "+zone);
    }


    public void Hit(GameObject attacker, int level, string location)
    {
        var front = FrontFacing(attacker);
        ParticleSystem prefab = GetPrefab(level);
        var hitZone = MeleeHitLocationData.locationData.GetHitLocationZone(location);
        var list = GetList(hitZone, front);

        var pos = RandomElem.GetElem(list);
        ParticleSystem blood = Instantiate(prefab);
        blood.transform.SetParent(pos.transform, false);
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

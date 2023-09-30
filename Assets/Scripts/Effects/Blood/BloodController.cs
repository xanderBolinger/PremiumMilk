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
        levelOneBloodPrefab = LoadPrefab(1);
        levelTwoBloodPrefab = LoadPrefab(2);
        levelThreeBloodPrefab = LoadPrefab(3);
        levelFourBloodPrefab = LoadPrefab(4);
        levelFiveBloodPrefab = LoadPrefab(5);

        frontBodyPositions = new List<GameObject>();
        frontHeadPositions = new List<GameObject>();
        frontLegsPositions = new List<GameObject>();
        rearBodyPositions = new List<GameObject>();
        rearHeadPositions = new List<GameObject>();
        rearLegsPositions = new List<GameObject>();

        CreateList(frontBodyPositions, "Body Zones Front");
        CreateList(frontHeadPositions, "Head Zones Front");
        CreateList(frontLegsPositions, "Leg Zones Front");
        CreateList(rearBodyPositions, "Body Zones Rear");
        CreateList(rearHeadPositions, "Head Zones Rear");
        CreateList(rearLegsPositions, "Leg Zones Rear");
    }

    private void CreateList(List<GameObject> list, string transformName) {
        Transform transform = gameObject.transform.Find("Blood Zones").Find(transformName);

        foreach(Transform child in transform)
            list.Add(child.gameObject);

    }

    private ParticleSystem LoadPrefab(int level)
    {
        GameObject asset = Resources.Load<GameObject>("Prefabs/Effects/Blood/BloodLevel"+level);

        return asset.GetComponent<ParticleSystem>();
    }

    [ClientRpc]
    public void RpcHit(string attackerName, int level, string location) {
        var attacker = CharacterController.GetCharacterObject(attackerName);

        StartCoroutine(Hit(attacker, level, location));
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

    private List<GameObject> GetList(HitZoneData.HitLocationZone zone, bool front) {
        if (zone == HitZoneData.HitLocationZone.Head) {
            return front ? frontHeadPositions : rearHeadPositions;
        } else if (zone == HitZoneData.HitLocationZone.Legs) {
            return front ? frontLegsPositions : rearLegsPositions;
        } else if (zone == HitZoneData.HitLocationZone.Body) {
            return front ? frontBodyPositions : rearBodyPositions;
        } else
            throw new System.Exception("Zone not found for zone: "+zone);
    }


    public IEnumerator Hit(GameObject attacker, int level, string location)
    {

        var animator = attacker.GetComponent<CharacterAnimator>();

        yield return new WaitUntil(() => animator.attackFinished);

        var front = FrontFacing(attacker);
        ParticleSystem prefab = GetPrefab(level);
        var hitZone = HitZoneData.locationData.GetHitLocationZone(location);
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

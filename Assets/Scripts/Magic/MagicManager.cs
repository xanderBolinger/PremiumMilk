using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MagicManager : NetworkBehaviour
{

    [SerializeField] GameObject spellEffect;
    [SerializeField] Transform spellStartPos;
    [SerializeField] GameObject seekerTarget;

    bool casted = false;

    private void Update()
    {
        if (seekerTarget == null || casted)
            return;

        SpawnSpellEffect();
        casted = true;
    }

    public void SpawnSpellEffect() {
        var obj = Instantiate(spellEffect, spellStartPos.position, Quaternion.identity);

        obj.GetComponent<Seeker>().Launch(seekerTarget);

        NetworkServer.Spawn(obj);
    }


}

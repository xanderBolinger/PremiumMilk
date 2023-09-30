using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static MagicManager;
using Character; 

public class CharacterMagic : NetworkBehaviour
{
    [SerializeField] Spell castSpellType;
    [SerializeField] string targetName;
    [SerializeField] Transform castPos;

    public bool castedSpell;

    // Client call
    public void CastSpell() {

        castedSpell = true;

        // This method gets called on server
        CmdCastSpell();
    }

    public void GetTargetName(GameObject selectedTarget) {
        targetName = selectedTarget.GetComponent<CharacterNetwork>().characterName;
    }

    // Called on server
    [Command]
    private void CmdCastSpell() {
        MagicManager.magicManager.CastSpell(castSpellType, targetName, castPos);
    }

}

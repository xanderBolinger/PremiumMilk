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

        // This method gets called on server, target name has to be passed because it is not set on the server 
        // Could use a sync var but they are kinda annoying
        CmdCastSpell(targetName);
    }

    // Use this method to set target name from your player input code once you have clicked on a selected target
    public void GetTargetName(GameObject selectedTarget) {
        targetName = selectedTarget.GetComponent<CharacterNetwork>().characterName;
    }

    // Called on server
    [Command]
    private void CmdCastSpell(string targetName) {
        MagicManager.magicManager.CastSpell(castSpellType, targetName, castPos);
    }

}

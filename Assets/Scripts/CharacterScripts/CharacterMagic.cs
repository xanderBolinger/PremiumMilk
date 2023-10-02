using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static MagicManager;

public class CharacterMagic : NetworkBehaviour
{
    [SerializeField] Spell castSpellType;
    [SerializeField] string targetName;
    [SerializeField] Transform castPos;

    public bool castedSpell;

    string casterName;

    CharacterAnimator characterAnimator;

    public void SetVariables(Spell castSpellType, string targetName) {
        this.castSpellType = castSpellType;
        this.targetName = targetName;
    }

    private void Awake()
    {
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    private IEnumerator Start()
    {
        var cn = GetComponent<CharacterNetwork>();

        yield return new WaitUntil(() => cn.GetCharacterSheet() != null);

        casterName = cn.GetCharacterSheet().name;        
    }

    // Client call
    public void CastSpell() {
        if (castedSpell)
            return;
        castedSpell = true;

        if(targetName!="")
            characterAnimator.RotateTowardsTarget(CharacterController.GetCharacterObject(targetName));

        StartCoroutine(CoroutineCast());
    }


    // Allows for time delay before cast command to allow additional time for model to rotate 
    IEnumerator CoroutineCast() {
        yield return new WaitForSecondsRealtime(0.1f);

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
        MagicManager.magicManager.CastSpell(castSpellType, casterName, targetName, castPos);
    }

}

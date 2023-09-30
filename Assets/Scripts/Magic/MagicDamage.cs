using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using static MeleeCombatManager;
public class MagicDamage : MonoBehaviour
{

    public void ApplyPhysicalDamage(string spell, int damage, string casterName, string targetName) {
        var casterObj = CharacterController.GetCharacterObject(casterName);
        var targetObj = CharacterController.GetCharacterObject(targetName);

        CharacterAnimator casterAnimator = casterObj.GetComponent<CharacterAnimator>();
        casterAnimator.attackFinished = true; 

        CharacterSheet target = targetObj.GetComponent<CharacterNetwork>().GetCharacterSheet();
        BloodController targetBloodController = targetObj.GetComponent<BloodController>();
        CharacterAnimator animator = targetObj.GetComponent<CharacterAnimator>();

        var injury = new Injury(damage, 0, damage/200, damage/10, spell, casterName, "body", 0);

        var combatants = GetCombatants(targetName);

        foreach (var c in combatants) {
            c.ApplyPain(damage / 200);
            c.ApplyShock(damage / 10);
        }

        target.medicalData.AddInjury(injury);
        target.medicalData.Knockout(0);

        animator.RpcHit(casterName);

        if (!target.Alive()) {
            animator.RpcDead(casterName);
        }

        targetBloodController.RpcHit(casterName, damage / 200, "Body");

        Debug.Log("apply damage from spell");
    }


}

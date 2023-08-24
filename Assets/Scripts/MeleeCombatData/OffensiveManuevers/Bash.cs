using static ExcelUtillity.MeleeHitLocation;

public class Bash : IOffensiveManuever
{
    public OffensiveManuevers.OffensiveManueverType manueverType => OffensiveManuevers.OffensiveManueverType.BASH;

    public int GetActivationCost()
    {
        return 0;
    }

    public string GetManeuverName()
    {
        return "Bash";
    }

    public int GetTargetNumber(Combatant attacker, MeleeDamageType meleeDamageType)
    {
        return attacker.meleeWeaponStatBlock.atnCut;
    }

    public void ResolveOffensiveManever(Exchange exchange, MeleeDamageType meleeDamageType)
    {
        exchange.ApplyMeleeHit(MeleeDamageType.BLUNT);
        exchange.SetAttackerInitative();
        exchange.reachWinnder = exchange.attacker;
    }

    public void SetSimultaneousDefense(int dice, SelectManuever selectManuever)
    {
        throw new System.NotImplementedException();
    }

    public bool SimultaneousDefense()
    {
        return false;
    }

    public bool RequiresShield() { return false; }

    public (bool, int) AlternateReachCost(int reachCost) {
        return (false, 0);
    }

    public bool weaponBeat { get; set; }
    public void SetWeaponBeat(bool weaponBeat) { this.weaponBeat = weaponBeat; }
}

using System.Collections;
using System.Collections.Generic;
using Character;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static MeleeCombatManager;

public class MeleeCombatManagerTests
{

    MeleeCombatManager manager;
    CharacterSheet characterSheetOne;
    CharacterSheet characterSheetTwo;
    CharacterSheet characterSheetThree;

    // A Test behaves as an ordinary method
    [SetUp]
    public void init()
    {
        var obj = new GameObject();
        manager = obj.AddComponent<MeleeCombatManager>();
        manager.firstExchange = true;
        manager.bouts = new List<Bout>();
        MeleeCombatManager.meleeCombatManager = manager;
        //var wep = MeleeWeaponReader.GetWeaponStatBlock("Kern Axe");
        characterSheetOne = MeleeCombatTests.CreateCharater1();
        characterSheetTwo = MeleeCombatTests.CreateCharacter2();
        characterSheetThree = CreateCharacter3();
        var wep = MeleeWeaponLoader.GetWeaponByName("Kern Axe");
        characterSheetOne.meleeCombatStats.weapon = wep;
        var wep2 = MeleeWeaponLoader.GetWeaponByName("Falchion");
        characterSheetTwo.meleeCombatStats.weapon = wep2;
    }

    private CharacterSheet CreateCharacter3() {
        Species human = new Species();
        human.speciesType = Species.SpeciesType.MEDIUM_SIZE;
        human.arms = 2;
        human.legs = 2;
        human.onlyLegs = false;
        Attributes attributes = new Attributes(10, 10, 10, 10, 10);
        MedicalData medicalData = new MedicalData(attributes, human);
        MeleeCombatStats meleeCombatStats = new MeleeCombatStats();
        meleeCombatStats.LearnProficiency(MeleeProficiencies.CutNThrust, 5);
        meleeCombatStats.SetCurrProf(MeleeProficiencies.CutNThrust);
        meleeCombatStats.CalcReflexes(attributes);
        var characterSheet = new CharacterSheet("test", human, attributes, medicalData, meleeCombatStats);

        var armor = ArmorLoader.ReadArmor();
        characterSheet.meleeCombatStats.armorPieces.Add(armor[0]);
        var wep = MeleeWeaponLoader.GetWeaponByName("Kern Axe");
        characterSheet.meleeCombatStats.weapon = wep;
        return characterSheet;
    }

    private (Combatant, Combatant) RedBlue() {
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        manager.Declare(combatantA, MeleeStatus.RED);
        manager.Declare(combatantB, MeleeStatus.BLUE);

        return (combatantA, combatantB);
    }

    private (Combatant, Combatant) BlueRed()
    {
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        manager.Declare(combatantA, MeleeStatus.BLUE);
        manager.Declare(combatantB, MeleeStatus.RED);

        return (combatantA, combatantB);
    }

    private (Combatant, Combatant) BlueBlue() {
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        manager.Declare(combatantA, MeleeStatus.BLUE);
        manager.Declare(combatantB, MeleeStatus.BLUE);

        return (combatantA, combatantB);
    }

    private (Combatant, Combatant) RedRed()
    {
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        manager.Declare(combatantA, MeleeStatus.RED);
        manager.Declare(combatantB, MeleeStatus.RED);

        return (combatantA, combatantB);
    }

    [Test]
    public void EnterCombatTests() {
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);

        Assert.AreEqual(1, manager.bouts.Count);
        Assert.AreEqual(characterSheetOne, manager.bouts[0].combatantA.characterSheet);
        Assert.AreEqual(characterSheetTwo, manager.bouts[0].combatantB.characterSheet);
    }

    [Test]
    public void DeclareTests()
    {
        var (combatantA, combatantB) = RedBlue();

        Assert.AreEqual(MeleeStatus.RED, combatantA.meleeDecision);
        Assert.AreEqual(MeleeStatus.BLUE, combatantB.meleeDecision);
    }

    [Test]
    public void GetPositionsRedBlue() {
        var (combatantA, combatantB) = RedBlue();
        var bout = manager.bouts[0];
        var attackers = manager.GetAttackers();
        var defenders = manager.GetDefenders();
        Assert.AreEqual(combatantA, attackers[bout][0]);
        Assert.AreEqual(combatantB, defenders[bout][0]);
    }

    [Test]
    public void GetPositionsBlueRed()
    {
        var (combatantA, combatantB) = BlueRed();
        var bout = manager.bouts[0];
        var attackers = manager.GetAttackers();
        var defenders = manager.GetDefenders();
        Assert.AreEqual(combatantB, attackers[bout][0]);
        Assert.AreEqual(combatantA, defenders[bout][0]);
    }

    [Test]
    public void GetPositionsBlueBlue() {
        var (combatantA, combatantB) = BlueBlue();
        var attackers = manager.GetAttackers();
        var defenders = manager.GetDefenders();
        Assert.AreEqual(0, attackers.Count);
        Assert.AreEqual(0, defenders.Count);
    }


    [Test]
    public void GetPositionsRedRed()
    {
        var (combatantA, combatantB) = RedRed();
        var bout = manager.bouts[0];
        var attackers = manager.GetAttackers();
        var defenders = manager.GetDefenders();
        Assert.AreEqual(1, attackers.Count);
        Assert.AreEqual(0, defenders.Count);
        Assert.AreEqual(combatantA, attackers[bout][0]);
        Assert.AreEqual(combatantB, attackers[bout][1]);
    }

    [Test]
    public void MultipleCombatantsTestDeclare() {
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        manager.EnterMeleeCombat(characterSheetOne, characterSheetThree);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        var combatantA1 = manager.bouts[1].combatantA;
        var combatantB2 = manager.bouts[1].combatantB;

        Assert.AreEqual(combatantA.characterSheet, combatantA1.characterSheet);

        manager.Declare(combatantA, MeleeStatus.RED);
        manager.Declare(combatantB, MeleeStatus.RED);
        manager.Declare(combatantA1, MeleeStatus.LEAVE_COMBAT);
        manager.Declare(combatantB2, MeleeStatus.RED);

        var attackers = manager.GetAttackers();
        var defenders = manager.GetDefenders();
        Assert.AreEqual(0, defenders.Count);
        Assert.AreEqual(2, attackers.Count);
    }

    [Test]
    public void CreateExchanges() {
        initExchanges();

        Assert.AreEqual(1, manager.redRed.Count);
        Assert.AreEqual(1, manager.redNone.Count);
        Assert.AreEqual(0, manager.redBlue.Count);
        Assert.AreEqual(0, manager.blueBlue.Count);
    }

    private void initExchanges() {
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        manager.EnterMeleeCombat(characterSheetOne, characterSheetThree);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        var combatantA1 = manager.bouts[1].combatantA;
        var combatantB2 = manager.bouts[1].combatantB;
        manager.Declare(combatantA, MeleeStatus.RED);
        manager.Declare(combatantB, MeleeStatus.RED);
        manager.Declare(combatantA1, MeleeStatus.LEAVE_COMBAT);
        manager.Declare(combatantB2, MeleeStatus.RED);
        var attackers = manager.GetAttackers();

        foreach (var attacker in attackers)
        {

            foreach (var a in attacker.Value)
            {
                a.SetSelectedManeuver(new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 10, 5, ExcelUtillity.MeleeHitLocation.MeleeDamageType.CUTTING,0));
            }

        }


        manager.CreateExchanges();
    }

    [Test]
    public void ResolveExchanges() {
        initExchanges();
        DiceRoller.AddNextTestValue(0);
        DiceRoller.AddNextTestValue(0);
        DiceRoller.AddNextTestValue(0);
        DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(0);
        DiceRoller.AddNextTestValue(0);
        DiceRoller.AddNextTestValue(0);
        DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(0);
        DiceRoller.AddNextTestValue(0);
        DiceRoller.AddNextTestValue(0);

        /*for(int i = 0; i < 5; i++)
            DiceRoller.AddNextTestValue(10);*/
        //DiceRoller.AddNextTestValue(1);
        for (int i = 0; i < 5; i++)
            DiceRoller.AddNextTestValue(10);
        //DiceRoller.AddNextTestValue(1);

        manager.ResolveExchanges();

        Assert.AreEqual(true, characterSheetOne.medicalData.GetPD() > 0);
        Assert.AreEqual(true, characterSheetTwo.medicalData.GetPD() > 0);
        Assert.AreEqual(false, characterSheetThree.medicalData.GetPD() > 0);
        Assert.AreEqual(true, manager.bouts[0].onPause);
        Assert.AreEqual(true, manager.bouts[1].onPause);

        manager.ResolveExchanges();

        Assert.AreEqual(false, manager.bouts[0].onPause);
        Assert.AreEqual(false, manager.bouts[1].onPause);
    }

    [Test]
    public void RefilPools() {
        
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        //manager.EnterMeleeCombat(characterSheetOne, characterSheetThree);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        manager.Declare(combatantA, MeleeStatus.RED);
        manager.Declare(combatantB, MeleeStatus.BLUE);

        Assert.AreEqual(false, combatantA.AssignCP(9, 0));
        Assert.AreEqual(true, combatantA.AssignCP(8, 0));
        Assert.AreEqual(true, combatantB.AssignCP(8, 0));
        combatantA.currentDice = combatantA.diceAssignedToBout;
        combatantB.currentDice = combatantB.diceAssignedToBout;
        combatantA.SetSelectedManeuver(new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 5, 5, ExcelUtillity.MeleeHitLocation.MeleeDamageType.CUTTING, 0));
        combatantB.SetSelectedManeuver(new SelectManuever(DefensiveManuevers.DefensiveManueverType.PARRY, 5, 0));

        for (int i = 0; i < 30; i++)
            DiceRoller.AddNextTestValue(1);

        manager.CreateExchanges();
        manager.ResolveExchanges();
        Assert.AreEqual(3, combatantA.currentDice);
        Assert.AreEqual(3, combatantB.currentDice);
        Assert.AreEqual(combatantB, manager.bouts[0].initativeCombatant);

        manager.bouts[0].initativeCombatant = null;

        combatantA.SetSelectedManeuver(new SelectManuever());
        combatantB.SetSelectedManeuver(new SelectManuever());

        manager.CreateExchanges();
        manager.ResolveExchanges();
        Assert.AreEqual(8, combatantA.currentDice);
        Assert.AreEqual(8, combatantB.currentDice);
        Assert.AreEqual(null, manager.bouts[0].initativeCombatant);
        DiceRoller.GetSuccess(30, 0);
    }

    [Test]
    public void KnockDownTest()
    {

        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        manager.EnterMeleeCombat(characterSheetOne, characterSheetThree);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        var combatantA1 = manager.bouts[1].combatantA;
        var combatantB1 = manager.bouts[1].combatantB;
        manager.Declare(combatantA, MeleeStatus.LEAVE_COMBAT);
        manager.Declare(combatantB, MeleeStatus.LEAVE_COMBAT);
        manager.Declare(combatantA1, MeleeStatus.LEAVE_COMBAT);
        manager.Declare(combatantB1, MeleeStatus.LEAVE_COMBAT);

        Assert.AreEqual(false, combatantA.AssignCP(9, 0));
        Assert.AreEqual(true, combatantA.AssignCP(8, 0));
        Assert.AreEqual(true, combatantB.AssignCP(8, 0));
        Assert.AreEqual(true, combatantA1.AssignCP(8, 0));
        Assert.AreEqual(true, combatantB1.AssignCP(8, 0));

        combatantA.currentDice = combatantA.diceAssignedToBout;
        combatantB.currentDice = combatantB.diceAssignedToBout;
        combatantA1.currentDice = combatantA.diceAssignedToBout;
        combatantB1.currentDice = combatantB.diceAssignedToBout;
        combatantA.SetSelectedManeuver(new SelectManuever());
        combatantB.SetSelectedManeuver(new SelectManuever());
        combatantA1.SetSelectedManeuver(new SelectManuever());
        combatantB1.SetSelectedManeuver(new SelectManuever());

        manager.CreateExchanges();
        manager.ResolveExchanges();

        manager.bouts[0].initativeCombatant = null;
        manager.bouts[1].initativeCombatant = null;

        combatantA.SetSelectedManeuver(new SelectManuever());
        combatantB.SetSelectedManeuver(new SelectManuever());
        combatantA1.SetSelectedManeuver(new SelectManuever());
        combatantB1.SetSelectedManeuver(new SelectManuever());
        combatantA.knockedDown = true;
        combatantA1.knockedDown = true;

        manager.CreateExchanges();
        manager.ResolveExchanges();
        Assert.AreEqual(4, combatantA.currentDice);
        Assert.AreEqual(4, combatantA1.currentDice);
    }

    [Test]
    public void ReachAndActivationCostTest() {
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        //manager.EnterMeleeCombat(characterSheetOne, characterSheetThree);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        manager.Declare(combatantA, MeleeStatus.RED);
        manager.Declare(combatantB, MeleeStatus.BLUE);
        combatantA.meleeWeaponStatBlock.reach = 0;
        combatantB.meleeWeaponStatBlock.reach = 1;

        Assert.AreEqual(1, MeleeCombatManager.GetReachCost(combatantA, combatantB, manager.bouts[0]));
        Assert.AreEqual(1, DefensiveManuevers.
            GetManuever(DefensiveManuevers.DefensiveManueverType.FULLEVASION).GetActivationCost());


        Assert.AreEqual(false, combatantA.AssignCP(9, 0));
        Assert.AreEqual(true, combatantA.AssignCP(8, 0));
        Assert.AreEqual(true, combatantB.AssignCP(8, 0));
        combatantA.currentDice = combatantA.diceAssignedToBout;
        combatantB.currentDice = combatantB.diceAssignedToBout;
        combatantA.SetSelectedManeuver(new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 7, 5, ExcelUtillity.MeleeHitLocation.MeleeDamageType.CUTTING, 1));
        combatantB.SetSelectedManeuver(new SelectManuever(DefensiveManuevers.DefensiveManueverType.FULLEVASION, 7, 1));
        for (int i = 0; i < 100; i++)
            DiceRoller.AddNextTestValue(0);

        manager.CreateExchanges();
        manager.ResolveExchanges();

        Assert.AreEqual(0, combatantA.currentDice);
        Assert.AreEqual(0, combatantB.currentDice);

        DiceRoller.ClearTestValues();
    }

    [Test]
    public void ExchangeCounterTest()
    {

        Assert.AreEqual(true, manager.firstExchange);

        manager.CreateExchanges();
        manager.ResolveExchanges();

        Assert.AreEqual(false, manager.firstExchange);

        manager.ResolveExchanges();

        Assert.AreEqual(true, manager.firstExchange);

    }

    [Test]
    public void ApplyPainTest() {
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        //manager.EnterMeleeCombat(characterSheetOne, characterSheetThree);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        manager.Declare(combatantA, MeleeStatus.RED);
        manager.Declare(combatantB, MeleeStatus.BLUE);

        Assert.AreEqual(false, combatantA.AssignCP(9, 0));
        Assert.AreEqual(true, combatantA.AssignCP(8, 0));
        Assert.AreEqual(true, combatantB.AssignCP(8, 0));

        combatantA.RefillDice();
        combatantB.RefillDice();
        manager.CreateExchanges();
        manager.ResolveExchanges();

        combatantA.ApplyPain(5);
        combatantB.ApplyPain(6);

        Assert.AreEqual(6, combatantA.diceAssignedToBout);
        Assert.AreEqual(5, combatantB.diceAssignedToBout);
        Assert.AreEqual(6, combatantA.currentDice);
        Assert.AreEqual(5, combatantB.currentDice);
        Assert.AreEqual(6, combatantA.characterSheet.meleeCombatStats.GetMaxCp(2));
        Assert.AreEqual(5, combatantB.characterSheet.meleeCombatStats.GetMaxCp(3));
    }

    [Test]
    public void ApplySockTest()
    {
        manager.EnterMeleeCombat(characterSheetOne, characterSheetTwo);
        //manager.EnterMeleeCombat(characterSheetOne, characterSheetThree);
        var combatantA = manager.bouts[0].combatantA;
        var combatantB = manager.bouts[0].combatantB;
        manager.Declare(combatantA, MeleeStatus.RED);
        manager.Declare(combatantB, MeleeStatus.BLUE);

        Assert.AreEqual(false, combatantA.AssignCP(9, 0));
        Assert.AreEqual(true, combatantA.AssignCP(8, 0));
        Assert.AreEqual(true, combatantB.AssignCP(8, 0));

        combatantA.RefillDice();
        combatantB.RefillDice();

        // 5 - 1 = 4 
        combatantA.ApplyShock(100);
        Assert.AreEqual(4, combatantA.currentDice);

        manager.CreateExchanges();
        manager.ResolveExchanges();

        combatantA.ApplyShock(180);
        Assert.AreEqual(0, combatantA.currentDice);
        Assert.AreEqual(4, combatantA.penalty);

        manager.CreateExchanges();
        manager.ResolveExchanges();

        Assert.AreEqual(4, combatantA.currentDice);

    }

}

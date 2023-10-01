using Character;

public class LightSpell : ISpellSystem
{
    public bool CanCast()
    {
        return false;
    }

    public void Cast(CharacterSheet characterSheet)
    {
        characterSheet.fatigueSystem.AddWork(0.5f, 10);
        
    }

    public bool CastFailed()
    {
        return false;
    }

    public bool CastSuccessful()
    {
        return false;
    }
}

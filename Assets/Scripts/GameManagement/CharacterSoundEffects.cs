using Mirror;
using UnityEngine;

public class CharacterSoundEffects : NetworkBehaviour
{
    public AudioSource death;
    public AudioSource walk;
    public AudioSource itemEquip;
    public AudioSource inventoryOpen;
    public AudioSource swing;
    public AudioSource hit;

    [ClientRpc]
    public void RpcHit() {
        Play(hit);
    }

    public void Walk() {
        Play(walk);
    }

    public void Inventory()
    {
        Play(inventoryOpen);
    }

    public void ItemEquip()
    {
        Play(itemEquip);
    }

    public void Death()
    {
        Play(death);
    }

    private void Play(AudioSource source)
    {
        if (source == null || source.isPlaying)
            return;
        source.Play();
    }

    public void Swing()
    {
        Play(swing);
    }
}

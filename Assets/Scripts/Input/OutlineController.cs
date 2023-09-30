using UnityEngine;

public class OutlineController : MonoBehaviour
{
    QuickOutline outline;

    private void Awake()
    {
        outline = GetComponent<QuickOutline>();
        TurnOffSelector();
    }

    public void TurnOffSelector()
    {
        outline.enabled = false;
    }

    public void TurnOnSelector()
    {
        outline.enabled = true;
    }
}

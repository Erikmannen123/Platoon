using UnityEngine;

public class Squad : MonoBehaviour
{
    PlayerInteractions playerInteractions;

    private void Awake()
    {
        playerInteractions = FindAnyObjectByType<PlayerInteractions>();
    }

    private void OnMouseDown()
    {
        playerInteractions.SelectedTarget = this.gameObject;
    }
}

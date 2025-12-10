using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    private PlayerInput input;

    InputAction interact;

    public GameObject selectedTarget;

    [SerializeField] private LayerMask interactMask;

    private void OnEnable()
    {
        input = GetComponent<PlayerInput>();
        input.enabled = true;
        interact = input.actions["Player/Attack"];
    }

    private void Update()
    {
        if (interact.WasPerformedThisFrame())
        {
            Clicked();
        }

        
    }

    void Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, interactMask))
        {
            switch (hit.collider.gameObject.tag)
            {
                case "Blue":
                    selectedTarget = hit.collider.gameObject;
                    break;

                case "Terrain":
                    if (selectedTarget != null)
                    {
                        I_PassTarget i_PassTarget = selectedTarget.GetComponent<I_PassTarget>();
                        if (i_PassTarget != null)
                        {
                            i_PassTarget.PassTarget((Vector3)hit.point);
                        }
                    }
                    break;
            }
        }
    }
}

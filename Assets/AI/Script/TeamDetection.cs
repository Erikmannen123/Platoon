using UnityEngine;

public class TeamDetection : MonoBehaviour
{
    [SerializeField] Material Blue;
    [SerializeField] Material Red;

    private void Awake()
    {
        if(this.transform.parent.gameObject.tag == "Blue")
        {
            this.gameObject.GetComponent<Renderer>().material = Blue;
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material = Red;
        }
    }
}

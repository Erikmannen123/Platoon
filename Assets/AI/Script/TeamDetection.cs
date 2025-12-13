using UnityEngine;

public class TeamDetection : MonoBehaviour
{
    [SerializeField] Renderer m_Renderer;
    [SerializeField] GameObject child;

    [SerializeField] Material Blue;
    [SerializeField] Material Red;

    bool colorsSet = false;

    private void Start()
    {
        if (!colorsSet)
        {
            SetColor(this.gameObject.tag);
        }
    }

    public void SetTeam(string tag)
    {
        if (tag == "Blue")
        {
            m_Renderer.material = Blue;
        }
        else
        {
            m_Renderer.material = Red;
        }

        this.gameObject.layer = LayerMask.NameToLayer(tag);

        child.layer = LayerMask.NameToLayer(tag);

        this.gameObject.tag = tag;

        child.tag = tag;


        colorsSet = true;
    }

    public void SetColor(string tag)
    {
        if (tag == "Blue")
        {
            m_Renderer.material = Blue;
        }
        else
        {
            m_Renderer.material = Red;
        }

        colorsSet = true;
    }
}

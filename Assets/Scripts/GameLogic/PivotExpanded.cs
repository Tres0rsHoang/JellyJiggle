using UnityEngine;

public class PivotExpanded : MonoBehaviour
{
    private bool expandedH = false;

    private bool expandedV = false;

    public void ExpandHorizontal()
    {
        if (!expandedH)
            transform.localScale += new Vector3(0.5f, 0, 0);
        expandedH = true;
    }

    public void ExpandVertical()
    {
        if (!expandedV)
            transform.localScale += new Vector3(0, 0, 0.5f);
        expandedV = true;
    }

    void OnDestroy()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }
}

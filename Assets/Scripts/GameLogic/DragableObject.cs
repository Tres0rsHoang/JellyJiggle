using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DragableObject : MonoBehaviour
{
    private float zCoord;
    private Vector3 offset;

    [HideInInspector]
    public Slot prevSlot;

    [HideInInspector]
    public List<Transform> parrentsAfterDrag;

    private bool disable = false;

    private void OnMouseDown()
    {
        if (disable)
        {
            return;
        }

        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        offset = transform.position - Camera.main.ScreenToWorldPoint(mousePoint);
        prevSlot = transform.parent.GetComponent<Slot>();

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    private void OnMouseDrag()
    {
        if (disable)
        {
            return;
        }

        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePoint) + offset;
        transform.position = new Vector3(targetPos.x, 0.25f, targetPos.z);

        if (parrentsAfterDrag.Count == 0)
        {
            return;
        }

        Slot currentSlot = parrentsAfterDrag.Last().gameObject.GetComponent<Slot>();

        foreach (Transform parrent in parrentsAfterDrag)
        {
            Slot pSlot = parrent.gameObject.GetComponent<Slot>();
            if (pSlot == currentSlot)
            {
                pSlot.TurnOnPanelColor();
            }
            else
            {
                pSlot.TurnOffPanelColor();
            }
        }
    }

    private void OnMouseUp()
    {
        if (disable)
        {
            return;
        }
        prevSlot.isOccupied = false;

        Transform parrent =
            parrentsAfterDrag.Count != 0 ? parrentsAfterDrag.Last() : prevSlot.transform;

        if (parrent != null)
        {
            transform.SetParent(parrent);
            Slot slot = parrent.GetComponent<Slot>();
            slot.isOccupied = true;

            StartCoroutine(GridBoard.Instance.Calculate());

            if (slot != prevSlot)
            {
                disable = true;
            }
        }
    }

    void Start()
    {
        parrentsAfterDrag.Add(transform.parent);
    }
}

using UnityEngine;

public class Slot : MonoBehaviour
{
    public Vector2Int gridPos;

    [HideInInspector]
    public bool isOccupied = false;

    [HideInInspector]
    public bool isCreateJellyBlock = false;

    [SerializeField]
    private GameObject jellyBlockPrefab;

    private Color defaultColor;
    public GameObject slotPanel;

    void Start()
    {
        slotPanel = transform.Find("SlotPanel").gameObject;
        defaultColor = slotPanel.GetComponent<MeshRenderer>().material.color;
    }

    void Update()
    {
        if (isOccupied)
        {
            Transform jellyTransform = transform.Find("JellyBlock");

            if (jellyTransform != null)
            {
                jellyTransform.SetAsFirstSibling();
                jellyTransform.localPosition = Vector3.zero;
            }
        }

        if (isCreateJellyBlock && !isOccupied)
        {
            GameObject jellyBlock = Instantiate(jellyBlockPrefab, transform);
            jellyBlock.name = "JellyBlock";
            isOccupied = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isOccupied)
        {
            DragableObject jellyDragable = other.gameObject.GetComponent<DragableObject>();
            jellyDragable.parrentsAfterDrag.Add(transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<DragableObject>(out var jellyDragable))
        {
            jellyDragable.parrentsAfterDrag.Remove(transform);
            TurnOffPanelColor();
        }
    }

    public void TurnOnPanelColor()
    {
        if (isCreateJellyBlock)
        {
            return;
        }

        slotPanel.GetComponent<MeshRenderer>().material.color = Color.darkRed;
    }

    public void TurnOffPanelColor()
    {
        if (isOccupied && transform.childCount > 1)
        {
            return;
        }
        slotPanel.GetComponent<MeshRenderer>().material.color = defaultColor;
    }

    public Vector2 GetPosition()
    {
        string slotName = gameObject.name;

        if (string.IsNullOrEmpty(slotName))
            return Vector2.zero;

        string[] parts = slotName.Split('_');

        if (parts.Length < 3)
        {
            Debug.LogWarning($"Invalid Slot Name: {slotName}");
            return Vector2.zero;
        }

        if (int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
        {
            return new Vector2(y, x);
        }

        Debug.LogWarning($"Get Error In Parsing Slot Name: {slotName}");
        return Vector2.zero;
    }

    public string GetColorString()
    {
        string result = "0000";
        if (!isOccupied)
        {
            return result;
        }

        Transform jelly = transform.Find("JellyBlock");
        if (jelly == null)
        {
            return result;
        }

        return jelly.GetComponent<JellyBlockManager>().GetColorString();
    }

    public void SetColorString(string colorCode)
    {
        Transform jelly = transform.Find("JellyBlock");
        if (jelly == null)
        {
            return;
        }
        if (colorCode == "0000")
        {
            Destroy(jelly.gameObject);
            isOccupied = false;
            TurnOffPanelColor();
            return;
        }
        jelly.GetComponent<JellyBlockManager>().SetColorString(colorCode);
    }
}

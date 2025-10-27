using UnityEngine;

public class JellyCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    public int slotQuantity = 1;
    public float spacing = 1.3f;

    public static JellyCreator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        slotQuantity = GameManager.Instance.data.GetJellyCreator();
        for (int i = 0; i < slotQuantity; i++)
        {
            Vector3 pos = new(i * spacing, 0, 0);
            GameObject slot = Instantiate(slotPrefab, pos, Quaternion.identity, transform);
            slot.GetComponent<Slot>().isCreateJellyBlock = true;
            slot.name = $"Creator_Slot_{i}";
        }
    }

    void LateUpdate()
    {
        GridBoard grid = GridBoard.Instance;
        float centerWidth =
            ((grid.width - 1) * grid.spacing / 2f) - (slotQuantity - 1) * spacing / 2f;
        float gridHeight = -(grid.height - 1) * grid.spacing;

        Vector3 bottomCenter = new(centerWidth, 0, gridHeight - 2);
        transform.position = bottomCenter;
    }
}

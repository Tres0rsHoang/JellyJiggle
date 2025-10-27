using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class JellyColor : MonoBehaviour
{
    [SerializeField]
    private Color color;
    private new MeshRenderer renderer;

    public void SetColor(Color newColor)
    {
        color = newColor;
    }

    public Color GetColor()
    {
        return color;
    }

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.material.color = color;
    }

    void Update()
    {
        renderer.material.color = color;
    }
}

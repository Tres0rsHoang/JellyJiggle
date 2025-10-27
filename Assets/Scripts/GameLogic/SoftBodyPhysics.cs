using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SoftBodyPhysics : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float Intensity = 1f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float Mass = 1f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float Damping = 0.75f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float Stiffness = 1f;

    private Mesh OriginalMesh,
        MeshClone;
    private new MeshRenderer renderer;
    private JellyVertex[] jv;
    private Vector3[] vertexArray;

    void Start()
    {
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;
        renderer = GetComponent<MeshRenderer>();

        jv = new JellyVertex[MeshClone.vertices.Length];

        for (int i = 0; i < MeshClone.vertices.Length; i++)
        {
            jv[i] = new JellyVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }
    }

    void FixedUpdate()
    {
        vertexArray = OriginalMesh.vertices;
        for (int i = 0; i < jv.Length; i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jv[i].ID]);
            float intensity =
                (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * Intensity;
            jv[i].Shake(target, Mass, Stiffness, Damping);
            target = transform.InverseTransformPoint(jv[i].Position);
            vertexArray[jv[i].ID] = Vector3.Lerp(vertexArray[jv[i].ID], target, intensity);
        }
        MeshClone.vertices = vertexArray;
    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity,
            force;

        public JellyVertex(int id, Vector3 pos)
        {
            ID = id;
            Position = pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)
        {
            force = (target - Position) * s;
            velocity = (velocity + force / m) * d;
            Position += velocity;

            if ((velocity + force + force / m).magnitude < 0.001f)
            {
                Position = target;
            }
        }
    }
}

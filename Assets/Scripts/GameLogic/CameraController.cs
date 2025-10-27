using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float margin = 1f;
    public float height = 15f;

    [Range(0f, 1f)]
    public float boardVerticalOffset = 0.1f;

    [Header("Tilt Settings")]
    [Range(45f, 80f)]
    public float tiltAngle = 75f;

    void LateUpdate()
    {
        GridBoard grid = GridBoard.Instance;
        if (grid == null)
            return;

        Camera cam = GetComponent<Camera>();
        if (cam == null || !cam.orthographic)
            return;

        float gridWidth = grid.width * grid.spacing;
        float gridHeight = grid.height * grid.spacing;

        float aspect = cam.aspect;
        float sizeByWidth = gridWidth / aspect / 2f;
        float sizeByHeight = gridHeight / 2f;
        float targetSize = Mathf.Max(sizeByWidth, sizeByHeight) + margin;

        cam.orthographicSize = targetSize;

        Vector3 center = new(
            (grid.width - 1) * grid.spacing / 2f,
            0,
            -(grid.height - 1) * grid.spacing / 2f
        );

        float verticalShift = (boardVerticalOffset - 0.5f) * 2f * targetSize;

        Quaternion tiltRotation = Quaternion.Euler(tiltAngle, 0f, 0f);

        Vector3 cameraOffset =
            Quaternion.Euler(tiltAngle, 0f, 0f) * new Vector3(0, 0, -targetSize * 2f);
        Vector3 cameraPos = center + cameraOffset + new Vector3(0, height, verticalShift);

        transform.SetPositionAndRotation(cameraPos, tiltRotation);
    }
}

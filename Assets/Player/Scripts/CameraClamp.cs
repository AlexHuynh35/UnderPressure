using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraClamp : MonoBehaviour
{
    public Transform target;
    public Tilemap tilemap;
    public int addedBounds;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    private Vector3 velocity = Vector3.zero;
    private float camHalfHeight;
    private float camHalfWidth;

    void Start()
    {
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight;

        BoundsInt bounds = tilemap.cellBounds;
        minBounds = tilemap.CellToWorld(bounds.min);
        maxBounds = tilemap.CellToWorld(bounds.max);
    }

    void LateUpdate()
    {
        Vector3 targetPos = target.position;

        float clampedX = Mathf.Clamp(targetPos.x, minBounds.x - addedBounds + camHalfWidth, maxBounds.x + addedBounds - camHalfWidth);
        float clampedY = Mathf.Clamp(targetPos.y, minBounds.y - addedBounds + camHalfHeight, maxBounds.y + addedBounds - camHalfHeight);
        Vector3 desiredPosition = new Vector3(clampedX, clampedY, -999);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.2f);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followTarget;
    public Vector3 cameraOffset;
    public Vector2 center;
    public Vector2 size;

    float height;

    // Start is called before the first frame update
    void Start()
    {
        height = Camera.main.orthographicSize;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = followTarget.transform.position + cameraOffset;
        float ly = size.y * 0.5f - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(transform.position.x, clampY, cameraOffset.z);
    }
}

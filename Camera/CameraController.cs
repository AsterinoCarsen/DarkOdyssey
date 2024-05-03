using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;

    public float smoothSpeed;
    public float yPosLimit;
    public Vector3 offset;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 desPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desPos, smoothSpeed);
        float smoothY = Mathf.Lerp(transform.position.y, desPos.y, smoothSpeed / 2.5f);

        transform.position = new Vector3(smoothPos.x, Mathf.Clamp(smoothY, yPosLimit, Mathf.Infinity), smoothPos.z);
    }
}

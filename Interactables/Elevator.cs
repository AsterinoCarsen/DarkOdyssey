using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float elevationDuration;
    public float elevationSpeed;

    private bool moveDir;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void MovePlatform()
    {
        moveDir = !moveDir;
        StartCoroutine(movePlatFormBridge());
    }

    private IEnumerator movePlatFormBridge()
    {
        if (moveDir == true)
        {
            rb.velocity = new Vector2(0, elevationSpeed);
        } else if (moveDir == false)
        {
            rb.velocity = new Vector2(0, -elevationSpeed);
        }
        yield return new WaitForSeconds(elevationDuration);
        rb.velocity = new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            MovePlatform();
        }
    }
}

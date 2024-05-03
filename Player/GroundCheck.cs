using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Tooltip("Defines whether or not you want specific game objects to reset jumps.")]
    public bool useGroundTag;
    [Tooltip("Name of the ground tag, only works if use ground tag is enabled.")]
    public string groundTagName;

    PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useGroundTag == false)
        {
            playerController.SetGround(true);
        } else if (useGroundTag == true)
        {
            if (collision.tag.Equals(groundTagName))
            {
                playerController.SetGround(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (useGroundTag == false)
        {
            playerController.SetGround(false);
        }
        else if (useGroundTag == true)
        {
            if (collision.tag.Equals(groundTagName))
            {
                playerController.SetGround(false);
            }
        }
    }
}

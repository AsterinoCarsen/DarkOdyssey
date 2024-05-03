using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarrier : MonoBehaviour
{
    private float enemiesLeft;

    void Update()
    {
        enemiesLeft = GameObject.FindGameObjectsWithTag("Guard").Length + GameObject.FindGameObjectsWithTag("Dog").Length + GameObject.FindGameObjectsWithTag("Electric").Length + GameObject.FindGameObjectsWithTag("Mage").Length + GameObject.FindGameObjectsWithTag("Rock").Length;
        if (enemiesLeft == 0)
        {
            Destroy(gameObject);
        }
    }
}

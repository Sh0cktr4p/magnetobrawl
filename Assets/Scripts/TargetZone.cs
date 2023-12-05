using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public Team Owner;

    private bool scored = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Payload"))
        {
            // Team owner bekommt 1 Punkt vong Punkten her.
            if(!scored) Director.Score(this.Owner);
            scored = !scored;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Payload payload;
    private string magnet;
    public Vector3 resetPos;

    void Start()
    {
        
        magnet = "M" + (int) Which;
    }

    public enum PlayerNum
    {
        P0 = 0,
        P1,
        P2,
        P3
    }

    public PlayerNum Which;
    public bool Stunned { get; set; }

    public Team Team
    {
        get
        {
            if (Which == PlayerNum.P0 || Which == PlayerNum.P1)
                return Team.Left;
            else
                return Team.Right;
        }
    }

    private void Update()
    {
        //if (Which == PlayerNum.P0)
            //GameObject.FindObjectOfType<Payload>().FuckingMagnets(transform.position);

        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Ded.");
    }
}
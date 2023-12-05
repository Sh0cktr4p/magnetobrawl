using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payload : MonoBehaviour
{
    public float StartSpeed, MaxSpeed, Accel;
    public float TurnStepMin, TurnStepMax;

    private float _speed;
    public float Radius; // Attraction falloff radius

    private void Start()
    {
        _speed = StartSpeed;
    }

    private void Update()
    {
        _speed += (MaxSpeed - StartSpeed) * Time.deltaTime * Accel;
        _speed = Mathf.Min(_speed, MaxSpeed); // Clamp
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    /// <summary>
    /// How do THEY work?????
    /// </summary>
    public void FuckingMagnets(Vector2 magnetPos, bool attract)
    {
        var factor = attract ? 1 : -1;
        var dir = new Vector3(magnetPos.x, magnetPos.y, transform.position.z) - transform.position;
        var distNorm = dir.magnitude / Radius;
        var turnStep = factor * Mathf.Lerp(TurnStepMin * Time.deltaTime, TurnStepMax * Time.deltaTime,
                           1 - distNorm * distNorm);
        dir.Normalize();
        if (Math.Abs(Vector3.Dot(dir, transform.up.normalized) - 1) < 0.001f) return;
        var rotatedDir = Vector3.RotateTowards(transform.up, dir, turnStep, 0);
        rotatedDir.z = 0;
        transform.up = rotatedDir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            Debug.Log("MEXICANS!!!");
            transform.Rotate(Vector3.forward, 180);
        }
    }
}
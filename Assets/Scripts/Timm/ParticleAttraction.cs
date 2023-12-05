using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttraction : MonoBehaviour {

	ParticleSystem pSystem;
	public float accForce;
	public float maxRadius;

	void Start () {
		pSystem = GetComponent<ParticleSystem>();		
	}
	
	// Update is called once per frame
	void Update () {
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[pSystem.particleCount];
		pSystem.GetParticles(particles);

		for (int p = 0; p < particles.Length; p++) {
			ParticleSystem.Particle part = particles[p];

			part.velocity += -part.position.normalized / part.position.magnitude * Time.deltaTime * accForce;

			if (Vector2.Dot(part.velocity, part.position + part.velocity * Time.deltaTime) > 0 && accForce > 0) {
				part.remainingLifetime = 0;
			}
			if (part.position.magnitude > maxRadius && accForce < 0) {
				part.remainingLifetime = 0;
			}

			particles[p] = part;
		}

		pSystem.SetParticles(particles, particles.Length);
	}
}

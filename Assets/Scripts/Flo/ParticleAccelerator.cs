using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ParticleAccelerator : MonoBehaviour {

    private ParticleSystem pSystem;
    public float accForce;
    public float maxDistance;

	// Use this for initialization
	void Start () {
        pSystem = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[pSystem.particleCount];
        pSystem.GetParticles(particles);

		if (pSystem.emission.enabled) {
			if (Random.Range(0,4f) < 1) {
				if (Ball.balls.Count > 1) {
					Ball b = Ball.balls[0];
					if ((Ball.balls[1].transform.position - transform.position).magnitude < (Ball.balls[0].transform.position - transform.position).magnitude) {
						b = Ball.balls[1];
					}
					ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
					emitParams.position = b.transform.position;

					pSystem.Emit(emitParams,1);
				}
			}
		}

        for(int i = 0; i <particles.Length; i++) {
            ParticleSystem.Particle part = particles[i];

            part.velocity = part.velocity + part.velocity.normalized * accForce * Time.deltaTime;
            if(part.position.magnitude> maxDistance) {
                part.remainingLifetime = 0;
            }
            particles[i] = part;
        }
        pSystem.SetParticles(particles,particles.Length);
	}
}

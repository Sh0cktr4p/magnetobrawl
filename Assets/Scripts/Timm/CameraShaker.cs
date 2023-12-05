using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {

	public static CameraShaker active;

	float currentShakeAmount;
	public float maxShakeAmount = 3;
	public float shakeStrength;
	public float decay;
	public Vector3 pos;

	void Start () {
		active = this;
		pos = transform.position;
	}
	
	void Update () {

		currentShakeAmount *= 1-decay * Time.deltaTime;
		transform.position = pos + (Vector3)Random.insideUnitCircle * currentShakeAmount * shakeStrength;

		if (Input.GetKeyDown(KeyCode.P)) {
			AddShake(9001);
		}

	}

	public static void AddShake(float strength) {
		active.currentShakeAmount = Mathf.Min(active.currentShakeAmount + strength, active.maxShakeAmount);
	}

}

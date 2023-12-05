using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeReducer : MonoBehaviour {

    public float reductionTime;
    public float reductionLimit = 0.1f;

	public bool growing;

	public AnimationCurve alpha;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

    // Update is called once per frame
    void Update() {

        Vector3 locScale = transform.localScale;
        float dt = Time.deltaTime;


		if (!growing) {
			transform.localScale = new Vector3(locScale.x - dt / reductionTime, locScale.y - dt / reductionTime, locScale.z - dt / reductionTime);
			if (transform.localScale.x < reductionLimit) {
				transform.localScale = Vector3.one;
				transform.Rotate(Vector3.forward, Random.Range(-400f, 400f));
			}

			Color c = spriteRenderer.color;
			c.a = alpha.Evaluate(transform.localScale.x);
			spriteRenderer.color = c;
		} else {
			transform.localScale = new Vector3(locScale.x + dt / reductionTime, locScale.y + dt / reductionTime, locScale.z + dt / reductionTime);
			if (transform.localScale.x > reductionLimit) {
				transform.localScale = Vector3.zero;
				transform.Rotate(Vector3.forward, Random.Range(-400f, 400f));
			}

			Color c = spriteRenderer.color;
			c.a = alpha.Evaluate(transform.localScale.x);
			spriteRenderer.color = c;
		}
    }
}

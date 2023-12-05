using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallState
{
    idle,
    docked,
    shot
};

public class Ball : MonoBehaviour {
    private BallState state;
    private Rigidbody2D rigid;
    private CircleCollider2D coll;

    public static List<Ball> balls;

    //Audio
    private AudioSource audioSrc;

    public AudioClip toWall;

    //Affinity
    public int team;

    [SerializeField]
    private float borderVelocity;
    
    public BallState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        if(balls == null)
        {
            balls = new List<Ball>();
        }
        balls.Add(this);

        audioSrc = GetComponent<AudioSource>();
	}

    public void Dock()
    {
        rigid.velocity = Vector2.zero;
        rigid.simulated = false;
        coll.enabled = false;
        state = BallState.docked;
    }

    public void Shoot(Vector2 dir)
    {
        transform.parent = null;
        rigid.simulated = true;
        coll.enabled = true;
        rigid.AddForce(dir, ForceMode2D.Impulse);
        state = BallState.shot;
    }
	
    public void Attract(Vector2 dir)
    {
        rigid.AddForce(dir, ForceMode2D.Force);
    }

	// Update is called once per frame
	void Update () {
		if(state == BallState.shot)
		{
		    GetComponent<SpriteRenderer>().color = Color.red * rigid.velocity.magnitude / 10;
            foreach (var playerMovement in FindObjectsOfType<PlayerMovement>())
            {
                Vector2 dir = playerMovement.transform.position - transform.position;
                if (Math.Abs(Vector2.Dot(dir.normalized, rigid.velocity.normalized) - 1) < 0.1f)
                    rigid.AddForce(Time.deltaTime * dir / dir.sqrMagnitude, ForceMode2D.Impulse);
            }
            if (rigid.velocity.magnitude < borderVelocity)
                state = BallState.idle;
        }
		else
		{
		    GetComponent<SpriteRenderer>().color = Color.white;
		}
        if (state == BallState.docked)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, Vector2.zero, 15 * Time.deltaTime);
        }
	}
    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.transform.CompareTag("Border") || collision.transform.CompareTag("Payload")) {
            audioSrc.PlayOneShot(toWall);
            if(state == BallState.shot)
            {
                CameraShaker.AddShake(0.1f);
            }
        }
        
    }

    void OnDestroy()
    {
        balls.Remove(this);    
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    moving,
    stunned,
    attracting
}

public class PlayerMovement : MonoBehaviour
{
    //Input strings
    private string horizontal;

    private string vertical;
    private string rHorizontal;
    private string rVertical;
    private string triggers;
    private string magnet;

    private int playerID;
    private bool ballDocked;
    private PlayerState state;
    private Coroutine alreadyStunned;
    private Rigidbody2D rigid;
    private CircleCollider2D coll;

    private Ball dockedBall;

    //Effects
    public GameObject attractingBallFX;

    public GameObject repellingBallFX;
    public GameObject attractingObjectiveFX;
    public GameObject repellingObjectiveFX;

    private Payload payload;

    public float repellingDuration = 1;
    public float Cooldown, CooldownStop;
    private float _cooldownTimer = 0;
    private UnityEngine.UI.Image _cooldownImage;

    //Audio
    private AudioSource audioSrc;

    public AudioClip toBall;
    public AudioClip getBall;
    public AudioClip magnetAttract;
    public AudioClip magnetRepell;

    public float magnetDuration = 1;
    private bool _attract;

    //Spawn Parameter
    public Vector3 spawnPoint;

    public float spawnDuration;
    public float spawnInterval;

    //General
    public int playersPerTeam = 2;

    private bool keyboardControl = false;

    public int PlayerID
    {
        get { return playerID; }
        set { playerID = value; }
    }

    public Vector2 FireDirection
    {
        get
        {
            Vector2 temp = (childTransform.position - transform.position).normalized;
            return temp; // Vector2.ClampMagnitude(temp + rigid.velocity, maxShootSpeed);
        }
    }

    [SerializeField] private int speed;

    [SerializeField] private float maxAttractDistance;

    [SerializeField] private float snapDistance;

    [SerializeField] private int attractFactor;

    [SerializeField] private int shootSpeed;

    [SerializeField] private float maxShootSpeed;

    [SerializeField] private float stunTime;

    [SerializeField] private Transform childTransform;

    // Use this for initialization
    void Start()
    {

		//controllerID = playerID;
        //Initialize axis
        horizontal = "H" + playerID;
        vertical = "V" + playerID;
        rHorizontal = "HR" + playerID;
        rVertical = "VR" + playerID;
        triggers = "T" + playerID;
        magnet = "M" + playerID;

        if (Input.GetJoystickNames().Length == 0) {
            keyboardControl = true;
        }

        state = PlayerState.moving;
        ballDocked = false;
        dockedBall = null;
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        _cooldownImage = GameObject.Find("Cooldown P" + playerID).GetComponent<UnityEngine.UI.Image>();
        payload = FindObjectOfType<Payload>();

        audioSrc = GetComponent<AudioSource>();
    }

    public void Stun(float factor)
    {
        if (alreadyStunned != null) StopCoroutine(alreadyStunned);
        state = PlayerState.stunned;
        alreadyStunned = StartCoroutine(StunPlayer(factor));
    }

    IEnumerator StunPlayer(float factor)
    {
        if (ballDocked) dockedBall.Shoot(FireDirection * shootSpeed / 10);
        ballDocked = false;
        dockedBall = null;
        childTransform.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        const float blinkDuration = 0.2f;
        for (float i = 0; i < stunTime * factor; i += blinkDuration)
        {
            GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(blinkDuration / 2);
            GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(blinkDuration / 2);
        }
        rigid.sharedMaterial.bounciness = 0;
        state = PlayerState.moving;
    }

    private void AttractPayload()
    {
        _attract = true;
        payload.FuckingMagnets(transform.position, true);
        repellingObjectiveFX.SetActive(false);
        attractingObjectiveFX.SetActive(true);
        if (audioSrc == null || audioSrc.clip != magnetAttract)
        {
            audioSrc.clip = magnetAttract;
            audioSrc.loop = true;
            audioSrc.Play();
            StartCoroutine(deactivateMagnet());
        }
    }

    private void RepellPayload()
    {
        _attract = false;
        payload.FuckingMagnets(transform.position, false);
        repellingObjectiveFX.SetActive(true);
        attractingObjectiveFX.SetActive(false);
        if (audioSrc == null || audioSrc.clip != magnetRepell)
        {
            audioSrc.clip = magnetRepell;
            audioSrc.loop = true;
            audioSrc.Play();
            StartCoroutine(deactivateMagnet());
        }
    }

    IEnumerator deactivateMagnet()
    {
        yield return new WaitForSeconds(magnetDuration);
        repellingObjectiveFX.SetActive(false);
        attractingObjectiveFX.SetActive(false);
        if (audioSrc.clip == magnetAttract || audioSrc.clip == magnetRepell)
        {
            audioSrc.clip = null;
            audioSrc.loop = false;
            audioSrc.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldownTimer > 0)
        {
            if (_cooldownTimer > Cooldown)
            {
                if (_attract) AttractPayload();
                else RepellPayload();
            }
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer > Cooldown)
                _cooldownImage.fillAmount = (_cooldownTimer - Cooldown) / CooldownStop;
            else
                _cooldownImage.fillAmount = 1 - _cooldownTimer / Cooldown;
            if (_cooldownTimer > Cooldown) return;
        }
        else
        {
            _cooldownImage.fillAmount = 1;
            //Check for input on magnet axis
            if (!ballDocked && Math.Abs(Input.GetAxisRaw(magnet)) > 0.001f)
            {
                rigid.velocity = Vector2.zero;
                state = PlayerState.attracting;
                if (Input.GetAxisRaw(magnet) > 0) AttractPayload();
                else RepellPayload();
                _cooldownTimer = Cooldown + CooldownStop;
                return;
            }
        }
        //Do nothing if stunned
        if (state == PlayerState.stunned) return;

        //Reset state
        repellingObjectiveFX.SetActive(false);
        attractingObjectiveFX.SetActive(false);
        state = PlayerState.moving;

        //Get input
        float hInput = Input.GetAxis(horizontal);
        hInput *= Mathf.Abs(hInput);
        float vInput = -Input.GetAxis(vertical);
        vInput *= Mathf.Abs(vInput);
        float hRInput = Input.GetAxis(rHorizontal);
        float vRInput = -Input.GetAxis(rVertical);
        float tInput = Input.GetAxis(triggers);

        Vector3 direction = new Vector2(hInput, vInput) * speed;
        rigid.velocity =
            Vector2.ClampMagnitude(
                Vector2.Lerp(rigid.velocity, rigid.velocity + (Vector2) direction, 5f * Time.deltaTime),
                speed); // * Time.deltaTime;

        //Set rotation
        if (keyboardControl && playerID == 0) {
            Vector3 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            Vector2 dir2 = new Vector2(dir.x, dir.y).normalized;
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.AngleAxis(
                    Mathf.Atan2(dir2.y, dir2.x) * 180 / Mathf.PI,
                    Vector3.forward
                ),
                Time.deltaTime * 100
            );

        }
        else {
            if (hRInput != 0 || vRInput != 0)
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.AngleAxis(Mathf.Atan2(vRInput, hRInput) * 180 / Mathf.PI, Vector3.forward),
                    Time.deltaTime * 10);
        }
        //Set ball attraction
        if (tInput > 0 && !ballDocked)
        {
            //Ball attraction
            
            foreach (Ball b in Ball.balls)
            {
                if ((b.transform.position - transform.position).magnitude < maxAttractDistance)
                {
                    Vector2 temp = (transform.position - b.transform.position);
                    Vector2 dir = temp.normalized;
                    float amt = tInput * attractFactor / (temp.sqrMagnitude);
                    b.Attract(dir * amt);

                }
            }
            attractingBallFX.SetActive(true);
        }
        else
        {
            attractingBallFX.SetActive(false);
        }
        if (!ballDocked)
        {
            //Ball docking
            foreach (Ball b in Ball.balls)
            {
                if ((b.transform.position - transform.position).magnitude < snapDistance && b.State == BallState.idle)
                {
                    ballDocked = true;
                    b.transform.parent = childTransform;
                    dockedBall = b;
                    b.Dock();
                    childTransform.gameObject.GetComponent<CircleCollider2D>().enabled = true;

                    if (audioSrc.clip == null || audioSrc.clip != getBall) {
                        //audioSrc.clip = getBall;
                        audioSrc.PlayOneShot(getBall);
                        //Debug.Log("")
                    }

                    break;
                }
            }
        }
        else if ((keyboardControl && playerID == 0 && Input.GetMouseButton(0) || tInput < 0) && ballDocked)
        {
            //Ball shooting; 
            dockedBall.team = playerID / playersPerTeam;
            dockedBall.Shoot(FireDirection * shootSpeed);
            ballDocked = false;
            dockedBall = null;
            childTransform.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            repellingBallFX.SetActive(true);
            StartCoroutine(deactivateFX(repellingBallFX, repellingDuration));
        }
    }

    IEnumerator deactivateFX(GameObject obj, float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return 0;
        }
        obj.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Debug.Log("Hit Ball");
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball.State == BallState.shot && ball.team != playerID / playersPerTeam)
            {
                Rigidbody2D ballRigid = collision.gameObject.GetComponent<Rigidbody2D>();
                CameraShaker.AddShake(1.5f * ballRigid.velocity.magnitude / shootSpeed);
                Stun(ballRigid.velocity.magnitude / shootSpeed);
                rigid.sharedMaterial.bounciness = 1.0f;
                rigid.AddForce(ballRigid.velocity, ForceMode2D.Impulse);
                audioSrc.clip = toBall;
                audioSrc.Play();
                Debug.Log("Playing Hitsound");
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Payload")
        {
            Debug.Log("Hit Payload");
            CameraShaker.AddShake(1);
            StartCoroutine(Respawn());
        }
    }


    IEnumerator Respawn() {
        //transform.GetComponent<TrailRenderer>().enabled = false;
        state = PlayerState.stunned;
        repellingBallFX.SetActive(false);
        attractingBallFX.SetActive(false);
        attractingObjectiveFX.SetActive(false);
        repellingObjectiveFX.SetActive(false);

        audioSrc.clip = toBall;
        audioSrc.Play();


        ballDocked = false;
        childTransform.gameObject.GetComponent<CircleCollider2D>().enabled = false;
		if (dockedBall != null) {
			dockedBall.State = BallState.idle;
			dockedBall.transform.parent = null;
			dockedBall.Shoot(Vector2.zero);
			ballDocked = false;
			dockedBall = null;
			childTransform.gameObject.GetComponent<CircleCollider2D>().enabled = false;
		}
        rigid.velocity = Vector2.zero;

        transform.position = spawnPoint;
        Renderer rend = GetComponent<Renderer>();
        rend.enabled = false;

        float timerDuration = 0;
        float timerBlink = 0;
        while (timerDuration < spawnDuration)
        {
            timerDuration += Time.deltaTime;
            timerBlink += Time.deltaTime;
            if (timerBlink > spawnInterval)
            {
                rend.enabled = !rend.enabled;
                timerBlink -= spawnInterval;
            }
            yield return null;
        }
        rend.enabled = true;
        //transform.GetComponent<TrailRenderer>().enabled = true;
        state = PlayerState.moving;
    }
}

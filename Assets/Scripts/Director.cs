using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Team
{
    Left,
    Right
}

public class Director : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject BallPrefab;
    public Text ScoreDisplay;
    public Text TimeDisplay;
    public int PlayTime = 5;

    public GameObject winscreen;
    public GameObject teamy;
    public GameObject teamb;
    public GameObject teamd;
    public Text score;

    public Color[] teamColors;

    public Transform spawnLeft;
    public Transform spawnRight;

    private GameObject[] players;
    private GameObject[] balls;


    private static int[] _score = new int[2];

    public static Director Instance
    {
        get { return GameObject.FindObjectOfType<Director>(); }
    }

    public static string ScoreString
    {
        get {
            return string.Format("<color=orange>{0}</color> - <color=blue>{1}</color>", _score[1], _score[0]);
        }
    }

    public static void Score(Team team)
    {
        Debug.Log("Team Scored: " + team);
        _score[(int) team]++;

        CameraShaker.AddShake(3.0f);
        Instance.ScoreDisplay.text = ScoreString;
        Instance.ScoreDisplay.GetComponent<Animator>().Play("ScoreDisplay");

        FindObjectOfType<Payload>().transform.SetPositionAndRotation(new Vector3(0, -0.8f, 0), Quaternion.identity);
    }

    public static void ResetGame()
    {
        for (var i = 0; i < _score.Length; i++)
            _score[i] = 0;
    }

    private void Start()
    {
        StartGame();
        StartCoroutine(GameTimer());
        TimeDisplay.text = string.Format("<color=cyan>{0}:{1:D2}</color>", PlayTime, 0);
        Instance.ScoreDisplay.supportRichText = true;
    }

    private IEnumerator GameTimer()
    {
        var seconds = 60 * PlayTime;
        
        while (seconds-- > 0)
        {
            yield return new WaitForSeconds(1);
            TimeDisplay.text = string.Format("<color=cyan>{0}:{1:D2}</color>", seconds / 60, seconds % 60);
        }
        winscreen.SetActive(true);
        score.text = "" + _score[0] + " : " + _score[1];
        if (_score[0] > _score[1])
        {
            teamy.SetActive(true);
            teamb.SetActive(false);
            teamd.SetActive(false);
        }
        else if (_score[0] < _score[1])
        {
            teamy.SetActive(false);
            teamb.SetActive(true);
            teamd.SetActive(false);
        }
        else
        {
            teamy.SetActive(false);
            teamb.SetActive(false);
            teamd.SetActive(true);
        }
    }

    private void StartGame()
    {
        players = new GameObject[4];
        players[0] = SpawnPlayer(new Vector3(-2, 1, 0), Player.PlayerNum.P0, spawnLeft.position);
        players[1] = SpawnPlayer(new Vector3(-2, -1, 0), Player.PlayerNum.P1, spawnLeft.position);
        players[2] = SpawnPlayer(new Vector3(2, 1, 0), Player.PlayerNum.P2, spawnRight.position);
        players[3] = SpawnPlayer(new Vector3(2, -1, 0), Player.PlayerNum.P3, spawnRight.position);
        balls = new GameObject[2];
        balls[0] = SpawnBall(new Vector3(-1, 0, 0));
        balls[1] = SpawnBall(new Vector3(1, 0, 0));
    }

    private GameObject SpawnPlayer(Vector3 position, Player.PlayerNum playerNum, Vector3 spawnPoint)
    {
        var go = Instantiate(PlayerPrefab, position, Quaternion.identity);
        go.GetComponent<Player>().Which = playerNum;
        go.GetComponent<PlayerMovement>().PlayerID = (int) playerNum;
        go.GetComponent<PlayerMovement>().spawnPoint = spawnPoint;
        return go;
    }

    private GameObject SpawnBall(Vector3 position)
    {
        return Instantiate(BallPrefab, position, Quaternion.identity);
    }
}
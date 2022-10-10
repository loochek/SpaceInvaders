using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        PlayerAlive,
        PlayerDead,
        GameOver,
        GameWin
    };

    private int LivesCount
    {
        get => livesCount;
        set
        {
            livesCount = value;
            UpdateUIText();
        }
    }
    
    public Vector3 respawnPos = new Vector3(0.0f, -4.0f);
    public float respawnDelay = 2.0f;
    public GameObject player;
    public int oneEnemyScoreAward = 100;
    public int winScoreAward = 10000;
    public GameObject scoreLabel;
    public GameObject livesLabel;
    public GameObject notificationLabel;
    public GameObject gridController;
    public float gameFinishDelay = 2.0f;

    public GameState CurrGameState { get; private set; } = GameState.PlayerDead;


    private int CurrScore
    {
        get => _currScore;
        set
        {
            _currScore = value;
            UpdateUIText();
        }
    }

    private TMP_Text _scoreLabelComponent;
    private TMP_Text _livesLabelComponent;
    private TMP_Text _notificationLabelComponent;

    private GameObject _player;

    [SerializeField]
    private int livesCount = 4;
    
    private int _currScore = 0;
    private float _respawnTime;
    private float _gameFinishTime;
    private bool _gameplayEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        _scoreLabelComponent = scoreLabel.GetComponent<TMP_Text>();
        _livesLabelComponent = livesLabel.GetComponent<TMP_Text>();
        _notificationLabelComponent = notificationLabel.GetComponent<TMP_Text>();
        
        // Initial
        UpdateUIText();
        
        _respawnTime = Time.time + respawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrGameState == GameState.PlayerDead && Time.time > _respawnTime)
        {
            if (_player is not null)
            {
                Destroy(_player);
            }
            
            _player = Instantiate(player, respawnPos, Quaternion.identity);
            LivesCount--;
            CurrGameState = GameState.PlayerAlive;
            Debug.Log($"Lives left: {LivesCount}");
        }

        if (_gameplayEnded && Time.time > _gameFinishTime)
        {
            FinishGame();
        }
    }

    public void OnPlayerDeath()
    {
        if (LivesCount > 0) {
            CurrGameState = GameState.PlayerDead;
            _respawnTime = Time.time + respawnDelay;
        }
        else
        {
            CurrGameState = GameState.GameOver;
            Debug.Log("Game over");
            ScheduleGameFinish();
        }
    }

    public void OnEnemyDeath()
    {
        CurrScore += oneEnemyScoreAward;
    }

    public void OnAllEnemiesDeath()
    {
        if (CurrGameState != GameState.GameOver)
        {
            CurrGameState = GameState.GameWin;
            CurrScore += winScoreAward;
            Debug.Log("Win!");
            ScheduleGameFinish();
        }
    }

    private void UpdateUIText()
    {
        _scoreLabelComponent.SetText($"Score: {CurrScore}");
        _livesLabelComponent.SetText($"Lives: {LivesCount}");
    }

    private void ScheduleGameFinish()
    {
        _gameplayEnded = true;
        _gameFinishTime = Time.time + gameFinishDelay;
    }

    private void FinishGame()
    {
        _player.SetActive(false);
        gridController.SetActive(false);
        
        switch (CurrGameState)
        {
            case GameState.GameOver:
                _notificationLabelComponent.SetText("Game Over");
                break;
            
            case GameState.GameWin:
                _notificationLabelComponent.SetText("Win");
                break;
            
            default:
                Debug.LogError("Non-finishing game state in FinishGame");
                break;
        }

        notificationLabel.SetActive(true);
    }
}
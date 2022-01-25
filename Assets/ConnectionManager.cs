using System;
using System.Collections.Generic;
using System.IO;
using BestHTTP;
using UnityEngine;
using BestHTTP.WebSocket;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.Rendering;
using WebGLInput = WebGLSupport.WebGLInput;

public class Item
{
    [JsonProperty("game_state")] public string gameState { get; set; }
    [JsonProperty("game_data")] public GameData gameData { get; set; }
    public DateTime timestamp { get; set; }
}

public class GameData
{
    public List<string> categories { get; set; }
    public Dictionary<string, List<string>> candidates { get; set; }
    public Dictionary<string, PlayerOverallResult> results { get; set; }
    public string letter { get; set; }
}

public class PlayerOverallResult
{
    public int score { get; set; }
    public List<PlayerSingleResult> results { get; set; }
}

public class PlayerSingleResult
{
    public string category_name { get; set; }
    public int score { get; set; }
    public string word { get; set; }
}

public class Config
{
    // do not change variables names
    public string player_id;
    public string room_id;
    public string server_address;
    public string player_nick;
    public string watchdog_address;
}

public class ConnectionManager : MonoBehaviour
{
    private static WebSocket webSocket;
    private bool isMyTurn;
    private Config config;
    private SecondStageControler completing;
    private ResultsManager results;
    private VotingManager voting;

    [SerializeField] GameObject waitingText;
    private GameObject[] disableUIs;
    private Timer timer;
    private Item item;


    private const float connectTimeout = 3;
    private float timeFromLastConnectionRequest = connectTimeout;
    private const float KeepAliveTimeout = 8;
    private float timeFromLastKeepAlive = 0;

    void Start()
    {
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        completing = GameObject.Find("Completing").GetComponent<SecondStageControler>();
        voting = GameObject.Find("Voting").GetComponent<VotingManager>();
        results = GameObject.Find("Results").GetComponent<ResultsManager>();

//        string r = UnityEngine.Random.Range(1, 10000).ToString();
//        Debug.Log("my id is: " + r);
//        config = new Config {player_id = "r", room_id = "1", server_address = "ws://localhost:5000/ws/", player_nick = "player"};
//        config = new Config
//        {
//            player_id = "1", room_id = "1", server_address = "ws://localhost:5000/test/", player_nick = "player"
//        };
        ClearDesk();
        waitingText.SetActive(true);
    }

    private void Update()
    {
        if (timeFromLastKeepAlive >= KeepAliveTimeout)
        {
            KeepAlive();
            timeFromLastKeepAlive = 0;
        }
        else
        {
            timeFromLastKeepAlive += Time.deltaTime;
        }

        if (config == null ||
        (string.IsNullOrEmpty(config.player_id) || webSocket != null) &&
            (string.IsNullOrEmpty(config.player_id) || webSocket.IsOpen)) return;
        if (timeFromLastConnectionRequest >= connectTimeout)
        {
            webSocket = ConnectToServer(config);
            timeFromLastConnectionRequest = 0;
        }
        else
        {
            timeFromLastConnectionRequest += Time.deltaTime;
        }
    }

    private void KeepAlive()
    {
        if (string.IsNullOrEmpty(config.watchdog_address)) return;
        var newUri = config.watchdog_address + "/keep_alive/" + config.player_id;
        Debug.Log(newUri);
        var request = new HTTPRequest(new Uri(newUri), methodType: HTTPMethods.Post);
        request.Send();
    }

    private WebSocket ConnectToServer(Config config)
    {
        Debug.Log("connectiong!");
        string fullAddress = Path.Combine(config.server_address + config.room_id + "/"
                                          + config.player_id + "/" + config.player_nick);

        webSocket = new WebSocket(new Uri(fullAddress));
        webSocket.OnMessage += OnMessageRecieved;
        webSocket.Open();

        return webSocket;
    }

    private void ClearDesk()
    {
        waitingText.SetActive(false);
        completing.ResetStage();
        voting.ResetStage();
        results.ResetStage();
    }

    private void OnMessageRecieved(WebSocket webSocket, string message)
    {
        ClearDesk();
        item = new Item();
        item = JsonConvert.DeserializeObject<Item>(message);
        Debug.Log(message);
        switch (item.gameState)
        {
            case "LOBBY":
                waitingText.SetActive(true);
                break;
            case "COMPLETING":
                completing.SetStage(item.gameData.letter, item.gameData.categories);
                break;
            case "VOTING":
                voting.SetVoting(item.gameData.candidates);
                break;
            case "SCORE_DISPLAY":
                Debug.Log("Results display");
                results.SetResults(item.gameData.results);
                break;
        }
        timer.SetTimer(item.timestamp);
    }

    static void SendUpdateToServer(string dictAsStr)
    {
//        string dictAsStr = JsonConvert.SerializeObject(dictToSend);
        webSocket.Send(dictAsStr);
    }

    public void ConfigFromJson(string json)
    {
        if (config == null)
            config = JsonUtility.FromJson<Config>(json);
    }

    public void SendResults()
    {
        string results = "";
        switch (item.gameState)
        {
            case "LOBBY":
                break;
            case "COMPLETING":
                results = completing.GetFields();
                break;
            case "VOTING":
                results = voting.GetVotingResults();
                break;
            case "SCORE_DISPLAY":
                break;
        }

        Debug.Log(results);
        if (!string.IsNullOrEmpty(results))
            SendUpdateToServer(results);
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using BestHTTP.WebSocket;
using Newtonsoft.Json;
using UnityEngine.Rendering;

public class Item
{
    [JsonProperty("game_state")] public string gameState { get; set; }

    public List<string> categories { get; set; }
    public Dictionary<string, List<string>> candidates { get; set; }

    public Dictionary<string, Dictionary<string, List<Dictionary<string, dynamic>>>> results { get; set; }

//    public Dictionary<string, string> nicks { get; set; }
    public string letter { get; set; }
    public DateTime timestamp { get; set; }
}


public class Config
{
    // do not change variables names names
    public string player_id;
    public string room_id;
    public string server_address;
    public string player_nick;
}

public class ConnectionManager : MonoBehaviour
{
    private static WebSocket webSocket;
    private bool isMyTurn;
    private Config config;
    private SecondStageControler completing;

    private VotingManager voting;
//    private Results results;


    [SerializeField] GameObject waitingText;
    private GameObject[] disableUIs;
    private Timer timer;


    private const float connectTimeout = 3;
    private float timeFromLastConnectionRequest = connectTimeout;

    void Start()
    {
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        completing = GameObject.Find("Completing").GetComponent<SecondStageControler>();
        voting = GameObject.Find("Voting").GetComponent<VotingManager>();
//        results = GameObject.Find("Results").GetComponent<Results>();


        config = new Config
        {
            player_id = "1", room_id = "1", server_address = "ws://localhost:5000/ws/", player_nick = "player"
        }; // todo
        ClearDesk();
    }

    private void Update()
    {
        if ((string.IsNullOrEmpty(config.player_id) || webSocket != null) &&
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
//        completing.deactivate();
//        voting.deactivate();
//        results.deactivate();
    }

    private void OnMessageRecieved(WebSocket webSocket, string message)
    {
        Item item = JsonConvert.DeserializeObject<Item>(message);
        Debug.Log(message);
        ClearDesk();
        switch (item.gameState)
        {
            case "LOBBY":
                waitingText.SetActive(true);
                break;
            case "COMPLETING":
                completing.SetStage(item.letter, item.categories);
                break;
            case "VOTING":
                voting.SetVoting(item.candidates);
                break;
            case "SCORE_DISPLAY":
                Debug.Log("Results display");
//                results.SetResults(item.results);
                break;
        }

//        timer.SetTimer(item.timestamp);
    }

    static void SendUpdateToServer(Dictionary<string, dynamic> dictToSend)
    {
        string dictAsStr = JsonConvert.SerializeObject(dictToSend);
        webSocket.Send(dictAsStr);
    }

    public void ConfigFromJson(string json)
    {
        if (config == null)
            config = JsonUtility.FromJson<Config>(json);
    }
}
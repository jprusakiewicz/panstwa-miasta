using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI text;
    private DateTime deadline;
    private DateTime delta;
    private ConnectionManager connectionManager;
    private bool hasSend = true;

    void Start()
    {
        connectionManager = GameObject.Find("GameController").GetComponent<ConnectionManager>();

        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        var delta = deadline - DateTime.Now;
        if (delta.Seconds < 0 && !hasSend)
        {
            connectionManager.SendResults();
            hasSend = true;
        }

        if (0 < delta.Seconds || delta.Seconds > 50)
        {
            var timeAsString = delta.Seconds.ToString();
            if (text.text != timeAsString)
            {
                text.text = timeAsString;
            }
        }
        else
            text.text = " ";
    }

    public void SetTimer(DateTime timestamp)
    {
        hasSend = false;
        deadline = timestamp;
    }
}
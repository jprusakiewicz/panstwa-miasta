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

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        var delta = deadline - DateTime.Now;
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
        deadline = timestamp;
    }
}
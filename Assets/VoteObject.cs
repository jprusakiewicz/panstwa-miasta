using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoteObject : MonoBehaviour
{
//    [SerializeField] private GameObject word;
    [SerializeField] private GameObject yesMark;
    [SerializeField] private GameObject noMark;
    Button yesButton;
    Button noButton;

    private bool? vote;

    void Start()
    {
        yesButton = yesMark.GetComponent<Button>();
        noButton = noMark.GetComponent<Button>();

        yesButton.onClick.AddListener(SetVoteTrue);
        noButton.onClick.AddListener(SetVoteFalse);

        RefreshVisualPress();
    }

    public void SetText(string word)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = word;
    }

    public Tuple<string, bool?> GetResults()
    {
        return new Tuple<string, bool?>(GetComponentInChildren<TextMeshProUGUI>().text, vote);
    }

    void SetVoteTrue()
    {
        vote = true;
        RefreshVisualPress();
    }

    void SetVoteFalse()
    {
        vote = false;
        RefreshVisualPress();
    }

    void RefreshVisualPress()
    {
        var currentYesColor = yesMark.GetComponent<Image>().color;
        var currentNoColor = noMark.GetComponent<Image>().color;

        if (vote == true)
        {
            noMark.GetComponent<Image>().color = new Color(currentNoColor.r, currentNoColor.g, currentNoColor.b, 0.55f);
            yesMark.GetComponent<Image>().color =
                new Color(currentYesColor.r, currentYesColor.g, currentYesColor.b, 1f);
        }
        else if (vote == false)
        {
            noMark.GetComponent<Image>().color = new Color(currentNoColor.r, currentNoColor.g, currentNoColor.b, 1f);
            yesMark.GetComponent<Image>().color =
                new Color(currentYesColor.r, currentYesColor.g, currentYesColor.b, 0.55f);
        }
        else
        {
            noMark.GetComponent<Image>().color = new Color(currentNoColor.r, currentNoColor.g, currentNoColor.b, 0.55f);
            yesMark.GetComponent<Image>().color =
                new Color(currentYesColor.r, currentYesColor.g, currentYesColor.b, 0.55f);
        }
    }
}
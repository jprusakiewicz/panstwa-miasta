using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsRow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetElements(string playerNick, PlayerOverallResult results)
    {
        List<PlayerSingleResult> playerSingleResult = results.results;

        int idx = 0;
        foreach (Transform child in transform)
        {
            if (idx != 0)
            {
                int score = playerSingleResult[idx - 1].score;
                Color c = new Color();
                switch (score)
                {
                    case 0:
                        c = Color.red;
                        break;
                    case 5:
                        c = Color.black;
                        break;
                    case 10:
                        c = Color.yellow;
                        break;
                    case 15:
                        c = Color.green;
                        break;
                    default:
                        c = Color.black;
                        break;
                }
                child.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text =
                    playerSingleResult[idx - 1].word;
                child.transform.Find("Points").gameObject.GetComponent<TextMeshProUGUI>().text =
                    score.ToString();
                child.transform.Find("Points").gameObject.GetComponent<TextMeshProUGUI>().color =
                    c;
            }
            else
            {
                child.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text = playerNick;
                child.transform.Find("Points").gameObject.GetComponent<TextMeshProUGUI>().text =
                    results.score.ToString();
            }

            idx++;
        }
    }
}
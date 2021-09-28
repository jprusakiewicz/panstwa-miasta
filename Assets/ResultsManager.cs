using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] private GameObject Content;
    [SerializeField] private GameObject HeadersPrefab;
    [SerializeField] private GameObject ResultsRowPrefab;
    GameObject[] UI;

    private List<string> categories;

    private void Start()
    {
        UI = GameObject.FindGameObjectsWithTag("Results_UI");

    }

    public void SetResults(Dictionary<string, PlayerOverallResult> results)
    {
        foreach (var obj in UI)
        {
            obj.SetActive(true);
        }
        SetUpCategoriesList(results);
        SetUpPlayersResults(results);
    }

    private void SetUpCategoriesList(Dictionary<string, PlayerOverallResult> results)
    {
        categories = new List<string>();
        foreach (var v in results.First().Value.results)
        {
            categories.Add(v.category_name);
        }

        GameObject newRow = Instantiate(HeadersPrefab, new Vector3(), gameObject.transform.rotation);
        newRow.transform.SetParent(Content.transform);
        Headers headers = newRow.GetComponentInChildren<Headers>();
        headers.SetHeaders(categories);
    }

    private void SetUpPlayersResults(Dictionary<string, PlayerOverallResult> results)
    {
        foreach (var player in results)
        {
            string playerNick = player.Key;
            PlayerOverallResult playerOverallResult = player.Value;

            GameObject newRow = Instantiate(ResultsRowPrefab, new Vector3(), gameObject.transform.rotation);
            newRow.transform.SetParent(Content.transform);
            ResultsRow resultsRow = newRow.GetComponentInChildren<ResultsRow>();
            resultsRow.SetElements(playerNick, playerOverallResult);
        }
    }

    public void ResetStage()
    {
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var obj in UI)
        {
            obj.SetActive(false);
        }
    }
}
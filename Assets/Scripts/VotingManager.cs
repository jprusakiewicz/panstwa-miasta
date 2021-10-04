using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class VotingManager : MonoBehaviour
{
    [SerializeField] private GameObject CategoryName;
    [SerializeField] private GameObject VotingItem;
    [SerializeField] private GameObject Content;
    GameObject[] UI;
    private List<GameObject> words = new List<GameObject>();
    Dictionary<string, List<VoteObject>> categories = new Dictionary<string, List<VoteObject>>();


    private void Start()
    {
        UI = GameObject.FindGameObjectsWithTag("Voting_UI");
    }


    public void SetVoting(Dictionary<string, List<string>> votingConfig)
    {
        foreach (var obj in UI)
        {
            obj.SetActive(true);
        }
    
        foreach (var category in votingConfig)
        {
            var newCategory = Instantiate(CategoryName, new Vector3(), Quaternion.identity);
            newCategory.GetComponent<TextMeshProUGUI>().text = category.Key;
            newCategory.transform.SetParent(Content.transform);

            var voteObjects = new List<VoteObject>();
            foreach (var word in category.Value)
            {
                var newWord = Instantiate(VotingItem, new Vector3(), gameObject.transform.rotation);
                newWord.transform.SetParent(Content.transform);
                words.Add(newWord);
                VoteObject voteObject = newWord.GetComponentInChildren<VoteObject>();
                voteObject.SetText(word);

                voteObjects.Add(voteObject);
            }

            categories.Add(category.Key, voteObjects);
        }
    }

    public void ResetStage()
    {
        foreach (var obj in UI)
        {
            obj.SetActive(false);
        }
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }
        words = new List<GameObject>();
        categories = new Dictionary<string, List<VoteObject>>();
    }

    public string GetVotingResults()
    {
        var results = new Dictionary<string, Dictionary<string, bool?>>();
        foreach (var category in categories)
        {
            var votingResults = new Dictionary<string, bool?>();
            foreach (var voting in category.Value)
            {
                var result = voting.GetResults();
                votingResults.Add(result.Item1, result.Item2);
            }

            results.Add(category.Key, votingResults);
        }

        return JsonConvert.SerializeObject(results);
    }
}
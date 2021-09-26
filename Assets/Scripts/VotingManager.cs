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
    [SerializeField] private bool juzCzas;
    private Dictionary<string, List<VoteObject>> a;

    void Start()
    {
        var c = new List<string>()
            {"jeden", "dwa", "trzy", "cztary"};
        var t = new Dictionary<string, List<string>>() {{"Państwo", c}, {"Miasto", c}};
        a = SetVoting(t);
    }

    private void Update()
    {
        if (juzCzas == true)
        {
            juzCzas = false;
            GetVotingResults(a);
        }
    }

    public Dictionary<string, List<VoteObject>> SetVoting(Dictionary<string, List<string>> votingConfig)
    {
        var categories = new Dictionary<string, List<VoteObject>>();

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
                VoteObject voteObject = newWord.GetComponentInChildren<VoteObject>();
                voteObject.SetText(word);

                voteObjects.Add(voteObject);
            }

            categories.Add(category.Key, voteObjects);
        }

        return categories;
    }

    public void GetVotingResults(Dictionary<string, List<VoteObject>> votings)
    {
        var results = new Dictionary<string, Dictionary<string, bool?>>();
        foreach (var category in votings)
        {
            var votingResults = new Dictionary<string, bool?>();
            foreach (var voting in category.Value)
            {
                var result = voting.GetResults();
                votingResults.Add(result.Item1, result.Item2);
            }
            results.Add(category.Key, votingResults);
        }
        Debug.Log(JsonConvert.SerializeObject(results));
    }
}
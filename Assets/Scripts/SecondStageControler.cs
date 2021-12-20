using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecondStageControler : MonoBehaviour
{
    [SerializeField] private GameObject wordPrefab;
    [SerializeField] private GameObject letterDisplay;
    [SerializeField] private GameObject firstWordPosition;
    [SerializeField] private GameObject gameController;
    GameObject[] UI;

    private List<GameObject> wordPrefabs = new List<GameObject>();

    private void Start()
    {
        UI = GameObject.FindGameObjectsWithTag("Completing_UI");
    }

    void SetCategories(List<string> categories)
    {
        wordPrefabs = new List<GameObject>();

        foreach (var obj in UI)
        {
            obj.SetActive(true);
        }

        int offset = 0;
        foreach (string category in categories)
        {
            var localPosition = firstWordPosition.transform.position;
            Vector3 newPosition = new Vector3(localPosition.x,
                localPosition.y + offset,
                localPosition.z);
            var newWord = Instantiate(wordPrefab, newPosition, gameObject.transform.rotation);
            newWord.transform.SetParent(gameObject.transform);
            newWord.GetComponent<Word>().SetCategory(category);
            newWord.GetComponent<Word>().SetManager(gameController);

            wordPrefabs.Add(newWord);
            offset -= 68;
        }
    }

    void SetLetter(string draftedLetter)
    {
        letterDisplay.GetComponent<TextMeshProUGUI>().text = draftedLetter;
    }


    public void ResetStage()
    {
        letterDisplay.GetComponent<TextMeshProUGUI>().text = "";
        foreach (var word in wordPrefabs)
        {
            Destroy(word.gameObject);
        }

        foreach (var obj in UI)
        {
            obj.SetActive(false);
        }
    }

    public void SetStage(string draftedLetter, List<string> categories)
    {
        SetLetter(draftedLetter);
        SetCategories(categories);
        // todo set timer?
    }

    public string GetFields()
    {
        var fields = new Dictionary<string, string>();
        foreach (GameObject category in wordPrefabs)
        {
            var wordField = category.GetComponent<Word>().GetField();
            fields.Add(wordField["category"], wordField["value"]);
        }

        var resultsWithCategory = new Dictionary<string, dynamic>();

        resultsWithCategory.Add("results", fields);
        resultsWithCategory.Add("gameState", "COMPLETING");
        return JsonConvert.SerializeObject(resultsWithCategory);
    }
}
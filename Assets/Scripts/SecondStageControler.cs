using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecondStageControler : MonoBehaviour
{
    [SerializeField] private GameObject wordPrefab;
    [SerializeField] private GameObject letterDisplay;
    [SerializeField] private GameObject firstWordPosition;
    private List<GameObject> wordPrefabs = new List<GameObject>();

    private void Start()
    {
        var c = new List<string>()
            {"one", "two", "three", "four"};
        SetStage("c", c);
    }

    void SetCategories(List<string> categories)
    {
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
            wordPrefabs.Add(newWord);
            offset -= 75;
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
    }
    public void SetStage(string draftedLetter, List<string> categories)
    {
        //todo reset stage
        SetLetter(draftedLetter);
        SetCategories(categories);
        // todo set timer?
    }

    public Dictionary<string, string> GetFields()
    {
        var fields = new Dictionary<string, string>();
        foreach (GameObject category in wordPrefabs)
        {
            var wordField = category.GetComponent<Word>().GetField();
            fields.Add(wordField["category"], wordField["value"]);
        }

        return fields;
    }
}
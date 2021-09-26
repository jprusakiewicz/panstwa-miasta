using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Word : MonoBehaviour
{
    [SerializeField] private GameObject category;
    [SerializeField] private GameObject inputForm;


    public void SetCategory(string categoryText)
    {
        category.GetComponent<TextMeshProUGUI>().text = categoryText;
    }

    public Dictionary<string, string> GetField()
    {
        var categoryText = category.GetComponent<TextMeshProUGUI>().text;
        var inputText = inputForm.GetComponent<TextMeshProUGUI>().text;
        return new Dictionary<string, string> {{"category",categoryText},{"value", inputText}};
    }
}

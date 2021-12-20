using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Word : MonoBehaviour
{
    [SerializeField] private GameObject category;
    [SerializeField] private GameObject inputForm;
    private ConnectionManager _connectionManager;

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

    public void SetManager(GameObject gameController)
    {
        _connectionManager = gameController.GetComponent<ConnectionManager>();
    }

    public void CallManager()
    {
        _connectionManager.SendResults();
    }
}

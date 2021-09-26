using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

public class FirstStageControler : MonoBehaviour
{
    private List<Toggle> toggles;

    void Start()
    {
        toggles = GetComponentsInChildren<Toggle>().ToList();
    }

    void Update()
    {        
//        Debug.Log(GetSelectedTopics().Count);
    }

    public List<Toggle> GetActiveToggles()
    {
        var activeToggles = from t in toggles 
                     where t.isOn select t;
        return activeToggles.ToList();
    }

    public List<string> GetSelectedTopics()
    {
        var activeToggles = GetActiveToggles();
        return activeToggles.ConvertAll(toggle => toggle.GetComponentInChildren<Text>().text);
    }
}
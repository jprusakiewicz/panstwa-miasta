using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Headers : MonoBehaviour
{
    private List<TextMeshProUGUI> cells;

    public void SetHeaders(List<string> categories)
    {
        int idx = 0;
        foreach (Transform child in transform)
        {
            if (idx != 0)
                child.gameObject.GetComponent<TextMeshProUGUI>().text = categories[idx - 1];
            idx++;
        }

    }
}

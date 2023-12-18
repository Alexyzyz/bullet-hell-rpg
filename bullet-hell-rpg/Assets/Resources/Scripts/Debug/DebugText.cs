using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{

    public static DebugText Instance { get; private set; }

    private TextMeshProUGUI tmp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Publics

    public void Print(string text)
    {
        tmp.text = text;
    }

}

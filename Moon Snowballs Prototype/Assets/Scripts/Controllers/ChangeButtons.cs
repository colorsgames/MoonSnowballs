using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeButtons : MonoBehaviour
{
    [SerializeField] private Sprite newImage;
    [SerializeField] private GameObject text;

    Button button;

    public bool isPC;

    private void Start()
    {
        if (!isPC) return;
        button = GetComponent<Button>();
        button.image.sprite = newImage;
        text.SetActive(true);
    }
}

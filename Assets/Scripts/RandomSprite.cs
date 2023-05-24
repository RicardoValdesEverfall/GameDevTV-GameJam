using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] Variations;

    void Start()
    {
        this.GetComponent<Image>().sprite = Variations[Random.Range(0, Variations.Length)];
    }
}

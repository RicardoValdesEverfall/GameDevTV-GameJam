using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Camera _environmentCamera_2D;
    [SerializeField] private float ScrollSpeed;
    [SerializeField] private RectTransform[] Layers;

    private RectTransform rt;

    //private float length;
    private float startPos;

   

    void Start()
    {
        if (_environmentCamera_2D == null) { _environmentCamera_2D = GameObject.FindGameObjectWithTag("2DRenderer").GetComponent<Camera>(); }
        startPos = transform.position.x;
        //length = 1920f;

        rt = this.GetComponent<RectTransform>();
    }

    void Update()
    {
       
    }

    public void ApplyScroll(float input) //-1 = moving left, 0 = idle, 1 = moving right
    {
        float distance = input * ScrollSpeed;

        rt.offsetMin -= new Vector2(distance, 0f);
        rt.offsetMax -= new Vector2(distance, 0f);

        
    }
}

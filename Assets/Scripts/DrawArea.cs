using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawArea : MonoBehaviour
{   
    [Header("References")]
    [SerializeField] GameObject linePrefab;

    LineRendererUI currentLine;
    Coroutine draw;

    private void Update(){
        if (!GlobalHandler.Instance.isDrawMode || !GlobalHandler.Instance.showClassAndCode) return;
        if (Input.GetMouseButtonDown(0) && IsMouseTouching()){
            StartDraw();
        }
        if((Input.GetMouseButtonUp(0) || !IsMouseTouching()) && draw != null){
            StopDraw();
        }
    }

    private void StartDraw(){
        draw = StartCoroutine(Draw());
    }

    private void StopDraw(){
        StopCoroutine(draw);
        draw = null;
        SaveLine();
        currentLine.thickness = GlobalHandler.Instance.currentWidth;
    }

    private IEnumerator Draw(){
        int points = 0;
        
        GameObject stroke = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        stroke.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        stroke.transform.SetParent(transform.parent);
        stroke.name = GlobalHandler.Instance.lineObjects.Count.ToString();

        currentLine = stroke.GetComponent<LineRendererUI>();
        currentLine.thickness = GlobalHandler.Instance.currentWidth;

        while(true){
            Vector3 position = Input.mousePosition;
            position.z = 0;
            Array.Resize(ref currentLine.points, currentLine.points.Length + 1);
            currentLine.points[points++] = position;
            currentLine.UpdateGeometry();
            yield return null;
        }
    }

    private bool IsMouseTouching(){
        return GetComponent<Collider2D>().OverlapPoint((Vector2)Input.mousePosition);
    }

    private void SaveLine(){
        GlobalHandler.Instance.lineStrokes.Add(GlobalHandler.Instance.lineStrokes.Count, currentLine.points);
        GlobalHandler.Instance.lineObjects.Add(currentLine.gameObject);
    }
} 


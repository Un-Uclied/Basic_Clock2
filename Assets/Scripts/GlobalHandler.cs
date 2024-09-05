using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalHandler : Singleton<GlobalHandler>
{   
    [SerializeField] private TimeSpan t;
    [Header("Settings")]
    [SerializeField] public bool isDrawMode;
    [SerializeField] public bool showWarning;
    [SerializeField] public bool showClassAndCode;
    [SerializeField] public bool showEndTime;
    [SerializeField] public int endOffest;
    [SerializeField] public float currentWidth;

    [SerializeField] public Dictionary<int, Vector2[]> lineStrokes = new Dictionary<int, Vector2[]>();

    [SerializeField] public List<GameObject> lineObjects;

    public Dictionary<int, TimeSpan> periodStartTimes = new Dictionary<int, TimeSpan>(){
       { 1 , new TimeSpan(9, 0, 0)},
       { 2 , new TimeSpan(10, 0, 0)},
       { 3 , new TimeSpan(11, 0, 0)},
       { 4 , new TimeSpan(11, 55, 0)},
    };

    public Dictionary<int, TimeSpan> periodEndTimes = new Dictionary<int, TimeSpan>(){
       { 1 , new TimeSpan(9, 45, 0)},
       { 2 , new TimeSpan(10, 45, 0)},
       { 3 , new TimeSpan(11, 45, 0)},
       { 4 , new TimeSpan(12, 40, 0)},
    };

    public Dictionary<int, string> periodClass = new Dictionary<int, string>(){
        { 1 , "과목"},
        { 2 , "과목"},
        { 3 , "과목"},
        { 4 , "과목"},
    };

    public Dictionary<int, string> classCode = new Dictionary<int, string>(){
        { 1 , "코드"},
        { 2 , "코드"},
        { 3 , "코드"},
        { 4 , "코드"},
    };
}

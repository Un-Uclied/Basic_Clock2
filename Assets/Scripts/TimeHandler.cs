using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class TimeHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isDebug;
    [SerializeField] private int debugHour;
    [SerializeField] private int debugMinutes;
    [Header("References")]
    [SerializeField] private TextMeshProUGUI mainClockText;
    [SerializeField] private TextMeshProUGUI amOrPmText;
    [SerializeField] private TextMeshProUGUI endTimeText;
    [Space]
    [SerializeField] private TextMeshProUGUI currentClassText;
    [SerializeField] private TextMeshProUGUI currentCodeText;
    [Space]
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private Color warningColor;
    [SerializeField] private Color errorColor;
    [Space]
    [SerializeField] private Button eraseBtn;
    [Header("Current")]
    [SerializeField] private int currentPeriod;
    [SerializeField] private string currentClass;
    [SerializeField] private string currentCode;
    [SerializeField] private string endTime;
    [SerializeField] private bool onTest;

    private void Start(){
        warningText.gameObject.SetActive(false);
    }

    void Update()
    {
        DateTime seoulTime = DateTime.Now;
        if (isDebug)
            seoulTime = new DateTime(2024, 9, 30, debugHour, debugMinutes, 0);

        UpdateTime(seoulTime);
        UpdateClassAndCode();
        UpdatePeriod(seoulTime.TimeOfDay);

        if (GlobalHandler.Instance.isDrawMode){
            currentClassText.text = $"   교시 :";
            currentCodeText.text = $"과목코드 :    ";
            
        }
        else{
            currentClassText.text = $"{currentPeriod}교시 : {currentClass}";
            currentCodeText.text = $"과목코드 : {currentCode}";
        }
        
        UpdateWarning(seoulTime.TimeOfDay);

        if (GlobalHandler.Instance.showClassAndCode){
            currentClassText.gameObject.SetActive(true);
            currentCodeText.gameObject.SetActive(true);
        }
        else{
            currentClassText.gameObject.SetActive(false);
            currentCodeText.gameObject.SetActive(false);
        }
        if (GlobalHandler.Instance.showEndTime){
            endTimeText.gameObject.SetActive(true);
        }
        else{
            endTimeText.gameObject.SetActive(false);
        }

        if (GlobalHandler.Instance.isDrawMode){
            eraseBtn.gameObject.SetActive(true);
        }
        else{
             eraseBtn.gameObject.SetActive(false);
        }
    }

    private void UpdateTime(DateTime time){
        mainClockText.text = $"{time.Hour} : {time.Minute.ToString("00")}";
        amOrPmText.text = time.ToString("tt");
    }

    private void UpdatePeriod(TimeSpan time){
        var start = GlobalHandler.Instance.periodStartTimes;
        var end = GlobalHandler.Instance.periodEndTimes;
        for(int i = 1; i <= 4; i++){
            if (time >= start[i] && time <= end[i]){
                currentPeriod = i;
                onTest = true;
                endTime = $"~ {end[i].Hours} : {end[i].Minutes}";
                break;
            }
            else{
                //if (onTest) DrawManager.Instance.EraseDrawings();
                onTest = false;
            }
        }
        if (onTest){
            endTimeText.text = endTime;
        }
        else{
            endTimeText.text = $"{endTime}\n인터미션";
        }
        
    }

    private void UpdateClassAndCode(){
        if (onTest){
            currentClass = GlobalHandler.Instance.periodClass[currentPeriod];
            currentCode = GlobalHandler.Instance.classCode[currentPeriod];
            endTime = $"~ {GlobalHandler.Instance.periodEndTimes[currentPeriod].Hours} : {GlobalHandler.Instance.periodEndTimes[currentPeriod].Minutes.ToString("00")}";
        }
        else{
            currentClass = "과목 종료";
            currentCode = "??";
            endTime = $"~ {GlobalHandler.Instance.periodStartTimes[currentPeriod + 1].Hours} : {GlobalHandler.Instance.periodStartTimes[currentPeriod + 1].Minutes.ToString("00")}";
        }

        
    }

    private void UpdateWarning(TimeSpan time){
        if (!GlobalHandler.Instance.showWarning){
            return;
        }
        if (time == GlobalHandler.Instance.periodEndTimes[currentPeriod]){
            warningText.text = "시험 종료";
            warningText.color = errorColor;
            warningText.gameObject.SetActive(true);
        }
        else if (time.Minutes + 5 >= GlobalHandler.Instance.periodEndTimes[currentPeriod].Minutes && onTest){
            warningText.text = $"시험 종료 {int.Parse((GlobalHandler.Instance.periodEndTimes[currentPeriod].Minutes - time.Minutes + GlobalHandler.Instance.endOffest).ToString("00"))}분전";
            warningText.color = warningColor;
            warningText.gameObject.SetActive(true);
        }
        else{
            if (warningText.gameObject.activeInHierarchy){
                warningText.gameObject.SetActive(false);
            }
        }
    }

    public void OnSettingButtonClick(){
        //DrawManager.Instance.SaveDrawingPositions();
        SceneManager.LoadScene("Settings");
    }

    public void OnErasingButtonClick(){
        DrawManager.Instance.EraseDrawings();
    }

}

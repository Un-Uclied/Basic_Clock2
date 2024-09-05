using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class SettingHandler : MonoBehaviour
{   
    [SerializeField] private bool canChangeIF;
    [SerializeField] private bool isChanging;
    [Header("Referecnes")]
    [SerializeField] private TextMeshProUGUI clock;

    [Space]
    [SerializeField] private Toggle showFiveMinWarningToggle;
    [SerializeField] private Toggle showClassAndCodeToggle;
    [SerializeField] private Toggle showEndTimeToggle;
    [SerializeField] private Toggle drawModeToggle;

    [Space]

    [SerializeField] private Button editStartButton;
    [SerializeField] private Button editSaveButton;

    [Space]
    
    [SerializeField] private TMP_InputField periodIF;
    [SerializeField] private TMP_InputField classIF;
    [SerializeField] private TMP_InputField codeIF;
    [Space]
    [SerializeField] private TMP_InputField startHIF;
    [SerializeField] private TMP_InputField startMIF;
    [Space]
    [SerializeField] private TMP_InputField endHIF;
    [SerializeField] private TMP_InputField endMIF;
    [Space]
    [SerializeField] private TextMeshProUGUI errorText;
    [Space]
    [SerializeField] private TMP_InputField endOffsetIF;
    [Space]
    [SerializeField] private TMP_InputField widthIF;

    [Header("Settings")]
    [SerializeField] private Color errorColor;
    [SerializeField] private Color successColor;

    [Header("Current")]
    [SerializeField] private string deniedCause;

    private void Start(){
        endOffsetIF.text = GlobalHandler.Instance.endOffest.ToString();
        widthIF.text = GlobalHandler.Instance.currentWidth.ToString();
        showFiveMinWarningToggle.isOn = GlobalHandler.Instance.showWarning;
        showClassAndCodeToggle.isOn = GlobalHandler.Instance.showClassAndCode;
        showEndTimeToggle.isOn = GlobalHandler.Instance.showEndTime;
        drawModeToggle.isOn = GlobalHandler.Instance.isDrawMode;
    }

    private void Update(){
        GlobalHandler.Instance.showWarning = showFiveMinWarningToggle.isOn;
        GlobalHandler.Instance.showClassAndCode = showClassAndCodeToggle.isOn;
        GlobalHandler.Instance.showEndTime = showEndTimeToggle.isOn;
        GlobalHandler.Instance.isDrawMode = drawModeToggle.isOn;

        DateTime seoulTime = DateTime.Now;
        clock.text = $"{seoulTime.Hour} : {seoulTime.Minute.ToString("00")}";
    }

    public void OnEditStartButtonClick(){
        if (!canChangeIF) return; 
        if (isChanging) {
            DenyEdit("아직 바꾸는 중입니다!");
            return;
        }
        if (!int.TryParse(periodIF.text, out int result)){
            DenyEdit("~교시가 정수가 아닙니다!");
            return;
        }
        if (int.TryParse(periodIF.text, out int res) && (res < 1 || res > 4)){
            DenyEdit("~교시가 1 ~ 4를 넘습니다!");
            return;
        }
        isChanging = true;
        startHIF.text = GlobalHandler.Instance.periodStartTimes[int.Parse(periodIF.text)].Hours.ToString();
        startMIF.text = GlobalHandler.Instance.periodStartTimes[int.Parse(periodIF.text)].Minutes.ToString("00");
        endHIF.text = GlobalHandler.Instance.periodEndTimes[int.Parse(periodIF.text)].Hours.ToString();
        endMIF.text = GlobalHandler.Instance.periodEndTimes[int.Parse(periodIF.text)].Minutes.ToString("00");
    
        classIF.text = GlobalHandler.Instance.periodClass[int.Parse(periodIF.text)];
        codeIF.text = GlobalHandler.Instance.classCode[int.Parse(periodIF.text)];
    }

    public void OnEditEndButtonClick(){
        if (!canChangeIF) return; 
        if (int.TryParse(periodIF.text, out int result) == false){
            DenyEdit("~교시가 정수가 아닙니다!");
            return;
        }
        if (periodIF.text == "" || classIF.text == "" || codeIF.text == "" || startHIF.text == "" || startMIF.text == "" || endHIF.text == "" || endMIF.text == ""){
            DenyEdit("입력칸중 몇개가 비었습니다!");
            return;
        }
        if (int.Parse(periodIF.text) < 1 && int.Parse(periodIF.text) > 4){
            DenyEdit("~교시가 1교시 전이거나 4교시후입니다!");
            return;
        }
        if (!int.TryParse(startHIF.text, out int resSHIF) || !int.TryParse(startMIF.text, out int resSMIF) || !int.TryParse(endHIF.text, out int resEHIF)|| !int.TryParse(endMIF.text, out int resMHIF)){
            DenyEdit("시간이 정수가 아닌것 같습니다!");
            return;
        }
        if (int.Parse(startHIF.text) < 1 || int.Parse(startHIF.text) > 12 || int.Parse(startMIF.text) < 0 || int.Parse(startMIF.text) >  60){
            DenyEdit("시작하는 시간을 다시 확인해주세요!");
            return;
        }
        if (int.Parse(endHIF.text) < 1 || int.Parse(endHIF.text) > 12 || int.Parse(endMIF.text) < 0 || int.Parse(endMIF.text) >  60){
            DenyEdit("끝나는 시간을 다시 확인해주세요!");
            return;
        }
        TimeSpan startTime = new TimeSpan(int.Parse(startHIF.text), int.Parse(startMIF.text), 0);
        TimeSpan endTime = new TimeSpan(int.Parse(endHIF.text), int.Parse(endMIF.text), 0);
        if (endTime < startTime){
            DenyEdit("끝나는 시간이 시작하는 시간보다 앞에 있습니다!");
            return;
        }

        GlobalHandler.Instance.periodClass[int.Parse(periodIF.text)] = classIF.text;
        GlobalHandler.Instance.classCode[int.Parse(periodIF.text)] = codeIF.text;
        GlobalHandler.Instance.periodStartTimes[int.Parse(periodIF.text)] = new TimeSpan(int.Parse(startHIF.text), int.Parse(startMIF.text), 0);
        GlobalHandler.Instance.periodEndTimes[int.Parse(periodIF.text)] = new TimeSpan(int.Parse(endHIF.text), int.Parse(endMIF.text), 0);

        isChanging = false;

        EditSuccess("~교시 설정 완료!");
    }

    public void UpdateEndOffest(){
        int offset = 0;
        if (int.TryParse(endOffsetIF.text, out int res)){
            offset = res;
        }else{ DenyEdit("오프셋이 정수가 아닌것 같습니다."); return;}
        if (offset < -3 || offset > 3) {DenyEdit("정해진 범위의 정수만 입력해주세요.\n범위 : -3 ~ 3"); return;}

        GlobalHandler.Instance.endOffest = offset;

        EditSuccess("오프셋 설정 완료!");
        
    }

    private async void DenyEdit(string cause){
        errorText.color = errorColor;
        errorText.text = cause;
        errorText.gameObject.SetActive(true);
        
        await Task.Delay(1500);
        errorText.gameObject.SetActive(false);
    }

    private async void EditSuccess(string msg){
        errorText.color = successColor;
        errorText.text = msg;
        errorText.gameObject.SetActive(true);

        await Task.Delay(1500);
        errorText.gameObject.SetActive(false);
    }

    public void OnExitButtonClick(){
        SceneManager.LoadScene("MainClock");
    }

    public void OnWidthSettingClick(){
        
        if (int.TryParse(widthIF.text, out int res)){
            if (1 < res && res > 10){
                DenyEdit("범위에 맞는 정수를 입력해 주세요!"); return;
            }
            GlobalHandler.Instance.currentWidth = res;
            EditSuccess("선두께 설정 완료!");
        }else{
            DenyEdit("선두께가 정수가 아닌것 같습니다."); return;
        }
        
    }
}

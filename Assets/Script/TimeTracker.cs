using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeTracker : MonoBehaviour
 {
    public static TimeTracker instance;
    public TimeSpan timeCounter;
    DateTime lastChecked;
 
    // public Text txtTime;
    public float updateFrequency = 0.1f;

    bool bRun = true;
 
    void Awake() {
        Debug.Log("Goodmorning from TimeTracker!");
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            Debug.Log("Deleting object from TimeTracker!");
        }
    }
     // Use this for initialization
     void Start()
     {
         string strVal = PlayerPrefs.GetString ( "TimeRun" , "" );
         long ticks = 0;
 
         long.TryParse ( strVal , out ticks );
 
         instance.timeCounter = new TimeSpan ( ticks );
 
         instance.lastChecked = DateTime.Now;
 
         StartCoroutine ( instance.CalcAndDisplay ( ) );
     }

    public void StartTimer() {
        if (!bRun) {
            bRun = true;
            lastChecked = DateTime.Now;
            StartCoroutine ( CalcAndDisplay ( ) );
        }
     }

    public void StopAndResetTimer() {
        StopTimer();
        PlayerPrefs.SetString ( "TimeRun" , "" );
        PlayerPrefs.Save ( );
    }

    public void StopTimer() {
        bRun = false;
     }
 
     void OnApplicationQuit()
     {
        Debug.Log("Quitting....");
         PlayerPrefs.SetString ( "TimeRun" , instance.timeCounter.Ticks.ToString ( ) );
         PlayerPrefs.Save ( );
     }
 
     IEnumerator CalcAndDisplay()
     {
         
 
         while ( bRun )
         {
             DateTime now = DateTime.Now;
 
             timeCounter += now - lastChecked;
 
             lastChecked = now;
 
            //  txtTime.text = "timePassed " +
            //      string.Format ( "{0:D2}:{1:D2}:{2:D2}" , timeCounter.Hours , timeCounter.Minutes , instance.timeCounter.Seconds );
 
             yield return new WaitForSeconds ( updateFrequency );
         }
     }
 }
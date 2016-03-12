using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class EditorScratchPad : MonoBehaviour {

    [MenuItem("Custom/Shields/Add quarter shield")]
    public static void AddQuarterShield(){
        Shield.Instance.AddShield(25);
    }

    [MenuItem("Custom/TimeScale/Time to .1f")]
    public static void TimeToPoint1(){
        Time.timeScale = .1f;
    }

    [MenuItem("Custom/TimeScale/Time to 1f")]
    public static void TimeTo1(){
        Time.timeScale = 1f;
    }

    [MenuItem("Custom/Transitions/Transition From Menu To Gameplay")]
    public static void TestTransitionFromMenuToGameplay(){
        TransitionRig.Instance.TransitionFromMenuToGameplay();
    }
}

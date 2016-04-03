using UnityEngine;
using System.Collections;

public class CoreSystems : Singleton<CoreSystems> {

    void Start(){
        Debug.Log(CoreSystems.Instance == null);
    }

}

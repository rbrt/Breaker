using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class Building : MonoBehaviour {

	public float height;
	public float width;
	public float depth;

	public GameObject[] elements;

	Building me;

	public void SetValues(float height, float width, float depth, GameObject[] elements){
		this.height = height;
		this.width = width;
		this.depth = depth;

		this.elements = elements;
	}

	void Awake(){
		me = this;
	}

	void Update(){
		var point = CameraController.ScreenPoint(transform.position + Vector3.right * width);

		if (point.x + (transform.localScale.x / 2) < -30){
			GenerateBackground.Instance.RecycleBuilding(ref me);
		}
	}

}

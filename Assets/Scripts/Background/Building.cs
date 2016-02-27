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

	public void SetValues(float height, float width, float depth, GameObject[] elements){
		this.height = height;
		this.width = width;
		this.depth = depth;

		this.elements = elements;
	}

}

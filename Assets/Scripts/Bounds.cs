using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class Bounds : MonoBehaviour {

	// Reasonable vertical bounds for enemies to exist within
	public const float yMax = 5;
	public const float yMin = -1;
}

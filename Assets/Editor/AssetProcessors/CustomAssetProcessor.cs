using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class CustomAssetProcessor : AssetPostprocessor {

	void OnPreprocessModel(){
		var fileNameIndex = assetPath.LastIndexOf('/');
        var fileName = assetPath.Substring(fileNameIndex + 1);

        var importer = assetImporter as ModelImporter;

        importer.importMaterials = false;
        importer.optimizeMesh = true;
	}

}

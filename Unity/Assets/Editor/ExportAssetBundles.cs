using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ExportAssetBundles {

	public const string RESOURCE_PATH = "Assets/Resources/AssetBundleResource";

	const string TARGET_PARAME = "-Assets/Resources/AssetBundleResource=";
	const string EXPORT_DIR = "-Assets/Resources/AssetBundleResource=";

	private static readonly Dictionary<string, BuildTarget> _targets = new Dictionary<string, BuildTarget>(){
		{"iOS", BuildTarget.iOS},
		{"Android", BuildTarget.Android}
	};

	[MenuItem("Assets/SetAssetBundleName")]
	public static void SetAssetBundleNameForAll() {
		// AssetBundleResource以下のファイルにAssetBundleNameを設定
		foreach (var path in AssetDatabase.GetAllAssetPaths()) {
			SetAssetBundleName(path);
		}

		AssetDatabase.RemoveUnusedAssetBundleNames();
	}

	[MenuItem("Assets/ExportFilesFromBatch")]
	public static void ExportFilesFromBatch() {
		//string targetStr = GetParameterArgument(TARGET_PARAME);
		string targetStr = "Android";
		if (string.IsNullOrEmpty(targetStr) || !_targets.ContainsKey(targetStr)) {
			Debug.LogError("target is invalid! "+TARGET_PARAME+"iOS or "+TARGET_PARAME+"Android");
			EditorApplication.Exit(1);
			return;
		}

		//string exportDir = GetParameterArgument(EXPORT_DIR);
		string exportDir = "Assets/AssetBundleData";
		exportDir = exportDir ?? Application.dataPath;
		Debug.Log("exportDir = " + exportDir);

		BuildTarget target = _targets[targetStr];
		Debug.Log("target = " + target);

		SetAssetBundleNameForAll ();

		var manifest = BuildPipeline.BuildAssetBundles(exportDir, BuildAssetBundleOptions.IgnoreTypeTreeChanges, target);

		// 命名規則を統一するためにSingleManifestのファイル名を変更
		/*if (manifest != null) {
			var spl = exportDir.Split('/');
			var folderName = spl[spl.Length - 1];
			var currentPath = exportDir + "/" +  folderName;
			if (File.Exists(currentPath)) {
				var spl2 = currentPath.Split('/');
				string newPath = "";
				int lastIndex = spl2.Length;
				for (int i = 0; i < lastIndex; i++) {
					newPath += (i == lastIndex - 1) ? spl2[i].ToLower() : spl2[i] + "/";
				}
				newPath += ".unity3d";
				File.Delete(newPath);
				Debug.Log ("newPath==="+newPath);
				File.Move(currentPath, newPath);
			}
		}*/
	}

	private static string[] GetParameterArgumentArray(string parameterName)
	{
		var arg = GetParameterArgument(parameterName);
		if (string.IsNullOrEmpty(arg)) return null;
		return arg.Split(',');
	}

	private static string GetParameterArgument(string parameterName)
	{
		foreach (var arg in System.Environment.GetCommandLineArgs())
		{
			if (arg.ToLower().StartsWith(parameterName.ToLower()))
			{
				return arg.Substring(parameterName.Length);
			}
		}
		return null;
	}

	public static void SetAssetBundleName(string path) {
		var importer = AssetImporter.GetAtPath(path);
		string assetBundleName = importer.assetBundleName;
		if (!path.StartsWith(ExportAssetBundles.RESOURCE_PATH)) {
			if (!string.IsNullOrEmpty( assetBundleName )) {
				importer.assetBundleName = "";
			}
			return;
		}

		string newName = path.Split('/').Last().Split('.').First() + ".unity3d";


		if (assetBundleName != newName) {
			var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
			// フォルダを弾く
			if (obj.GetType() == typeof(DefaultAsset)) {
				return;
			}
			if (newName != newName.ToLower()) {
				importer.assetBundleName = "";
				Debug.LogError("Invalid Name = " + newName);
				return;
			}

			Debug.Log("new name = " + newName);
			importer.assetBundleName = newName;
		}
	}

}
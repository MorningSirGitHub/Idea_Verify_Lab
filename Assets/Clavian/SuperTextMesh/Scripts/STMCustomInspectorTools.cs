//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
static class STMCustomInspectorTools {
	public static void DrawTitleBar(Object myObject, SuperTextMesh stm){
		if(myObject != null){
			EditorGUILayout.BeginHorizontal();
		//ping button:
			if(GUILayout.Button("Ping")){
				//EditorUtility.FocusProjectWindow(); this doesn't work for some reason
				EditorGUIUtility.PingObject(myObject); //select this object
			}
		//name:
			EditorGUI.BeginChangeCheck();
			myObject.name = EditorGUILayout.TextField(myObject.name);
			if(EditorGUI.EndChangeCheck()){
				AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(myObject), myObject.name);
				//Undo.RecordObject (myObject, "Change Asset Name");
				AssetDatabase.Refresh();
				stm.data = null;
			}
		//delete button:
			if(GUILayout.Button("X")){
				//AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(myObject));
				AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(myObject));
				//Undo.DestroyObjectImmediate(myObject);
				AssetDatabase.Refresh();
				stm.data = null; //make this refresh, too
			}
			EditorGUILayout.EndHorizontal();
		}
	}
	/*
	public static void OnUndoRedo(){
		AssetDatabase.Refresh();
	}
	*/
	public static void DrawCreateFolderButton(string buttonText, string parentFolder, string newFolder, SuperTextMesh stm){
		if(GUILayout.Button(buttonText)){
			AssetDatabase.CreateFolder("Assets/Clavian/SuperTextMesh/Resources/" + parentFolder, newFolder);
			AssetDatabase.Refresh();
			stm.data = null;
		}
	}
	public static void DrawCreateNewButton(string buttonText, string folderName, string typeName, SuperTextMesh stm){
		if(GUILayout.Button(buttonText)){
			ScriptableObject newData = NewData(typeName);
			if(newData != null){
				AssetDatabase.CreateAsset(newData,AssetDatabase.GenerateUniqueAssetPath("Assets/Clavian/SuperTextMesh/Resources/" + folderName)); //save to file
				//Undo.undoRedoPerformed += OnUndoRedo; //subscribe to event
				//Undo.RegisterCreatedObjectUndo(newData, buttonText);
				AssetDatabase.Refresh();
				stm.data = null;
			}
		}
	}
	public static ScriptableObject NewData(string myType){
		switch(myType){
			case "STMAudioClipData": return ScriptableObject.CreateInstance<STMAudioClipData>();
			case "STMAutoClipData": return ScriptableObject.CreateInstance<STMAutoClipData>();
			case "STMColorData": return ScriptableObject.CreateInstance<STMColorData>();
			case "STMDelayData": return ScriptableObject.CreateInstance<STMDelayData>();
			case "STMDrawAnimData": return ScriptableObject.CreateInstance<STMDrawAnimData>();
			case "STMFontData": return ScriptableObject.CreateInstance<STMFontData>();
			case "STMGradientData": return ScriptableObject.CreateInstance<STMGradientData>();
			case "STMJitterData": return ScriptableObject.CreateInstance<STMJitterData>();
			case "STMMaterialData": return ScriptableObject.CreateInstance<STMMaterialData>();
			case "STMQuadData": return ScriptableObject.CreateInstance<STMQuadData>();
			case "STMSoundClipData": return ScriptableObject.CreateInstance<STMSoundClipData>();
			case "STMTextureData": return ScriptableObject.CreateInstance<STMTextureData>();
			case "STMVoiceData": return ScriptableObject.CreateInstance<STMVoiceData>();
			case "STMWaveData": return ScriptableObject.CreateInstance<STMWaveData>();
			default: Debug.Log("New data type unknown."); return null;
		}
	}
	public static void DrawMaterialEditor(Material mat){
		//Just set these directly, why not. It's a custom inspector already, no need to bog this down even more
		Undo.RecordObject(mat, "Changed Super Text Mesh Material");
		int originalQueue = mat.renderQueue;
		mat.shader = (Shader)EditorGUILayout.ObjectField("Shader", mat.shader, typeof(Shader), false);
		if(mat.HasProperty("_ShadowColor")){
			//EditorGUILayout.PropertyField(shadowColor);
			mat.SetColor("_ShadowColor",EditorGUILayout.ColorField("Shadow Color",mat.GetColor("_ShadowColor")));
		}
		if(mat.HasProperty("_ShadowDistance")){
			//EditorGUILayout.PropertyField(shadowDistance);
			mat.SetFloat("_ShadowDistance",EditorGUILayout.FloatField("Shadow Distance",mat.GetFloat("_ShadowDistance")));
		}
		if(mat.HasProperty("_ShadowAngle")){
			//EditorGUILayout.PropertyField(shadowAngle);
			mat.SetFloat("_ShadowAngle",EditorGUILayout.Slider("Shadow Angle",mat.GetFloat("_ShadowAngle"),0f,360f));
		}
		if(mat.HasProperty("_OutlineColor")){
			//EditorGUILayout.PropertyField(outlineColor);
			mat.SetColor("_OutlineColor",EditorGUILayout.ColorField("Outline Color",mat.GetColor("_OutlineColor")));
		}
		if(mat.HasProperty("_OutlineWidth")){
			//EditorGUILayout.PropertyField(outlineWidth);
			mat.SetFloat("_OutlineWidth",EditorGUILayout.FloatField("Outline Width",mat.GetFloat("_OutlineWidth")));
		}
		if(mat.HasProperty("_Blend")){
			//EditorGUILayout.PropertyField(shaderBlend);
			mat.SetFloat("_Blend",EditorGUILayout.Slider("Blend",mat.GetFloat("_Blend"),0f,1f));
		}
		if(mat.HasProperty("_SDFCutoff")){
			mat.SetFloat("_SDFCutoff",EditorGUILayout.Slider("SDF Cutoff",mat.GetFloat("_SDFCutoff"),0f,1f));
		}
		mat.renderQueue = EditorGUILayout.IntField("Render Queue", originalQueue);
	}
}
#endif

//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Font Data", menuName = "Super Text Mesh/Font Data", order = 1)]
public class STMFontData : ScriptableObject{
	//public string name;
	public Font font;
	[Tooltip("Only effects dynamic fonts.")]
	[Range(1,512)] public int quality = 64; 
	public FilterMode filterMode = FilterMode.Bilinear; //default

	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty font = serializedData.FindProperty("font");
		SerializedProperty quality = serializedData.FindProperty("quality");
		SerializedProperty filterMode = serializedData.FindProperty("filterMode");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(font);
		if(this.font != null){
			if(!this.font.dynamic){
				EditorGUI.BeginDisabledGroup(!this.font.dynamic);
				this.quality = EditorGUILayout.IntSlider("Quality",this.font.fontSize,1,512);
				EditorGUI.EndDisabledGroup();
			}else{
				EditorGUILayout.PropertyField(quality);
			}
			EditorGUILayout.PropertyField(filterMode);
		}
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}
//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class STMWaveControl{
	public AnimationCurve curveX; //unfortunately, there isn't a way to completely instantiate a sine/cos wave that loops in just one line of code, so I can't do that, here
	public AnimationCurve curveY;
	public AnimationCurve curveZ;
	[Range(0f,1f)]
	public float offset; //to be multiplied by 6. timing offset
	public Vector3 speed; //how wide the curve is... so how fast it'll animate
	public Vector3 strength; //how far the curve will move the letters
	public Vector3 density; //how wide the curve is on letters?
}
[CreateAssetMenu(fileName = "New Wave Data", menuName = "Super Text Mesh/Wave Data", order = 1)]
public class STMWaveData : ScriptableObject{
	//public string name;
	public bool animateFromTimeDrawn = false;
	//public bool showMainFoldout = false;
	public STMWaveControl main;
	[Tooltip("Use these below values?")] //this should be getting hidden but it's not...
	public bool individualVertexControl = false;
	//public bool showIndividualFoldouts = false;
	public STMWaveControl topLeft;
	public STMWaveControl topRight;
	public STMWaveControl bottomLeft;
	public STMWaveControl bottomRight;

	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty animateFromTimeDrawn = serializedData.FindProperty("animateFromTimeDrawn");
		SerializedProperty main = serializedData.FindProperty("main");
		SerializedProperty individualVertexControl = serializedData.FindProperty("individualVertexControl");
		SerializedProperty topLeft = serializedData.FindProperty("topLeft");
		SerializedProperty topRight = serializedData.FindProperty("topRight");
		SerializedProperty bottomLeft = serializedData.FindProperty("bottomLeft");
		SerializedProperty bottomRight = serializedData.FindProperty("bottomRight");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(animateFromTimeDrawn);
		EditorGUILayout.PropertyField(main,true);
		EditorGUILayout.PropertyField(individualVertexControl);
		if(individualVertexControl.boolValue){
			EditorGUILayout.PropertyField(topLeft,true);
			EditorGUILayout.PropertyField(topRight,true);
			EditorGUILayout.PropertyField(bottomLeft,true);
			EditorGUILayout.PropertyField(bottomRight,true);
		}
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}
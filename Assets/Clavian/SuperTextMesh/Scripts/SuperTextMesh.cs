//Copyright (c) 2016-2017 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using UnityEngine.Events; //for the OnComplete event
#if UNITY_EDITOR
using UnityEditor; //for loading default stuff and menu thing
#endif
using System.Linq; //for sorting inspector stuff by creation date, and removing doubles from lists
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; //for in-game UI stuff

#if UNITY_5_4_OR_NEWER
using UnityEngine.SceneManagement; //for OnSceneLoaded
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(SuperTextMesh))]
[CanEditMultipleObjects] //sure why not
public class SuperTextMeshEditor : Editor{
	override public void OnInspectorGUI(){
		serializedObject.Update(); //for onvalidate stuff!
		var stm = target as SuperTextMesh; //get this text mesh as a component
		//Gather variables to display and use
		SerializedProperty text = serializedObject.FindProperty("_text"); //get the inspector's text field
		SerializedProperty font = serializedObject.FindProperty("font");
		SerializedProperty color = serializedObject.FindProperty("color");
		SerializedProperty richText = serializedObject.FindProperty("richText");
		SerializedProperty readDelay = serializedObject.FindProperty("readDelay");
		SerializedProperty speedReadScale = serializedObject.FindProperty("speedReadScale");
		SerializedProperty ignoreTimeScale = serializedObject.FindProperty("ignoreTimeScale");
		SerializedProperty disableAnimatedText = serializedObject.FindProperty("disableAnimatedText");
		SerializedProperty drawOrder = serializedObject.FindProperty("drawOrder");

		SerializedProperty drawAnimName = serializedObject.FindProperty("drawAnimName"); //so changing value calls onvalidate
		
		SerializedProperty unreadDelay = serializedObject.FindProperty("unreadDelay");
		SerializedProperty undrawOrder = serializedObject.FindProperty("undrawOrder");
		SerializedProperty undrawAnimName = serializedObject.FindProperty("undrawAnimName");

		SerializedProperty audioSource = serializedObject.FindProperty("audioSource");
		SerializedProperty audioClips = serializedObject.FindProperty("audioClips");
		SerializedProperty stopPreviousSound = serializedObject.FindProperty("stopPreviousSound");
		SerializedProperty minPitch = serializedObject.FindProperty("minPitch");
		SerializedProperty maxPitch = serializedObject.FindProperty("maxPitch");
		SerializedProperty pitchMode = serializedObject.FindProperty("pitchMode");
		SerializedProperty overridePitch = serializedObject.FindProperty("overridePitch");
		SerializedProperty speedReadPitch = serializedObject.FindProperty("speedReadPitch");
		SerializedProperty perlinPitchMulti = serializedObject.FindProperty("perlinPitchMulti");

		SerializedProperty size = serializedObject.FindProperty("size");
		SerializedProperty quality = serializedObject.FindProperty("quality");
		SerializedProperty filterMode = serializedObject.FindProperty("filterMode");
		SerializedProperty style = serializedObject.FindProperty("style");
		SerializedProperty baseOffset = serializedObject.FindProperty("baseOffset");
		SerializedProperty lineSpacing = serializedObject.FindProperty("lineSpacing");
		SerializedProperty characterSpacing = serializedObject.FindProperty("characterSpacing");
		SerializedProperty tabSize = serializedObject.FindProperty("tabSize");
		SerializedProperty autoWrap = serializedObject.FindProperty("autoWrap");
		SerializedProperty wrapText = serializedObject.FindProperty("wrapText");
		SerializedProperty lineCountLimit = serializedObject.FindProperty("lineCountLimit");

		SerializedProperty breakText = serializedObject.FindProperty("breakText");
		SerializedProperty insertHyphens = serializedObject.FindProperty("insertHyphens");
		SerializedProperty anchor = serializedObject.FindProperty("anchor");
		SerializedProperty alignment = serializedObject.FindProperty("alignment");
		SerializedProperty textMat = serializedObject.FindProperty("textMat");

		SerializedProperty onCompleteEvent = serializedObject.FindProperty("onCompleteEvent");
		SerializedProperty customEvent = serializedObject.FindProperty("customEvent");
		SerializedProperty onUndrawnEvent = serializedObject.FindProperty("onUndrawnEvent");
		SerializedProperty onRebuildEvent = serializedObject.FindProperty("onRebuildEvent");
		SerializedProperty modifyVertices = serializedObject.FindProperty("modifyVertices");
		SerializedProperty vertexMod = serializedObject.FindProperty("vertexMod");

		SerializedProperty debugMode = serializedObject.FindProperty("debugMode");
		SerializedProperty autoRead = serializedObject.FindProperty("autoRead");
		SerializedProperty rememberReadPosition = serializedObject.FindProperty("rememberReadPosition");

	//Actually Drawing it to the inspector:
		Rect r = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, 0f); //get width on inspector, minus scrollbar

		GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout); //create a new foldout style, for the bold foldout headers
		foldoutStyle.fontStyle = FontStyle.Bold; //set it to look like a header
	//TEXT DATA ICON
		//Object textDataObject = stm.data; //get text data object
		GUIStyle textDataStyle = new GUIStyle(EditorStyles.label);
		//textDataStyle.fixedWidth = 14;
		//textDataStyle.fixedHeight = 14;
		//Get Texture2D one of these two ways:
		//Texture2D textDataIcon = AssetDatabase.LoadAssetAtPath("Assets/Clavian/SuperTextMesh/Scripts/SuperTextMeshDataIcon.png", typeof(Texture2D)) as Texture2D;
		Texture2D textDataIcon = EditorGUIUtility.ObjectContent(stm.data, typeof(SuperTextMeshData)).image as Texture2D;
		textDataStyle.normal.background = textDataIcon; //apply
		textDataStyle.active.background = textDataIcon;
		if(GUI.Button(new Rect(r.width - 2, r.y, 16, 16), new GUIContent("", "Edit TextData"), textDataStyle)){ //place at exact spot
			//EditorWindow.GetWindow()
			//EditorUtility.FocusProjectWindow();
			//Selection.activeObject = textDataObject; //go to textdata!
			//EditorGUIUtility.PingObject(textDataObject);
			stm.data.textDataEditMode = !stm.data.textDataEditMode; //show textdata inspector!
			//if(stm.data.textDataEditMode){
			//	stm.data = null;
			//}
		}
	
		if(stm.data.textDataEditMode){//show textdata file instead
			var serializedData = new SerializedObject(stm.data);
			serializedData.Update();

			SerializedProperty dataDisableAnimatedText = serializedData.FindProperty("disableAnimatedText");
			SerializedProperty dataAutoVoice = serializedData.FindProperty("autoVoice");

		//Draw it!
			EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.HelpBox("Editing Text Data. Click the [T] to exit!", MessageType.None, true);

			stm.data.showEffectsFoldout = EditorGUILayout.Foldout(stm.data.showEffectsFoldout, "Effects", foldoutStyle);
			if(stm.data.showEffectsFoldout){
				EditorGUI.indentLevel++;
			//Waves:
				stm.data.showWavesFoldout = EditorGUILayout.Foldout(stm.data.showWavesFoldout, "Waves", foldoutStyle);
				if(stm.data.showWavesFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMWaveData> i in stm.data.waves.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each wave
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Wave", "STMWaves/New Wave.asset", "STMWaveData", stm);
				}
			//Jitters:
				stm.data.showJittersFoldout = EditorGUILayout.Foldout(stm.data.showJittersFoldout, "Jitters", foldoutStyle);
				if(stm.data.showJittersFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMJitterData> i in stm.data.jitters.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Jitter", "STMJitters/New Jitter.asset", "STMJitterData", stm);
				}
			//Draw Animations:
				stm.data.showDrawAnimsFoldout = EditorGUILayout.Foldout(stm.data.showDrawAnimsFoldout, "DrawAnims", foldoutStyle);
				if(stm.data.showDrawAnimsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMDrawAnimData> i in stm.data.drawAnims.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New DrawAnim", "STMDrawAnims/New DrawAnim.asset", "STMDrawAnimData", stm);
				}
				EditorGUI.indentLevel--;
			}
			stm.data.showTextColorFoldout = EditorGUILayout.Foldout(stm.data.showTextColorFoldout, "Text Color", foldoutStyle);
			if(stm.data.showTextColorFoldout){
				EditorGUI.indentLevel++;
			//Colors:
				stm.data.showColorsFoldout = EditorGUILayout.Foldout(stm.data.showColorsFoldout, "Colors", foldoutStyle);
				if(stm.data.showColorsFoldout){
				//Gather all data
					foreach(KeyValuePair<string, STMColorData> i in stm.data.colors.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Color", "STMColors/New Color.asset", "STMColorData", stm);
				}
			//Gradients:
				stm.data.showGradientsFoldout = EditorGUILayout.Foldout(stm.data.showGradientsFoldout, "Gradients", foldoutStyle);
				if(stm.data.showGradientsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMGradientData> i in stm.data.gradients.OrderBy(x => -x.Value.GetInstanceID())){ //reorder so the order stays consistent
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Gradient", "STMGradients/New Gradient.asset", "STMGradientData", stm);
				}
			//Textures:
				stm.data.showTexturesFoldout = EditorGUILayout.Foldout(stm.data.showTexturesFoldout, "Textures", foldoutStyle);
				if(stm.data.showTexturesFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMTextureData> i in stm.data.textures.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Texture", "STMTextures/New Texture.asset", "STMTextureData", stm);
				}
				EditorGUI.indentLevel--;
			}
			stm.data.showInlineFoldout = EditorGUILayout.Foldout(stm.data.showInlineFoldout, "Inline", foldoutStyle);
			if(stm.data.showInlineFoldout){
				EditorGUI.indentLevel++;
			//Delays:
				stm.data.showDelaysFoldout = EditorGUILayout.Foldout(stm.data.showDelaysFoldout, "Delays", foldoutStyle);
				if(stm.data.showDelaysFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMDelayData> i in stm.data.delays.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Delay", "STMDelays/New Delay.asset", "STMDelayData", stm);
				}
			//Voices:
				stm.data.showVoicesFoldout = EditorGUILayout.Foldout(stm.data.showVoicesFoldout, "Voices", foldoutStyle);
				if(stm.data.showVoicesFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMVoiceData> i in stm.data.voices.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Voice", "STMVoices/New Voice.asset", "STMVoiceData", stm);
				}
			//Fonts:
				stm.data.showFontsFoldout = EditorGUILayout.Foldout(stm.data.showFontsFoldout, "Fonts", foldoutStyle);
				if(stm.data.showFontsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMFontData> i in stm.data.fonts.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Font", "STMFonts/New Font.asset", "STMFontData", stm);
				}
			//AudioClips:
				stm.data.showAudioClipsFoldout = EditorGUILayout.Foldout(stm.data.showAudioClipsFoldout, "AudioClips", foldoutStyle);
				if(stm.data.showAudioClipsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMAudioClipData> i in stm.data.audioClips.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Audio Clip", "STMAudioClips/New Audio Clip.asset", "STMAudioClipData", stm);
				}
			//Sound Clips:
			//This one's a bit different! Since it's folders of clips...
				stm.data.showSoundClipsFoldout = EditorGUILayout.Foldout(stm.data.showSoundClipsFoldout, "Sound Clips", foldoutStyle);
				if(stm.data.showSoundClipsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMSoundClipData> i in stm.data.soundClips.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Sound Clip", "STMSoundClips/New Sound Clip.asset", "STMSoundClipData", stm);
				}
			//Quads:
				stm.data.showQuadsFoldout = EditorGUILayout.Foldout(stm.data.showQuadsFoldout, "Quads", foldoutStyle);
				if(stm.data.showQuadsFoldout){
					EditorGUILayout.HelpBox("For information on how this works, please refer to the sample image under 'Quads' in the documentation.", MessageType.None, true);
				//Gather all data:
					foreach(KeyValuePair<string, STMQuadData> i in stm.data.quads.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Quad", "STMQuads/New Quad.asset", "STMQuadData", stm);
				}
			//Materials:
				stm.data.showMaterialsFoldout = EditorGUILayout.Foldout(stm.data.showMaterialsFoldout, "Materials", foldoutStyle);
				if(stm.data.showMaterialsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMMaterialData> i in stm.data.materials.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Material", "STMMaterials/New Material.asset", "STMMaterialData", stm);
				}

				EditorGUI.indentLevel--;
			}

			stm.data.showAutomaticFoldout = EditorGUILayout.Foldout(stm.data.showAutomaticFoldout, "Automatic", foldoutStyle);
			if(stm.data.showAutomaticFoldout){
				EditorGUI.indentLevel++;
			//AutoDelays:
				stm.data.showAutoDelaysFoldout = EditorGUILayout.Foldout(stm.data.showAutoDelaysFoldout, "AutoDelays", foldoutStyle);
				if(stm.data.showAutoDelaysFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMDelayData> i in stm.data.autoDelays.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Auto Delay", "STMAutoDelays/New Auto Delay.asset", "STMDelayData", stm);
				}
			//AutoClips:
				stm.data.showAutoClipsFoldout = EditorGUILayout.Foldout(stm.data.showAutoClipsFoldout, "AutoClips", foldoutStyle);
				if(stm.data.showAutoClipsFoldout){
				//Gather all data:
					//STMSoundClipData[] allAutoClips = Resources.LoadAll<STMSoundClipData>("STMAutoClips").OrderBy(x => -x.GetInstanceID()).ToArray(); //do this so order keeps consistent
					foreach(KeyValuePair<string, STMAutoClipData> i in stm.data.autoClips.OrderBy(x => -x.Value.GetInstanceID())){
						if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Auto Clip", "STMAutoClips/New Auto Clip.asset", "STMAutoClipData", stm);
				}
				EditorGUI.indentLevel--;
			}
			stm.data.showMasterFoldout = EditorGUILayout.Foldout(stm.data.showMasterFoldout, "Master", foldoutStyle);
			if(stm.data.showMasterFoldout){
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(dataDisableAnimatedText, true);
				EditorGUILayout.PropertyField(dataAutoVoice, true);
				EditorGUI.indentLevel--;
			}
			if(GUILayout.Button("Refresh Database")){
				stm.data = null;
			}

			serializedData.ApplyModifiedProperties();
		}else{ //draw actual text mesh inspector:
			
			EditorGUILayout.PropertyField(text);

			stm.showAppearanceFoldout = EditorGUILayout.Foldout(stm.showAppearanceFoldout, "Appearance", foldoutStyle);
			if(stm.showAppearanceFoldout){
				EditorGUILayout.PropertyField(font);
				EditorGUILayout.PropertyField(color); //richtext default stuff...
				EditorGUILayout.PropertyField(size);

				EditorGUI.BeginDisabledGroup(!stm.font.dynamic);
				EditorGUILayout.PropertyField(style);
				EditorGUI.EndDisabledGroup();

				EditorGUILayout.PropertyField(richText);

				EditorGUILayout.Space(); //////////////////SPACE

				EditorGUI.BeginDisabledGroup(!stm.font.dynamic);
				EditorGUILayout.PropertyField(quality); //text rendering
				EditorGUI.EndDisabledGroup();

				EditorGUILayout.PropertyField(filterMode);
				if(!stm.uiMode){
					UnityEngine.Rendering.ShadowCastingMode shadowMode = stm.r.shadowCastingMode;
					stm.r.shadowCastingMode = (UnityEngine.Rendering.ShadowCastingMode)EditorGUILayout.EnumPopup("Shadow Casting Mode", shadowMode);
				}
				//EditorGUILayout.BeginHorizontal();
				//if(GUILayout.Button("Ping")){
					//EditorUtility.FocusProjectWindow();
				//	EditorGUIUtility.PingObject(stm.textMat); //select this object
				//}
				EditorGUILayout.PropertyField(textMat); //appearance
				//EditorGUILayout.EndHorizontal();

				if(stm.textMat != null){
					if(stm.textMat.HasProperty("_Cutoff")){
						stm.textMat.SetFloat("_Cutoff",0.0001f);
					}
					stm.showMaterialFoldout = EditorGUILayout.Foldout(stm.showMaterialFoldout, "Material", foldoutStyle);
					if(stm.showMaterialFoldout){ //show shader settings
						STMCustomInspectorTools.DrawMaterialEditor(stm.textMat);
					}
				}
			}

			//EditorGUILayout.Space(); //////////////////SPACE
			stm.showPositionFoldout = EditorGUILayout.Foldout(stm.showPositionFoldout, "Position", foldoutStyle);
			if(stm.showPositionFoldout){
				EditorGUILayout.PropertyField(baseOffset); //physical stuff
				
				if(stm.uiMode){
					string[] anchorNames = new string[]{"Top", "Middle", "Bottom"};
					int[] anchorValues = new int[]{0,3,6};
					stm.anchor = (TextAnchor)EditorGUILayout.IntPopup("Anchor", (int)Mathf.Floor((float)stm.anchor / 3f) * 3, anchorNames, anchorValues);
				}else{
					EditorGUILayout.PropertyField(anchor);
				}
				//if(!uiMode.boolValue){ //restrict this to non-ui only...?
					EditorGUILayout.PropertyField(alignment);
				//}
				EditorGUILayout.Space(); //////////////////SPACE
				EditorGUILayout.PropertyField(lineSpacing); //text formatting
				EditorGUILayout.PropertyField(characterSpacing);
				EditorGUILayout.PropertyField(tabSize);
				EditorGUILayout.Space(); //////////////////SPACE
				if(!stm.uiMode){ //wrapping text works differently for UI:
					EditorGUILayout.PropertyField(autoWrap); //automatic...
					if(autoWrap.floatValue > 0f){
						EditorGUILayout.PropertyField(breakText);
						EditorGUILayout.PropertyField(insertHyphens);
					}
				}else{
					EditorGUILayout.PropertyField(wrapText);
					if(wrapText.boolValue){
						EditorGUILayout.PropertyField(breakText);
						EditorGUILayout.PropertyField(insertHyphens);
					}
				}
				EditorGUILayout.PropertyField(lineCountLimit);
				EditorGUILayout.Space(); //////////////////SPACE
				EditorGUILayout.PropertyField(modifyVertices);
				if(modifyVertices.boolValue){
					EditorGUILayout.PropertyField(vertexMod);
				}
			}
			//EditorGUILayout.Space(); //////////////////SPACE
			stm.showTimingFoldout = EditorGUILayout.Foldout(stm.showTimingFoldout, "Timing", foldoutStyle);
			if(stm.showTimingFoldout){
				EditorGUILayout.PropertyField(ignoreTimeScale);
				EditorGUILayout.PropertyField(disableAnimatedText);
				EditorGUILayout.Space(); //////////////////SPACE
				EditorGUILayout.PropertyField(readDelay); //technical stuff
				if(readDelay.floatValue > 0f){
					EditorGUILayout.PropertyField(autoRead);
					EditorGUILayout.PropertyField(rememberReadPosition);
					EditorGUILayout.PropertyField(drawOrder);
					EditorGUILayout.PropertyField(drawAnimName);
					//stuff that needs progamming to work:
					stm.showFunctionalityFoldout = EditorGUILayout.Foldout(stm.showFunctionalityFoldout, "Functionality", foldoutStyle);
					if(stm.showFunctionalityFoldout){
						EditorGUILayout.PropertyField(speedReadScale);
						EditorGUILayout.Space(); //////////////////SPACE
						EditorGUILayout.PropertyField(unreadDelay);
						EditorGUILayout.PropertyField(undrawOrder);
						EditorGUILayout.PropertyField(undrawAnimName);
					}
					//GUIContent drawAnimLabel = new GUIContent("Draw Animation", "What draw animation will be used. Can be customized with TextData.");
					//selectedAnim.intValue = EditorGUILayout.Popup("Draw Animation", selectedAnim.intValue, stm.DrawAnimStrings());
					stm.showAudioFoldout = EditorGUILayout.Foldout(stm.showAudioFoldout, "Audio", foldoutStyle);
					if(stm.showAudioFoldout){
						//EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel); //HEADER
						EditorGUILayout.PropertyField(audioSource);
						if(audioSource.objectReferenceValue != null){ //flag
							EditorGUILayout.PropertyField(audioClips, true); //yes, show children
							EditorGUILayout.PropertyField(stopPreviousSound);
							EditorGUILayout.PropertyField(pitchMode);
							if(pitchMode.enumValueIndex == (int)SuperTextMesh.PitchMode.Normal){
								//nothing!
							}
							else if(pitchMode.enumValueIndex == (int)SuperTextMesh.PitchMode.Single){
								EditorGUILayout.PropertyField(overridePitch);
							}
							else{ //random between two somethings
								EditorGUILayout.PropertyField(minPitch);
								EditorGUILayout.PropertyField(maxPitch);
							}
							if(pitchMode.enumValueIndex == (int)SuperTextMesh.PitchMode.Perlin){
								EditorGUILayout.PropertyField(perlinPitchMulti);
							}
							if(speedReadScale.floatValue < 1000f){
								EditorGUILayout.PropertyField(speedReadPitch);
							}
						}
					}
					stm.showEventFoldout = EditorGUILayout.Foldout(stm.showEventFoldout, "Events", foldoutStyle);
					if(stm.showEventFoldout){
						EditorGUILayout.PropertyField(onRebuildEvent);
						EditorGUILayout.PropertyField(onCompleteEvent);
						EditorGUILayout.PropertyField(onUndrawnEvent);
						EditorGUILayout.PropertyField(customEvent);
					}
				}
			}
			//EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.PropertyField(debugMode);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
#endif

[HelpURL("Assets/Clavian/SuperTextMesh/Documentation/SuperTextMesh.html")] //make the help open local documentation
[AddComponentMenu("Mesh/Super Text Mesh", 3)] //allow it to be added as a component
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class SuperTextMesh : MonoBehaviour { //MaskableGraphic... rip
	//foldout bools for editor. not on the GUI script, cause they get forgotten
	public bool showAppearanceFoldout = true;
	public bool showMaterialFoldout = true;
	public bool showPositionFoldout = true;
	public bool showTimingFoldout = false;
	public bool showFunctionalityFoldout = false;
	public bool showAudioFoldout = false;
	public bool showEventFoldout = false;
	
	#if UNITY_EDITOR
	//Add to the gameobject menu:
	[MenuItem("GameObject/3D Object/Super 3D Text", false, 4000)] //instantiate a prefab of this
	private static void MakeNewText(MenuCommand menuCommand){
	    //Create a game object
	    GameObject textFab = Resources.Load("STMPrefabs/New Super Text") as GameObject;
	    GameObject newTextMesh = Instantiate(textFab); //instantiate prefab from assets
	    newTextMesh.transform.name = textFab.name; //so it doesn't have "(Clone)" after
		GameObjectUtility.SetParentAndAlign(newTextMesh, menuCommand.context as GameObject); //Ensure it gets reparented if this was a context click (otherwise does nothing)
		Undo.RegisterCreatedObjectUndo(newTextMesh, "Create " + newTextMesh.name); //Register the creation in the undo system
		Selection.activeObject = newTextMesh;
	}
	[MenuItem("GameObject/UI/Super Text", false, 2001)] //instantiate a prefab of this
	private static void MakeNewUIText(MenuCommand menuCommand){
	    //Create a game object
	   GameObject textFab = Resources.Load("STMPrefabs/Super Text") as GameObject;
	    GameObject newTextMesh = Instantiate(textFab); //instantiate prefab from assets
	    newTextMesh.transform.name = textFab.name; //so it doesn't have "(Clone)" after
		GameObjectUtility.SetParentAndAlign(newTextMesh, menuCommand.context as GameObject); //Ensure it gets reparented if this was a context click (otherwise does nothing)
		Undo.RegisterCreatedObjectUndo(newTextMesh, "Create " + newTextMesh.name); //Register the creation in the undo system
		Selection.activeObject = newTextMesh;
		//force-attach to canvas if it exists, or auto-create new one.
		Canvas myCanvas = (Canvas)FindObjectOfType(typeof(Canvas)); //find a canvas in the scene
		if(myCanvas == null){ //create a new canvas to parent to!
			GameObject newObject = new GameObject();
			newObject.transform.name = "Canvas";
			myCanvas = newObject.AddComponent<Canvas>();
			myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			newObject.AddComponent<CanvasScaler>();
			newObject.AddComponent<GraphicRaycaster>();
			Undo.RegisterCreatedObjectUndo(newObject, "Created New Canvas for UI Super Text");
		}
		newTextMesh.transform.SetParent(myCanvas.transform);
	}
	#endif
	
	private SuperTextMeshData _data; 
	public SuperTextMeshData data{
		get{
			if(_data == null){
				_data = Resources.Load("SuperTextMeshData") as SuperTextMeshData; //load textdata
				_data.RebuildDictionaries(); //rebuild dictionaries
			}
			return _data;
		}set{
			_data = value; //for setting it to be null so this gets redone!
		}
	}
	private Transform _t;
	public Transform t{
		get{
			if(_t == null) _t = this.transform;
			return _t;
		}
	}
	private MeshFilter _f;
	public MeshFilter f{
		get{
			if(_f == null) _f = t.GetComponent<MeshFilter>();
			if(_f == null) _f = t.gameObject.AddComponent<MeshFilter>();
			return _f;
		}
	}
	private MeshRenderer _r;
	public MeshRenderer r{
		get{
			if(_r == null) _r = t.GetComponent<MeshRenderer>();
			if(_r == null) _r = t.gameObject.AddComponent<MeshRenderer>();
			return _r;
		}
	}
	private CanvasRenderer _c;
	public CanvasRenderer c{
		get{
			if(_c == null) _c = t.GetComponent<CanvasRenderer>();
			if(_c == null) _c = t.gameObject.AddComponent<CanvasRenderer>();
			return _c;
		}
	}
	public bool uiMode; //is it in UI mode? please don't change this manually

	private List<STMTextInfo> info = new List<STMTextInfo>(); //switching this out for an array & using temp lists makes less appear in the deep profiler, but has no effect on GC
	private List<int> lineBreaks = new List<int>(); //what characters are line breaks
	[TextArea(3,10)] //[Multiline] also works, but i like this better
	[UnityEngine.Serialization.FormerlySerializedAs("text")]
	public string _text = "<c=rainbow><w>Hello, World!";
	public string text{
		get{
			return this._text;
		}
		set{
			this._text = value;
			Rebuild(); //auto-rebuild
		}
	}
	public string Text{ //legacy fix since v1.6
		get{ //just do the same as text
			return text;
		}
		set{
			text = value;
		}
	}
	[HideInInspector] public string drawText; //text, after removing junk
	[HideInInspector] public string hyphenedText; //text, with junk added to it
	[Tooltip("Font to be used by this text mesh. .rtf, .otf, and Unity fonts are supported.")]
	public Font font;
	[Tooltip("Default color of the text mesh. This can be changed with the <c> tag! See the docs for more info.")]
	public Color32 color = Color.white;
	[Tooltip("Will the text listen to tags like <b> and <i>? See docs for a full list of tags.")]
	public bool richText = true; //care about formatting like <b>?
	[Tooltip("Delay in seconds between letters getting read out. Disabled if set to 0.")]
	public float readDelay = 0f; //disabled if 0.
	
	[Tooltip("Multiple of time for when speeding up text. Set it to a big number like 1000 to show all text immediately.")]
	public float speedReadScale = 2f; //for speeding thru text, this will be the delay. set to 0 to display instantly.
	[Tooltip("Whether reading uses deltaTime or fixedDeltaTime")]
	public bool ignoreTimeScale = true;
	public float GetDeltaTime{
		get{
			return data.disableAnimatedText || disableAnimatedText ? 0f : ignoreTimeScale && !applicationFocused ? Time.unscaledDeltaTime : Time.deltaTime;
		}
	}
	public float GetTime{
		get{
			return data.disableAnimatedText || disableAnimatedText ? 0f : ignoreTimeScale && !applicationFocused ? Time.unscaledTime : Time.time;
		}
	}
	public float GetDeltaTime2{//for when the text is getting read out
		get{
			return ignoreTimeScale && !applicationFocused ? Time.unscaledDeltaTime : Time.deltaTime;
		}
	}
	public bool disableAnimatedText = false; //disable for just this mesh
	/*
	public float GetTime2{ 
		get{
			return ignoreTimeScale ? Time.unscaledTime : Time.time;
		}
	}*/

	//public int selectedAnim = 0; //what draw animation is selected currently.....
	[Tooltip("Name of what draw animation will be used. Case-sensitive.")]
	public string drawAnimName = "Appear"; //this is a string instead of a custom dropdown so reordering saved animations can't change it

	[Tooltip("Delay between letters, for undrawing.")]
	public float unreadDelay = 0.05f;
	[Tooltip("Undraw order.")]
	public DrawOrder undrawOrder = DrawOrder.AllAtOnce;
	[Tooltip("Undraw animation name.")]
	public string undrawAnimName = "Appear";

	[Tooltip("Audio source for read sound clips. Sound won't be played if null.")]
	public AudioSource audioSource;
	[Tooltip("Default sound to be read by the above audio source. Can be left null to make no sound by default.")]
	public AudioClip[] audioClips;
	[Tooltip("Should a new letter's sound stop a previous one and play, or let the old one keep playing?")]
	public bool stopPreviousSound = true;

	[Tooltip("Pitch options for reading out text.")]
	public PitchMode pitchMode = PitchMode.Normal;
	public enum PitchMode{
		Normal,
		Single,
		Random,
		Perlin
	}
	[Tooltip("New pitch for the sound clip.")]
	[Range(0f,3f)]
	public float overridePitch = 1f;
	[Tooltip("Minimum pitch for random pitches. If same or greater than max pitch, this will be the pitch.")]
	[Range(0f,3f)]
	public float minPitch = 0.9f;
	[Tooltip("Maximum pitch for random pitches.")]
	[Range(0f,3f)]
	public float maxPitch = 1.2f;
	[Range(-2f,2f)]
	[Tooltip("This amount will be ADDED to the pitch when speedreading. Speedreading uses the delay from 'Fast Delay'")]
	public float speedReadPitch = 0f;
	[Tooltip("Multiple for how fast the perlin noise will advance.")]
	public float perlinPitchMulti = 1.0f;
	private bool speedReading = false;
	private bool skippingToEnd = false; //alt version of speedread that just skips to the end

	[HideInInspector] public bool reading = false; //is text currently being read out? this is public so it can be used by other scripts!
	private Coroutine readRoutine; //coroutine that handles reading out text
	[HideInInspector] public bool unreading = false;

	[Tooltip("Size in local space for letters, by default. Can be changed with the <s> tag.")]
	public float size = 1f; //size of letter in local space! not percentage of quality. letters can have diff sizes individually
	[Range(1,500)]
	[Tooltip("Point size of text. Try to keep it as small as possible while looking crisp!")]
	public int quality = 64; //actual text size. point size
	[Tooltip("Choose 'Point' for a crisp look. You'll probably want that for pixel fonts!")]
	public FilterMode filterMode = FilterMode.Bilinear;
	//TODO: completely redraw text texture upon quality change. 2016-06-07 note: might have already done this
	[Tooltip("Default letter style. Can be changed with the <i> and <b> tags, using rich text.")]
	public FontStyle style = FontStyle.Normal;
	[Tooltip("Additional offset for the text mesh from the transform, in local space.")]
	public Vector3 baseOffset = Vector3.zero; //for offsetting z, mainly
	[Tooltip("Adjust line spacing between multiple lines of text. 1 is the default for the font.")]
	public float lineSpacing = 1.0f;
	[Tooltip("Adjust additional spacing between characters. 0 is default.")]
	public float characterSpacing = 0.0f;
	[Tooltip("How far tabs indent.")]
	public float tabSize = 4.0f;
	[Tooltip("Distance in local space before a line break is automatically inserted at the previous space. Disabled if set to 0.")]
	public float autoWrap = 12f; //if text on one row exceeds this, insert line break at previously available space
	private float AutoWrap{ //get autowrap limit OR ui bounds
		get{
			if(uiMode && wrapText) return (float)((RectTransform)t).rect.width; //get wrap limit, within left and right bounds!
			else if(uiMode && !wrapText) return 0f;
			return autoWrap;
		}
	}

	[Tooltip("Should text wrap at the edge of the bounding box, or go over?")]
	public bool wrapText = true; 
	[Tooltip("With auto wrap, should large words be split to fit in the box?")]
	public bool breakText = true;
	[Tooltip("When large words are split, Should a hyphen be inserted?")]
	public bool insertHyphens = true;
	[Tooltip("The anchor point of the mesh. For UI text, this also controls the alignment.")]
	public TextAnchor anchor = TextAnchor.UpperLeft;
	[Tooltip("Decides where text should align to. Uses the Auto Wrap box as bounds.")]
	public Alignment alignment = Alignment.Left;
	public enum Alignment{
		Left,
		Center,
		Right,
		Justified,
		ForceJustified
	}
	[Tooltip("Sets a limit for how many lines of text can be shown, total. Good for dialogue boxes or high score lists. Disabled when set to 0.")]
	public int lineCountLimit = 0;
	[Tooltip("The material to be used by this text mesh. Look under 'STM' in the shader menu for more compatible shaders.")]
	public Material textMat; //material to use on all submeshes or whatever by default. will always be textMats[0]
	public Mesh textMesh; //keep track of mesh

	private bool areWeAnimating = false; //do we need to update every frame?

	[HideInInspector] public Vector3 rawTopLeftBounds;
	[HideInInspector] public Vector3 rawBottomRightBounds;

	[HideInInspector] public Vector3 topLeftBounds;
	[HideInInspector] public Vector3 topRightBounds;
	[HideInInspector] public Vector3 bottomLeftBounds;
	[HideInInspector] public Vector3 bottomRightBounds;
	[HideInInspector] public Vector3 centerBounds;

	public UnityEvent onCompleteEvent; //when the mesh is done drawing
	public UnityEvent onUndrawnEvent; //for when undrawing finishes
	public UnityEvent onRebuildEvent; //when rebuild() is called

	[System.Serializable] public class CustomEvent : UnityEvent<string, STMTextInfo>{} //tag, textinfo, 
	public CustomEvent customEvent;
	public bool modifyVertices = false;
	[System.Serializable] public class VertexMod : UnityEvent<Vector3[], Vector3[], Vector3[]>{}
	public VertexMod vertexMod;

	public bool debugMode = false; //pretty much just here to un-hide inspector stuff

	[HideInInspector] public float totalReadTime = 0f;
	[HideInInspector] public float totalUnreadTime = 0f;
	[HideInInspector] public float currentReadTime = 0f; //what position in the mesh it's currently at. Right now, this is just so jitters don't animate more than they should when you speed past em.

	//generate these with ur vert calls or w/e!!!
	private Vector3[] endVerts = new Vector3[0];
	private Color32[] endCol32 = new Color32[0];
	//private int[] endTriangles = new int[0];
	private Vector2[] endUv = new Vector2[0];
	private Vector2[] endUv2 = new Vector2[0]; //overlay images
	private Vector3[] startVerts = new Vector3[0];
	private Color32[] startCol32 = new Color32[0];
	private Vector3[] midVerts = new Vector3[0];
	private Color32[] midCol32 = new Color32[0];

	private List<SubMeshData> subMeshes = new List<SubMeshData>();

	private float timeDrawn; //Time.time when the mesh was drawn. or Time.unscaledTime, depending

	[Tooltip("Decides if the mesh will read out automatically when rebuilt.")]
	public bool autoRead = true;
	[Tooltip("Decides if the mesh will remember where it was if disabled/enabled while reading.")]
	public bool rememberReadPosition = true;

	public enum DrawOrder{
		LeftToRight,
		AllAtOnce,
		OneWordAtATime,
		Random,
		RightToLeft,
		ReverseLTR
	}
	[Tooltip("What order the text will draw in. 'All At Once' will ignore read delay. 'Robot' displays one word at a time. If set to 'Random', Read Delay becomes the time it'll take to draw the whole mesh.")]
	public DrawOrder drawOrder = DrawOrder.LeftToRight;

	private bool callReadFunction = false; //will the read function need to be called?

	private int pauseCount = 0; //for <pause>. total amount of <pause>s that were reached while reading multiple times
	private int currentPauseCount = 0; //amount of pauses found this rebuild cycle

	STMDrawAnimData UndrawAnim{
		get{
			if(data.drawAnims.ContainsKey(undrawAnimName)){
				return data.drawAnims[undrawAnimName];
			}else if(data.drawAnims.ContainsKey("Appear")){
				return data.drawAnims["Appear"];
			}else{
				//Debug.Log("'Appear' draw animation isn't defined!"); //sometimes this'll get called on awake... oH
				data = null;
				return null;
			}
		}
	}
/*
	public string[] DrawAnimStrings(){ //get strings for the dropdown thing
		string[] myStrings = new string[data.drawAnims.Count];
		for(int i=0, iL=myStrings.Length; i<iL; i++){
			myStrings[i] = data.drawAnims[i].name;
		}
		if(selectedAnim >= myStrings.Length){
			selectedAnim = 0; //don't go over if one gets deleted
		}
		return myStrings;
	}
*/
	private bool applicationFocused = true;
	void OnApplicationFocus(bool focused){
		//this is needed cause without it, if mesh is ignoring time scale and application comes back into focus, time will skip forward.
		if(!Application.runInBackground){ //only care if the application cares
			applicationFocused = focused;
		}
	}
	void OnDrawGizmosSelected(){ //draw boundsssss
		//if(autoWrap > 0f){ //bother to draw bounds?
			Gizmos.color = Color.blue;
			RecalculateBounds();
			Gizmos.DrawLine(topLeftBounds, topRightBounds); //top
			Gizmos.DrawLine(topLeftBounds, bottomLeftBounds); //left
			Gizmos.DrawLine(topRightBounds, bottomRightBounds); //right
			Gizmos.DrawLine(bottomLeftBounds, bottomRightBounds); //bottom
			//Gizmos.DrawSphere(centerBounds, 0.1f);
		//}
	}
	void OnFontTextureRebuilt(Font changedFont){
        Rebuild(currentReadTime, currentReadTime > 0f ? true : autoRead); //the font texture attached to this mesh has changed. a rebuild is neccesary.
    }
	#if UNITY_5_4_OR_NEWER
	void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){
		if(this != null){ //stupid
			Rebuild(autoRead); //otherwise, texture goes missing. miiiight not be neccesary in 5.4
		}
	}
	#else
	void OnLevelWasLoaded(int level){
		Rebuild(autoRead); //otherwise, texture goes missing. 
	}
	#endif
	
	void OnEnable(){
		Init();
		if(Application.isPlaying && Time.time > 0f){ //cause autoread to work
			if(callReadFunction && rememberReadPosition){ //remember read position?
				if(currentReadTime == 0f){ //same as Start()
					Rebuild(autoRead);
				}else if(currentReadTime >= totalReadTime){ //it finished reading?
					SetMesh(currentReadTime); //skip to end w/o events
				}else{ //it was halfway thru
					Rebuild(currentReadTime, autoRead);
				}
			}else{ //act like Start()
				Rebuild(autoRead);
			}
		}
	}
	void Start(){
		textMesh = null; //THIS IS NEEDED TO PREVENT THE DUPLICATION GLITCH
		Init(); 
		Rebuild(autoRead);
	}
	void OnDisable(){
		//Debug.Log("Disabled!");
		UnInit();
		if(uiMode){
			DestroyImmediate(textMesh);
			c.Clear();
		}else{
			DestroyImmediate(f.sharedMesh);
		}
	}
	void OnDestroy(){
		UnInit();
	}
	void Init(){
		//uiMode = t is RectTransform;

		#if UNITY_5_4_OR_NEWER
		SceneManager.sceneLoaded += OnSceneLoaded; //hopefully not an issue if called multiple times?
		#endif
		Font.textureRebuilt += OnFontTextureRebuilt;
	}
	void UnInit(){
		#if UNITY_5_4_OR_NEWER
		SceneManager.sceneLoaded -= OnSceneLoaded;
		#endif
		Font.textureRebuilt -= OnFontTextureRebuilt;
		StopReadRoutine();
	}
	void StopReadRoutine(){
		if(readRoutine != null){
			StopCoroutine(readRoutine); //stop routine, just in case
			reading = false;
		}
	}
	/*
	private void AddRebuildCall(){
		if(Font.textureRebuilt == null || !Font.textureRebuilt.GetInvocationList().Contains(OnFontTextureRebuilt)){
			Font.textureRebuilt += OnFontTextureRebuilt;
		}
	}
	private void RemoveRebuildCall(){
		Font.textureRebuilt -= OnFontTextureRebuilt;
	}
	*/
	#if UNITY_EDITOR //just for updating when the editor changes
	private bool validateAppearance = false;
	#endif
	void OnValidate() {
		if(!font.dynamic){
			if(font.fontSize > 0){
				quality = font.fontSize;
			}else{
				Debug.Log("You're probably using a custom font! \n Unity's got a bug where custom fonts have their size set to 0 by default and there's no way to change that! So to avoid this error, here's a solution: \n * Drag any font into Unity. Set it to be 'Unicode' or 'ASCII' in the inspector, depending on the characters you want your font to have. \n * Set 'Font Size' to whatever size you want 'quality' to be locked at. \n * Click the gear in the corner of the inspector and 'Create Editable Copy'. \n * Now, under the array of 'Character Rects', change size to 0 to clear everything. \n * Now you have a brand new font to edit that has a font size that's not zero! Yeah!");
			}
			//quality = UseThisFont.fontSize == 0 ? 64 : UseThisFont.fontSize; //for getting around fonts with a default size of 0.
			//Debug.Log("Font size is..." + UseThisFont.fontSize);
			style = FontStyle.Normal;
		}
		if(size < 0f){size = 0f;}
		if(readDelay < 0f){readDelay = 0f;}
		if(autoWrap < 0f){autoWrap = 0f;}
		if(lineCountLimit < 0){lineCountLimit = 0;}
		if(minPitch > maxPitch){minPitch = maxPitch;}
		if(maxPitch < minPitch){maxPitch = minPitch;}
		if(speedReadScale < 0.01f){speedReadScale = 0.01f;}
		#if UNITY_EDITOR
		validateAppearance = true;
		#endif
	}
	#if UNITY_EDITOR
	public void HideInspectorStuff(){
		HideFlags flag = HideFlags.HideInInspector;
		switch(debugMode){
			case true: flag = HideFlags.None; break;//don't hide!
		}
		if(uiMode){
			for(int i=0, iL=c.materialCount; i<iL; i++){
				if(c.GetMaterial(i) != null){
					c.GetMaterial(i).hideFlags = flag;
				}
			}
			c.hideFlags = flag;
		}else{
			for(int i=0, iL=r.sharedMaterials.Length; i<iL; i++){ //hide shared materials
				if(r.sharedMaterials[i] != null){
					r.sharedMaterials[i].hideFlags = flag;
				}
			}
			r.hideFlags = flag; //hide mesh renderer and filter.
			f.hideFlags = flag;
		}
	}
	#endif
	public static void RebuildAll(){ //this uses FindObjectsOfType and is very intensive! Only use when loading.
		SuperTextMesh[] allSTMs = FindObjectsOfType<SuperTextMesh>();
		for(int i=0, iL=allSTMs.Length; i<iL; i++){
			allSTMs[i].Rebuild();
		}
	}
	public void Rebuild(){
		Rebuild(0f, true);
	}
	public void Rebuild(bool readAutomatically){
		Rebuild(0f, readAutomatically);
	}
	public void Rebuild(float startTime){
		Rebuild(startTime, true);
	}
	public void Rebuild(float startTime, bool readAutomatically){
		if(onRebuildEvent != null) onRebuildEvent.Invoke();
		if(startTime < totalReadTime) pauseCount = 0; //only reset if reeading from the very start, not appending
		currentPauseCount = 0;
		currentReadTime = 0f;
		totalReadTime = 0f;
		timeDrawn = GetTime; //remember what time it started!
		reading = false;
		unreading = false; //reset this, incase it was fading out
		speedReading = false; //2016-06-09 thank u drak
		skippingToEnd = false;
		uiMode = t is RectTransform;
		//Initialize:
		if(uiMode){ //UI mode
				if(font == null && textMat == null){
					size = 32;
					color = new Color32(50,50,50,255);
				}
				if(textMat == null){
					textMat = Resources.Load<Material>("DefaultSTMMaterials/UIDefault");
				}
				if(font == null){
					font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				}
		}else{
			if(font == null){
				font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			}
			if(textMat == null){
				textMat = Resources.Load<Material>("DefaultSTMMaterials/Default");
			}
		}
		RebuildTextInfo();
		if(audioSource != null){//initialize an audio source, if there's one. these settings are needed to behave properly
			audioSource.loop = false;
			audioSource.playOnAwake = false;
		}
		if(callReadFunction && Application.isPlaying){
			if(readAutomatically){
				//Debug.Log("Autoreading");
				Read(startTime);
			}else{
				StopReadRoutine();
				//Debug.Log("Not autoreading");
				SetMesh(0f); //show nothing
			}
		}else{
			//Debug.Log("No need to read");
			StopReadRoutine();
			//SetMesh(-1f); //skip to end
			ShowAllText();
		}
		ApplyMaterials();
	}
	void Update() {
		#if UNITY_EDITOR
        if(!Application.isPlaying){ //do same thing as onvalidate
            Rebuild(autoRead); //doing it this way avoids the material getting lost when duplicating
        }
		if(validateAppearance && t.gameObject.activeInHierarchy == true && Application.isPlaying && Time.time > 0f){ 
			Rebuild(autoRead);
			validateAppearance = false;
		}
		#endif
 
		if(font != null && textMat != null && textMesh != null){ //TODO: make this only get called if something changed, or it's animating
			//RequestAllCharacters(); //keep characters on font texture 
			//v1.4.2: not sure if neccesary, thanks to OnFontTextureRebuilt()?
			//but I'm not sure. it does seem to use a bit of CPU for more meshes, though
			if(!reading && areWeAnimating && currentReadTime >= totalReadTime){
				currentReadTime += GetDeltaTime; //keep updating this, for the jitters
			}
			if(!reading && !unreading && areWeAnimating && (readDelay == 0f || currentReadTime >= totalReadTime)){ //if the mesh needs to keep updating after it's been read out
				//if(currentReadTime >= totalReadTime){ //as long as it's set to auto read, or the current read time is above total read time
					//Debug.Log(currentReadTime + "/" + totalReadTime);
					SetMesh(-1f);
				//}
			}
		}
	}
	void UpdatePreReadMesh(bool undrawingMesh){ //update data needed for pre-existing mesh
		UpdateMesh();

		startCol32 = new Color32[hyphenedText.Length * 4];
		startVerts = new Vector3[hyphenedText.Length * 4];

		STMDrawAnimData myUndrawAnim = UndrawAnim; //just in case...
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			STMDrawAnimData myAnimData = undrawingMesh ? myUndrawAnim : info[i].drawAnimData; //which animation data to use?

			if(info[i].drawAnimData.startColor != Color.clear){ //use a specific start color
				startCol32[4*i + 0] = myAnimData.startColor;
				startCol32[4*i + 1] = myAnimData.startColor;
				startCol32[4*i + 2] = myAnimData.startColor;
				startCol32[4*i + 3] = myAnimData.startColor;
			}else{ //same color but transparent, for better lerping
				startCol32[4*i + 0] = new Color32(endCol32[4*i + 0].r,endCol32[4*i + 0].g,endCol32[4*i + 0].b,0);
				startCol32[4*i + 1] = new Color32(endCol32[4*i + 1].r,endCol32[4*i + 1].g,endCol32[4*i + 1].b,0);
				startCol32[4*i + 2] = new Color32(endCol32[4*i + 2].r,endCol32[4*i + 2].g,endCol32[4*i + 2].b,0);
				startCol32[4*i + 3] = new Color32(endCol32[4*i + 3].r,endCol32[4*i + 3].g,endCol32[4*i + 3].b,0);
			}
			Vector3 middle = new Vector3((endVerts[4*i + 0].x + endVerts[4*i + 1].x + endVerts[4*i + 2].x + endVerts[4*i + 3].x) * 0.25f,
														(endVerts[4*i + 0].y + endVerts[4*i + 1].y + endVerts[4*i + 2].y + endVerts[4*i + 3].y) * 0.25f,
														(endVerts[4*i + 0].z + endVerts[4*i + 1].z + endVerts[4*i + 2].z + endVerts[4*i + 3].z) * 0.25f);
			
			startVerts[4*i + 0] = Vector3.Scale((endVerts[4*i + 0] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * info[i].size);
			startVerts[4*i + 1] = Vector3.Scale((endVerts[4*i + 1] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * info[i].size);
			startVerts[4*i + 2] = Vector3.Scale((endVerts[4*i + 2] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * info[i].size);
			startVerts[4*i + 3] = Vector3.Scale((endVerts[4*i + 3] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * info[i].size);
		}
	}
	public void Read(){
		Read(0f);
	}
	public void Read(float startTime){
		StopReadRoutine();
		readRoutine = StartCoroutine(ReadOutText(startTime));
		//now we have a mesh with nothing on it!
	}
	//cause I keep accidentally typing this? I think this name is better, might swap this in the future
	public void Unread(){UnRead();}
	public void Undraw(){UnRead();}
	public void UnDraw(){UnRead();}
	public void UnRead(){
		//Mesh finalMesh = ShowAllText(); //this is working
		readRoutine = StartCoroutine(UnReadOutText());
	}
	public void SpeedRead(){
		if(reading){
			speedReading = true;
		}
	}
	public void SkipToEnd(){
		if(reading){
			skippingToEnd = true;
		}
	}
	public void RegularRead(){ //return to normal reading speed
		speedReading = false;
	}
	public void ShowAllText(){
		ShowAllText(false); //actually show all text
	}
	private void ShowAllText(bool unreadingMesh){
		speedReading = false;
		lowestLine = lineBreaks.Count-1;
		StopReadRoutine();
		SetMesh(totalReadTime + 1.0f, unreadingMesh);
		//Invoke complete events:
		if(!unreadingMesh){
			reading = false;
			onCompleteEvent.Invoke();
		}else{
			//unreading = false; //nope! Gotta stay in this state til it gets drawn again
			onUndrawnEvent.Invoke();
		}
	}
	public void Append(string newText){
		_text += newText;
		Rebuild(totalReadTime, autoRead); //start reading from this point
	}
	public bool Continue(){ //continue reading after being paused. returns true if more text needs to be read
		//goes one extra so that true/false is returned properly
		if(currentPauseCount >= pauseCount){ //still need to continue?
			pauseCount++;
			Rebuild(totalReadTime, autoRead);
			return true;
		}
		return false;
	}

	//int lastNum = -1; //the last index to be invoked on the previous cycle, so sounds can't play twice for the same letter!
	//List<int> alreadyInvoked = new List<int>(); //list on indexes that have already been invoked so events cant happen twice
	public int latestNumber = -1; //declare here as a public variable, so the current character drawn can be reached at any time
	public int lowestLine = 0; //lowest line being shown currently (number counts up)
	void UpdateDrawnMesh(float myTime, bool undrawingMesh){
		UpdateMesh();
		UpdatePreReadMesh(undrawingMesh);
		//TODO: ^^^ all this stuff, you only have to call again if areweanimating is true.

		STMDrawAnimData myUndrawAnim = UndrawAnim; //get the undraw animation, locally
		//get modified drawnMesh!
		midVerts = new Vector3[hyphenedText.Length * 4];
		midCol32 = new Color32[hyphenedText.Length * 4];

		bool playedSoundThisFrame = false;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //for each point
			//lerp between start and end
			//Debug.Log((myTime - info[i].readTime) / info[i].animTime);
			STMDrawAnimData myAnimData = undrawingMesh ? myUndrawAnim : info[i].drawAnimData; //which anim to use
			float myReadTime = undrawingMesh ? info[i].unreadTime : info[i].readTime; //what timing to use
			//animate properly! (is there a way to do this by manipulating anim time?? yeah probably tbh)
			float divideAnimAmount = myAnimData.animTime == 0f ? 0.0000001f : myAnimData.animTime; //so it doesn't get NaN'd
			float divideFadeAmount = myAnimData.fadeTime == 0f ? 0.0000001f : myAnimData.fadeTime; //how long fading will last...
			float myAnimPos = (myTime - myReadTime) / divideAnimAmount; // on a range between 0-1 on the curve, the position of the animation
			float myFadePos = (myTime - myReadTime) / divideFadeAmount;

			if(undrawingMesh){ //flip the range! so it lerps backwards
				myAnimPos = 1f - myAnimPos;
				myFadePos = 1f - myFadePos;
			}
			if(myAnimPos > 0f && !undrawingMesh && i > latestNumber){ 
				DoEvent(i); //do every event up to this integer
				if(!playedSoundThisFrame){
					PlaySound(i); //only play one sound this frame, from the first letter drawn this frame
				}
				latestNumber = Mathf.Max(latestNumber, i); //find latest number to start from next frame
				lowestLine = Mathf.Max(lowestLine, info[i].line); //technically highest line
			}
			/*
			It'd be more efficient to update only vertices that need to be updated.
			However... this isn't done, because it looks bad when a mesh gets laggier as it goes on.
			*/
			midVerts[4*i+0] = Vector3.Lerp(startVerts[4*i+0],endVerts[4*i+0],myAnimData.animCurve.Evaluate(myAnimPos));
			midVerts[4*i+1] = Vector3.Lerp(startVerts[4*i+1],endVerts[4*i+1],myAnimData.animCurve.Evaluate(myAnimPos));
			midVerts[4*i+2] = Vector3.Lerp(startVerts[4*i+2],endVerts[4*i+2],myAnimData.animCurve.Evaluate(myAnimPos));
			midVerts[4*i+3] = Vector3.Lerp(startVerts[4*i+3],endVerts[4*i+3],myAnimData.animCurve.Evaluate(myAnimPos));

			midCol32[4*i+0] = Color.Lerp(startCol32[4*i+0],endCol32[4*i+0],myAnimData.fadeCurve.Evaluate(myFadePos));
			midCol32[4*i+1] = Color.Lerp(startCol32[4*i+1],endCol32[4*i+1],myAnimData.fadeCurve.Evaluate(myFadePos));
			midCol32[4*i+2] = Color.Lerp(startCol32[4*i+2],endCol32[4*i+2],myAnimData.fadeCurve.Evaluate(myFadePos));
			midCol32[4*i+3] = Color.Lerp(startCol32[4*i+3],endCol32[4*i+3],myAnimData.fadeCurve.Evaluate(myFadePos));
		}
		//ShouldPlaySound(undrawingMesh);
	}
	/*
	void ShouldPlaySound(bool undrawingMesh){
		//int alreadyInvokedCount = alreadyInvoked.Count;
		if(!undrawingMesh){ //dont play sounds if undrawing...
			if(latestNumber != lastNum){ //new number?
				PlaySound(latestNumber); //only play one sound, from the most recent number
				lastNum = latestNumber;
			}
		}else{
			lastNum = -1;
		}
	}
	*/
	bool AreColorsTheSame(Color32 col1, Color32 col2){
		if(col1.r == col2.r && col1.g == col2.g && col1.b == col2.b && col1.a == col2.a){
			return true;
		}
		return false;
	}
	IEnumerator ReadOutText(float startTime){
		//Lerp certain vertices betwwen startmesh and endmesh
		//like, the mesh made by CreatePreReadMesh() and CreateMesh().
		reading = true;
		float timer = startTime;
		currentReadTime = startTime;
		if(startTime.Equals(0f)){ //for append()
//			alreadyInvoked.Clear();
			//lastNum = -1;
			latestNumber = 0;
			lowestLine = 0;
			DoEvent(0); //do 1st event
			PlaySound(0); //play 1st sound
		}else if(startTime.Equals(-1f)){
			DoEvent(0);	//so 1st event doesn't get skipped
		}
		//Debug.Log("Total read time is: " + totalReadTime);
		while(timer < totalReadTime){ //check for null incase the mesh gets deleted midway
			//Debug.Log("Doing this while loop!" + timer);
			if(skippingToEnd){
				timeDrawn -= totalReadTime; //why not (solves jitters not catching up)
				timer = totalReadTime;
			}
			SetMesh(timer);
			
			float delta = GetDeltaTime2;
			delta *= speedReading ? speedReadScale : 1f;
			timer += delta;
			currentReadTime = timer; //I could just use this as the timer, but w/e
			yield return null;
		}
		if(latestNumber != hyphenedText.Length-1){ //catch missed events
			PlaySound(hyphenedText.Length-1); //play final sound! Yep, this seems to fix that 2016-10-13
			DoEvent(hyphenedText.Length-1); //2016-11-02 but only if it hasn't been played yet
		}
		if(info.Count > hyphenedText.Length){
			DoEvent(info.Count-1); //do tacked-on event, 
		}
		currentReadTime = totalReadTime;
		ShowAllText(); //just in case!
	}
	IEnumerator UnReadOutText(){
		unreading = true;
		float timer = 0f; //always start at 0
		while(timer < totalUnreadTime){ //check for null incase the mesh gets deleted midway
			SetMesh(timer, true);
			timer += GetDeltaTime2;
			yield return null;
		}
		ShowAllText(true);
	}
	/*
	bool IsThisLetterAnimating(int i){ //return true if this letter is animating in some way, not related to drawanim
		if(info[i].waveData != null || info[i].jitterData != null &&
			(info[i].gradientData != null && info[i].gradientData.scrollSpeed != 0) ||
			(info[i].textureData != null && info[i].textureData.scrollSpeed != Vector2.zero)){
			return true;
		}
		return false;
	}
	*/
	void DoEvent(int i){
		if(info[i].ev.Count > 0){ //invoke events...
			for(int j=0, jL=info[i].ev.Count; j<jL; j++){
				customEvent.Invoke(info[i].ev[j], info[i]); //call the event!
			}
			info[i].ev.Clear(); //since you only want events to be invoked once, I don't see the harm in clearing them this way instead of keeping track
		}
		if(info[i].ev2.Count > 0){ //end repeating events!
			for(int j=0, jL=info[i].ev2.Count; j<jL; j++){
				customEvent.Invoke(info[i].ev2[j], info[i]); //call the event!
			}
			info[i].ev2.Clear();
		}
	}
	/*
	char NameToSpecialKey(string name){ //for getting specific autoclips. for things that cant be used in file names
		switch(name.ToLower()){ //also for autodelays
			case "space": return ' ';
			case "tab": return '\t';
			case "line break": case "linebreak": return '\n';
			case "exclamation": case "exclamationpoint": case "exclamation point": return '!';
			case "question": case "questionmark": case "question mark": return '?';
			case "semicolon": return ';';
			case "colon": return ':';
			case "tilde": return '~';
			case "period": return '.';
			case "comma": return ',';
			case "number sign": case "hashtag": return '#';
			case "percent": case "percentsign": case "percent sign": return '%';
			case "ampersand": return '&';
			case "asterix": return '*';
			case "backslash": return '\\';
			case "openbrace": case "open brace": return '{';
			case "closebrace": case "close brace": return '}';
			default: return name[0];
		}
	}
	*/
	string SpecialKeyToName(char ch){ //since you can't put these in filenames
		switch(char.ToLower(ch)){
			case ' ': return "space";
			case '\t': return "tab";
			case '\n': return "line break";
			case '!': return "exclamation point";
			case '?': return "question mark";
			case ';': return "semicolon";
			case ':': return "colon";
			case '~': return "tilde";
			case '.': return "period";
			case ',': return "comma";
			case '#': return "number sign";
			case '%': return "percent";
			case '&': return "ampersand";
			case '*': return "asterix";
			case '\\': return "backslash";
			case '/': return "forwardslash";
			case '{': return "openbrace";
			case '}': return "closebrace";
			default: return new string(ch,1).ToLower(); //always ignore case
		}
	}
	void PlaySound(int i){
		if(audioSource != null){//audio stuff
			if(info[i].stopPreviousSound || !audioSource.isPlaying){
				audioSource.Stop();
				string nameToSearch = SpecialKeyToName(hyphenedText[i]);
				AudioClip mySoundClip = null;
				if(info[i].soundClipData != null){
					STMSoundClipData.AutoClip tmpAutoClip = info[i].soundClipData.clips.Find(x => x.name.ToLower() == nameToSearch); //find auto clip
					if(tmpAutoClip != null){
						mySoundClip = tmpAutoClip.clip;
					}
				}
				STMAutoClipData myAutoClip = null;
				if(data.autoClips.ContainsKey(nameToSearch.ToUpper())){ //case
					myAutoClip = data.autoClips[nameToSearch.ToUpper()];
				}else if(data.autoClips.ContainsKey(nameToSearch)){
					myAutoClip = data.autoClips[nameToSearch];
				}

				if(mySoundClip != null){ //use the one attached to this character
					audioSource.clip = mySoundClip;
				}else if(myAutoClip != null){ //if there's a sound clip for this character in Text Data
					audioSource.clip = myAutoClip.clip;
				}else if(info[i].audioClipData != null){ //use sounds attached to character
					audioSource.clip = info[i].audioClipData.clips.Length > 0 ? info[i].audioClipData.clips[Random.Range(0,info[i].audioClipData.clips.Length)] : null;
				}else if(audioClips.Length > 0){ //use a random audio clip
					audioSource.clip = audioClips.Length > 0 ? audioClips[Random.Range(0,audioClips.Length)] : null; //get one of the clips
				}else{
					audioSource.clip = null;
				}
				if(audioSource.clip != null){
					switch(info[i].pitchMode){
						case PitchMode.Perlin:
							audioSource.pitch = (Mathf.PerlinNoise(GetTime * perlinPitchMulti, 0f) * (info[i].maxPitch - info[i].minPitch)) + info[i].minPitch; //perlin noise
							break;
						case PitchMode.Random:
							audioSource.pitch = Random.Range(info[i].minPitch,info[i].maxPitch);
							break;
						case PitchMode.Single:
							audioSource.pitch = info[i].overridePitch;
							break;
						default:
							audioSource.pitch = 1f; //because of speedread pitch
							break;
					}
					if(speedReading){
						audioSource.pitch += info[i].speedReadPitch;
					}
					audioSource.Play();
				}
			}
		}
	}
	//TODO: 2016-06-11 actually, im guessing that these values are a bitmask? you could prob just add & subtract em! but w/e
	FontStyle AddStyle(FontStyle original, FontStyle newStyle){
		if(font.dynamic){
			switch(original){
				case FontStyle.Bold:
					switch(newStyle){
						case FontStyle.Italic:
							return FontStyle.BoldAndItalic;
						default:
							return original;
					}
				case FontStyle.Italic:
					switch(newStyle){
						case FontStyle.Bold:
							return FontStyle.BoldAndItalic;
						default:
							return original;
					}
				case FontStyle.BoldAndItalic:
					return original;
				default: //normal text
					return newStyle;
			}	
		}else{
			return FontStyle.Normal; //non-dynamic fonts can't handle bold/italic
		}
	}
	FontStyle SubtractStyle(FontStyle original, FontStyle subStyle){ //only bold and italic can be added and removed
		if(font.dynamic){
			switch(original){
				case FontStyle.Bold:
					switch(subStyle){
						case FontStyle.Bold:
							return FontStyle.Normal;
						default:
							return original;
					}
				case FontStyle.Italic:
					switch(subStyle){
						case FontStyle.Italic:
							return FontStyle.Normal;
						default:
							return original;
					}
				case FontStyle.BoldAndItalic:
					switch(subStyle){
						case FontStyle.Bold:
							return FontStyle.Italic;
						case FontStyle.Italic:
							return FontStyle.Bold;
						default:
							return original; //just in case?
					}
				default: //normal
					return FontStyle.Normal;
			}
		}else{
			return FontStyle.Normal; //non-dynamic fonts can't handle bold/italic
		}
	}
	bool ValidHexcode (string hex){ //check if a hex code contains the right amount of characters, and allowed characters
		if(hex.Length != 3 && hex.Length != 4 && hex.Length != 6 && hex.Length != 8){ //hex code, alpha hex
			return false;
		}
		string allowedLetters = "0123456789ABCDEFabcdef";
		for(int i=0; i<hex.Length; i++){
			if(!allowedLetters.Contains(hex[i].ToString())){
				return false; //invalid string!!!
			}
		}
		return true;
	}
	Color32 HexToColor(string hex){ //convert a hex code string to a color
		if(hex.Length == 8){ //RGBA (FF00FF00)
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			byte a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,a);
		}
		if(hex.Length == 4){ //single-byte for RGBA (F0F0)
			byte r = byte.Parse(hex.Substring(0,1) + hex.Substring(0,1), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(1,1) + hex.Substring(1,1), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(2,1) + hex.Substring(2,1), System.Globalization.NumberStyles.HexNumber);
			byte a = byte.Parse(hex.Substring(3,1) + hex.Substring(3,1), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,a);
		}
		if(hex.Length == 3){ //single-byte for RGB (F0F)
			byte r = byte.Parse(hex.Substring(0,1) + hex.Substring(0,1), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(1,1) + hex.Substring(1,1), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(2,1) + hex.Substring(2,1), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,255);
		}
		else{ //RGB (FF00FF)
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,255);
		}
	}
	STMColorData GetColor(string myCol){
		if(data.colors.ContainsKey(myCol)){ //check textdata for a named color
			return data.colors[myCol];
		}
		if(ValidHexcode(myCol)){ //might be a hexcode?
			STMColorData thisCol2 = ScriptableObject.CreateInstance<STMColorData>();
			thisCol2.color = HexToColor(myCol);
			return thisCol2;
		}
		//still no?
		STMColorData thisCol = ScriptableObject.CreateInstance<STMColorData>();
		switch(myCol){ //see if it's a default unity color
			case "red": thisCol.color = Color.red; break;
			case "green": thisCol.color = Color.green; break;
			case "blue": thisCol.color = Color.blue; break;
			case "yellow": thisCol.color = Color.yellow; break;
			case "cyan": thisCol.color = Color.cyan; break;
			case "magenta": thisCol.color = Color.magenta; break;
			case "grey": thisCol.color = Color.grey; break;
			case "gray": thisCol.color = Color.gray; break;
			case "black": thisCol.color = Color.black; break;
			case "clear": thisCol.color = Color.clear; break;
			case "white": thisCol.color = Color.white; break;
			default: thisCol.color = color; break; //default color of mesh
		}
		return thisCol;
	}
	string ParseText(string myText){//return a cleaned up string and update textinfo!
		info.Clear();
		if(data.autoVoice.Length > 0) myText = data.autoVoice + myText; //add automatic voice to the start
		//set defaults:
		STMTextInfo myInfo = new STMTextInfo(this); //info for this one character, carried over from last
		for(int i=0; i<myText.Length; i++){ //for each character to parse thru,
			if(info.Count == i && i > 0){ //no other delay yet...? /hasnt checkedAgain yet
				if(data.autoDelays.ContainsKey(SpecialKeyToName(myText[i-1])) && (myText[i] == ' ' || myText[i] == '\n' || myText[i] == '\t')){ //only if next character is "free". So strings like "Oh......... okay." only see the last delay on periods!
					myInfo.delayData = data.autoDelays[SpecialKeyToName(myText[i-1])];
				}
			}
			bool checkAgain = false;
			if(richText && myText[i] == '<'){ //check for count so a pointless debug doesn't appear on rebuild
				int closingIndex = myText.IndexOf(">",i); 
				int equalsIndex = closingIndex > -1 ? myText.IndexOf("=",i, closingIndex-i) : -1; //only look forward for a specific amount of characters
				//Get either closing index or squals index, depending on the kinda tag:
				int endIndex = (equalsIndex > -1 && closingIndex > -1) ? Mathf.Min(equalsIndex,closingIndex) : closingIndex;//for figuring out what the "tag" is
				if(closingIndex != -1){//don't do anything if there's no closing tag at all
					string myTag = myText.Substring(i, endIndex-i+1); //this is the "TAG" like "<c=" or "<br>"
					//only if there's for sure a string TO get
					//Debug.Log("Index is " + i + " equals index is " + equalsIndex + " closing index is " + closingIndex);
					string myString = equalsIndex > -1 ? myText.Substring(equalsIndex+1,closingIndex-equalsIndex-1) : "";//this is the "STRING" like "fire" or "blue"
					//Debug.Log("Found this tag: '" + myTag + "'! And this string: '" + myString + "'.");
					bool clearAfter = true;
					bool exitLoopAfter = false;
					string insertAfter = "";
					switch(myTag){
					//Line Break
						case "<br>":
							insertAfter = '\n'.ToString(); //insert a line break
							break;
					//Color
						case "<c=":
							myInfo.colorData = null; //clear to default
							myInfo.gradientData = null;
							myInfo.textureData = null;

							if(data.textures.ContainsKey(myString)){// is this a texture?
								myInfo.textureData = data.textures[myString];
								//AddMaterial(myInfo.) add material here
							}else{
								//is it a custom color tag?
								if(data.gradients.ContainsKey(myString)){ //is this a gradient?
									myInfo.gradientData = data.gradients[myString];
								}
								else{ //no? try for HEX code & default color
									myInfo.colorData = GetColor(myString);
								}
							}
							break;
						case "</c>":
							myInfo.colorData = null; //clear to default
							myInfo.gradientData = null;
							myInfo.textureData = null;
							break;
					//Size
						case "<s=":
							float thisSize;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisSize)){ //parse as a float
								myInfo.size = thisSize * size; //set size relative to the one set in inspector!
							}
							break;
						case "<size=":
							float thisSize2;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisSize2)){ //parse as a float
								myInfo.size = thisSize2; //set size directly!
							}
							break;
						case "</s>":
						case "</size>":
							myInfo.size = size;
							break;
					//Delay
						case "<d=":
							if(data.delays.ContainsKey(myString)){ //is there a delay defined in textdata?
								myInfo.delayData = data.delays[myString];//NOTE: delays get overridden, not added
							}else{ //see if it's an integer
								int thisDelay2;
								if(int.TryParse(myString, out thisDelay2)){ //parse as an int
									myInfo.delayData = ScriptableObject.CreateInstance<STMDelayData>(); //create new delay data
									myInfo.delayData.count = thisDelay2;
								}
							}
							break;
						case "<d>":
							if(data.delays.ContainsKey("default")){ //is there a delay defined?
								myInfo.delayData = data.delays["default"];
							}else{
								Debug.Log("Default delay isn't defined!");
							}
							break;
					//Timing
						case "<t=":
							float thisTiming;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisTiming)){ //parse as a float
								if(thisTiming < 0f) thisTiming = 0f; //or else it'll cause a loop!
								myInfo.readTime = thisTiming; //set time to be this float
							}
							break;
					//Event
						case "<e=":
							myInfo.ev.Add(myString); //remember the event!
							break;
					//Repeating Event
						case "<e2=":
							myInfo.ev2.Add(myString); //remember the event!
							break;
						case "</e>":
						case "</e2>":
							myInfo.ev2.Clear(); //forget all. Kinda janky, but whatever. It'll do for now!
							break;
					//Voice
						case "<v=":
							if(data.voices.ContainsKey(myString)){
								insertAfter = data.voices[myString].text; //add the text to the string!
							}
							break;
						case "</v>":
							//myInfo.voiceData = null; //forget it!
							//return everything to default.
							info[i] = new STMTextInfo(this);
							break;
					//Font
						case "<f=": //this switches the font for the whole mesh, but I might as well include it
						case "<font=":
							if(data.fonts.ContainsKey(myString)){
								//useFont = data.fonts[myString].font; //remember the font in this wayyy
								myInfo.fontData = data.fonts[myString];
							}
							break; //switching to a non-dynamic font can return some errors
						case "</f>":
						case "</font>":
							//useFont = null; //forget it!
							myInfo.fontData = null;
							break;
					//Quad
						case "<q=":
						case "<quad=":
							if(data.quads.ContainsKey(myString)){
								//if this letter doesn't already have quad data:
								if(myInfo.quadData == null){
                                    myInfo.quadData = data.quads[myString].CopyTo();
                                    //myInfo.quadData = data.quads[myString];
                                    insertAfter = "`"; //a character to get used for the quad, in hyphenedtext
								}

							}
							break;
					//Material
						case "<m=":
						case "<material=":
							if(data.materials.ContainsKey(myString)){
								myInfo.materialData = data.materials[myString];
							}
							break;
						case "</m>":
						case "</material>":
							myInfo.materialData = null;
							break;
					//Bold & Italic
						case "<b>":
							myInfo.ch.style = AddStyle(myInfo.ch.style, FontStyle.Bold); //mark this character as bold
							break;
						case "</b>":
							myInfo.ch.style = SubtractStyle(myInfo.ch.style, FontStyle.Bold);
							break;
						case "<i>":
							myInfo.ch.style = AddStyle(myInfo.ch.style, FontStyle.Italic); //mark this character as italic
							break;
						case "</i>":
							myInfo.ch.style = SubtractStyle(myInfo.ch.style, FontStyle.Italic);
							break;
					//Waves
						case "<w=":
							if(data.waves.ContainsKey(myString)){ //is it a preset?
								myInfo.waveData = data.waves[myString];
							}
							break;
						case "<w>":
							if(data.waves.ContainsKey("default")){
								myInfo.waveData = data.waves["default"]; //mark this character as bold
							}else{
//								Debug.Log("Default wave isn't defined!");
								//Resources.UnloadAsset(thisWave); //force it to search again??
							}
							break;
						case "</w>":
							myInfo.waveData = null;
							break;
					//Jitters
						case "<j=":
							if(data.jitters.ContainsKey(myString)){ //is it a preset?
								myInfo.jitterData = data.jitters[myString];
							}
							break;
						case "<j>":
							if(data.jitters.ContainsKey("default")){
								myInfo.jitterData = data.jitters["default"];
							}else{
								Debug.Log("Default jitter isn't defined!");
								//Resources.UnloadAsset(thisJitter); //force it to search again?
							}
							break;
						case "</j>":
							myInfo.jitterData = null;
							break;
					//Alignment
						case "<a=":
							switch(myString.ToLower()){ //not case sensitive, for some reason? why not
								case "left": myInfo.alignment = Alignment.Left; break;
								case "right": myInfo.alignment = Alignment.Right; break;
								case "center": case "centre": myInfo.alignment = Alignment.Center; break;
								case "just": case "justify": case "justified": myInfo.alignment = Alignment.Justified; break;
								case "just2": case "justify2": case "justified2": myInfo.alignment = Alignment.ForceJustified; break;
							}
							break;
						case "</a>":
							myInfo.alignment = alignment; //return to default for mesh
							break;
					//Audio Settings
						case "<stopPreviousSound=":
							switch(myString.ToLower()){
								case "true": myInfo.stopPreviousSound = true; break;
								case "false": myInfo.stopPreviousSound = false; break;
							}
							break;
						case "</stopPreviousSound>":
							myInfo.stopPreviousSound = stopPreviousSound; //reset to default
							break;
						case "<pitchMode=":
							switch(myString.ToLower()){
								case "normal": myInfo.pitchMode = PitchMode.Normal; break;
								case "single": myInfo.pitchMode = PitchMode.Single; break;
								case "random": myInfo.pitchMode = PitchMode.Random; break;
								case "perlin": myInfo.pitchMode = PitchMode.Perlin; break;
							}
							break;
						case "</pitchMode>":
							myInfo.pitchMode = pitchMode; //return to default
							break;
						case "<overridePitch=":
							float thisOverridePitch;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisOverridePitch)){ //parse as a float
								myInfo.overridePitch = thisOverridePitch; //set time to be this float
							}
							break;
						case "</overridePitch>":
							myInfo.overridePitch = overridePitch;
							break;
						case "<minPitch=":
							float thisMinPitch;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisMinPitch)){ //parse as a float
								myInfo.minPitch = thisMinPitch; //set time to be this float
							}
							break;
						case "</minPitch>":
							myInfo.minPitch = minPitch;
							break;
						case "<maxPitch=":
							float thisMaxPitch;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisMaxPitch)){ //parse as a float
								myInfo.maxPitch = thisMaxPitch; //set time to be this float
							}
							break;
						case "</maxPitch>":
							myInfo.maxPitch = maxPitch;
							break;
						case "<speedReadPitch=":
							float thisSpeedReadPitch;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisSpeedReadPitch)){ //parse as a float
								myInfo.speedReadPitch = thisSpeedReadPitch; //set time to be this float
							}
							break;
						case "</speedReadPitch>":
							myInfo.speedReadPitch = speedReadPitch;
							break;
						case "<readDelay=":
							float thisReadDelay;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisReadDelay)){ //parse as a float
								myInfo.readDelay = thisReadDelay; //set time to be this float
							}
							break;
						case "</readDelay>":
							myInfo.readDelay = readDelay;
							break;
						case "<drawAnim=":
							if(data.drawAnims.ContainsKey(myString)){
								myInfo.drawAnimData = data.drawAnims[myString]; //set draw animation
							}else if(data.drawAnims.ContainsKey("Appear")){
								myInfo.drawAnimData = data.drawAnims["Appear"]; //get first one
							}else{
								Debug.Log("'Appear' draw animation isn't defined!");
							}
							break;
						case "</drawAnim>":
							if(data.drawAnims.ContainsKey(drawAnimName)){
								myInfo.drawAnimData = data.drawAnims[drawAnimName]; //return to default
							}else if(data.drawAnims.ContainsKey("Appear")){
								myInfo.drawAnimData = data.drawAnims["Appear"]; //get first one
							}else{
								Debug.Log("'Appear' draw animation isn't defined!");
							}
							break;
						case "<drawOrder=":
							switch(myString.ToLower()){
								case "lefttoright": case "ltr": myInfo.drawOrder = DrawOrder.LeftToRight; break;
								case "allatonce": case "all": myInfo.drawOrder = DrawOrder.AllAtOnce; break;
								case "onewordatatime": case "robot": myInfo.drawOrder = DrawOrder.OneWordAtATime; break;
								case "random": myInfo.drawOrder = DrawOrder.Random; break;
								case "righttoleft": case "rtl": myInfo.drawOrder = DrawOrder.RightToLeft; break;
								case "reverseltr": myInfo.drawOrder = DrawOrder.ReverseLTR; break;
							}
							break;
						case "</drawOrder>":
							myInfo.drawOrder = drawOrder; //return to default
							break;
						case "<clips=":
							myInfo.soundClipData = data.soundClips[myString];
							break;
						case "</clips>":
							myInfo.soundClipData = null;
							break;
						case "<audioClips=":
							if(data.audioClips.ContainsKey(myString)){
								myInfo.audioClipData = data.audioClips[myString];
							}
							break;
						case "</audioClips>":
							myInfo.audioClipData = null;
							break;
						case "<indent=": //set the indent to be here
							float thisIndent;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisIndent)){ //parse as a float
								myInfo.indent = thisIndent; //set time to be this float
							}
							break;
						case "</indent>": //return to normal
							myInfo.indent = 0f;
							break;
						case "<pause>":
							//tell mesh to pause here
							currentPauseCount++;
							if(Application.isPlaying && currentPauseCount > pauseCount) exitLoopAfter = true; //only during playmode for display purposes and cause it can break text
							break;
					//Default
						default:
							clearAfter = false;//DONT remove characters and do stuff
							break;
					}
					if(clearAfter){
						myText = myText.Remove(i,closingIndex+1-i);
						//Debug.Log("Removing '" + myText.Substring(i,closingIndex+1-i) + "'. The string is now: '" + myText + "'.");
						myText = myText.Insert(i,insertAfter);
						checkAgain = true;
					}
					if(exitLoopAfter){
						//keep track of last pause, and skip over it
						myText = myText.Remove(i,myText.Length-i); //remove everything after
						break;
					}
				}
			}
			if(info.Count - 1 == i){
				info[i] = new STMTextInfo(myInfo); //update older one, it was checking again
				//Debug.Log("Updating older character " + myText[i].ToString() + " to be " + info[i].style);
			}else{
				info.Add(new STMTextInfo(myInfo) ); //add new HAS TO BE NEW OR ELSE IT JUST REFERENCES
			}
			if(checkAgain){
				i--;
			}else{ //stuff that gets reset!! single-use tags.
				myInfo.delayData = null;// reset
				myInfo.quadData = null;
				myInfo.ev.Clear();
				myInfo.readTime = -1f;
			}
		}
		return myText;
	}
	int GetFontSize(Font myFont, STMTextInfo myInfo){ //so dynamic and non-dynamic fonts can be used together
		if(!myFont.dynamic && myFont.fontSize != 0){
			return myFont.fontSize; //always go w/ non-dynamic size first
		}
		if(myInfo.fontData != null){
			return myInfo.fontData.quality; //then set quality
		}
		if(myInfo.ch.size != 0){
			return myInfo.ch.size; //then natural quality
		}
		return quality; //default
	}
	void RequestAllCharacters(){ //by calling this every frame, should keep specific letters in the texture? not sure if it's a waste
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
			myFont.RequestCharactersInTexture(hyphenedText[i].ToString(), GetFontSize(myFont,info[i]), info[i].ch.style); //request characters to draw
			//and special characters:
			myFont.RequestCharactersInTexture("-", GetFontSize(myFont,info[i]), FontStyle.Normal); //still call this, for when you're inserting hyphens anyway
		}
	}
	void RebuildTextInfo(){ 
		drawText = ParseText(text); //remove parsing junk (<col>, <b>), and fill textinfo again
		Vector3 pos = new Vector3(info.Count > 0 ? info[0].indent : 0f, 0f, 0f); //keep track of where to place this text
		lineBreaks.Clear(); //index of line break characters, for centering

		hyphenedText = string.Copy(drawText);
		if( ((uiMode && wrapText) || (!uiMode)) && AutoWrap > 0f){ //use autowrap?
			//XXXXXX TODOOOOO AHHH see if setting "quality" to be info[i].ch.size has any issues, now: 2016-10-26
			for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //first, get character info...
				Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
				myFont.RequestCharactersInTexture(hyphenedText[i].ToString(), GetFontSize(myFont,info[i]), info[i].ch.style); //request characters to draw
				CharacterInfo ch;
				if(myFont.GetCharacterInfo(hyphenedText[i], out ch, GetFontSize(myFont,info[i]), info[i].ch.style)){ //does this character exist?
					info[i].ch = ch; //remember character info!
				}//else, don't draw anything! this charcter won't have info
			}
			float lineWidth = info.Count > 0 ? info[0].indent : 0f;
			int indexOffset = 0;
			for(int i=0, iL=hyphenedText.Length; i<iL; i++){
				Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
				CharacterInfo spaceCh; //moved these into this loop 2016-10-26
				myFont.GetCharacterInfo(' ', out spaceCh, GetFontSize(myFont,info[i]), style); //get data for space
				CharacterInfo hyphenCh;
				myFont.RequestCharactersInTexture("-", GetFontSize(myFont,info[i]), style); //still call this, for when you're inserting hyphens anyway
				myFont.GetCharacterInfo('-', out hyphenCh, GetFontSize(myFont,info[i]), style);
				//float hyphenWidth = hyphenCh.advance * (info[i].size / info[i].ch.size); //have hyphen size match last character in row
				if(hyphenedText[i] == '\n'){ //is this character a line break?
					lineWidth = 0f; //new line, reset
				}else if(hyphenedText[i] == '\t'){ // linebreak with a tab...
					lineWidth += 0.5f * tabSize * info[i].size;
				}else{
					lineWidth += info[i].Advance(characterSpacing, info[i].ch.size).x;
				}
				//TODO: watch out for natural hyphens going over bounds limits
				if(lineWidth > AutoWrap){
					int myBreak = hyphenedText.LastIndexOf(' ',i); //safe spot to do a line break, can be a hyphen
					int myHyphenBreak = hyphenedText.LastIndexOf('-',i);
					int myTabBreak = hyphenedText.LastIndexOf('\t',i); //can break at a tab, too!
					int myActualBreak = Mathf.Max(new int[]{myBreak, myHyphenBreak, myTabBreak}); //get the largest of all 3
					int lastBreak = hyphenedText.LastIndexOf('\n',i); //last place a ine break happened
					if(!breakText && myActualBreak != -1 && myActualBreak > lastBreak){ //is there a space to do a line break? (and no hyphens...) AND we're not breaking text up at all
						//
						if(myActualBreak == myHyphenBreak){ //the break is at a hyphen
							hyphenedText = hyphenedText.Insert(myActualBreak+1, '\n'.ToString());
							info.Insert(myActualBreak+1,new STMTextInfo(info[myActualBreak], spaceCh));
							i = myActualBreak+1; //go back
							//if(AutoWrap < info[i - indexOffset].size){ //otherwise, it'll loop foreverrr
							//	i += 1;
							//}
							iL += 1;
							indexOffset += 1;
						}else{
							hyphenedText = hyphenedText.Remove(myActualBreak, 1); //this is wrong, don't remove the space ooops
							hyphenedText = hyphenedText.Insert(myActualBreak, '\n'.ToString());
							i = myActualBreak;
						}
						
						lineWidth = info[i].indent; //reset
					}else if(i != 0){ //split it here! but not if it's the first character
						if(insertHyphens){
							hyphenedText = hyphenedText.Insert(i, "-\n");
							//Debug.Log("This needs a hyphen: " + hyphenedText);
							info.Insert(i,new STMTextInfo(info[i - indexOffset], spaceCh));
							info.Insert(i,new STMTextInfo(info[i - indexOffset], hyphenCh));
							if(AutoWrap < info[i - indexOffset].size){ //otherwise, it'll loop foreverrr
								i += 2;
							}
							iL += 2;
							indexOffset += 2;
						}else{
							hyphenedText = hyphenedText.Insert(i, "\n");
							info.Insert(i,new STMTextInfo(info[i - indexOffset], spaceCh));
							if(AutoWrap < info[i - indexOffset].size){ //otherwise, it'll loop foreverrr
								i += 1;
							}
							iL += 1;
							indexOffset += 1;
						}
						lineWidth = info[i].indent; //reset
					}//no need to check for following space, it'll come up anyway
				}
			}
		}else{
			for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //from character info...
				Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
				//vvvv very important
				myFont.RequestCharactersInTexture(hyphenedText[i].ToString(), GetFontSize(myFont,info[i]), info[i].ch.style); //request characters to draw
				//font.RequestCharactersInTexture(System.Text.Encoding.UTF8.GetString(System.BitConverter.GetBytes(info[i].ch.index)), GetFontSize(myFont,info[i]), info[i].ch.style); //request characters to draw
				CharacterInfo ch;
				if(myFont.GetCharacterInfo(hyphenedText[i], out ch, GetFontSize(myFont,info[i]), info[i].ch.style)){ //does this character exist?
					info[i].ch = ch; //remember character info!
				}//else, don't draw anything! this charcter won't have info
			}
		}
		//get position
		int currentLineCount = 0;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //for each character to draw...
			Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
			//CharacterInfo ch; //moved this code to the loop above^^^^
			//if(myFont.GetCharacterInfo(hyphenedText[i], out ch, quality, info[i].ch.style)){ //does this character exist?
			//	info[i].ch = ch; //remember character info!
			//}//else, don't draw anything! this charcter won't have info
			float myQuality = (float)GetFontSize(myFont,info[i]);
			info[i].pos = pos; //save this position data!
			info[i].line = currentLineCount;
			if(hyphenedText[i] == '\n'){//drop a line
				if(i == 0){ //first character is a line break?
					lineBreaks.Add(0);
				}else{
					lineBreaks.Add(i-1);
				}
				//start new row at the X position of the indent character
				pos = new Vector3(info[i].indent, pos.y ,0); //assume left-orintated for now. go back to start of row
				pos -= new Vector3(0, myQuality * lineSpacing, 0) * (size / myQuality); //drop down
				currentLineCount++;
			}
			else if(iL - 1 == i){ //last character, and not a line break?
				lineBreaks.Add(i);
			}
			else if(hyphenedText[i] == '\t'){//tab?
				pos += new Vector3(myQuality * 0.5f * tabSize, 0,0) * (info[i].size / myQuality);
			}
			else{// Advance character position
				pos += info[i].Advance(characterSpacing,myQuality);
			}//remember position data for whatever
		}
		lineBreaks = lineBreaks.Distinct().ToList(); //remove doubles, preventing horizontal offset glitch
		
		ApplyOffsetDataToTextInfo(); //just to clean up this very long function...
		UpdateRTLDrawOrder();
		ApplyTimingDataToTextInfo();
		ApplyUnreadTimingDataToTextInfo();
		PrepareSubmeshes();
	}
	void ApplyOffsetDataToTextInfo(){ //this works!!! ahhhh!!!
		float[] allMaxes = new float[lineBreaks.Count];
		for(int i=0, iL=lineBreaks.Count; i<iL; i++){
			//get max x data from this line
			allMaxes[i] = info[lineBreaks[i]].TopRightVert.x;
			if(float.IsNaN(allMaxes[i])){
				allMaxes[i] = 0f; //for rows that are just linebreaks! take THAT
			}
		}
		float maxX = Mathf.Max(allMaxes);
		Vector3 offset = -baseOffset; //apply anchor offset
		if(uiMode){
			//ALIGN TO WHATEVER UI BOX HERE!!!
			RectTransform tr = t as RectTransform; //(RectTransform(t)) also works!
			Vector3 xtraOffset = Vector3.zero;
			//TODO: during play mode, this doesn't update right...
			switch(anchor){
				case TextAnchor.UpperLeft: xtraOffset = new Vector3(tr.rect.xMin, tr.rect.yMax, 0f); break;
				case TextAnchor.UpperCenter: xtraOffset = new Vector3((tr.rect.xMin + tr.rect.xMax) / 2f, tr.rect.yMax, 0f); break;
				case TextAnchor.UpperRight: xtraOffset = new Vector3(tr.rect.xMax, tr.rect.yMax, 0f); break;
				case TextAnchor.MiddleLeft: xtraOffset = new Vector3(tr.rect.xMin, (tr.rect.yMin + tr.rect.yMax) / 2f, 0f); break;
				case TextAnchor.MiddleCenter: xtraOffset = new Vector3((tr.rect.xMin + tr.rect.xMax) / 2f, (tr.rect.yMin + tr.rect.yMax) / 2f, 0f); break;
				case TextAnchor.MiddleRight: xtraOffset = new Vector3(tr.rect.xMax, (tr.rect.yMin + tr.rect.yMax) / 2f, 0f); break;
				case TextAnchor.LowerLeft: xtraOffset = new Vector3(tr.rect.xMin, tr.rect.yMin, 0f); break;
				case TextAnchor.LowerCenter: xtraOffset = new Vector3((tr.rect.xMin + tr.rect.xMax) / 2f, tr.rect.yMin, 0f); break;
				case TextAnchor.LowerRight: xtraOffset = new Vector3(tr.rect.xMax, tr.rect.yMin, 0f); break;
			}
			offset -= xtraOffset;
		}
		float lowestVert = size;
		float rightestVert = 0f;
		int rowStart = 0; //index of where this row starts
		for(int i=0, iL=lineBreaks.Count; i<iL; i++){ //for each line of text //2016-06-09 new alignment script
			float myOffsetRight = maxX - info[lineBreaks[i]].TopRightVert.x; //empty space on this row
			if(AutoWrap > 0f){
				myOffsetRight += AutoWrap - maxX;
			}
			int spaceCount = 0;
			for(int j=rowStart, jL=lineBreaks[i]+1; j<jL; j++){ //see how many spaces there are
				if(hyphenedText[j] == ' '){
					spaceCount++;
				}
			}
			float justifySpace = spaceCount > 0 ? myOffsetRight / (float)spaceCount : 0f;
			int passedSpaces = 0;
			for(int j=rowStart, jL=lineBreaks[i]+1; j<jL; j++){//if this character is in the range...
				if(hyphenedText[j] == ' '){
					passedSpaces++;
				}
				//Debug.Log("Aligning character " + j + ", which is: '" + hyphenedText[j] + "'.");
				switch(info[j].alignment){
					case Alignment.Center:
						info[j].pos.x += myOffsetRight / 2f; //use half of empty space
						break;
					case Alignment.Right:
						info[j].pos.x += myOffsetRight;
						break;
					case Alignment.Justified:
						if(jL != hyphenedText.Length && drawText[jL - (hyphenedText.Length - drawText.Length)] != '\n'){ //not the very last row, or a row with a linebreak?
							info[j].pos.x += justifySpace * passedSpaces;
						}
						break;
					case Alignment.ForceJustified:
						info[j].pos.x += justifySpace * passedSpaces; //justify no matter what
						break;
					//do nothing for left-aligned
				}
				if(lineCountLimit == 0 || info[j].line < lineCountLimit){ //only keep counting if it's not past the line count limit
					lowestVert = Mathf.Min(lowestVert, info[j].pos.y); //yeah this works. shouldn't change with waves/weird letters
				}
				rightestVert = Mathf.Max(rightestVert, info[j].BottomRightVert.x);
			}
			rowStart = lineBreaks[i]+1;
		}
		//float upperY = size; //push down
		//float lowerY = size * (lineBreaks.Count - 1) * lineSpacing;
		if(autoWrap > 0f){//anchor to box instead of text
			switch(anchor){ //yep, just use autowrap limit
				case TextAnchor.UpperLeft: offset += new Vector3(0, size, 0); break;
				case TextAnchor.UpperCenter: offset += new Vector3(autoWrap * 0.5f, size, 0); break;
				case TextAnchor.UpperRight: offset += new Vector3(autoWrap, size, 0); break;
				case TextAnchor.MiddleLeft: offset += new Vector3(0, (size + lowestVert) * 0.5f, 0); break;
				case TextAnchor.MiddleCenter: offset += new Vector3(autoWrap * 0.5f, (size + lowestVert) * 0.5f, 0); break;
				case TextAnchor.MiddleRight: offset += new Vector3(autoWrap, (size + lowestVert) * 0.5f, 0); break;
				case TextAnchor.LowerLeft: offset += new Vector3(0, lowestVert, 0); break;
				case TextAnchor.LowerCenter: offset += new Vector3(autoWrap * 0.5f, lowestVert, 0); break;
				case TextAnchor.LowerRight: offset += new Vector3(autoWrap, lowestVert, 0); break;
			}
		}else{
			switch(anchor){
				case TextAnchor.UpperLeft: offset += new Vector3(0, size, 0); break;
				case TextAnchor.UpperCenter: offset += new Vector3(maxX * 0.5f, size, 0); break;
				case TextAnchor.UpperRight: offset += new Vector3(maxX, size, 0); break;
				case TextAnchor.MiddleLeft: offset += new Vector3(0, (size + lowestVert) * 0.5f, 0); break;
				case TextAnchor.MiddleCenter: offset += new Vector3(maxX * 0.5f, (size + lowestVert) * 0.5f, 0); break;
				case TextAnchor.MiddleRight: offset += new Vector3(maxX, (size + lowestVert) * 0.5f, 0); break;
				case TextAnchor.LowerLeft: offset += new Vector3(0, lowestVert, 0); break;
				case TextAnchor.LowerCenter: offset += new Vector3(maxX * 0.5f, lowestVert, 0); break;
				case TextAnchor.LowerRight: offset += new Vector3(maxX, lowestVert, 0); break;
			}
		}
		for(int i=0, iL=info.Count; i<iL; i++){ //apply all offsets
			info[i].pos -= offset;
		}
		if(uiMode){ //fix boundary ending up too wide on non-wrapped UI text
			RectTransform tr = t as RectTransform;
			maxX = tr.rect.xMax;
		}
		rawTopLeftBounds = new Vector3(offset.x, offset.y - size, offset.z); //scale to show proper bunds even when parent is scaled weird
		rawBottomRightBounds = new Vector3(AutoWrap > 0f ? offset.x - AutoWrap : !uiMode ?  offset.x - rightestVert : -maxX, 
															offset.y - lowestVert, 
															offset.z);
	
		RecalculateBounds();
	}
	void RecalculateBounds(){
		topLeftBounds = rawTopLeftBounds;
		topRightBounds = new Vector3(rawBottomRightBounds.x, rawTopLeftBounds.y, rawTopLeftBounds.z);
		bottomLeftBounds = new Vector3(rawTopLeftBounds.x, rawBottomRightBounds.y, rawBottomRightBounds.z);
		bottomRightBounds = rawBottomRightBounds;

		topLeftBounds = t.rotation * topLeftBounds;
		topRightBounds = t.rotation * topRightBounds;
		bottomLeftBounds = t.rotation * bottomLeftBounds;
		bottomRightBounds = t.rotation * bottomRightBounds;

		topLeftBounds.Scale(t.lossyScale);
		topRightBounds.Scale(t.lossyScale);
		bottomLeftBounds.Scale(t.lossyScale);
		bottomRightBounds.Scale(t.lossyScale);

		topLeftBounds = t.position - topLeftBounds; //do this last, so previous transforms are based around 0
		topRightBounds = t.position - topRightBounds;
		bottomLeftBounds = t.position - bottomLeftBounds;
		bottomRightBounds = t.position - bottomRightBounds;

		centerBounds = Vector3.Lerp(topLeftBounds, bottomRightBounds, 0.5f);
	}

	private int[] drawOrderRTL;
	void UpdateRTLDrawOrder (){ //update the RTL draw info, if needed
		//if(drawOrder == DrawOrder.RightToLeft || undrawOrder == DrawOrder.RightToLeft){ //actually calculate? eh, do it anyway
		drawOrderRTL = new int[hyphenedText.Length];
		int currentLine = 0;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			int lastEnd = currentLine > 0 ? lineBreaks[currentLine-1] + 1 : 0;
			if(currentLine < lineBreaks.Count){
				drawOrderRTL[i] = -i + lineBreaks[currentLine] + lastEnd;
				if(lineBreaks[currentLine] == i){ //this was the last character in this row
					//Debug.Log("The end of this line was: " + lineBreaks[currentLine]);
					currentLine++;
				}
			}
		}
		//}
	}
	void ApplyTimingDataToTextInfo(){
		float currentTiming = 0f;
		float furthestPoint = 0f;
		bool needsToRead = false;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			int myIndex = i;
			switch(info[i].drawOrder){
				case DrawOrder.RightToLeft: myIndex = drawOrderRTL[i]; break;
				case DrawOrder.ReverseLTR: myIndex = -i + iL - 1; break;
			}
			if(info[i].readDelay > 0f){ //a delay hasn't been set for this letter, yet
				needsToRead = true;
			}
			float additionalDelay = info[myIndex].delayData != null ? info[myIndex].delayData.count : 0f; //if there's no additional delay data attached... no additional delay
			//get the time it'll be drawn at...
			if(info[myIndex].readTime < 0f){ //if a time hasn't been set for this letter yet
				switch(info[i].drawOrder){
					case DrawOrder.AllAtOnce:
						info[i].readTime = currentTiming;
						break;
					case DrawOrder.Random:
						info[i].readTime = Random.Range(0f,info[i].readDelay);
						break;
					case DrawOrder.OneWordAtATime:
						if(hyphenedText[i] == ' ' || hyphenedText[i] == '\n' || hyphenedText[i] == '\t' || hyphenedText[i] == '-'){ //only advance timing on a space, line break, or tab, or hyphen!
							currentTiming += i == 0 ? additionalDelay * info[i].readDelay : info[i].readDelay + (additionalDelay * info[i].readDelay);
						}
						info[i].readTime = currentTiming;
						break;	
					case DrawOrder.RightToLeft:
						info[myIndex].readTime = currentTiming; //reverse order!
						currentTiming += myIndex == 0 ? additionalDelay * info[i].readDelay : info[i].readDelay + (additionalDelay * info[i].readDelay);
						break;
					case DrawOrder.ReverseLTR:
						currentTiming += i == 0 ? additionalDelay * info[i].readDelay : info[i].readDelay + (additionalDelay * info[i].readDelay);
						info[myIndex].readTime = currentTiming;
						break;
					default: //Left To Right
						//dont add extra for first character
						currentTiming += i == 0 ? additionalDelay * info[i].readDelay : info[i].readDelay + (additionalDelay * info[i].readDelay);
						info[i].readTime = currentTiming;
						break;
				}
			}else{
				currentTiming = info[myIndex].readTime; //pick up from here
			}
			float maxAnimTime = info[i].drawAnimData != null ? Mathf.Max(info[i].drawAnimData.animTime, info[i].drawAnimData.fadeTime) : 0f; //just for initialization, so an error doesn't get returned. drawanim should never be null, really
			furthestPoint = Mathf.Max(info[myIndex].readTime + maxAnimTime, furthestPoint);
		}
		totalReadTime = furthestPoint; //save it!
		callReadFunction = needsToRead;
	}
	void ApplyUnreadTimingDataToTextInfo(){
		//the other on the switch statement is different than the function above on purpose... might change in the future
		//things have to be done in a slightly different order
		float currentTiming = 0f;
		float furthestPoint = 0f;
		STMDrawAnimData myDrawAnim = UndrawAnim; //store undrawing animation since it'll be called multiple times
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			int myIndex = i;
			switch(undrawOrder){
				case DrawOrder.RightToLeft: myIndex = drawOrderRTL[i]; break;
				case DrawOrder.ReverseLTR: myIndex = -i + iL - 1; break;
			}
			switch(undrawOrder){
				case DrawOrder.AllAtOnce:
					info[i].unreadTime = currentTiming;
					break;
				case DrawOrder.Random:
					info[i].unreadTime = Random.Range(0f,unreadDelay);
					break;
				case DrawOrder.OneWordAtATime:
					info[i].unreadTime = currentTiming;
					if(hyphenedText[i] == ' ' || hyphenedText[i] == '\n' || hyphenedText[i] == '\t' || hyphenedText[i] == '-'){ //only advance timing on a space, line break, or tab, or hyphen!
						currentTiming += unreadDelay;
					}
					break;	
				case DrawOrder.RightToLeft:
					currentTiming += unreadDelay;
					info[myIndex].unreadTime = currentTiming;
					break;
				case DrawOrder.ReverseLTR:
					currentTiming += unreadDelay;
					info[myIndex].unreadTime = currentTiming;
					break;
				default:
					info[i].unreadTime = currentTiming;
					currentTiming += unreadDelay; //<<< this is applied in opposide order as normal read info
					break;
			}
			float maxAnimTime = myDrawAnim != null ? Mathf.Max(myDrawAnim.animTime, myDrawAnim.fadeTime) : 0f;
			furthestPoint = Mathf.Max(info[myIndex].unreadTime + maxAnimTime, furthestPoint);
		}
		totalUnreadTime = furthestPoint; //save it!
	}
	Vector3 WaveValue(STMTextInfo myInfo, STMWaveControl wave){ //multiply offset by 6 because ??????? seems to work
		//float currentTime = GetTime;
		float myTime = myInfo.waveData.animateFromTimeDrawn ? GetTime - timeDrawn - myInfo.readTime : GetTime;
		return new Vector3(wave.curveX.Evaluate(((myTime * wave.speed.x) + wave.offset * 6f) + (myInfo.pos.x * wave.density.x / myInfo.size)) * wave.strength.x * myInfo.size, 
							wave.curveY.Evaluate(((myTime * wave.speed.y) + wave.offset * 6f) + (myInfo.pos.x * wave.density.y / myInfo.size)) * wave.strength.y * myInfo.size, 
							wave.curveZ.Evaluate(((myTime * wave.speed.z) + wave.offset * 6f) + (myInfo.pos.x * wave.density.z / myInfo.size)) * wave.strength.z * myInfo.size); //multiply by universal size;
	}
	Vector3 JitterValue(STMTextInfo myInfo, STMJitterData jit){
		Vector3 myJit = Vector3.zero;
		float myTime = currentReadTime - myInfo.readTime; //time that's different for each letter
		switch(jit.perlin){
			case true:
				myJit = new Vector3( //weird perlin jitter... could use some work, but it's a jitter effect that scales with time
						jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * (jit.distance.Evaluate(Mathf.PerlinNoise(jit.perlinTimeMulti*myTime+myInfo.pos.x, 0f)) * jit.amount * (Mathf.PerlinNoise(jit.perlinTimeMulti*myTime+myInfo.pos.x, 0f) - 0.5f)),
						jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * (jit.distance.Evaluate(Mathf.PerlinNoise(jit.perlinTimeMulti*myTime+myInfo.pos.x+30f, 0f)) * jit.amount * (Mathf.PerlinNoise(jit.perlinTimeMulti*myTime+myInfo.pos.x+30f, 0f) - 0.5f)),
						0) * myInfo.size; //multiply by universal size
				break;
			default:
				myJit = new Vector3(//ditance over time... so jitters can also only happen AS a letter is drawn.
						jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * jit.distance.Evaluate(Random.value) * jit.amount * (Random.value - 0.5f),//make jit follow curve
						jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * jit.distance.Evaluate(Random.value) * jit.amount * (Random.value - 0.5f),
						0) * myInfo.size; //multiply by universal size
				break;
		}
		return myJit;
	}
	void PrepareSubmeshes(){
		//since this only needs to be calculated during Rebuild(), putting this in its own function.

		subMeshes = new List<SubMeshData>(); //include default submesh
		subMeshes.Add(new SubMeshData(this)); //add default submesh
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //go thru all info
			//This also only needs to be changed on rebuild(), move it sometime 2016-10-26 TODO
			//get and assign submesh/triangles for this letter
			SubMeshData thisSubMesh = DoesSubmeshExist(this,info[i]); //is there a submesh for this texture yet?
			//Debug.Log("This info's font is " + info[i].fontData);
			if(thisSubMesh == null){ //doesn't exist yet??
				thisSubMesh = new SubMeshData(this, info[i]); //create new
				subMeshes.Add(thisSubMesh); //and add to submesh list
			}
			//vvvv doing is this way creates garbage
			//thisSubMesh.tris.AddRange(new int[]{4*i+0,4*i+1,4*i+2,4*i+0,4*i+2,4*i+3}); //add tris for this letter
			//vvvv this way seems fine tho
			thisSubMesh.tris.Add(4*i + 0);
			thisSubMesh.tris.Add(4*i + 1);
			thisSubMesh.tris.Add(4*i + 2);
			thisSubMesh.tris.Add(4*i + 0);
			thisSubMesh.tris.Add(4*i + 2);
			thisSubMesh.tris.Add(4*i + 3);
		}


		//subMeshes = new SubMeshData[subMeshCount]; //create an array to hold all these sebmeshes

	}
	void UpdateMesh() { //set the data for the endmesh
		//Mesh mesh = new Mesh();
		areWeAnimating = false;
		//if(hyphenedText.Length > 0){ //bother to draw it?
		// Generate a mesh for the characters we want to print.
		endVerts = new Vector3[hyphenedText.Length * 4];
		//endTriangles = new int[hyphenedText.Length * 6];
		endUv = new Vector2[hyphenedText.Length * 4];
		endUv2 = new Vector2[hyphenedText.Length * 4]; //overlay images
		endCol32 = new Color32[hyphenedText.Length * 4];

		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
		//Vertex data:
		//animated stuffffff
			Vector3 jitterValue = Vector3.zero;
			if(info[i].jitterData != null && !data.disableAnimatedText && !disableAnimatedText){ //just dont jitter if animating text is overridden
				areWeAnimating = true;
				jitterValue = JitterValue(info[i], info[i].jitterData); //get jitter data
			}
			Vector3 waveValue = Vector3.zero; //universal
			Vector3 waveValueTopLeft = Vector3.zero;
			Vector3 waveValueTopRight = Vector3.zero;
			Vector3 waveValueBottomRight = Vector3.zero;
			Vector3 waveValueBottomLeft = Vector3.zero;
			if(info[i].waveData != null && info[i].size != 0){
				areWeAnimating = true;
				waveValue = WaveValue(info[i], info[i].waveData.main);
				if(info[i].waveData.individualVertexControl){
					waveValueTopLeft = WaveValue(info[i], info[i].waveData.topLeft);
					waveValueTopRight = WaveValue(info[i], info[i].waveData.topRight);
					waveValueBottomRight = WaveValue(info[i], info[i].waveData.bottomRight);
					waveValueBottomLeft = WaveValue(info[i], info[i].waveData.bottomLeft);
				}
			}
			Vector3 lowestLineOffset = lineCountLimit > 0 && lowestLine > lineCountLimit-1 ? new Vector3(0, info[i].ch.size * lineSpacing, 0) * (size /info[i].ch.size) * (lowestLine - lineCountLimit + 1) : Vector3.zero; //drop down
			//if text isn't different, you don't need to update UVs, or triangles
			//only need to update vertices of animated text
			//only need to update color of text w/ animated colors
			if(lineCountLimit > 0 && info[i].line < lowestLine - lineCountLimit + 1){
				endVerts[4*i + 0] = Vector3.zero;
				endVerts[4*i + 1] = Vector3.zero;
				endVerts[4*i + 2] = Vector3.zero;
				endVerts[4*i + 3] = Vector3.zero;
			}else{
				if(info[i].quadData == null){
					endVerts[4*i + 0] = (info[i].TopLeftVert + jitterValue + waveValueTopLeft + waveValue) + lowestLineOffset;
					endVerts[4*i + 1] = (info[i].TopRightVert + jitterValue + waveValueTopRight + waveValue) + lowestLineOffset;
					endVerts[4*i + 2] = (info[i].BottomRightVert + jitterValue + waveValueBottomRight + waveValue) + lowestLineOffset;
					endVerts[4*i + 3] = (info[i].BottomLeftVert + jitterValue + waveValueBottomLeft + waveValue) + lowestLineOffset;
					
				//Assign text UVs
					//this only needs to be changed on Rebuild()
					endUv[4*i + 0] = info[i].ch.uvTopLeft;
					endUv[4*i + 1] = info[i].ch.uvTopRight;
					endUv[4*i + 2] = info[i].ch.uvBottomRight;
					endUv[4*i + 3] = info[i].ch.uvBottomLeft;
				}else{
					endVerts[4*i + 0] = (info[i].RelativePos2(info[i].quadData.TopLeftVert) + jitterValue + waveValueTopLeft + waveValue) + lowestLineOffset;
					endVerts[4*i + 1] = (info[i].RelativePos2(info[i].quadData.TopRightVert) + jitterValue + waveValueTopRight + waveValue) + lowestLineOffset;
					endVerts[4*i + 2] = (info[i].RelativePos2(info[i].quadData.BottomRightVert) + jitterValue + waveValueBottomRight + waveValue) + lowestLineOffset;
					endVerts[4*i + 3] = (info[i].RelativePos2(info[i].quadData.BottomLeftVert) + jitterValue + waveValueBottomLeft + waveValue) + lowestLineOffset;
				
					endUv[4*i + 0] = info[i].quadData.UvTopLeft(GetTime);
					endUv[4*i + 1] = info[i].quadData.UvTopRight(GetTime);
					endUv[4*i + 2] = info[i].quadData.UvBottomRight(GetTime);
					endUv[4*i + 3] = info[i].quadData.UvBottomLeft(GetTime);

					if(info[i].quadData.columns > 1 && info[i].quadData.animDelay > 0f){
						areWeAnimating = true;
					}
				}
			}
			
		//Scrolling Textures:
			//make sure last character isn't a tab, space, or line break.
			if(info[i].textureData != null && (i != iL-1 || (i == iL-1 && info[i].TopRightVert != Vector3.zero))){ //not last character nothing!
				if(info[i].textureData.scrollSpeed != Vector2.zero){
					areWeAnimating = true; //update this every frame
				}
				Vector2 uvOffset = new Vector2(GetTime * info[i].textureData.scrollSpeed.x, GetTime * info[i].textureData.scrollSpeed.y); //animated offset
				//Vector2 uvMulti = Vector2
				
				float uv2Scale = 1f;
				if(info[i].textureData.scaleWithText){
					uv2Scale = 1f / info[i].size;
				}
				
				if(info[i].textureData.relativeToLetter){//keep uvs relative to each letter?
					//just draw texture as a square
					endUv2[4*i + 0] = uv2Scale * ((Vector2)endVerts[4*i + 0] - (Vector2)info[i].pos) + uvOffset - info[i].textureData.offset;
					endUv2[4*i + 1] = uv2Scale * ((Vector2)endVerts[4*i + 1] - (Vector2)info[i].pos) + uvOffset - info[i].textureData.offset;
					endUv2[4*i + 2] = uv2Scale * ((Vector2)endVerts[4*i + 2] - (Vector2)info[i].pos) + uvOffset - info[i].textureData.offset;
					endUv2[4*i + 3] = uv2Scale * ((Vector2)endVerts[4*i + 3] - (Vector2)info[i].pos) + uvOffset - info[i].textureData.offset;
				}else{
					endUv2[4*i + 0] = uv2Scale * (Vector2)endVerts[4*i + 0] + uvOffset - info[i].textureData.offset;
					endUv2[4*i + 1] = uv2Scale * (Vector2)endVerts[4*i + 1] + uvOffset - info[i].textureData.offset;
					endUv2[4*i + 2] = uv2Scale * (Vector2)endVerts[4*i + 2] + uvOffset - info[i].textureData.offset;
					endUv2[4*i + 3] = uv2Scale * (Vector2)endVerts[4*i + 3] + uvOffset - info[i].textureData.offset;
				}
			}

			if(info[i].quadData != null){ //quad silhouette?
				if(!info[i].quadData.silhouette){
					endUv2[4*i + 0] = endUv[4*i+0]; //same
					endUv2[4*i + 1] = endUv[4*i+1];
					endUv2[4*i + 2] = endUv[4*i+2];
					endUv2[4*i + 3] = endUv[4*i+3];
				}
			}

		//Color data:
			if(info[i].quadData != null && !info[i].quadData.silhouette){ //if it's a quad but not a silhouette
				endCol32[4*i + 0] = Color.white; //set color to be white, so it doesn't interfere with texture
				endCol32[4*i + 1] = Color.white;
				endCol32[4*i + 2] = Color.white;
				endCol32[4*i + 3] = Color.white;
			}else if(info[i].gradientData != null){ //gradient speed + gradient spread
				if(info[i].gradientData.scrollSpeed != 0){
					areWeAnimating = true;
				}
				switch(info[i].gradientData.direction){
					case STMGradientData.GradientDirection.Vertical:
						switch(info[i].gradientData.smoothGradient){
							case false: //hard gradient
								endCol32[4*i + 0] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (info[i].pos.y * info[i].gradientData.gradientSpread / info[i].size),1f));
								endCol32[4*i + 1] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (info[i].pos.y * info[i].gradientData.gradientSpread / info[i].size),1f));
								endCol32[4*i + 2] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (info[i].pos.y * info[i].gradientData.gradientSpread / info[i].size),1f));
								endCol32[4*i + 3] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (info[i].pos.y * info[i].gradientData.gradientSpread / info[i].size),1f));
								break;
							default:
								endCol32[4*i + 0] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + ((info[i].pos.y + info[i].size) * info[i].gradientData.gradientSpread / info[i].size),1f));
								endCol32[4*i + 1] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + ((info[i].pos.y + info[i].size) * info[i].gradientData.gradientSpread / info[i].size),1f));
								endCol32[4*i + 2] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (info[i].pos.y * info[i].gradientData.gradientSpread / info[i].size),1f));
								endCol32[4*i + 3] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (info[i].pos.y * info[i].gradientData.gradientSpread / info[i].size),1f));
								break;
						}
						break;
					default: //horizontal
						switch(info[i].gradientData.smoothGradient){
							case false:
								endCol32[4*i + 0] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (endVerts[4*i + 0].x * info[i].gradientData.gradientSpread / info[i].size),1f)); //this works!
								endCol32[4*i + 1] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (endVerts[4*i + 0].x * info[i].gradientData.gradientSpread / info[i].size),1f));
								endCol32[4*i + 2] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (endVerts[4*i + 0].x * info[i].gradientData.gradientSpread / info[i].size),1f));
								endCol32[4*i + 3] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (endVerts[4*i + 0].x * info[i].gradientData.gradientSpread / info[i].size),1f));
								break;
							default://smooth gradient
								endCol32[4*i + 0] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (endVerts[4*i + 0].x * info[i].gradientData.gradientSpread / info[i].size),1f)); //this works!
								endCol32[4*i + 1] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (endVerts[4*i + 1].x * info[i].gradientData.gradientSpread / info[i].size),1f));
								endCol32[4*i + 2] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (endVerts[4*i + 2].x * info[i].gradientData.gradientSpread / info[i].size),1f));
								endCol32[4*i + 3] = info[i].gradientData.gradient.Evaluate(Mathf.Repeat((GetTime * info[i].gradientData.scrollSpeed) + (endVerts[4*i + 3].x * info[i].gradientData.gradientSpread / info[i].size),1f));
								break;
						}
						break;
				}
			}else if(info[i].textureData != null){ //it has a texture
				endCol32[4*i + 0] = Color.white; //set color to be white, so it doesn't interfere with texture
				endCol32[4*i + 1] = Color.white;
				endCol32[4*i + 2] = Color.white;
				endCol32[4*i + 3] = Color.white;
			}else if(info[i].colorData != null){ //use colordata
				endCol32[4*i + 0] = info[i].colorData.color;
				endCol32[4*i + 1] = info[i].colorData.color;
				endCol32[4*i + 2] = info[i].colorData.color;
				endCol32[4*i + 3] = info[i].colorData.color;
			}else{ //use default color
				endCol32[4*i + 0] = color;
				endCol32[4*i + 1] = color;
				endCol32[4*i + 2] = color;
				endCol32[4*i + 3] = color;
			}
		}
		//If you want to modify vertices (curve, offset, etc) you can do it directly, here
		//ApplyCurveToVertices(endVerts);
		if(modifyVertices){
			Vector3[] middles = new Vector3[hyphenedText.Length]; //create an array with the middle of each letter
			Vector3[] positions = new Vector3[hyphenedText.Length];
			for(int i=0, iL=hyphenedText.Length; i<iL; i++){
				middles[i] = info[i].Middle;
				positions[i] = info[i].pos;
			}
			vertexMod.Invoke(endVerts, middles, positions); //modify end verts externally
			areWeAnimating = true; //just in case, so things like the sketch effect work.
		}
		//TODO: assign normals by hand instead of using this. but really, whatever. You dont need normals.
		//mesh.RecalculateNormals(); //2016-07-05 i dont need to do this
		//}
		if(data.disableAnimatedText || disableAnimatedText){
			areWeAnimating = false; //override constant updates
		}
		//else{
			//mesh.Optimize(); //not sure if this would actually help, since verts will rarely be shared
		//}
		//return mesh;
	}
	void SetMesh(float timeValue){
		SetMesh(timeValue, false);
	}
	//actually update the mesh attached to the meshfilter
	void SetMesh(float timeValue, bool undrawingMesh){ //0 => start mesh, < 0 => end mesh, > 0 => midway mesh
		if(textMesh == null){
			textMesh = new Mesh(); //create the mesh initially
			textMesh.MarkDynamic(); //just do it
		}
		textMesh.Clear();
		if(text.Length > 0){
			if(reading || unreading){ //which set to use...?
				UpdateDrawnMesh(timeValue, undrawingMesh);
				textMesh.vertices = midVerts;
				textMesh.colors32 = midCol32;
			}else if(timeValue == 0f){
				UpdatePreReadMesh(undrawingMesh); //pas this so it know which animation to use. always renders a pre-read mesh
				textMesh.vertices = startVerts;
				textMesh.colors32 = startCol32;
			}else{
				UpdateMesh();
				textMesh.vertices = endVerts;
				textMesh.colors32 = endCol32;
			}

			textMesh.uv = endUv; //this technically only needs to be set on Rebuild()
			textMesh.uv2 = endUv2; //use 2nd texture...

			//apply tris and submeshes
			if(subMeshes.Count > 1){ //this also only needs to be set on Rebuild()
				//use submeshes instead of setting triangles for entire mesh:
				textMesh.subMeshCount = subMeshes.Count;
				for(int i=0, iL=subMeshes.Count; i<iL; i++){
					textMesh.SetTriangles(subMeshes[i].tris, i); //apply to mesh
				}
			}else{
				textMesh.subMeshCount = 1;
				//set triangles for entire mesh:
				//textMesh.triangles = subMeshes[0].tris.ToArray();
				textMesh.SetTriangles(subMeshes[0].tris, 0); //causes less garbage?
			}
			//textMesh.UploadMeshData(false); //send to graphics API manually...?
		}
		if(uiMode){ //UI mode
			c.SetMesh(textMesh);
			if(!wrapText){
				RectTransform rect = c.transform as RectTransform;
	            rect.sizeDelta = new Vector2(textMesh.bounds.size.x, textMesh.bounds.size.y);
	            LayoutRebuilder.MarkLayoutForRebuild(rect);
			}
		}else{
			f.sharedMesh = textMesh; //I dont think this has to be set multiple times but w/e
		}
	}

	void ClearMaterials(){
		//clear r.sharedMaterials, here
		if(uiMode){
			for(int i=0, iL=c.materialCount; i<iL; i++){
				DestroyImmediate(c.GetMaterial(i));
			}
			c.materialCount = 0;
		}else{
			for(int i=0, iL=r.sharedMaterials.Length; i<iL; i++){
				DestroyImmediate(r.sharedMaterials[i]);
			}
		}
	}
	void ApplyMaterials(){ //turn submesh data into material data
		//do a check first to see if materials need to change
		ClearMaterials();

		Material[] newMats = new Material[subMeshes.Count];
		for(int i=0, iL=newMats.Length; i<iL; i++){
			newMats[i] = subMeshes[i].AsMaterial;
			//different details will have to be set here,
		}
		if(uiMode){//for now, simple way to disallow multiple materials on canvas, since it seems to cause a crash
			//2017-02-12 fixed it??
			//2017-04-14 FIXED IT
			#if UNITY_2017_1_OR_NEWER
				c.GetComponentInParent<Canvas>().additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
				c.materialCount = newMats.Length+1;
				for(int j=0; j<c.materialCount-1; j++){
					c.SetMaterial(newMats[j],j);
				}
			#else
				c.materialCount = 1;
				c.SetMaterial(newMats[0],0);
			#endif
		}else{
			r.sharedMaterials = newMats; //update!
		}
		#if UNITY_EDITOR
		HideInspectorStuff(); //this is the only time you're really gonna need this, so OnValidate() makes sense...?
		#endif
	}
	SubMeshData DoesSubmeshExist(SuperTextMesh stm, STMTextInfo info){ //find a submesh that this character can exist on
		for(int i=0, iL=subMeshes.Count; i<iL; i++){
			bool safe = true;

			if(info.materialData != null){ //it has material data?
				if(subMeshes[i].refMat != info.materialData.material){ //if the two materials dont match
					safe = false;
				}
			}else{ //there's no material data on this letter, so compare to STM default
				if(subMeshes[i].refMat != stm.textMat){ //
					safe = false;
				}
			}

			if(info.textureData != null){ //there's texture data?
				if(subMeshes[i].refMask != info.textureData.texture){ //if the two textures dont match...
					//return subMeshes[i];
					//Debug.Log("Existing textures dont match.");
					safe = false; //not the same!
				}
			}else{
				//vvv check for this, since quads can use the refmask, too
				if(info.quadData == null && subMeshes[i].refMask != null){ //if this submesh has texture data, is not null too
					safe = false;
					//Debug.Log("non-Existing textures dont match.");
				}
			}

			if(info.fontData != null){
				if(subMeshes[i].refFont != info.fontData.font){
					//return subMeshes[i];
					safe = false;
					//Debug.Log("Existing fonts dont match.");
				}
			}
			//submesh data ALWAYS has font, fontdata might not
			else{ //no fontdata on the mesh?
				if(subMeshes[i].refFont != stm.font){
					safe = false;
					//Debug.Log("non-Existing fonts dont match.");
				}
			}
			//TODO: check for silhouette differences, too?
			if(info.quadData != null){ //if it has quad data 
				if(subMeshes[i].refTex != info.quadData.texture){ //if the two textures aren't the same...
					safe = false;
				}
				if((subMeshes[i].refTex == subMeshes[i].refMask) == info.quadData.silhouette){ //if they're not both a silhouette
					safe = false;
				}
			}else{ //no quad data
				if(subMeshes[i].refTex != null){ //but the submesh does have it
					safe = false;
				}
			}
			

			if(safe){
				return subMeshes[i]; //the two submeshes are the same!
			}
		}
		//return new SubMeshData(stm, info);
		return null;
	}
}
[System.Serializable]
public class SubMeshData { //used internally by STM for keeping track of submeshes
	//public string name;
	public List<int> tris = new List<int>(); 
	public Material refMat; //material these tris will reference
	public Font refFont; //maybe make these FontData, TextureData, ShaderData?
	public Texture refTex; //for quads/inline images
	public Texture refMask; //masks/textures/non-silhouette quads
	public Vector2 maskTiling;
	public FilterMode refFilter;
	
	public SubMeshData(SuperTextMesh stm){ //create default
		this.refMat = stm.textMat; //default text material
		this.refFont = stm.font;
		this.refFilter = stm.filterMode;
	}
	public SubMeshData(SuperTextMesh stm, STMTextInfo info){ //from different data types
		//this.refMask = texData.texture;
		this.refMat = info.materialData != null ? info.materialData.material : stm.textMat;
		this.refFont = info.fontData != null ? info.fontData.font : stm.font;
		this.refFilter = info.quadData != null ? info.quadData.filterMode : info.fontData != null ? info.fontData.filterMode : stm.filterMode;
		this.refMask = info.textureData != null ? info.textureData.texture : null;
		this.maskTiling = info.textureData != null ? info.textureData.tiling : Vector2.one;
		if(info.quadData != null && !info.quadData.silhouette){ //nah, use quad instead...
			this.refMask = info.quadData.texture;
		}
		this.refTex = info.quadData != null ? info.quadData.texture : null;
	}
	public Material AsMaterial{
		get{
			//create new material
			Material newMat = new Material(refMat.shader);
			newMat.CopyPropertiesFromMaterial(refMat);
			newMat.SetTexture("_MainTex", refTex ?? refFont.material.mainTexture); //go w/ reftex unless its null, then use font
			newMat.SetTexture("_MaskTex", refMask);
			newMat.SetTextureScale("_MaskTex", maskTiling);
			newMat.mainTexture.filterMode = refFilter;
			return newMat;
		}
	}
}
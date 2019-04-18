//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Quad Data", menuName = "Super Text Mesh/Quad Data", order = 1)]
public class STMQuadData : ScriptableObject{
	public Texture texture; //materials should automatically join if texture matches
	[Tooltip("If a quad is a silhouette, it won't use the color from its texture, just the alpha. If it's a silhouette, it can be effected by text color.")]
	public bool silhouette = false;
	public FilterMode filterMode = FilterMode.Bilinear; //default
	//[Range(1,64)]

	public int columns = 1; //how many columns it will have, MUST stay above 0
	public int rows = 1;
	public int iconIndex = 0; //for non-animating icons

	

	public float animDelay = 0f; //delay between frames, if <= 0, no animation
	public int[] frames;
	//public int currentFrame = 0; //keep track of frame
	public Vector2 size = Vector2.one; //1,1 means full width & height of a normal letter. determines spacing.
	public Vector3 offset = Vector3.zero;
	//public Vector2 tiling = Vector2.one; //the uvs of the texture to use
	//public Vector2 offset = Vector2.zero;
	public float advance; //spacing afterwards...?
/*
	public Vector2 tiling{ //get the tiling for this texture
		get{

		}
	}
*/
    public STMQuadData CopyTo()
    {
        STMQuadData result = new STMQuadData();
        result.texture = this.texture;
        result.silhouette = this.silhouette;
        result.filterMode = this.filterMode;
        result.columns = this.columns;
        result.rows = this.rows;
        result.iconIndex = this.iconIndex;
        result.animDelay = this.animDelay;
        result.frames = this.frames;
        result.size = this.size;
        result.offset = this.offset;
        result.advance = this.advance;
        return result;
    }
    public Vector3 TopLeftVert{ //shorthand for the corners
		get{
			return new Vector3(0f, size.y, 0f) + offset; 
		}
	}
	public Vector3 TopRightVert{ //shorthand for the corners
		get{
			return new Vector3(size.x, size.y, 0f) + offset;
		}
	}
	public Vector3 BottomRightVert{ //shorthand for the corners
		get{
			return new Vector3(size.x, 0f, 0f) + offset;
		}
	}
	public Vector3 BottomLeftVert{ //shorthand for the corners
		get{
			return new Vector3(0f, 0f, 0f) + offset;
		}
	}

	public Vector2 UvTopLeft(float myTime){ 
		return new Vector2(0f, uvSize.y) + UvOffset(myTime); 
	}
	public Vector2 UvTopRight(float myTime){ 
		return uvSize + UvOffset(myTime);
	}
	public Vector2 UvBottomRight(float myTime){ 
		return new Vector2(uvSize.x, 0f) + UvOffset(myTime);
	}
	public Vector2 UvBottomLeft(float myTime){ 
		return UvOffset(myTime);
	}
	private Vector2 uvSize {
		get{
			return new Vector2(1f / (float)columns, 1f / (float)rows);
		}
	}
	private Vector2 UvOffset(float myTime){
		FixColumnCount();
		/*
		XNXX N would be at position "13"
		XXOX O would be at position "10"
		XXXX grid width is columns wide
		XXXX use modulo
		*/
		int myIconIndex;
		if((columns > 1 || rows > 1) && animDelay > 0f && frames.Length > 0f){
			myIconIndex = frames[(int)Mathf.Floor(myTime / animDelay) % frames.Length]; //dont bother wrapping frames, since they're set manually
			//myIconIndex %= (columns * columns); //wrap...
		}else{
			myIconIndex = iconIndex % (columns * rows); //wrap it!
		}
		
		//10.4 seconds in, 0.2 sec delay, get index 2 (3rd one)
		
		int row = (int)Mathf.Floor((float)myIconIndex / (float)rows);
		//int row = myIconIndex % rows;
		int column = myIconIndex % columns;
		return new Vector2((float)column / (float)columns, (float)row / (float)rows);
	}
	void OnValidate(){
		FixColumnCount();
	}
	void FixColumnCount(){
		if(columns < 1) columns = 1;
		if(rows < 1)rows = 1;
	}
	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty texture = serializedData.FindProperty("texture");
		SerializedProperty filterMode = serializedData.FindProperty("filterMode");
		SerializedProperty columns = serializedData.FindProperty("columns");
		SerializedProperty rows = serializedData.FindProperty("rows");
		SerializedProperty iconIndex = serializedData.FindProperty("iconIndex");
		SerializedProperty size = serializedData.FindProperty("size");
		SerializedProperty offset = serializedData.FindProperty("offset");
		SerializedProperty advance = serializedData.FindProperty("advance");
		SerializedProperty animDelay = serializedData.FindProperty("animDelay");
		SerializedProperty frames = serializedData.FindProperty("frames");
		SerializedProperty silhouette = serializedData.FindProperty("silhouette");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(texture);
		if(this.texture != null){
			EditorGUILayout.PropertyField(silhouette);
			EditorGUILayout.PropertyField(filterMode);
			EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.PropertyField(columns);
			EditorGUILayout.PropertyField(rows);
			EditorGUILayout.Space(); //////////////////SPACE
			if(this.animDelay <= 0f){
				EditorGUILayout.PropertyField(iconIndex); //use single icon index
			}
			if(this.columns > 1){
				EditorGUILayout.PropertyField(animDelay);
				if(this.animDelay > 0f){
					EditorGUILayout.PropertyField(frames, true); //iterate thru multiple
				}
			}
			EditorGUILayout.Space(); //////////////////SPACE
		}
		EditorGUILayout.PropertyField(size);
		EditorGUILayout.PropertyField(offset);
		EditorGUILayout.PropertyField(advance);
		EditorGUILayout.Space(); //////////////////SPACE
		//FixColumnCount();
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif

}
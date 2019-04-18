//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //converting arrays to dictionaries
using System.IO; //for getting folders
///NOTE: only re-create STMTextInfo if RAW TEXT changes
#if UNITY_EDITOR
using UnityEditor; //for loading default stuff and menu thing
#endif
[System.Serializable]
public class STMTextInfo{ //info for an individual letter. used internally by super text mesh. References back to textdata. (info[i], per-letter)
	//MAKE SURE TO ADD VARIABLE TO CONSTRUCTOR WHEN MAKING A NEW ONE
	//public Font font; //what font this character info is... wait you have FontData, otherwise use mesh default
	public CharacterInfo ch; //contains uv data, point size (quality), style, etc
	//public char c; //nah, just sync it w/ hyphenedText[]. it works better cause hyphenedtext will always be shorter
	public Vector3 pos; //where the bottom-left corner is

	public float readTime = -1f; //at what time it will start to get read.
	public float unreadTime = -1f;
	public int line = 0; //what line this text is on
	public float indent = 0f; //distance from left-oriented margin that a new row will start from
	public STMDrawAnimData drawAnimData; //which draw animation will be used
	public float size; //localspace size
	public SuperTextMesh.Alignment alignment; //how this text will be aligned
	public SuperTextMesh.DrawOrder drawOrder;
	//public Color32 color = Color.white; //the actual color it will render at, not the color data!
	//public int delayCount; //the delay count...

	public List<string> ev = new List<string>(); //event strings.
	public List<string> ev2 = new List<string>(); //repeating event strings.
	public STMColorData colorData; //reference or store...
	public STMGradientData gradientData; //reference stuff??
	public STMTextureData textureData;

	//vvvADDITIONAL delay!
	public STMDelayData delayData; //units to delay for, before this text is shown. multiple of read speed. ADDITIONAL DELAY
	
	public STMWaveData waveData;
	public STMJitterData jitterData;

//	public STMImageData imageData; //if not null, put this referenced image inline
	
	//public STMVoiceData voiceData; //if not null, this will override the text mesh's settings????? ??????
	public float readDelay = 0f; //delay between lettes, for setting up timing
	public STMAudioClipData audioClipData;
	public bool stopPreviousSound;
	public SuperTextMesh.PitchMode pitchMode;
	public float overridePitch;
	public float minPitch;
	public float maxPitch;
	public float speedReadPitch;
	public STMFontData fontData;
	public STMQuadData quadData;
	public STMMaterialData materialData;
	public STMSoundClipData soundClipData;
	//public int submeshNumber = -1;

	public Vector3 TopLeftVert{ //return position in local space
		get{
			return RelativePos( new Vector3(ch.minX, ch.maxY, 0f) ); 
		}
	}
	public Vector3 TopRightVert{ //return position in local space
		get{
			return RelativePos( new Vector3(ch.maxX, ch.maxY, 0f) );
		}
	}
	public Vector3 BottomRightVert{ //return position in local space
		get{
			return RelativePos( new Vector3(ch.maxX, ch.minY, 0f) );
		}
	}
	public Vector3 BottomLeftVert{ //return position in local space
		get{
			return RelativePos( new Vector3(ch.minX, ch.minY, 0f) );
		}
	}
	public Vector3 Middle{
		get{
			return RelativePos( new Vector3((ch.minX + ch.maxX) * 0.5f, (ch.minY + ch.maxY) * 0.5f, 0f) );
		}
	}
	public Vector3 RelativePos(Vector3 yeah){
		return pos + yeah * (size / ch.size); //ch.size is quality
	}
	public Vector3 RelativePos2(Vector3 yeah){ //for quads
		return pos + yeah * size; //ch.size is quality
	}
	public Vector3 Advance(float extraSpacing, float myQuality){ //for getting letter position and autowrap data
		if(quadData != null){
			return new Vector3((quadData.size.x + quadData.advance) + (extraSpacing * size), 0,0) * (size);
		}else{
			return new Vector3(ch.advance + (extraSpacing * size), 0,0) * (size / myQuality);
		}
	}
	public STMTextInfo(){ //dont use this unless ur gonna override it
		this.ch = new CharacterInfo();
		this.pos = Vector3.zero;
		this.line = 0;
		this.indent = 0;
		//this.colorData = ScriptableObject.CreateInstance<STMColorData>();
//		this.colorData.color = Color.white;
		this.size = 16;
		//this.delayData = ScriptableObject.CreateInstance<STMDelayData>();
		this.ev = new List<string>();
		this.ev2 = new List<string>();
		this.readTime = -1f;
		this.unreadTime = -1f;
	}
	public STMTextInfo(SuperTextMesh stm){ //for setting "defaults"
		this.ch.style = stm.style;
		//this.colorData = ScriptableObject.CreateInstance<STMColorData>();
//		this.colorData.color = stm.color;
		this.indent = 0;
		this.size = stm.size;
		this.alignment = stm.alignment;
		this.stopPreviousSound = stm.stopPreviousSound;
		this.pitchMode = stm.pitchMode;
		this.overridePitch = stm.overridePitch;
		this.minPitch = stm.minPitch;
		this.maxPitch = stm.maxPitch;
		this.speedReadPitch = stm.speedReadPitch;
		this.readDelay = stm.readDelay;
		this.drawAnimData = Resources.Load<STMDrawAnimData>("STMDrawAnims/" + stm.drawAnimName);
		if(this.drawAnimData == null){
			STMDrawAnimData[] tmpDrawAnims = Resources.LoadAll<STMDrawAnimData>("STMDrawAnims");
			if(tmpDrawAnims.Length > 0){
				this.drawAnimData = tmpDrawAnims[0]; //get first one
			}
		}
		this.drawOrder = stm.drawOrder;
	}

	public STMTextInfo(STMTextInfo clone, CharacterInfo ch) : this(clone){ //clone everything but character. used for auto hyphens
		this.ch = ch;
		this.quadData = null; //yeah or else it gets weird. it's gonna be a hyphen/space so whatever!!
	}
	
	public STMTextInfo(STMTextInfo clone){
		this.ch = clone.ch;
		this.pos = clone.pos;
		this.line = clone.line;
		this.indent = clone.indent;
		this.ev = new List<string>(clone.ev);
		this.ev2 = new List<string>(clone.ev2);
		this.colorData = clone.colorData;
		//this.colorData = ScriptableObject.CreateInstance<STMColorData>();
//		this.colorData.color = clone.colorData.color;
		this.gradientData = clone.gradientData;
		this.textureData = clone.textureData;
		this.size = clone.size;
		//this.delayData = ScriptableObject.CreateInstance<STMDelayData>();
//		this.delayData.count = clone.delayData.count;
		this.delayData = clone.delayData;
		this.waveData = clone.waveData;
		this.jitterData = clone.jitterData;
		this.alignment = clone.alignment;

		this.readTime = clone.readTime;
		this.unreadTime = clone.unreadTime;
		this.drawAnimData = clone.drawAnimData;

		//this.clipsFolderName = clone.clipsFolderName;
		this.audioClipData = clone.audioClipData;
		this.stopPreviousSound = clone.stopPreviousSound;
		this.pitchMode = clone.pitchMode;
		this.overridePitch = clone.overridePitch;
		this.minPitch = clone.minPitch;
		this.maxPitch = clone.maxPitch;
		this.speedReadPitch = clone.speedReadPitch;
		this.readDelay = clone.readDelay;
		this.drawOrder = clone.drawOrder;

		//this.submeshNumber = clone.submeshNumber;
		this.fontData = clone.fontData;
		this.quadData = clone.quadData;
		this.materialData = clone.materialData;
		this.soundClipData = clone.soundClipData;
	}
}
/*
NEW TAGS TO ADD:
character spacing tag
line height tag? (work automatically with size)
midway center alignment?? oh boy... maybe make it a special tag that can only be used at the start?
*/

/*
[System.Serializable]
public class STMImageData : ScriptableObject{ //inline images
	public string name;
	public Texture[] frames;
	public int rate;
}
*/

[CreateAssetMenu(fileName = "New Text Data", menuName = "Super Text Mesh/Super Text Mesh Data", order = 0)]
public class SuperTextMeshData : ScriptableObject { //the actual textdata manager file
	
	[HideInInspector] public bool textDataEditMode = false; //whther this will show on objects or not


	[HideInInspector] public bool showEffectsFoldout = false;

	[HideInInspector] public bool showWavesFoldout = false;
	public Dictionary<string,STMWaveData> waves = new Dictionary<string,STMWaveData>();
	[HideInInspector] public bool showJittersFoldout = false;
	public Dictionary<string,STMJitterData> jitters = new Dictionary<string,STMJitterData>();
	[HideInInspector] public bool showDrawAnimsFoldout = false;
	public Dictionary<string,STMDrawAnimData> drawAnims = new Dictionary<string,STMDrawAnimData>();


	[HideInInspector] public bool showTextColorFoldout = false;

	[HideInInspector] public bool showColorsFoldout = false;
	public Dictionary<string,STMColorData> colors = new Dictionary<string,STMColorData>();
	[HideInInspector] public bool showGradientsFoldout = false;
	public Dictionary<string,STMGradientData> gradients = new Dictionary<string,STMGradientData>();
	[HideInInspector] public bool showTexturesFoldout = false;
	public Dictionary<string,STMTextureData> textures = new Dictionary<string,STMTextureData>();


    [HideInInspector] public bool showInlineFoldout = false;

    [HideInInspector] public bool showDelaysFoldout = false;
    public Dictionary<string,STMDelayData> delays = new Dictionary<string,STMDelayData>();
    [HideInInspector] public bool showVoicesFoldout = false;
    public Dictionary<string,STMVoiceData> voices = new Dictionary<string,STMVoiceData>();
    [HideInInspector] public bool showFontsFoldout = false;
    public Dictionary<string,STMFontData> fonts = new Dictionary<string,STMFontData>();
    [HideInInspector] public bool showSoundClipsFoldout = false;
    public Dictionary<string,STMSoundClipData> soundClips = new Dictionary<string,STMSoundClipData>();
    //public List<bool> showSoundClipFoldout = new List<bool>();
    [HideInInspector] public bool showAudioClipsFoldout = false;
    public Dictionary<string,STMAudioClipData> audioClips = new Dictionary<string,STMAudioClipData>();
    [HideInInspector] public bool showQuadsFoldout = false;
    public Dictionary<string,STMQuadData> quads = new Dictionary<string,STMQuadData>();
    [HideInInspector] public bool showMaterialsFoldout = false;
    public Dictionary<string,STMMaterialData> materials = new Dictionary<string,STMMaterialData>();


    [HideInInspector] public bool showAutomaticFoldout = false;

    [HideInInspector] public bool showAutoClipsFoldout = false;
    public Dictionary<string,STMAutoClipData> autoClips = new Dictionary<string,STMAutoClipData>();
    [HideInInspector] public bool showAutoDelaysFoldout = false;
    public Dictionary<string,STMDelayData> autoDelays = new Dictionary<string,STMDelayData>();


    [HideInInspector] public bool showMasterFoldout = true;

    public bool disableAnimatedText = false;
	public string autoVoice;

    public void RebuildDictionaries(){
    	waves = Resources.LoadAll<STMWaveData>("STMWaves").ToDictionary(x => x.name, x => x);
    	jitters = Resources.LoadAll<STMJitterData>("STMJitters").ToDictionary(x => x.name, x => x);
    	drawAnims = Resources.LoadAll<STMDrawAnimData>("STMDrawAnims").ToDictionary(x => x.name, x => x);

    	colors = Resources.LoadAll<STMColorData>("STMColors").ToDictionary(x => x.name, x => x);
    	gradients = Resources.LoadAll<STMGradientData>("STMGradients").ToDictionary(x => x.name, x => x);
    	textures = Resources.LoadAll<STMTextureData>("STMTextures").ToDictionary(x => x.name, x => x);

    	delays = Resources.LoadAll<STMDelayData>("STMDelays").ToDictionary(x => x.name, x => x);
    	voices = Resources.LoadAll<STMVoiceData>("STMVoices").ToDictionary(x => x.name, x => x);
    	fonts = Resources.LoadAll<STMFontData>("STMFonts").ToDictionary(x => x.name, x => x);

    	soundClips = Resources.LoadAll<STMSoundClipData>("STMSoundClips").ToDictionary(x => x.name, x => x);
    	audioClips = Resources.LoadAll<STMAudioClipData>("STMAudioClips").ToDictionary(x => x.name, x => x);
    	quads = Resources.LoadAll<STMQuadData>("STMQuads").ToDictionary(x => x.name, x => x);
    	materials = Resources.LoadAll<STMMaterialData>("STMMaterials").ToDictionary(x => x.name, x => x);

    	autoClips = Resources.LoadAll<STMAutoClipData>("STMAutoClips").ToDictionary(x => x.name, x => x);
    	autoDelays = Resources.LoadAll<STMDelayData>("STMAutoDelays").ToDictionary(x => x.name, x => x);

    }
}

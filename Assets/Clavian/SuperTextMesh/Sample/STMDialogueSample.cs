using UnityEngine;
using System.Collections;

public class STMDialogueSample : MonoBehaviour {
	public SuperTextMesh textMesh;
	public KeyCode advanceKey = KeyCode.Return;
	public SpriteRenderer advanceKeySprite;
	private Vector3 advanceKeyStartScale = Vector3.one;
	public Vector3 advanceKeyScale = Vector3.one;
	public float advanceKeyTime = 1f;
	public string[] lines;
	private int currentLine = 0;

	void Start () {
		advanceKeyStartScale = advanceKeySprite.transform.localScale;
		Apply();
	}
	public void CompletedDrawing(){
		Debug.Log("I completed reading! Done!");
	}
	public void CompletedUnreading(){
		Debug.Log("I completed unreading!! Bye!");
		Apply(); //go to next line
	}
	void Apply () {
		
		//isDoneFading = false;
		textMesh.Text = lines[currentLine]; //invoke accessor so rebuild() is called
		currentLine++; //move to next line of dialogue...
		currentLine %= lines.Length; //or loop back to first one
	}
	void Update () {
		if(Input.GetKey(advanceKey)){
			if(textMesh.reading){ //is text being read out?
				textMesh.SpeedRead(); //show all text, or speed up
			}
			else if(!textMesh.Continue() && !textMesh.unreading){ //call Continue(), if no need to continue, advance to next box.
				textMesh.UnRead();
				//Apply();
			}
			advanceKeySprite.transform.localScale = advanceKeyScale;
		}else{
			advanceKeySprite.transform.localScale = Vector3.Lerp(advanceKeySprite.transform.localScale, advanceKeyStartScale, Time.deltaTime * advanceKeyTime);
		}
		if(Input.GetKeyUp(advanceKey)){
			textMesh.RegularRead(); //return to normal reading speed, if possible.
		}
	}
}
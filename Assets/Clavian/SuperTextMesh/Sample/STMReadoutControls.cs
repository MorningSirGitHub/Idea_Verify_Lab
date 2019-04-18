using UnityEngine;
using System.Collections;

public class STMReadoutControls : MonoBehaviour {
	public SuperTextMesh stm;
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			stm.Read();
		}
	}
}

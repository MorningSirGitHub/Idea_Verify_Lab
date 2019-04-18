//Copyright (c) 2017 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

public class STMContinue : MonoBehaviour {
	public SuperTextMesh stm;
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			stm.Continue();
		}
	}
}

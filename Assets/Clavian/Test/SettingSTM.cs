using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingSTM : MonoBehaviour
{

    public SuperTextMesh superTextMesh;

    private Vector2 m_HPQuadsSize;
    private Vector2 m_BGQuadsSize;

    void Start()
    {
        m_HPQuadsSize = superTextMesh.data.quads["test"].size;
        m_BGQuadsSize = superTextMesh.data.quads["BG"].size;

        superTextMesh.data.quads["test"].size.x = 5;
        superTextMesh.data.quads["BG"].size.x = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (superTextMesh.data.quads["BG"].size.x < 0)
                return;

            superTextMesh.data.quads["test"].size.x += Time.deltaTime * 5;
            superTextMesh.data.quads["BG"].size.x -= Time.deltaTime * 5;
            superTextMesh.Rebuild();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (superTextMesh.data.quads["test"].size.x < 0)
                return;

            superTextMesh.data.quads["test"].size.x -= Time.deltaTime * 5;
            superTextMesh.data.quads["BG"].size.x += Time.deltaTime * 5;
            superTextMesh.Rebuild();
        }

        if (Input.GetKeyDown(KeyCode.P))
            UnityEditor.AssetDatabase.CreateAsset(superTextMesh.textMesh, UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Clavian/Test/MeshView/MeshView.asset"));
    }

    void OnDisable()
    {
        superTextMesh.data.quads["test"].size = m_HPQuadsSize;
        superTextMesh.data.quads["BG"].size = m_BGQuadsSize;
    }

}

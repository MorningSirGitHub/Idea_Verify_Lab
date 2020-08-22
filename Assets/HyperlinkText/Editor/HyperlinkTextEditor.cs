#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using Object = UnityEngine.Object;

[CustomEditor(typeof(HyperlinkText), true)]
[CanEditMultipleObjects]
public class HyperlinkTextEditor : UnityEditor.UI.TextEditor
{

    #region 创建UI

    private Material m_Selection;
    private static bool m_ShowPickerWindow = false;
    private static string m_DefaultEmojiMaterialPath = "Assets/Resources/Emoji/Default/default_mat.mat";
    private SerializedProperty m_UnderLineOffset;
    private static HyperlinkText m_HyperlinkText;
    private SerializedProperty m_EmojiType;

    protected override void OnEnable()
    {
        m_HyperlinkText = target as HyperlinkText;
        m_EmojiType = serializedObject.FindProperty("EmojiType");
        m_UnderLineOffset = serializedObject.FindProperty("UnderLineOffset");

        base.OnEnable();
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(m_EmojiType);
        EditorGUILayout.PropertyField(m_UnderLineOffset);

        //if (GUILayout.Button("ShowPickerWindow"))
        if (m_ShowPickerWindow)
        {
            m_ShowPickerWindow = false;
            var controlID = EditorGUIUtility.GetControlID(FocusType.Keyboard);
            EditorGUIUtility.ShowObjectPicker<Material>(m_Selection, false, "emoji_mat", controlID);
            m_Selection = EditorGUIUtility.GetObjectPickerObject() as Material;
        }
        if (Event.current.commandName == "ObjectSelectorUpdated")
        {

        }
        else if (Event.current.commandName == "ObjectSelectorClosed")
        {

        }

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }

    [MenuItem("GameObject/UI/HyperlinkText", false, 1999)]
    static public void AddHyperlinkText(MenuCommand menuCommand)
    {
        var hyperlinkText = CreateHyperlinkText();
        PlaceUIElement(hyperlinkText.gameObject, menuCommand);
    }
    private static HyperlinkText CreateHyperlinkText()
    {
        //DefaultControls.CreateText(new DefaultControls.Resources());
        var go = CreateUIElement("HyperlinkText", new Vector2(200, 30));
        var hyperText = go.AddComponent<HyperlinkText>();
        hyperText.text = "New HyperlinkText";
        hyperText.font = Font.CreateDynamicFontFromOSFont("Arial", 20);
        hyperText.alignment = TextAnchor.MiddleCenter;
        hyperText.supportRichText = true;
        hyperText.raycastTarget = true;
        hyperText.material = AssetDatabase.LoadAssetAtPath<Material>(m_DefaultEmojiMaterialPath);
        //CheckMaterial(hyperText);
        return hyperText;
    }
    private static GameObject CreateUIElement(string name, Vector2 size)
    {
        var child = new GameObject(name);
        var rectTransform = child.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = size;
        return child;
    }
    private static void PlaceUIElement(GameObject element, MenuCommand menuCommand)
    {
        var parent = menuCommand.context as GameObject;
        var explicitParentChoice = true;
        if (parent == null)
        {
            parent = GetOrCreateCanvasGameObject();
            explicitParentChoice = false;
        }

        if (parent.GetComponentsInParent<Canvas>(true).Length == 0)
        {
            var canvasObj = CreateNewUI();
            canvasObj.transform.SetParent(parent.transform, false);
            parent = canvasObj;
        }

        SceneManager.MoveGameObjectToScene(element, parent.scene);

        Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
        Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
        Undo.SetCurrentGroupName("Create " + element.name);

        GameObjectUtility.SetParentAndAlign(element, parent);

        if (!explicitParentChoice)
            SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

        Selection.activeGameObject = element;
    }
    private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
    {
        SceneView sceneView = SceneView.lastActiveSceneView;

        // Couldn't find a SceneView. Don't set position.
        if (sceneView == null || sceneView.camera == null)
            return;

        // Create world space Plane from canvas position.
        Vector2 localPlanePosition;
        Camera camera = sceneView.camera;
        Vector3 position = Vector3.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
        {
            // Adjust for canvas pivot
            localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
            localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

            localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
            localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

            // Adjust for anchoring
            position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
            position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

            Vector3 minLocalPosition;
            minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
            minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

            Vector3 maxLocalPosition;
            maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
            maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

            position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
            position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
        }

        itemTransform.anchoredPosition = position;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.localScale = Vector3.one;
    }
    static public GameObject GetOrCreateCanvasGameObject()
    {
        var selectedGo = Selection.activeGameObject;

        var canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
        if (IsValidCanvas(canvas))
            return canvas.gameObject;

        var canvasArray = GameObject.FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvasArray.Length; i++)
        {
            if (IsValidCanvas(canvasArray[i]))
                return canvasArray[i].gameObject;
        }

        return CreateNewUI();
    }
    static bool IsValidCanvas(Canvas canvas)
    {
        if (canvas == null || !canvas.gameObject.activeInHierarchy)
            return false;

        if (EditorUtility.IsPersistent(canvas) || (canvas.hideFlags & HideFlags.HideInHierarchy) != 0)
            return false;

        return true;
    }
    static public GameObject CreateNewUI(GameObject parent = null)
    {
        var root = new GameObject("Canvas");
        root.layer = LayerMask.NameToLayer("UI");
        var canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        root.AddComponent<CanvasScaler>();
        root.AddComponent<GraphicRaycaster>();
        var esys = GameObject.FindObjectOfType<EventSystem>();
        if (esys == null)
        {
            var eventSystem = new GameObject("EventSystem");
            GameObjectUtility.SetParentAndAlign(eventSystem, parent);
            esys = eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
        }
        Selection.activeGameObject = esys.gameObject;
        return root;
    }

    private static void CheckMaterial(HyperlinkText text)
    {
        // 默认材质都为 Emoji
        Material material = null;
        var matPath = m_DefaultEmojiMaterialPath;
        if (File.Exists(matPath))
        {
            material = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        }
        // 没有必要再去检测和创建，在创建图集的时候已经创建了资源，如果没有就空置 Text 的材质
        if (PickerTypeOf(new string[] { "", "mat" }, ref material))
        {
            material.shader = Shader.Find("UI/EmojiFont");
        }
        else
        {
            material = new Material(Shader.Find("UI/EmojiFont"));
            var removeIndex = matPath.LastIndexOf("/");
            var defaultPath = matPath.Remove(removeIndex);
            AssetDatabase.CreateAsset(material, defaultPath);
        }
        text.material = material;
    }
    private static bool PickerTypeOf<T>(string[] filterType, ref T assets) where T : UnityEngine.Object
    {
        if (assets != null)
            return true;

        var path = EditorUtility.OpenFilePanelWithFilters("请选择Emoji材质路径", Application.dataPath, filterType);
        var startIndex = path.IndexOf("Assets");
        path = path.Substring(startIndex);
        path.Replace('\\', '/');
        assets = AssetDatabase.LoadAssetAtPath<T>(path);
        return assets;
    }

    #endregion

    #region 创建Emoji表情图集

    private const string Title = "Hyperlink Text";

    [MenuItem("Tools/Build Emoji Packer &R")]
    static void EmojiPacker()
    {
        var searchWindow = EditorTools.CreatePathSelectWindow("Emoji 资源选择", new Vector2(300, 500), string.Empty, false);
        searchWindow.SearchPath = "Assets/EmojiSource";
        searchWindow.OutputPath = "Assets/Resources/Emoji";
        searchWindow.SetOKListener((objlist, pngList) =>
        {
            for (int i = 0; i < objlist.Count; i++)
            {
                Selection.activeObject = objlist[i];
                BuildEmoji(pngList[i]);
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(pngList[i], typeof(Object)));
            }
        });
    }
    static void BuildEmoji(string pngPath)
    {
        Dictionary<string, List<AssetInfo>> textureAssetsDic = new Dictionary<string, List<AssetInfo>>();
        Texture2D[] textures = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);

        EditorTools.ResetProgressBar(textures.Length);
        int totalFrames = 0, size = 0;
        foreach (var texture in textures)
        {
            if (texture == null)
                continue;

            var match = Regex.Match(texture.name, "^(([a-zA-Z0-9]+(-?))+)(_([0-9]+))?$");//name_index or name (name = xxx-xxx-xxx...)
            if (!match.Success)
            {
                Debug.Log(texture.name + " 不匹配命名规则，跳过\n");
                continue;
            }
            int index;
            if (!int.TryParse(match.Groups[3].Value, out index))
                index = 1;

            List<AssetInfo> infos;
            string title = match.Groups[1].Value;
            if (!textureAssetsDic.TryGetValue(title, out infos))
            {
                infos = new List<AssetInfo>();
                textureAssetsDic.Add(title, infos);
            }
            infos.Add(new AssetInfo() { index = index, texture = texture });

            size = Math.Max(texture.width, size);
            size = Math.Max(texture.height, size);

            totalFrames++;
            EditorTools.UpdateProgressBar(Title, "正在过滤选中表情...{0}", texture.name);
        }

        EditorTools.ResetProgressBar(textureAssetsDic.Values.Count);
        foreach (var info in textureAssetsDic.Values)
        {
            info.Sort((x, y) => y.index - x.index);
            EditorTools.UpdateProgressBar(Title, "正在进行动态表情排序...");
        }

        if (totalFrames == 0)
            return;

        // 计算图集大小 仅支持 2^n
        int lineCount = 0;
        int texSize = ComputeAtlasSize(totalFrames, ref size, ref lineCount);
        if (texSize < 1)
        {
            EditorUtility.DisplayDialog("Hyperlink Text", "未能构建合适大小的图集", "退出");
            EditorTools.ResetProgressBar();
            return;
        }

        var keys = textureAssetsDic.Keys.ToList();
        keys.Sort((a, b) => string.Compare(a, b, StringComparison.Ordinal));

        EditorTools.ResetProgressBar(totalFrames);
        List<SpriteInfo> sprites = new List<SpriteInfo>();
        Texture2D atlas = new Texture2D(texSize, texSize);
        int idx = 0;
        foreach (var key in keys)
        {
            var assetsInfoList = textureAssetsDic[key];
            sprites.Add(new SpriteInfo(key, assetsInfoList.Count, idx, idx / lineCount, idx % lineCount));
            foreach (var assetInfo in assetsInfoList)
            {
                int w = assetInfo.texture.width;
                int h = assetInfo.texture.height;

                int x = idx % lineCount;
                int y = idx / lineCount;

                Color[] colors = assetInfo.texture.GetPixels(0, 0, w, h);
                atlas.SetPixels(x * size, y * size, w, h, colors);

                idx++;
                EditorTools.UpdateProgressBar(Title, "正在生成图集...{0}", key);
            }
        }

        EditorTools.ResetProgressBar(sprites.Count);
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Key\tFrame\tIndex\tLineNum\tLineIndex");
        foreach (var spriteInfo in sprites)
        {
            builder.AppendLine(spriteInfo.ToString());
            EditorTools.UpdateProgressBar(Title, "正在生成配置表...{0}", spriteInfo.ToString());
        }

        EditorTools.ResetProgressBar();
        //string pngPath = EditorUtility.SaveFilePanel("Select Save Path", "Assets/_Res/UI/Sprites/ResourcesAB/Emoji", "default", "png");
        if (string.IsNullOrEmpty(pngPath))
            return;

        var txtPath = pngPath.Replace(".png", "_config.txt");
        byte[] bytes = atlas.EncodeToPNG();
        File.WriteAllBytes(pngPath, bytes);
        File.WriteAllText(txtPath, builder.ToString());
        AssetDatabase.ImportAsset(pngPath);

        var matPath = pngPath.Replace(".png", "_mat.mat");
        var shader = Shader.Find("UI/EmojiFont");
        var hasMat = File.Exists(matPath);
        var material = hasMat ? AssetDatabase.LoadAssetAtPath<Material>(matPath) : new Material(shader);
        var emojiTex = AssetDatabase.LoadAssetAtPath<Texture2D>(pngPath);
        material.SetTexture("_EmojiTex", emojiTex);
        material.SetFloat("_EmojiSize", (float)size / texSize);
        material.SetFloat("_LineCount", lineCount);
        if (hasMat)
        {
            material.shader = shader;
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
        }
        else
        {
            AssetDatabase.CreateAsset(material, matPath);
        }
        AssetDatabase.Refresh();
    }
    static int ComputeAtlasSize(int count, ref int size, ref int lineCount)
    {
        size = GetWrapSize(size);
        int total = count * size * size;
        // 最大图集2048
        for (int i = 5; i < 12; i++)
        {
            int w = (int)Mathf.Pow(2, i);
            if (total <= w * w)
            {
                lineCount = w / size;
                Debug.LogFormat("Atlas Size: [{0}]        LineCount: [{1}]\n", w, lineCount);
                return w;
            }
        }
        return 0;
    }
    static int GetWrapSize(int size)
    {
        for (int i = 0; i < 12; i++)
        {
            int s = (int)Mathf.Pow(2, i);
            if (s < size)
                continue;

            Debug.LogFormat("Single Emoji Size: [{0}]        Original Emoji Size: [{1}]\n", s, size);
            return s;
        }

        return 0;
    }
    private class AssetInfo
    {
        public int index;
        public Texture2D texture;
    }
    private class SpriteInfo
    {
        public string title;
        public int frame;
        public int index;
        public int lineNum;
        public int lineIndex;

        public SpriteInfo(string title, int frame, int index, int lineNum, int lineIndex)
        {
            this.title = title;
            this.frame = frame;
            this.index = index;
            this.lineNum = lineNum;
            this.lineIndex = lineIndex;
        }

        public override string ToString()
        {
            return $"{title}\t{frame}\t{index}\t{lineNum}\t{lineIndex}";
            return string.Format(title + "\t" + frame + "\t" + index);
        }
    }

    #endregion

}

#endif

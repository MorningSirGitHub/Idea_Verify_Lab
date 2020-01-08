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

public class HyperlinkTextEditor : UnityEditor.UI.TextEditor
{

    #region 创建UI

    private static int currentPickerWindow = -1;
    private const string m_ShaderSearchFilter = "emoji";
    private const string m_ShaderSearchType = " t:Shader";
    private const string m_EmojiShaderKey = "HyperlinkEmojiShader";
    private const string m_MaterialSearchFilter = "emoji";
    private const string m_MaterialSearchType = " t:Material";
    private const string m_EmojiMaterialKey = "HyperlinkEmojiMaterial";
    private static string m_DefaultEmojiMaterialPath
    {
        get
        {
            var nativeCache = PlayerPrefs.GetString(m_EmojiMaterialKey);
            if (string.IsNullOrEmpty(nativeCache))
                return "Assets/HyperlinkText/output/emoji.mat";

            return nativeCache;
        }
    }

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();

    //    if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
    //    {
    //        currentPickerWindow = -1;

    //    }
    //}

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
        var text = go.AddComponent<HyperlinkText>();
        text.text = "New HyperlinkText";
        CheckMaterial(text);
        return text;
    }
    private static Material CheckMaterial(HyperlinkText text)
    {
        Material material = null;
        var removeIndex = m_DefaultEmojiMaterialPath.LastIndexOf("/");
        var matPath = m_DefaultEmojiMaterialPath.Remove(removeIndex);
        if (AssetDatabase.IsValidFolder(matPath))
        {
            material = AssetDatabase.LoadAssetAtPath<Material>(m_DefaultEmojiMaterialPath);
        }
        else
        {
            material = PickerTypeOf(text.material, m_EmojiMaterialKey, new string[] { "", "mat" });
            //material = PickerTypeOf(text.material, m_MaterialSearchFilter);
            //PlayerPrefs.SetString(m_EmojiMaterialKey, AssetDatabase.GetAssetPath(material));
        }
        if (material.shader == null)
        {
            material.shader = PickerTypeOf(material.shader, m_EmojiShaderKey, new string[] { "", "shader" });
            //material.shader = PickerTypeOf(material.shader, m_ShaderSearchFilter);
            //PlayerPrefs.SetString(m_EmojiShaderKey, AssetDatabase.GetAssetPath(material.shader));
        }
        text.material = material;
        return material;
    }
    private static T PickerTypeOf<T>(T selection, string savedKey, string[] filterType) where T : UnityEngine.Object
    {
        var path = EditorUtility.OpenFilePanelWithFilters("请选择Emoji材质路径", Application.dataPath, filterType);
        var startIndex = path.IndexOf("Assets");
        path = path.Substring(startIndex);
        path.Replace('\\', '/');
        var asset = AssetDatabase.LoadAssetAtPath<T>(path);
        PlayerPrefs.SetString(savedKey, path);
        return asset;
    }
    private static T PickerTypeOf<T>(T selection, string filterType, FocusType focusType = FocusType.Passive, bool allowSceneObjs = false) where T : UnityEngine.Object
    {
        //currentPickerWindow = EditorGUIUtility.GetControlID(focusType) + 100;
        var controlID = EditorGUIUtility.GetControlID(focusType);
        EditorGUIUtility.ShowObjectPicker<T>(selection, allowSceneObjs, filterType, controlID);
        return EditorGUIUtility.GetObjectPickerObject() as T;
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

    #endregion

    #region 创建Emoji表情图集

    static int progress = 0, totalProgress = 0;
    static void UpdateProgressBar(string info)
    {
        EditorUtility.DisplayProgressBar("Hyperlink Text", info, ++progress / totalProgress);//更新已选中Emoji表情图集中...
    }
    static void ResetProgressBar(int total = 0)
    {
        progress = 0;
        totalProgress = total;
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("Tools/Emoji Build &R")]
    static void Build()
    {
        Dictionary<string, List<AssetInfo>> dic = new Dictionary<string, List<AssetInfo>>();
        Texture2D[] textures = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);

        ResetProgressBar(textures.Length);
        // get all select textures
        int totalFrames = 0;
        int size = 0;
        foreach (var texture in textures)
        {
            Match match = Regex.Match(texture.name, "^([a-zA-Z0-9]+)(_([0-9]+))?$");//name_idx; name
            if (!match.Success)
            {
                Debug.Log(texture.name + " 不匹配命名规则，跳过.");
                continue;
            }
            int index;
            if (!int.TryParse(match.Groups[3].Value, out index))
            {
                index = 1;
            }

            List<AssetInfo> infos;
            string title = match.Groups[1].Value;
            if (!dic.TryGetValue(title, out infos))
            {
                infos = new List<AssetInfo>();
                dic.Add(title, infos);
            }
            infos.Add(new AssetInfo() { index = index, texture = texture });

            if (texture.width > size)
                size = texture.width;
            if (texture.height > size)
                size = texture.width;

            totalFrames++;
            UpdateProgressBar("正在过滤已选中表情...");
        }

        ResetProgressBar(dic.Values.Count);
        // sort frames
        foreach (var info in dic.Values)
        {
            info.Sort(new Comparison<AssetInfo>((a, b) => a.index <= b.index ? 1 : 0));
            UpdateProgressBar("表情组合排序中...");
        }

        // compute atlas size, support n*n only
        int lineCount = 0;
        int texSize = ComputeAtlasSize(totalFrames, ref size, ref lineCount);
        if (texSize < 1)
        {
            EditorUtility.DisplayDialog("Hyperlink Text", "未能构建合适大小的图集", "退出");
            return;
        }

        // sort keys
        var keys = dic.Keys.ToList();
        keys.Sort(new Comparison<string>((a, b) => String.Compare(a, b, StringComparison.Ordinal)));

        int total = 0;
        foreach (var key in keys)
        {
            total += dic[key].Count;
        }
        ResetProgressBar(total);
        // build atlas
        List<SpriteInfo> sprites = new List<SpriteInfo>();
        Texture2D atlas = new Texture2D(texSize, texSize);
        int idx = 0;
        foreach (var key in keys)
        {
            sprites.Add(new SpriteInfo(key, dic[key].Count, idx));
            foreach (var assetInfo in dic[key])
            {
                int w = assetInfo.texture.width;
                int h = assetInfo.texture.height;

                int x = idx % lineCount;
                int y = idx / lineCount;

                Color[] colors = assetInfo.texture.GetPixels(0, 0, w, h);
                atlas.SetPixels(x * size, y * size, w, h, colors);

                idx++;
                UpdateProgressBar("生成图集中...");
            }
        }

        ResetProgressBar(sprites.Count);
        // build emoji config
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Key\tFrame\tIndex");
        foreach (var spriteInfo in sprites)
        {
            builder.AppendLine(spriteInfo.ToString());
            UpdateProgressBar("生成配置表中...");
        }

        ResetProgressBar();
        // select save folder
        string pngPath = EditorUtility.SaveFilePanelInProject("Select Save Path", "emoji", "png", "");
        if (string.IsNullOrEmpty(pngPath))
            return;

        var txtPath = pngPath.Replace(".png", ".txt");
        byte[] bytes = atlas.EncodeToPNG();
        File.WriteAllBytes(pngPath, bytes);
        File.WriteAllText(txtPath, builder.ToString());
        AssetDatabase.ImportAsset(pngPath);

        // create material
        var matPath = pngPath.Replace(".png", ".mat");
        Shader shader = Shader.Find("UI/EmojiFont");
        var hasMat = Path.HasExtension(matPath);
        Material material = hasMat ? AssetDatabase.LoadAssetAtPath<Material>(matPath) : new Material(shader);
        Texture2D emojiTex = AssetDatabase.LoadAssetAtPath<Texture2D>(pngPath);
        material.SetTexture("_EmojiTex", emojiTex);
        material.SetFloat("_EmojiSize", size * 1.0f / texSize);
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
    static int ComputeAtlasSize(int count, ref int size, ref int x)
    {
        size = GetWrapSize(size);
        int total = count * size * size;
        for (int i = 5; i < 12; i++)
        {
            int w = (int)Mathf.Pow(2, i);
            if (total <= w * w)
            {
                x = w / size;
                return w;
            }
        }
        return 0;
    }
    static int GetWrapSize(int size)
    {
        //最大图集2048
        for (int i = 0; i < 12; i++)
        {
            int s = (int)Mathf.Pow(2, i);
            if (s >= size)
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

        public SpriteInfo(string title, int frame, int index)
        {
            this.title = title;
            this.frame = frame;
            this.index = index;
        }

        public override string ToString()
        {
            return String.Format(title + "\t" + frame + "\t" + index);
        }
    }

    #endregion

}

#endif

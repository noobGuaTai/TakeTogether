using PlasticPipe.PlasticProtocol.Messages;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillTreeWindow : EditorWindow
{
    public SkillTreeGraphView graphView;
    public SkillTreeInspectorAsset inspectorAsset;
    public SkillTreeInspectorNode inspectorNode;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("SkillTreeWindow/Editor ...")]
    public static void OpenWindow()
    {
        SkillTreeWindow wnd = GetWindow<SkillTreeWindow>();
        wnd.titleContent = new GUIContent("SkillTreeWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C#");
        //root.Add(label);

        // Instantiate UXML
        //Debug.Log($"SkillTreeWindow: m_VisualTreeAsset={m_VisualTreeAsset}");
        m_VisualTreeAsset.CloneTree(root);

        // fxxk you unity!!!
        //var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GameResources/UI/Editor/SkillTreeEditor/SkillTreeWindow.uxml");
        //root.Add(visualTree.Instantiate());


        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
        //    "Assets/GameResources/UI/Editor/SkillTreeEditor/SkillTreeWindow.uss");
        //root.styleSheets.Add(styleSheet);

        graphView = root.Q<SkillTreeGraphView>("SkillTreeGraphView");
        inspectorAsset = root.Q<SkillTreeInspectorAsset>("SkillTreeInspectorAsset");
        inspectorNode = root.Q<SkillTreeInspectorNode>("SkillTreeInspectorNode");
        graphView.onSelectedNodeViewChange += (SkillTreeGraphView who) => {
            inspectorNode.nodeView = who.selectedNodeView;
            inspectorNode.Refresh();
        };
    }

    private void OnSelectionChange()
    {
        var selectedSkillTreeAsset = Selection.activeObject as SkillTreeAsset;
        if (selectedSkillTreeAsset != null) {
            if (graphView == null || inspectorAsset == null)
            {
                Debug.LogWarning($"SkillTreeWindow: g:[{graphView}],i:[{inspectorAsset}]");
                return;
            }

            Debug.Log($"SkillTreeWindow: load new asset [{selectedSkillTreeAsset}]");
            graphView.treeAsset = selectedSkillTreeAsset;
            graphView.Refresh();

            inspectorAsset.treeAsset = selectedSkillTreeAsset;
            inspectorAsset.Refresh();
        }
    }
}

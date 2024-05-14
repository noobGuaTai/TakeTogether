
using Codice.CM.WorkspaceServer.DataStore;
using Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using YooAsset.Editor;


public class SkillTreeGraphView : GraphView
{
    public enum NodeType
    {
        SKILL,
        ROOT
    }
    public class Node : UnityEditor.Experimental.GraphView.Node
    {

        public NodeType nodeType = NodeType.SKILL;
        public SkillTreeNodeAsset nodeAsset;
        public Port inPort;
        public Port outPort;
        public Action<SkillTreeGraphView.Node> onSelected;

        public Node(SkillTreeNodeAsset nodeAsset)
        {
            title = nodeAsset.keyName;
            name = $"node: {nodeAsset.keyName}";
            this.nodeAsset = nodeAsset;

            inPort = InstantiatePort(
                Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inPort.portName = "";
            inPort.portColor = new Color(0.9f, 0.9f, 0.7f);

            outPort = InstantiatePort(
                Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            outPort.portName = "";
            outPort.portColor = new Color(0.7f, 0.9f, 0.9f);

            inputContainer.Add(inPort);
            outputContainer.Add(outPort);

            var titleLable = this.Q<Label>("title-label");
            titleLable.style.fontSize = 22;
            titleLable.style.color = Color.white;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            nodeAsset.editorPosition = newPos;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            if (onSelected != null)
            {
                onSelected(this);
            }
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            if (onSelected != null)
            {
                onSelected(null);
            }
        }

        public void SetSkillName(string skillName)
        {
            nodeAsset.keyName = skillName;
            this.Q<Label>("title-label").text = nodeAsset.keyName;
        }

        public void SetRoot()
        {
            if(inputContainer.Contains(inPort))
                this.inputContainer.Remove(inPort);
            this.Q<Label>("title-label").style.fontSize = 24;
            this.Q<Label>("title-label").style.unityFontStyleAndWeight = FontStyle.Bold;
            this.Q<VisualElement>("title").style.backgroundColor = new Color(0.2f, 0.3f, 0.5f);
            nodeType = NodeType.ROOT;
        }
    }
    public new class UxmlFactory : UxmlFactory<SkillTreeGraphView, GraphView.UxmlTraits> { }

    public SkillTreeAsset treeAsset;
    public Dictionary<SkillTreeNodeAsset, Node> nodeAsset2View = new Dictionary<SkillTreeNodeAsset, Node>();
    private Node _selectedNodeView;
    public delegate void onSelectedNodeViewChangeDelegate(SkillTreeGraphView who);
    public onSelectedNodeViewChangeDelegate onSelectedNodeViewChange;

    public Node selectedNodeView
    {
        set
        {
            _selectedNodeView = value;
            onSelectedNodeViewChange(this);
        }
        get
        {
            return _selectedNodeView;
        }
    }


    public SkillTreeGraphView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
            "Assets/GameResources/UI/Editor/SkillTreeEditor/SkillTreeWindow.uss");
        styleSheets.Add(styleSheet);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        evt.menu.AppendAction("Add new skill", AddNodeButton,
            treeAsset != null ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
        evt.menu.AppendAction("Add new skill root", AddRootNodeButton,
            treeAsset != null ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
    }

    public void AddRootNodeButton(DropdownMenuAction action)
    {
        var nodeAsset = treeAsset.AddNode("#ROOT", SkillTreeAsset.NodeType.ROOT);
        var nodeView = AddNode(nodeAsset);
        nodeView.SetRoot();
    }

    public void AddNodeButton(DropdownMenuAction action)
    {
        var nodeAsset = treeAsset.AddNode();
        AddNode(nodeAsset);
    }

    public Node AddNode(SkillTreeNodeAsset nodeAsset)
    {
        var nodeView = new Node(nodeAsset);
        nodeView.SetPosition(nodeAsset.editorPosition);
        nodeView.onSelected = OnSelectedNodeChange;
        AddElement(nodeView);
        if (nodeAsset.keyName.StartsWith("#ROOT"))
            nodeView.SetRoot();
        return nodeView;
    }

    public void OnSelectedNodeChange(SkillTreeGraphView.Node who)
    {
        selectedNodeView = who;
    }

    public void Refresh()
    {
        graphViewChanged -= OnGraphViewChanged;

        DeleteElements(graphElements);

        nodeAsset2View.Clear();
        var badKeys = new List<string>();
        foreach (var nodeAssetPair in treeAsset.nodes)
        {
            if (nodeAssetPair.Key != nodeAssetPair.Value.keyName) {
                badKeys.Add(nodeAssetPair.Key);
                continue;
            }
            var nodeAsset = nodeAssetPair.Value;
            var nodeView = AddNode(nodeAsset);

            nodeView.SetPosition(nodeAsset.editorPosition);
            nodeView.onSelected = OnSelectedNodeChange;
            nodeAsset2View.Add(nodeAsset, nodeView);
        }
        for(int i = 0;i < badKeys.Count;i++) {
            treeAsset.nodes.Remove(badKeys[i]);
        }
        // adding root node for skills which have not parent

        foreach (var nodeAssetPair in treeAsset.nodes)
        {
            var stNode = nodeAssetPair.Value;
            var stNodeView = nodeAsset2View[stNode];
            // remove null
            stNode.outDegressNodes.Where((SkillTreeNodeAsset asset) => {
                return asset == null;
            }).ToList().ForEach((SkillTreeNodeAsset nullAsset) => { stNode.outDegressNodes.Remove(nullAsset); });
            foreach (var edNode in stNode.outDegressNodes)
            {
                var edNodeView = nodeAsset2View[edNode];
                var edge = stNodeView.outPort.ConnectTo(edNodeView.inPort);
                AddElement(edge);
            }
        }

        graphViewChanged += OnGraphViewChanged;
    }

    public GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        Undo.RecordObject(treeAsset, "Modify GraphView Asset");
        
        if (graphViewChange.elementsToRemove != null)
        {
            foreach (var item in graphViewChange.elementsToRemove)
            {
                var node = item as Node;
                if (node != null)
                {
                    RemoveElement(node);
                    treeAsset.RemoveNode(node.nodeAsset);
                }
                var edge = item as UnityEditor.Experimental.GraphView.Edge;
                if (edge != null)
                {
                    RemoveElement(edge);
                    var stNodeAsset = (edge.output.node as Node).nodeAsset;
                    var edNodeAsset = (edge.input.node as Node).nodeAsset;

                    stNodeAsset.outDegressNodes.Remove(edNodeAsset);
                    edNodeAsset.inDegreeNodes.Remove(stNodeAsset);
                }
            }
        }

        if (graphViewChange.edgesToCreate != null)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                var inputNodeView = edge.input.node as SkillTreeGraphView.Node;
                var outputNodeView = edge.output.node as SkillTreeGraphView.Node;

                if (inputNodeView == null || outputNodeView == null)
                {
                    Debug.LogError($"skillTreeView create Edge [{inputNodeView}]->[{outputNodeView}]");
                    continue;
                }
                var inputNodeAsset = inputNodeView.nodeAsset;
                var outputNodeAsset = outputNodeView.nodeAsset;
                // port out -> in
                inputNodeAsset.inDegreeNodes.Add(outputNodeAsset);
                outputNodeAsset.outDegressNodes.Add(inputNodeAsset);
                //outputNodeAsset.skillLevel = inputNodeAsset.skillLevel + 1;

                AssetDatabase.SaveAssets();
            }
        }

        EditorUtility.SetDirty(treeAsset);
        AssetDatabase.SaveAssetIfDirty(treeAsset);
        return graphViewChange;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(p =>
        {
            return
                p.node != startPort.node &&
                p.direction != startPort.direction;
        }).ToList();
    }
}

    public class SkillTreeSplitView : SplitView
    {
        public new class UxmlFactory : UxmlFactory<SkillTreeSplitView, SplitView.UxmlTraits> { }
    }

    public class SkillTreeInspectorAsset : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<SkillTreeInspectorAsset, VisualElement.UxmlTraits> { }

        public SkillTreeAsset treeAsset;

        public void Refresh()
        {
        }
    }

public class SkillTreeInspectorNode : VisualElement
{
    public new class UxmlFactory : UxmlFactory<SkillTreeInspectorNode, VisualElement.UxmlTraits> { }

    public SkillTreeGraphView.Node nodeView;
    public Editor editor = null;
    public SkillTreeAsset treeAsset;

    public void Refresh() {
        var skillNameBackground = this.Q<VisualElement>("Background");
        var oldToolbar = skillNameBackground.Q<ToolbarPopupSearchField>("toolbar");
        if (oldToolbar != null) { skillNameBackground.Remove(oldToolbar); }

        var editorContainer = this.Q<IMGUIContainer>("editor-container");
        if (editorContainer != null) { Remove(editorContainer); }
        if (editor != null) {
            Editor.DestroyImmediate(editor);
            editor = null;
        }
        if (nodeView != null) {
            var toolbar = new ToolbarPopupSearchField(); toolbar.name = "toolbar";

            toolbar.RegisterValueChangedCallback((ChangeEvent<String> ce) => {
                //Debug.Log($"skillTree window: {ce.previousValue} -> {ce.newValue}");
                nodeView.SetSkillName(ce.newValue);
                UpdateMenu(ce.newValue, toolbar);
            });

            toolbar.style.width = 200;
            toolbar.style.height = 30;
            toolbar.style.flexGrow = 0;
            var button = toolbar.Q<UnityEngine.UIElements.Button>("unity-search");
            button.style.width = 27;
            button.style.height = 24;
            var textField = toolbar.Q<TextField>();
            textField.value = nodeView.nodeAsset.typeName;
            UpdateMenu(textField.value, toolbar);

            textField.style.fontSize = 22;
            var textElement = textField.Q<TextElement>();
            textElement.style.fontSize = 16;
            textElement.style.color = Color.white;

            Add(skillNameBackground);
            skillNameBackground.Add(toolbar);

            editor = Editor.CreateEditor(nodeView.nodeAsset);
            IMGUIContainer container = new IMGUIContainer(() => {
                editor.OnInspectorGUI();
            });
            container.name = "editor-container";
            Add(container);

            if (nodeView.nodeType == SkillTreeGraphView.NodeType.ROOT)
                toolbar.visible = false;
            else toolbar.visible = true;
        }
    }

    // update skill-names poped up
    void UpdateMenu(string keyword, ToolbarPopupSearchField toolbar) {
        toolbar.menu.ClearItems();
        var keywords = keyword.Split(' ');
        var skillNames = TypeCache.GetTypesDerivedFrom<Skill.SkillBase>().ToList()
           .Select(type => type.Name.Substring(5)).ToList();

        Func<List<string>> getCandidate = () => {
            if (keywords[0] == "[Undefined]") {
                return skillNames;
            }
            else return skillNames.Where(name => keywords.All(
                  key => name.IndexOf(key, StringComparison.OrdinalIgnoreCase) != -1)).ToList();
        };


        foreach (var skillName in getCandidate()) {
            toolbar.menu.AppendAction(skillName, (DropdownMenuAction action) => {
                toolbar.value = skillName;
                treeAsset.ChangeNodeSkillName(nodeView.nodeAsset, skillName);
            });
        };

        //Debug.Log(toolbar.menu.MenuItems().Count);
        if (toolbar.menu.MenuItems().Count == 0) {
            toolbar.menu.AppendAction("- No matching reuslt -", (DropdownMenuAction action) => { });

        }
    }
}
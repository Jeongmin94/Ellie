using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheKiwiCoder
{
    public class CreateNodeWindow : ScriptableObject, ISearchWindowProvider
    {
        private Texture2D icon;
        private bool isSourceParent;
        private EditorUtility.ScriptTemplate[] scriptFileAssets;
        private NodeView source;
        private BehaviourTreeView treeView;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"))
            };

            // Action nodes can only be added as children
            if (isSourceParent || source == null)
            {
                tree.Add(new SearchTreeGroupEntry(new GUIContent("Actions")) { level = 1 });
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
                foreach (var type in types)
                {
                    Action invoke = () => CreateNode(type, context);
                    tree.Add(new SearchTreeEntry(new GUIContent($"{type.Name}")) { level = 2, userData = invoke });
                }
            }

            {
                tree.Add(new SearchTreeGroupEntry(new GUIContent("Composites")) { level = 1 });
                {
                    var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
                    foreach (var type in types)
                    {
                        Action invoke = () => CreateNode(type, context);
                        tree.Add(new SearchTreeEntry(new GUIContent($"{type.Name}")) { level = 2, userData = invoke });
                    }
                }
            }

            {
                tree.Add(new SearchTreeGroupEntry(new GUIContent("Decorators")) { level = 1 });
                {
                    var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
                    foreach (var type in types)
                    {
                        Action invoke = () => CreateNode(type, context);
                        tree.Add(new SearchTreeEntry(new GUIContent($"{type.Name}")) { level = 2, userData = invoke });
                    }
                }
            }

            {
                tree.Add(new SearchTreeGroupEntry(new GUIContent("New Script...")) { level = 1 });

                Action createActionScript = () => CreateScript(scriptFileAssets[0], context);
                tree.Add(new SearchTreeEntry(new GUIContent("New Action Script")) { level = 2, userData = createActionScript });

                Action createCompositeScript = () => CreateScript(scriptFileAssets[1], context);
                tree.Add(new SearchTreeEntry(new GUIContent("New Composite Script")) { level = 2, userData = createCompositeScript });

                Action createDecoratorScript = () => CreateScript(scriptFileAssets[2], context);
                tree.Add(new SearchTreeEntry(new GUIContent("New Decorator Script")) { level = 2, userData = createDecoratorScript });
            }


            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var invoke = (Action)searchTreeEntry.userData;
            invoke();
            return true;
        }

        private TextAsset GetScriptTemplate(int type)
        {
            var projectSettings = BehaviourTreeProjectSettings.GetOrCreateSettings();

            switch (type)
            {
                case 0:
                    if (projectSettings.scriptTemplateActionNode)
                    {
                        return projectSettings.scriptTemplateActionNode;
                    }

                    return BehaviourTreeEditorWindow.Instance.scriptTemplateActionNode;
                case 1:
                    if (projectSettings.scriptTemplateCompositeNode)
                    {
                        return projectSettings.scriptTemplateCompositeNode;
                    }

                    return BehaviourTreeEditorWindow.Instance.scriptTemplateCompositeNode;
                case 2:
                    if (projectSettings.scriptTemplateDecoratorNode)
                    {
                        return projectSettings.scriptTemplateDecoratorNode;
                    }

                    return BehaviourTreeEditorWindow.Instance.scriptTemplateDecoratorNode;
            }

            Debug.LogError("Unhandled script template type:" + type);
            return null;
        }

        public void Initialise(BehaviourTreeView treeView, NodeView source, bool isSourceParent)
        {
            this.treeView = treeView;
            this.source = source;
            this.isSourceParent = isSourceParent;

            icon = new Texture2D(1, 1);
            icon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            icon.Apply();

            scriptFileAssets = new EditorUtility.ScriptTemplate[]
            {
                new() { templateFile = GetScriptTemplate(0), defaultFileName = "NewActionNode", subFolder = "Actions" },
                new() { templateFile = GetScriptTemplate(1), defaultFileName = "NewCompositeNode", subFolder = "Composites" },
                new() { templateFile = GetScriptTemplate(2), defaultFileName = "NewDecoratorNode", subFolder = "Decorators" }
            };
        }

        public void CreateNode(Type type, SearchWindowContext context)
        {
            var editorWindow = BehaviourTreeEditorWindow.Instance;

            var windowMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent, context.screenMousePosition - editorWindow.position.position);
            var graphMousePosition = editorWindow.treeView.contentViewContainer.WorldToLocal(windowMousePosition);
            var nodeOffset = new Vector2(-75, -20);
            var nodePosition = graphMousePosition + nodeOffset;

            // #TODO: Unify this with CreatePendingScriptNode
            NodeView createdNode;
            if (source != null)
            {
                if (isSourceParent)
                {
                    createdNode = treeView.CreateNode(type, nodePosition, source);
                }
                else
                {
                    createdNode = treeView.CreateNodeWithChild(type, nodePosition, source);
                }
            }
            else
            {
                createdNode = treeView.CreateNode(type, nodePosition, null);
            }

            treeView.SelectNode(createdNode);
        }

        public void CreateScript(EditorUtility.ScriptTemplate scriptTemplate, SearchWindowContext context)
        {
            var editorWindow = BehaviourTreeEditorWindow.Instance;

            var windowMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent, context.screenMousePosition - editorWindow.position.position);
            var graphMousePosition = editorWindow.treeView.contentViewContainer.WorldToLocal(windowMousePosition);
            var nodeOffset = new Vector2(-75, -20);
            var nodePosition = graphMousePosition + nodeOffset;

            EditorUtility.CreateNewScript(scriptTemplate, source, isSourceParent, nodePosition);
        }

        public static void Show(Vector2 mousePosition, NodeView source, bool isSourceParent = false)
        {
            var screenPoint = GUIUtility.GUIToScreenPoint(mousePosition);
            var searchWindowProvider = CreateInstance<CreateNodeWindow>();
            searchWindowProvider.Initialise(BehaviourTreeEditorWindow.Instance.treeView, source, isSourceParent);
            var windowContext = new SearchWindowContext(screenPoint, 240, 320);
            SearchWindow.Open(windowContext, searchWindowProvider);
        }
    }
}
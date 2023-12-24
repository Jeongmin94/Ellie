using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheKiwiCoder
{
    public class NewScriptDialogView : VisualElement
    {
        private Button confirmButton;
        private bool isSourceParent;
        private Vector2 nodePosition;

        private EditorUtility.ScriptTemplate scriptTemplate;
        private NodeView source;
        private TextField textField;

        public void CreateScript(EditorUtility.ScriptTemplate scriptTemplate, NodeView source, bool isSourceParent, Vector2 position)
        {
            this.scriptTemplate = scriptTemplate;
            this.source = source;
            this.isSourceParent = isSourceParent;
            nodePosition = position;

            style.visibility = Visibility.Visible;

            var background = this.Q<VisualElement>("Background");
            var titleLabel = this.Q<Label>("Title");
            textField = this.Q<TextField>("FileName");
            confirmButton = this.Q<Button>();

            titleLabel.text = $"New {scriptTemplate.subFolder.TrimEnd('s')} Script";

            textField.focusable = true;
            RegisterCallback<PointerEnterEvent>(e => { textField[0].Focus(); });

            textField.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
                {
                    OnConfirm();
                }
            });

            confirmButton.clicked -= OnConfirm;
            confirmButton.clicked += OnConfirm;

            background.RegisterCallback<PointerDownEvent>(e =>
            {
                e.StopImmediatePropagation();
                Close();
            });
        }

        private void Close()
        {
            style.visibility = Visibility.Hidden;
        }

        private void OnConfirm()
        {
            var scriptName = textField.text;

            var newNodePath = $"{BehaviourTreeEditorWindow.Instance.settings.newNodePath}";
            if (AssetDatabase.IsValidFolder(newNodePath))
            {
                var destinationFolder = Path.Combine(newNodePath, scriptTemplate.subFolder);
                var destinationPath = Path.Combine(destinationFolder, $"{scriptName}.cs");

                Directory.CreateDirectory(destinationFolder);

                var parentPath = Directory.GetParent(Application.dataPath);

                var templateString = scriptTemplate.templateFile.text;
                templateString = templateString.Replace("#SCRIPTNAME#", scriptName);
                var scriptPath = Path.Combine(parentPath.ToString(), destinationPath);

                if (!File.Exists(scriptPath))
                {
                    File.WriteAllText(scriptPath, templateString);

                    // TODO: There must be a better way to survive domain reloads after script compiling than this
                    BehaviourTreeEditorWindow.Instance.pendingScriptCreate.pendingCreate = true;
                    BehaviourTreeEditorWindow.Instance.pendingScriptCreate.scriptName = scriptName;
                    BehaviourTreeEditorWindow.Instance.pendingScriptCreate.nodePosition = nodePosition;
                    if (source != null)
                    {
                        BehaviourTreeEditorWindow.Instance.pendingScriptCreate.sourceGuid = source.node.guid;
                        BehaviourTreeEditorWindow.Instance.pendingScriptCreate.isSourceParent = isSourceParent;
                    }

                    AssetDatabase.Refresh();
                    confirmButton.SetEnabled(false);
                    EditorApplication.delayCall += WaitForCompilation;
                }
                else
                {
                    Debug.LogError($"Script with that name already exists:{scriptPath}");
                    Close();
                }
            }
            else
            {
                Debug.LogError($"Invalid folder path:{newNodePath}. Check the project configuration settings 'newNodePath' is configured to a valid folder");
            }
        }

        private void WaitForCompilation()
        {
            if (EditorApplication.isCompiling)
            {
                EditorApplication.delayCall += WaitForCompilation;
                return;
            }

            confirmButton.SetEnabled(true);
            Close();
        }

        public new class UxmlFactory : UxmlFactory<NewScriptDialogView, UxmlTraits>
        {
        }
    }
}
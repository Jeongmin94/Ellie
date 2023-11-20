using UnityEditor;

public class RenameToInstaceID
{
    [MenuItem("GameObject/" + nameof(RenameToInstanceID))]
    static void RenameToInstanceID()
    {
        foreach (var x in Selection.gameObjects)
        {
            x.name = $"GameObject {x.GetInstanceID()}";
            EditorUtility.SetDirty(x);
        }
    }
}


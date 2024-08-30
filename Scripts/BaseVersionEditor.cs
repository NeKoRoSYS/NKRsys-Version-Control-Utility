using UnityEngine;
using UnityEditor;

namespace NeKoRoSYS.VersionControl
{
    [CustomEditor(typeof(PlayerSettings))]
    public class BaseVersionEditor : Editor
    {
        private const string BaseVersionKey = "BaseVersion";
        private string baseVersion;
        private Editor defaultEditor;
        private const string Tooltip = "Set the *actual* version in this field and not in the Version field below. The latter is now auto-generated according to the Version Incrementer script.\n\n(eg. Base Version = 1.0.1; Version = 1.0.1.MMDDYY.BuildNumber)";

        private void OnEnable()
        {
            defaultEditor = CreateEditor(targets, typeof(Editor).Assembly.GetType("UnityEditor.PlayerSettingsEditor"));
            baseVersion = EditorPrefs.GetString(BaseVersionKey, "1.0.0");
        }
    
        private void OnDisable()
        {
            if (defaultEditor != null) DestroyImmediate(defaultEditor);
        }
    
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Base Version", Tooltip), GUILayout.Width(EditorGUIUtility.labelWidth));
            baseVersion = EditorGUILayout.TextField(baseVersion);
            EditorGUILayout.EndHorizontal();
            if (GUI.changed) EditorPrefs.SetString(BaseVersionKey, baseVersion);
            defaultEditor.OnInspectorGUI();
        }
    
        public static string GetBaseVersion() => EditorPrefs.GetString(BaseVersionKey, "1.0.0");
    }
}
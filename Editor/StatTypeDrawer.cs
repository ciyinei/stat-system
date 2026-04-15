#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using StatSystem;

[CustomPropertyDrawer(typeof(StatType))]
public class StatTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty valueProp = property.FindPropertyRelative("_value");
        SerializedProperty nameProp  = property.FindPropertyRelative("_name");

        if (valueProp == null || nameProp == null)
        {
            EditorGUI.LabelField(position, label.text, "Serialize fields not found.");
            EditorGUI.EndProperty();
            return;
        }

        StatType[] allTypes = StatTypeRegistry.GetAll();

        if (allTypes.Length == 0)
        {
            EditorGUI.LabelField(position, label.text, "No StatTypes registered.");
            EditorGUI.EndProperty();
            return;
        }

        string[] displayNames = System.Array.ConvertAll(allTypes, t => t.ToString());
        int currentIndex = System.Array.FindIndex(allTypes, t => t.Value == valueProp.intValue);
        if (currentIndex < 0) currentIndex = 0;

        int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, displayNames);

        valueProp.intValue  = allTypes[selectedIndex].Value;
        nameProp.stringValue = allTypes[selectedIndex].Name;

        EditorGUI.EndProperty();
    }
}
#endif
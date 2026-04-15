using UnityEditor;

namespace StatSystem
{
#if UNITY_EDITOR

    [InitializeOnLoad]
    public static class StatTypeEditorInitializer
    {
        static StatTypeEditorInitializer()
        {
            // Accessing any static field forces the class to initialize,
            // which triggers all Register() calls
            var _ = StatType.MaxHealth;
        }
    }
#endif
}

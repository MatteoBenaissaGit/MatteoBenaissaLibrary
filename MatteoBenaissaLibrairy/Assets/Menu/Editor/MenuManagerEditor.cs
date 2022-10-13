namespace Menu.Editor
{
    
    using UnityEditor;
    using UnityEngine;

#if UNITY_EDITOR
    [CustomEditor(typeof(MenuManager))]
    public class MenuManagerEditor : UnityEditor.Editor
    {
        private MenuManager menuManagerScript;

        private void OnEnable()
        {
            menuManagerScript = (MenuManager)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            //buttons
            if (GUILayout.Button("Set Menu Type Simple"))
            {
                menuManagerScript.SetMenuTypeSimple();
            }

            if (GUILayout.Button("Set Menu Type Side Slide"))
            {
                menuManagerScript.SetMenuTypeSideSlide();
            }

            #region Warnings

            //references check
            MenuReferences simpleMenuReferences = menuManagerScript.SimpleMenuReferences;
            MenuReferences sideSlideMenuReferences = menuManagerScript.SideSlideMenuReferences;

            if (simpleMenuReferences.MenuGameObject == null ||
                simpleMenuReferences.PlayButtonTransform == null ||
                simpleMenuReferences.CreditsButtonTransform == null ||
                simpleMenuReferences.QuitButtonTransform == null ||
                sideSlideMenuReferences.MenuGameObject == null ||
                sideSlideMenuReferences.PlayButtonTransform == null ||
                sideSlideMenuReferences.CreditsButtonTransform == null ||
                sideSlideMenuReferences.QuitButtonTransform == null)
            {
                EditorGUILayout.HelpBox("References missing", MessageType.Warning, true);
            }

            if (string.IsNullOrEmpty(menuManagerScript.PlaySceneName) ||
                string.IsNullOrEmpty(menuManagerScript.CreditSceneName))
            {
                EditorGUILayout.HelpBox("Scenes names missing", MessageType.Warning, true);
            }

            #endregion

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
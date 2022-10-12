namespace Menu
{
#if UNITY_EDITOR

    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(MenuManager))]
    public class MenuManagerEditor : Editor
    {
        private MenuManager menuManagerScript;

        private void OnEnable () 
        {
            menuManagerScript = (MenuManager)target;
        }
        
        public override void OnInspectorGUI()
        {
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
            MenuReferences simpleMenuReferences = menuManagerScript._simpleMenuReferences;
            MenuReferences sideSlideMenuReferences = menuManagerScript._sideSlideMenuReferences;

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

            if (string.IsNullOrEmpty(menuManagerScript._playSceneName) ||
                string.IsNullOrEmpty(menuManagerScript._creditSceneName))
            {
                EditorGUILayout.HelpBox("Scenes names missing", MessageType.Warning, true);
            }

            #endregion
            
        }

    }

#endif
}



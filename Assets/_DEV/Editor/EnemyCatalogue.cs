using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class EnemyCatalogue : OdinMenuEditorWindow
{
    public const string ENEMY_DATA_FILEPATH = "Assets/Data/Enemies";

    [MenuItem("Redwire/Enemy Catalogue")]
    private static void OpenWindow()
    {
        GetWindow<EnemyCatalogue>().Show();

    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;

        tree.Add("+ Create New Enemy", new CreateNewEnemyData());
        tree.AddAllAssetsAtPath("Enemy Data", ENEMY_DATA_FILEPATH, typeof(EnemySO));
        return tree;
    }

    public class CreateNewEnemyData
    {
        public CreateNewEnemyData()
        {
            enemyData = ScriptableObject.CreateInstance<EnemySO>();
            enemyData.enemyName = "New Enemy Data";
        }

        [InlineEditor(Expanded = true)]
        public EnemySO enemyData;

        [Button("Add New Enemy SO")]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(enemyData, ENEMY_DATA_FILEPATH + "/" + enemyData.enemyName + ".asset");
            AssetDatabase.SaveAssets();
        }
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();

            if (SirenixEditorGUI.ToolbarButton("Delete Current"))
            {
                Enemy asset = selected.SelectedValue as Enemy;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }

        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }
}

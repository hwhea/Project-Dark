using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class ItemCatalogue : OdinMenuEditorWindow
{
    public const string ITEM_DATA_FILEPATH = "Assets/Data/Items";

    [MenuItem("Redwire/Item Catalogue")]
    private static void OpenWindow()
    {
        GetWindow<ItemCatalogue>().Show();

    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;

        tree.Add("+ Create New Item", new CreateNewItemData());
        tree.AddAllAssetsAtPath("Item Data", ITEM_DATA_FILEPATH, typeof(SOFabricatorItem));
        return tree;
    }

    public class CreateNewItemData
    {
        public CreateNewItemData()
        {
            ItemData = ScriptableObject.CreateInstance<SOFabricatorItem>();
            ItemData.ItemName = "New Item Data";
        }

        [InlineEditor(Expanded = true)]
        public SOFabricatorItem ItemData;

        [Button("Add New Item SO")]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(ItemData, ITEM_DATA_FILEPATH + "/" + ItemData.ItemName + ".asset");
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
                SOFabricatorItem asset = selected.SelectedValue as SOFabricatorItem;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }

        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }
}

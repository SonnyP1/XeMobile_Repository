using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{

    public static class MaterialFixer
    {
        [MenuItem("Assets/Material/Update Error Material to Standard Material")]
        private static void UpdateErrorMaterialToStandard()
        {
            string[] allMaterialAssets = AssetDatabase.FindAssets("t:Material");
            foreach(string asset in allMaterialAssets)
            {
                string materialPath = AssetDatabase.GUIDToAssetPath(asset);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                if(AssetDatabase.Equals(mat.shader, Shader.Find("Hidden/InternalErrorShader")))
                {
                    mat.shader = Shader.Find("Standard");
                }
            }
        }
    }
}

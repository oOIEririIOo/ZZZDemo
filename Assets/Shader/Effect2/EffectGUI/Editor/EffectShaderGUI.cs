using UnityEditor;
using UnityEngine;
using System.Linq;

public class EffectShaderGUI : ShaderGUI
{
    private MaterialEditor editor;
    private MaterialProperty[] properties;
    private Material mat;
    private static GUIContent staticLabel = new GUIContent();
    private bool isMain = true;
    private bool isOther = false;
    private MaterialProperty _SrcBlend;
    private MaterialProperty _DstBlend;
    private MaterialProperty _ZWriteMode;
    
    public override void OnGUI (MaterialEditor editor, MaterialProperty[] properties)
    {
        this.editor = editor;
        this.properties = properties;
        this.mat = this.editor.target as Material;
        _SrcBlend = FindProperty("_SrcBlend");
        _DstBlend = FindProperty("_DstBlend");
        _ZWriteMode = FindProperty("_ZWriteMode");
        DoButton();
        DoMain();
        DoMask();
        DoNoise();
        DoDissolve();
        DoFlowMap();
        DoSoftPartical();
        DoOther();
    }

    void DoMain()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("主参数",GUILayout.Height(20)))
        {
            isMain = !isMain;
        }
        EditorGUILayout.EndHorizontal();
        if (isMain)
        {
            MaterialProperty _CullMode = FindProperty("_CullMode");
            editor.ShaderProperty(_CullMode,MakeLabel(_CullMode));
            MaterialProperty _BaseMap = FindProperty("_BaseMap");
            MaterialProperty _BaseColor = FindProperty("_BaseColor");
            editor.TexturePropertySingleLine(MakeLabel(_BaseMap,"Albedo(RGB)"), _BaseMap,_BaseColor);
            editor.TextureScaleOffsetProperty(_BaseMap);
            MaterialProperty _MixAlpha = FindProperty("_MixAlpha");
            editor.ShaderProperty(_MixAlpha,MakeLabel(_MixAlpha));
            MaterialProperty _TillingBase = FindProperty("_TillingBase");
            editor.ShaderProperty(_TillingBase,MakeLabel(_TillingBase));
            if (_TillingBase.floatValue == 1)
            {
                GUILayout.Label("主纹理流动：U方向Custom1.z，V方向Custom1.w");
            }
        }
    }

    void DoMask()
    {
        if (mat.IsKeywordEnabled("_USEMASK_ON"))
        {
            GUI.color = Color.green;
        }
        else
        {
            GUI.color = Color.white;
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("遮罩",GUILayout.Height(20)))
        {
            if (mat.IsKeywordEnabled("_USEMASK_ON"))
            {
                mat.DisableKeyword("_USEMASK_ON");
            }
            else
            {
                mat.EnableKeyword("_USEMASK_ON");
            }
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;
        if (mat.IsKeywordEnabled("_USEMASK_ON"))
        {
            MaterialProperty _MaskChannel = FindProperty("_MaskChannel");
            editor.ShaderProperty(_MaskChannel,MakeLabel(_MaskChannel));
            MaterialProperty _MaskTex = FindProperty("_MaskTex");
            editor.TexturePropertySingleLine(MakeLabel(_MaskTex,"Mask"), _MaskTex);
            editor.TextureScaleOffsetProperty(_MaskTex);
        }

    }

    void DoNoise()
    {
        if (mat.IsKeywordEnabled("_USENOISE_ON"))
        {
            GUI.color = Color.green;
        }
        else
        {
            GUI.color = Color.white;
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("扭曲",GUILayout.Height(20)))
        {
            if (mat.IsKeywordEnabled("_USENOISE_ON"))
            {
                mat.DisableKeyword("_USENOISE_ON");
            }
            else
            {
                mat.EnableKeyword("_USENOISE_ON");
            }
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;
        if (mat.IsKeywordEnabled("_USENOISE_ON"))
        {
            MaterialProperty _NoiseTex = FindProperty("_NoiseTex");
            editor.TexturePropertySingleLine(MakeLabel(_NoiseTex,"_NoiseTex"), _NoiseTex);
            editor.TextureScaleOffsetProperty(_NoiseTex);
            MaterialProperty _NoiseU2 = FindProperty("_NoiseU2");
            editor.ShaderProperty(_NoiseU2,MakeLabel(_NoiseU2));
            if (_NoiseU2.floatValue == 0)
            {
                MaterialProperty _NoiseScale = FindProperty("_NoiseScale");
                editor.ShaderProperty(_NoiseScale,MakeLabel(_NoiseScale));
            }
            else
            {
                GUILayout.Label("扭曲强度：Custom2.x");
            }

        }
    }
    
    void DoDissolve()
    {
        if (mat.IsKeywordEnabled("_USEDISSOLVE_ON"))
        {
            GUI.color = Color.green;
        }
        else
        {
            GUI.color = Color.white;
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("溶解",GUILayout.Height(20)))
        {
            if (mat.IsKeywordEnabled("_USEDISSOLVE_ON"))
            {
                mat.DisableKeyword("_USEDISSOLVE_ON");
            }
            else
            {
                mat.EnableKeyword("_USEDISSOLVE_ON");
            }
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;
        if (mat.IsKeywordEnabled("_USEDISSOLVE_ON"))
        {
            MaterialProperty _DissolveTex = FindProperty("_DissolveTex");
            editor.TexturePropertySingleLine(MakeLabel(_DissolveTex,"_DissolveTex"), _DissolveTex);
            editor.TextureScaleOffsetProperty(_DissolveTex);
            MaterialProperty _DissolvelU2 = FindProperty("_DissolvelU2");
            editor.ShaderProperty(_DissolvelU2,MakeLabel(_DissolvelU2));
            if (_DissolvelU2.floatValue == 0)
            {
                MaterialProperty _DissolveInt = FindProperty("_DissolveInt");
                editor.ShaderProperty(_DissolveInt,MakeLabel(_DissolveInt));
            }
            else
            {
                GUILayout.Label("溶解值：Custom2.y");
            }
            MaterialProperty _DissolveHardness = FindProperty("_DissolveHardness");
            editor.ShaderProperty(_DissolveHardness,MakeLabel(_DissolveHardness));
            MaterialProperty _DissolveEdg = FindProperty("_DissolveEdg");
            editor.ShaderProperty(_DissolveEdg,MakeLabel(_DissolveEdg));
            MaterialProperty _DissolveColor = FindProperty("_DissolveColor");
            editor.ShaderProperty(_DissolveColor,MakeLabel(_DissolveColor));
        }
    }
    
    void DoFlowMap()
    {
        if (mat.IsKeywordEnabled("_FLOWMAP_ON"))
        {
            GUI.color = Color.green;
        }
        else
        {
            GUI.color = Color.white;
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("纹理流动",GUILayout.Height(20)))
        {
            if (mat.IsKeywordEnabled("_FLOWMAP_ON"))
            {
                mat.DisableKeyword("_FLOWMAP_ON");
            }
            else
            {
                mat.EnableKeyword("_FLOWMAP_ON");
            }
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;
        if (mat.IsKeywordEnabled("_FLOWMAP_ON"))
        {
            MaterialProperty _MainMaskFlow = FindProperty("_MainMaskFlow");
            editor.ShaderProperty(_MainMaskFlow,MakeLabel(_MainMaskFlow));
            MaterialProperty _NoiseDissFlow = FindProperty("_NoiseDissFlow");
            editor.ShaderProperty(_NoiseDissFlow,MakeLabel(_NoiseDissFlow));
        }
    }
    void DoSoftPartical()
    {
        if (_SrcBlend.floatValue==1 && _DstBlend.floatValue == 0)
        {
            return;
        }

        if (mat.IsKeywordEnabled("_SOFTPARTICAL_ON"))
        {
            GUI.color = Color.yellow;
        }
        else
        {
            GUI.color = Color.white;
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("软粒子",GUILayout.Height(20)))
        {
            if (mat.IsKeywordEnabled("_SOFTPARTICAL_ON"))
            {
                mat.DisableKeyword("_SOFTPARTICAL_ON");
            }
            else
            {
                mat.EnableKeyword("_SOFTPARTICAL_ON");
            }
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;
        if (mat.IsKeywordEnabled("_SOFTPARTICAL_ON"))
        {
            MaterialProperty _SoftDistance = FindProperty("_SoftDistance");
            editor.ShaderProperty(_SoftDistance,MakeLabel(_SoftDistance));
        }
    }
    
    private void DoOther()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("其它参数",GUILayout.Height(20)))
        {
            isOther = !isOther;
        }
        EditorGUILayout.EndHorizontal();
        if (isOther)
        {
            MaterialProperty _SrcBlend = FindProperty("_SrcBlend");
            MaterialProperty _DstBlend = FindProperty("_DstBlend");
            MaterialProperty _ZWriteMode = FindProperty("_ZWriteMode");
            MaterialProperty _ZTestMode = FindProperty("_ZTestMode");
            editor.ShaderProperty(_SrcBlend,MakeLabel(_SrcBlend));
            editor.ShaderProperty(_DstBlend,MakeLabel(_DstBlend));
            editor.ShaderProperty(_ZWriteMode,MakeLabel(_ZWriteMode));
            editor.ShaderProperty(_ZTestMode,MakeLabel(_ZTestMode));
            //editor.ShaderProperty(renderQueue,MakeLabel(renderQueue));
            editor.DoubleSidedGIField();
            //绘制调节队列的控件
            editor.RenderQueueField();
        }
        
    }
    
    private void DoButton()
    {
        GUI.color = Color.green;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("清理冗余"))
        {
            ClearMaterial();
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;
        //没有找到属性直接返回
        if (_SrcBlend == null || _DstBlend == null)
            return;
        if (_SrcBlend.type != MaterialProperty.PropType.Float || _DstBlend.type != MaterialProperty.PropType.Float)
            return;
        
        if (_SrcBlend.floatValue==1 && _DstBlend.floatValue == 0)
        {
            GUI.color = Color.green;
        }
        else
        {
            GUI.color = Color.white;
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("设置为不透明"))
        {
            _SrcBlend.floatValue = 1;
            _DstBlend.floatValue = 0;
            _ZWriteMode.floatValue = 1;
                
            foreach (Material m in _SrcBlend.targets)
            {
                if (m.shader.renderQueue == 2000)
                {
                    m.renderQueue = -1;//fromShader
                }
                else
                {
                    m.renderQueue = 2000;
                }
                m.DisableKeyword("_SOFTPARTICAL_ON");
            }
        }
        GUI.color = Color.white;
        if (_SrcBlend.floatValue == 5 && _DstBlend.floatValue == 10 )
        {
            GUI.color = Color.green;
        }
        else
        {
            GUI.color = Color.white;
        }
        if (GUILayout.Button("设置为Alpha"))
        {
            _SrcBlend.floatValue = 5;
            _DstBlend.floatValue = 10;
            _ZWriteMode.floatValue = 0;
            foreach (Material m in _SrcBlend.targets)
            {
                if (m.shader.renderQueue == 3000)
                {
                    m.renderQueue = -1;//fromShader
                }
                else
                {
                    m.renderQueue = 3000;
                }
            }
        }
        GUI.color = Color.white;
        
        if (_SrcBlend.floatValue == 1 && _DstBlend.floatValue == 1 )
        {
            GUI.color = Color.green;
        }
        else
        {
            GUI.color = Color.white;
        }
        if (GUILayout.Button("设置为ADD"))
        {
            _SrcBlend.floatValue = 1;
            _DstBlend.floatValue = 1;
            _ZWriteMode.floatValue = 0;
            foreach (Material m in _SrcBlend.targets)
            {
                if (m.shader.renderQueue == 3000)
                {
                    m.renderQueue = -1;//fromShader
                }
                else
                {
                    m.renderQueue = 3000;
                }
            }
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
    }
    private void ClearMaterial()
    {
        bool isChange = false;
        if (mat!=null)
        {
            SerializedObject m_serializedObject = new SerializedObject(mat);
            Shader shader = mat.shader;
            var shaderKeys = shader.keywordSpace.keywordNames;

            var matKeys = mat.shaderKeywords;
            foreach (string matKey in matKeys)
            {
                bool noUse = false;
                if (!shaderKeys.Contains(matKey))
                {
                    noUse = true;
                }
                if (noUse)
                {
                    mat.DisableKeyword(matKey);
                    isChange = true;
                }
            }
            
            //清理贴图信息
            SerializedProperty m_TexEnvs_properties = m_serializedObject.FindProperty("m_SavedProperties.m_TexEnvs");
            for (int i = 0; i < m_TexEnvs_properties.arraySize; i++)
            {
                string propName = m_TexEnvs_properties.GetArrayElementAtIndex(i).displayName;
                //如果属性不存在，则清理这个属性和附带的贴图
                if (!mat.HasProperty(propName))
                {
                    mat.SetTexture(propName,null);
                    m_TexEnvs_properties.DeleteArrayElementAtIndex(i);
                    i--;
                    isChange = true;
                }
                else
                {
                    if (CheckShader(propName))
                    {
                        mat.SetTexture(propName,null);
                        isChange = true;
                    }
                }
            }
            //清理Float信息
            SerializedProperty m_Floats_properties = m_serializedObject.FindProperty("m_SavedProperties.m_Floats");
            for (int i = 0; i < m_Floats_properties.arraySize; i++)
            {
                string propName = m_Floats_properties.GetArrayElementAtIndex(i).displayName;
                if (!mat.HasProperty(propName))
                {
                    m_Floats_properties.DeleteArrayElementAtIndex(i);
                    i--;
                    isChange = true;
                }
            }
            //清理Color信息
            SerializedProperty m_Colors_properties = m_serializedObject.FindProperty("m_SavedProperties.m_Colors");
            for (int i = 0; i < m_Colors_properties.arraySize; i++)
            {
                string propName = m_Colors_properties.GetArrayElementAtIndex(i).displayName;
                if (!mat.HasProperty(propName))
                {
                    m_Colors_properties.DeleteArrayElementAtIndex(i);
                    i--;
                    isChange = true;
                }
            }

            if (isChange)
            {
                m_serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(mat);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("MaterialName:"+ mat.name + "...Clear!");
            }
        }
    }
    
    private bool CheckShader(string propName)
    {
        if (!mat.IsKeywordEnabled("_USEMASK_ON"))
        {
            if (propName == "_MaskTex")
            {
                return true;
            }
        }
        if (!mat.IsKeywordEnabled("_USENOISE_ON"))
        {
            if (propName == "_NoiseTex")
            {
                return true;
            }
        }
        if (!mat.IsKeywordEnabled("_USEDISSOLVE_ON"))
        {
            if (propName == "_DissolveTex")
            {
                return true;
            }
        }
        return false;
    }
    MaterialProperty FindProperty(string name)
    {
        return FindProperty(name, properties);
    }
    static GUIContent MakeLabel(MaterialProperty property,string tooltip = null)
    {
        staticLabel.text = property.displayName;
        staticLabel.tooltip = tooltip;
        return staticLabel;
    }

}

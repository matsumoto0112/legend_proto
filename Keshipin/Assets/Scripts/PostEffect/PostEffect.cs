using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class PostEffect : MonoBehaviour
{
    [SerializeField]
    private Shader _shader;
    private Material _material;

    void Start()
    {
        _material = new Material(_shader);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, _material); // ポストエフェクト
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PostEffect))]
public class PostEffectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

}
#endif

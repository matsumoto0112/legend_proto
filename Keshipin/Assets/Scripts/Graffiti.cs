using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graffiti : MonoBehaviour
{
    [SerializeField, Range(4, 1024)]
    private int texSize = 32;
    [SerializeField]
    private Transform obj;
    [SerializeField, Range(0.0f, 10.0f)]
    private float area = 0.5f;
    [SerializeField, Range(0.0f, 10.0f)]
    private float speed = 3.0f;

    [SerializeField] private Shader graffiti_sha_;
    [SerializeField] private Texture2D graffiti_tex_;
    private Material graffiti_mat_;

    private Color[,] colors;
    private Texture2D graffiti_msk_;
    private MeshRenderer mr;

    private Color Alpha_One { get { return Color.white; } }
    private Color Alpha_Zero { get { return Color.white * 0.0f; } }

    // Start is called before the first frame update
    void Start()
    {
        CreateColors();
        CreateTexture();
        CreateMaterial();
        SetMaterial();
        mr = GetComponent<MeshRenderer>();
        mr.material = graffiti_mat_;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        RandomMask();
        ResetTexture();
        DrawTexture();
    }

    private void Move()
    {
        if (obj == null) return;
        var mov = Vector3.zero;
        mov.x = Input.GetAxis("Horizontal");
        mov.z = Input.GetAxis("Vertical");
        mov.Normalize();
        obj.position += mov * speed * Time.deltaTime;
    }
    private void RandomMask()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        for (int y = 0; y < colors.GetLength(0); y++)
        {
            for (int x = 0; x < colors.GetLength(1); x++)
            {
                SetColor((Random.value < 0.5f ? Alpha_One : Alpha_Zero), x, y);
            }
        }
        CreateTexture();
        SetMaterial();
        Debug.Log("RandomMask");
    }
    private void ResetTexture()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        for (int y = 0; y < colors.GetLength(0); y++)
        {
            for (int x = 0; x < colors.GetLength(1); x++)
            {
                Vector2 point = (new Vector2((float)x / (float)texSize, (float)y / (float)texSize));
                SetColor(Alpha_One, x, y);
            }
        }
    }
    private void DrawTexture()
    {
        if (obj == null) return;
        var raycast = new List<RaycastHit>(Physics.RaycastAll(obj.position, Vector3.down));
        raycast.RemoveAll(i => i.transform != transform);
        if (raycast.Count <= 0) return;
        var hit = raycast[0];
        var drawPoint = new Vector2(hit.textureCoord.x /** graffiti_msk_.width*/, hit.textureCoord.y /** graffiti_msk_.height*/);
        for (int y = 0; y < colors.GetLength(0); y++)
        {
            for (int x = 0; x < colors.GetLength(1); x++)
            {
                Vector2 point = (new Vector2((float)x / (float)graffiti_msk_.width, (float)y / (float)graffiti_msk_.height));
                //SetColor(((point).magnitude <= area ? Alpha_Zero : Alpha_One), x, y);
                SetColor(((point - drawPoint).magnitude <= area ? Alpha_Zero : colors[y, x]), x, y);
                //if((point - drawPoint).magnitude <= area)
                //{
                //    Debug.Log("( "+point + " - " + drawPoint+" ) <= "+area+" >> "+ ((point - drawPoint).magnitude <= area));
                //}
            }
        }
        CreateTexture();
        SetMaterial();
    }

    private Color[,] CreateColors()
    {
        colors = new Color[texSize, texSize];

        for (int y = 0; y < texSize; y++)
        {
            for (int x = 0; x < texSize; x++)
            {
                colors[y, x] = Color.white;
            }
        }

        return colors;
    }
    private Texture2D CreateTexture()
    {
        graffiti_msk_ = Create.GetTex2D(colors);
        return graffiti_msk_;
    }

    private void CreateMaterial()
    {
        if (graffiti_sha_ == null || Shader.Find(graffiti_sha_.name) == null)
        {
            return;
        }
        graffiti_mat_ = new Material(graffiti_sha_);
    }
    private void SetMaterial()
    {
        graffiti_mat_.SetTexture("_MainTex", graffiti_tex_);
        graffiti_mat_.SetTexture("_MaskTex", graffiti_msk_);
    }

    private void SetColor(Color color, int x, int y)
    {
        var xSize = (x < 0 || colors.GetLength(1) <= x);
        var ySize = (y < 0 || colors.GetLength(0) <= y);
        if (xSize || ySize) return;

        colors[y, x] = color;
        graffiti_msk_.SetPixel(x, y, color);
    }
}

using UnityEngine;
using UnityEngine.UI.ProceduralImage;

[ModifierID("Free")]
public class FreeModifier : ProceduralImageModifier
{
    [SerializeField] private Vector4 radius;
    [SerializeField, Range(0, 100)] private float skewTopRight = 0.5f;
    [SerializeField, Range(0, 100)] private float skewBottomRight = 0.5f;
    [SerializeField, Range(0, 100)] private float skewTopLeft = 0.5f;
    [SerializeField, Range(0, 100)] private float skewBottomLeft = 0.5f;

    public Vector4 Radius
    {
        get { return radius; }
        set
        {
            radius = value;
            _Graphic.SetVerticesDirty();
        }
    }

    public float SkewTopRight
    {
        get { return skewTopRight; }
        set
        {
            skewTopRight = value;
            _Graphic.SetVerticesDirty();
        }
    }

    public float SkewBottomRight
    {
        get { return skewBottomRight; }
        set
        {
            skewBottomRight = value;
            _Graphic.SetVerticesDirty();
        }
    }

    public float SkewTopLeft
    {
        get { return skewTopLeft; }
        set
        {
            skewTopLeft = value;
            _Graphic.SetVerticesDirty();
        }
    }

    public float SkewBottomLeft
    {
        get { return skewBottomLeft; }
        set
        {
            skewBottomLeft = value;
            _Graphic.SetVerticesDirty();
        }
    }

    #region implemented abstract members of ProceduralImageModifier

    public override Vector4 CalculateRadius(Rect imageRect)
    {
        return radius;
    }

    #endregion

    protected void OnValidate()
    {
        radius.x = Mathf.Max(0, radius.x);
        radius.y = Mathf.Max(0, radius.y);
        radius.z = Mathf.Max(0, radius.z);
        radius.w = Mathf.Max(0, radius.w);
        _Graphic.SetVerticesDirty();
    }
}
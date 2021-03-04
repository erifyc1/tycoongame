using UnityEngine;

public class Utils
{
    public static string GetDirection(Direction orientation, bool north, bool east, bool south, bool west)
    {
        bool left = orientation switch
        {
            Direction.NORTH => west,
            Direction.EAST => north,
            Direction.SOUTH => east,
            Direction.WEST => south,
            _ => false
        };
        bool back = orientation switch
        {
            Direction.NORTH => south,
            Direction.EAST => west,
            Direction.SOUTH => north,
            Direction.WEST => east,
            _ => false
        };
        bool right = orientation switch
        {
            Direction.NORTH => east,
            Direction.EAST => south,
            Direction.SOUTH => west,
            Direction.WEST => north,
            _ => false
        };

        if (!left && !right)
        {
            return "conveyor";
        }
        else if (!back)
        {
            if (!left && right)
            {
                return "conv_right";
            }
            else if (left && !right)
            {
                return "conv_left";
            }
            else if (left && right)
            {
                return "conv_m_both";
            }
        }
        else if (back)
        {
            if (!left && right)
            {
                return "conv_m_right";
            }
            else if (left && !right)
            {
                return "conv_m_left";
            }
            else if (left && right)
            {
                return "conv_m_full";
            }
        }
        Debug.Log("shit happened it didnt work");
        return "fuck";
    }


    public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                material.SetOverrideTag("RenderType", "");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                material.SetOverrideTag("RenderType", "TransparentCutout");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                break;
            case BlendMode.Fade:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case BlendMode.Transparent:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
        }
    }

}




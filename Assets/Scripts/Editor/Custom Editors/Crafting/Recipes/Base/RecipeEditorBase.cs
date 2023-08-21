using UnityEngine;
using UnityEditor;

using static EditorUtils;

public abstract class RecipeEditorBase : Editor
{
    protected float width = 100f;
    protected Material material;
    protected Color backgroundColor = new(0.169f, 0.169f, 0.169f);
    protected ItemBase[][] items;
    protected string[][] itemsName;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        RecipeBase recipe = (RecipeBase)target;
        ItemBase itemTarget = recipe.Target;

        // Basic parameters
        DrawProperty(serializedObject, "Name");

        // Success parameters
        DrawProperty(serializedObject, "AlwaysSuccess");
        
        if (recipe is ISuccessRate successRate &&
            !successRate.AlwaysSuccess)
        {
            DrawProperty(serializedObject, "SuccessRate");
        }

        DrawProperty(serializedObject, "TargetQuantity");

        material = recipe.Material;

        DrawItemCenter(itemTarget);

        Horizontal(() =>
        {
            FlexibleSpace(() =>
            {
                DrawPropertyRaw(serializedObject, "Target",
                    GUILayout.Width(width));
            });
        });

        for (int i = 0; i < items.Length; ++i)
        {
            Horizontal(() =>
            {
                for (int j = 0; j < items[i].Length; ++j)
                {
                    FlexibleSpace(() =>
                    {
                        Vertical(() =>
                        {
                            DrawItem(items[i][j]);
                            DrawPropertyRaw(serializedObject,
                                itemsName[i][j], GUILayout.Width(width));
                        });
                    });
                }
            });
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected void DrawItemCenter(ItemBase item)
    {
        if (item)
        {
            if (item.Atlas)
            {
                DrawSpriteFromSheetCenter(item.Sprite,
                    width, material, backgroundColor);
            }
            else
            {
                DrawSpriteCenter(item.Sprite, width,
                    material, backgroundColor);
            }
        }
        else
        {
            DrawBoxCenter(width, backgroundColor);
        }
    }

    protected void DrawItem(ItemBase item)
    {
        if (item)
        {
            if (item.Atlas)
            {
                DrawSpriteFromSheet(item.Sprite, width,
                    material, backgroundColor);
            }
            else
            {
                DrawSprite(item.Sprite, width, material,
                    backgroundColor);
            }
        }
        else
        {
            DrawBox(width, backgroundColor);
        }
    }
}
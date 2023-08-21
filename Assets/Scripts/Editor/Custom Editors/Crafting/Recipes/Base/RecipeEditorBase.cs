using UnityEngine;
using UnityEditor;

using static EditorUtils;

public abstract class RecipeEditorBase : Editor
{
    protected float _width = 100f;
    protected Material _material;
    protected Color _backgroundColor = new(0.169f, 0.169f, 0.169f);
    protected ItemBase[][] _items;
    protected string[][] _itemsName;

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

        _material = recipe.Material;

        DrawItemCenter(itemTarget);

        Horizontal(() =>
        {
            FlexibleSpace(() =>
            {
                DrawPropertyRaw(serializedObject, "Target",
                    GUILayout.Width(_width));
            });
        });

        for (int i = 0; i < _items.Length; ++i)
        {
            Horizontal(() =>
            {
                for (int j = 0; j < _items[i].Length; ++j)
                {
                    FlexibleSpace(() =>
                    {
                        Vertical(() =>
                        {
                            DrawItem(_items[i][j]);
                            DrawPropertyRaw(serializedObject,
                                _itemsName[i][j], GUILayout.Width(_width));
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
                    _width, _material, _backgroundColor);
            }
            else
            {
                DrawSpriteCenter(item.Sprite, _width,
                    _material, _backgroundColor);
            }
        }
        else
        {
            DrawBoxCenter(_width, _backgroundColor);
        }
    }

    protected void DrawItem(ItemBase item)
    {
        if (item)
        {
            if (item.Atlas)
            {
                DrawSpriteFromSheet(item.Sprite, _width,
                    _material, _backgroundColor);
            }
            else
            {
                DrawSprite(item.Sprite, _width, _material,
                    _backgroundColor);
            }
        }
        else
        {
            DrawBox(_width, _backgroundColor);
        }
    }
}

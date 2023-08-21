using UnityEngine;
using UnityEditor;

using static EditorUtils;

[CustomEditor(typeof(BasicRecipe))]
public class BasicRecipeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BasicRecipe recipe = (BasicRecipe)target;
        ItemBase itemTarget = recipe.Target;

        float width = 100f;

        DrawProperty(serializedObject, "Name");
        DrawProperty(serializedObject, "TargetQuantity");

        Color backgroundColor = new(0.169f, 0.169f, 0.169f);
        Material material = recipe.Material;

        DrawItemCenter(itemTarget);

        Horizontal(() =>
        {
            FlexibleSpace(() =>
            {
                DrawPropertyRaw(serializedObject, "Target",
                    GUILayout.Width(width));
            });
        });

        ItemBase[][] items = { new []{ recipe.Row1Col1, recipe.Row1Col2 },
            new []{ recipe.Row2Col1, recipe.Row2Col2 } };
        string[][] itemsName = { new []{ "Row1Col1", "Row1Col2" },
            new []{ "Row2Col1", "Row2Col2" } };

        for (int i = 0; i < items.Length; ++i)
        {
            Horizontal(() =>
            {
                FlexibleSpace(() =>
                {
                    Vertical(() =>
                    {
                        DrawItem(items[i][0]);
                        DrawPropertyRaw(serializedObject, itemsName[i][0],
                            GUILayout.Width(width));
                    });
                });

                FlexibleSpace(() =>
                {
                    Vertical(() =>
                    {
                        DrawItem(items[i][1]);
                        DrawPropertyRaw(serializedObject, itemsName[i][1],
                            GUILayout.Width(width));
                    });
                });
            });
        }

        serializedObject.ApplyModifiedProperties();

        void DrawItemCenter(ItemBase item)
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
                DrawBoxCenter(width);
            }
        }

        void DrawItem(ItemBase item)
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
                DrawBox(width);
            }
        }
    }
}

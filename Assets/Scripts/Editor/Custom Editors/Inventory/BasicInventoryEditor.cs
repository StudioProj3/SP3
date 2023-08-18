using UnityEditor;

[CustomEditor(typeof(BasicInventory))]
public class BasicInventoryEditor : ReorderEditor
{
    protected override void OnEnable()
    {
        base.OnEnable();

        AddProperty("MaxNumSlots");
        AddProperty("MaxPerSlot");
        AddField("_itemInitializerList");
    }
}

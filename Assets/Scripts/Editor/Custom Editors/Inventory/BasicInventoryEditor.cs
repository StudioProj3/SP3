using UnityEditor;

[CustomEditor(typeof(BasicInventory))]
public class BasicInventoryEditor : ReorderEditorBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        AddProperty("MaxNumSlots");
        AddProperty("MaxPerSlot");
        AddField("_itemInitializerList");

        AddProperty("EnableSave");
        AddProperty("SaveID");
        AddProperty("Format");
    }
}

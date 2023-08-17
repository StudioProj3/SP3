using System.Collections.Generic;
using System.Linq;

using UnityEngine;

// using static DebugUtils;

public class UIShop : MonoBehaviour
{
    [SerializeField]
    private List<ShopItem> _materials = new();

    private void OnValidate()
    {
    }

    private void Start()
    {
        var notNullMaterials = _materials.Where(m => m is not null);
        var distinctMaterials = notNullMaterials.Distinct();
        // Assert(notNullMaterials.Count() == distinctMaterials.Count(), "There is a duplicate in the materials.");
    }
}
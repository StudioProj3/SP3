using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public interface ISwordWeapon : IBeginUseHandler, IEndUseHandler
{
    string AnimationName { get; }
}

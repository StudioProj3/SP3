public interface ISwordWeapon :
    IBeginUseHandler, IEndUseHandler
{
    string AnimationName { get; }
}

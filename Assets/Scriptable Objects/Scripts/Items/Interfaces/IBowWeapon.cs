public interface IBowWeapon : 
    IBeginUseHandler, IUseHandler, IEndUseHandler
{
    string AnimationName { get; }
}

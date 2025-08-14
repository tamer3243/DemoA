public interface IPlayerInput
{
    float GetHorizontal();
    float GetVertical();
    bool IsJumpPressed();
    bool IsDashPressed();


    bool IsAttackPressed();
    bool IsSwapPressed();
}

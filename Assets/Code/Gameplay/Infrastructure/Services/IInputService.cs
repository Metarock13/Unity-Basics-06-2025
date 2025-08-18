namespace Code.Gameplay.Infrastructure.Services
{
  public interface IInputService
  {
    float GetVerticalMoveAxis();
    float GetHorizontalMoveAxis();
    float GetVerticalLookAxis();
    float GetHorizontalLookAxis();
    bool HasMoveInput();
    bool HasLookInput();
    bool IsJumpBtnUp();
    bool HasShootInput();
  }
}
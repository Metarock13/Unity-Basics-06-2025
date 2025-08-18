using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Gameplay.Infrastructure.Services
{
  public class StandaloneInputService : MonoBehaviour, IInputService
  {
    public float GetVerticalMoveAxis() => 
      UnityEngine.Input.GetAxis("Vertical");

    public float GetHorizontalMoveAxis() => 
      UnityEngine.Input.GetAxis("Horizontal");

    public float GetVerticalLookAxis() => 
      Mouse.current.delta.value.y;

    public float GetHorizontalLookAxis() => 
      Mouse.current.delta.value.x;

    public bool HasMoveInput() => 
      GetHorizontalMoveAxis() != 0 || GetVerticalMoveAxis() != 0;

    public bool HasLookInput() => 
      Mouse.current.delta.value.x != 0 || Mouse.current.delta.value.y != 0;

    public bool IsJumpBtnUp() => 
      UnityEngine.Input.GetKeyUp(KeyCode.Space);

    public bool HasShootInput() => 
      UnityEngine.Input.GetMouseButton(0);
  }
}
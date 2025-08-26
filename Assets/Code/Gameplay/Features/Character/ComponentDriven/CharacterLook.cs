using Code.Gameplay.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Features.Character.ComponentDriven
{
    public class CharacterLook : MonoBehaviour
    {
        [SerializeField] private float _mouseSensitivity = 1f;
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private StandaloneInputService _inputService;

        private float _xRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (PauseMenu.IsPaused) return;

            float sens = PlayerPrefs.GetFloat("sensitivity", _mouseSensitivity);
            float xLook = _inputService.GetHorizontalLookAxis() * sens * Time.unscaledDeltaTime * 100f;
            float yLook = _inputService.GetVerticalLookAxis() * sens * Time.unscaledDeltaTime * 100f;

            _xRotation -= yLook;
            _xRotation = Mathf.Clamp(_xRotation, -80, 40);

            if (_cameraRoot) _cameraRoot.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            if (_playerBody) _playerBody.Rotate(Vector3.up * xLook);
        }
    }
}
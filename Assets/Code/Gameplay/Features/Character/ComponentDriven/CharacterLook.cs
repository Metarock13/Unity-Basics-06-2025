using System;
using Code.Gameplay.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Features.Character.ComponentDriven
{
   public class CharacterLook : MonoBehaviour
   {
      [SerializeField] private float _mouseSensitivity;
      [SerializeField] private Transform _cameraRoot;
      [SerializeField] private Transform _playerBody;
      [SerializeField] private StandaloneInputService _inputService;

      private float _xLook;
      private float _yLook;
      private float _xRotation;

      private void Start()
      {
         Cursor.lockState = CursorLockMode.Locked;
      }

      private void Update()
      {
         Look();
      }

      private void Look()
      {
         _xLook = _inputService.GetHorizontalLookAxis() * _mouseSensitivity * Time.deltaTime;
         _yLook = _inputService.GetVerticalLookAxis() * _mouseSensitivity * Time.deltaTime;

         _xRotation -= _yLook;
         _xRotation = Mathf.Clamp(_xRotation, -80, 40);

         _cameraRoot.localRotation = Quaternion.Euler(_xRotation, 0, 0);

         _playerBody.Rotate(Vector3.up * _xLook);
      }
   }
}
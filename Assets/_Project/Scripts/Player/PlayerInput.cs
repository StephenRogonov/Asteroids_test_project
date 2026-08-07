using _Project.Scripts.PlayerWeapons;
using _Project.Scripts.UI;
using System;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
    public class PlayerInput : IDisposable
    {
        private ShipMovement _shipMovement;
        private WeaponTrigger _weaponTrigger;
        private PlayerControls _playerControls;
        private PauseModel _pauseController;

        public PlayerInput(
            PlayerControls playerControls,
            WeaponTrigger uiController,
            PauseModel pauseButton
            )
        {
            _playerControls = playerControls;
            _weaponTrigger = uiController;
            _pauseController = pauseButton;

            SubscribeToActions();
        }

        public void Init(ShipMovement shipMovement)
        {
            _shipMovement = shipMovement;
        }

        private void SubscribeToActions()
        {
            _playerControls.Enable();

            _playerControls.Player.MoveForward.performed += Move;
            _playerControls.Player.MoveForward.canceled += Move;

            _playerControls.Player.RotateLeft.performed += PlayerRotateLeft;
            _playerControls.Player.RotateLeft.canceled += PlayerRotateLeft;

            _playerControls.Player.RotateRight.performed += PlayerRotateRight;
            _playerControls.Player.RotateRight.canceled += PlayerRotateRight;

            _playerControls.Player.ShootMissile.performed += ShootMissile;
            _playerControls.Player.ShootLaser.performed += ShootLaser;

            _playerControls.Player.Pause.performed += Pause;
        }

        public void Dispose()
        {
            _playerControls.Disable();

            _playerControls.Player.MoveForward.performed -= Move;
            _playerControls.Player.MoveForward.canceled -= Move;

            _playerControls.Player.RotateLeft.performed -= PlayerRotateLeft;
            _playerControls.Player.RotateLeft.canceled -= PlayerRotateLeft;

            _playerControls.Player.RotateRight.performed -= PlayerRotateRight;
            _playerControls.Player.RotateRight.canceled -= PlayerRotateRight;

            _playerControls.Player.ShootMissile.performed -= ShootMissile;
            _playerControls.Player.ShootLaser.performed -= ShootLaser;

            _playerControls.Player.Pause.performed -= Pause;
        }

        private void Move(InputAction.CallbackContext context)
        {
            if (_shipMovement != null)
            {
                if (context.performed)
                {
                    _shipMovement.Move(true);
                }
                else if (context.canceled)
                {
                    _shipMovement.Move(false);
                }
            }
        }

        private void PlayerRotateLeft(InputAction.CallbackContext context)
        {
            if (_shipMovement != null)
            {
                if (context.performed)
                {
                    _shipMovement.Rotate(1f);
                }
                else if (context.canceled)
                {
                    _shipMovement.Rotate(0f);
                }
            }
        }

        private void PlayerRotateRight(InputAction.CallbackContext context)
        {
            if (_shipMovement != null)
            {
                if (context.performed)
                {
                    _shipMovement.Rotate(-1f);
                }
                else if (context.canceled)
                {
                    _shipMovement.Rotate(0f);
                }
            }
        }

        private void ShootMissile(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _weaponTrigger.ShootMissile();
            }
        }

        private void ShootLaser(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _weaponTrigger.ShootLaser();
            }
        }

        private void Pause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _pauseController.PauseGame();
            }
        }
    }
}
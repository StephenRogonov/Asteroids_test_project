using UnityEngine;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class ScreenWrap : MonoBehaviour
    {
        private Camera _mainCamera;
        private Rigidbody2D _rigidbody;
        private Vector2 _screenPosition;

        private float _rightSideOfTheScreen;
        private float _leftSideOfTheScreen;
        private float _topOfTheScreen;
        private float _bottomOfTheScreen;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _rigidbody = GetComponent<Rigidbody2D>();

            _rightSideOfTheScreen = _mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
            _leftSideOfTheScreen = _mainCamera.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
            _topOfTheScreen = _mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
            _bottomOfTheScreen = _mainCamera.ScreenToWorldPoint(new Vector2(0f, 0f)).y;
        }

        private void Update()
        {
            _screenPosition = _mainCamera.WorldToScreenPoint(transform.position);

            if (_screenPosition.x <= 0 && _rigidbody.linearVelocity.x < 0)
            {
                transform.position = new Vector2(_rightSideOfTheScreen, transform.position.y);
            }
            else if (_screenPosition.x >= Screen.width && _rigidbody.linearVelocity.x > 0)
            {
                transform.position = new Vector2(_leftSideOfTheScreen, transform.position.y);
            }
            else if (_screenPosition.y >= Screen.height && _rigidbody.linearVelocity.y > 0)
            {
                transform.position = new Vector2(transform.position.x, _bottomOfTheScreen);
            }
            else if (_screenPosition.y <= 0 && _rigidbody.linearVelocity.y < 0)
            {
                transform.position = new Vector2(transform.position.x, _topOfTheScreen);
            }
        }
    }
}
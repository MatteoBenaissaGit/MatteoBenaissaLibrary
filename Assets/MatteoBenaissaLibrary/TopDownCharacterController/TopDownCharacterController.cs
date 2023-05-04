using System;
using MatteoBenaissaLibrary.Attributes.ReadOnly;
using UnityEngine;

namespace MatteoBenaissaLibrary.TopDownCharacterController
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TopDownCharacterController : MonoBehaviour
    {
        [SerializeField, Range(0,20)] private float _speed = 5;
        [SerializeField, Range(0,1)] private float _deceleration = 0.5f;
        [SerializeField, Range(0,1)] private float _acceleration = 0.5f;
        
        [ReadOnly] public bool CanMove = true;
        
        private Vector2 _inputs;
        private Rigidbody2D _rigidbody;
        private SpriteView.SpriteView _spriteView;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteView = GetComponent<SpriteView.SpriteView>();
            
            _spriteView.OnActionEnd.AddListener(EndAction);
        }

        private void OnDestroy()
        {
            _spriteView.OnActionEnd.RemoveListener(EndAction);
        }

        private void Update()
        {
            HandleMovementInputs();
        }

        private void FixedUpdate()
        {
            ApplyAnimation();
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            float speed = CanMove && _inputs.magnitude > 0.1f ? _speed : 0;
            _rigidbody.velocity = _inputs.normalized * speed;
        }

        private void HandleMovementInputs()
        {
            //get raw inputs
            float rawInputX = Input.GetAxisRaw("Horizontal");
            float rawInputY = Input.GetAxisRaw("Vertical");
            //lerp current value toward raw
            float lerpValue = Mathf.Abs(rawInputX) + Mathf.Abs(rawInputY) < 0.1f ? _deceleration : _acceleration;
            float lerpX = Mathf.Lerp(_inputs.x, rawInputX, lerpValue);
            float lerpY = Mathf.Lerp(_inputs.y, rawInputY, lerpValue);
            //assign input value
            _inputs = new Vector2(lerpX, lerpY);
        }
        
        private void ApplyAnimation()
        {
            transform.localScale = new Vector3(1 * Math.Sign(_inputs.x), 1, 1);
            if (transform.localScale.x == 0)
            {
                transform.localScale = Vector3.one;
            }
            
            _spriteView.PlayState(_rigidbody.velocity.magnitude > 0.1f ? "Walk" : "Idle");
        }

        private void EndAction()
        {
            CanMove = true;
        }
    }
}
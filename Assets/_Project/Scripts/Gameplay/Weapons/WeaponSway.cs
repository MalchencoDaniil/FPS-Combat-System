using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private float _mouseX, _mouseY;

    [SerializeField] private float _smooth = 2, _multiplier = 3;

    private void Update()
    {
        MyInput();
        SwayWeapun();
    }

    private void MyInput()
    {
        _mouseX = Input.GetAxisRaw("Mouse X") * _multiplier;
        _mouseY = Input.GetAxisRaw("Mouse Y") * _multiplier;
    }

    private void SwayWeapun()
    {
        var _rotationX = Quaternion.AngleAxis(-_mouseY, Vector3.right);
        var _rotationY = Quaternion.AngleAxis(_mouseX, Vector3.up);

        var _targetRotation = _rotationX * _rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, _targetRotation, _smooth * Time.deltaTime);
    }
}
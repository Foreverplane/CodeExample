using UnityEngine;

public class CarView : View {
    [SerializeField]
    private float _SpeedMin, _SpeedMax;

    [SerializeField]
    private float _minX, _maxX;
    [SerializeField]
    private float _minZ, _maxZ;

    [SerializeField]
    private Vector3 _movementVector;

    private float _Speed;

    void Awake() {
        _Speed = Random.Range(_SpeedMin, _SpeedMax);
    }

    void Update() {
        transform.Translate(_movementVector * _Speed * Time.deltaTime, Space.World);
        var currentPosition = transform.position;
        if (currentPosition.x > _maxX) {
            currentPosition.x = _minX;
            transform.position = currentPosition;
            _Speed = Random.Range(_SpeedMin, _SpeedMax);
        }

        if (currentPosition.x < _minX) {
            currentPosition.x = _maxX;
            transform.position = currentPosition;
            _Speed = Random.Range(_SpeedMin, _SpeedMax);
        }

        if (currentPosition.z > _maxZ) {
            currentPosition.z = _minZ;
            transform.position = currentPosition;
            _Speed = Random.Range(_SpeedMin, _SpeedMax);
        }

        if (currentPosition.z < _minZ) {
            currentPosition.z = _maxZ;
            transform.position = currentPosition;
            _Speed = Random.Range(_SpeedMin, _SpeedMax);
        }

    }
}
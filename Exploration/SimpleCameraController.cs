using UnityEngine;

// **** Backbone of the Field camera

public class SimpleCameraController : MonoBehaviour
{
    #region PUBLIC FIELDS

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _distanceToTarget = 10.0f;

    [SerializeField]
    private float _smoothTime = 0.1f;

    // Define the boundary box collider
    [SerializeField]
    private BoxCollider boundaryBox;

    #endregion

    #region FIELDS

    private Vector3 _followVelocity;

    #endregion

    #region PROPERTIES

    public Transform target
    {
        get => _target;
        set => _target = value;
    }

    public float distanceToTarget
    {
        get => _distanceToTarget;
        set => _distanceToTarget = Mathf.Max(0.0f, value);
    }

    #endregion

    #region MONOBEHAVIOUR

    public void OnValidate()
    {
        distanceToTarget = _distanceToTarget;
    }

    public void Start()
    {
        if (_target == null)
            return;

        transform.position = target.position - transform.forward * distanceToTarget;
    }

    public void LateUpdate()
    {
        if (_target == null || boundaryBox == null)
            return;

        Vector3 targetPosition = target.position - transform.forward * distanceToTarget;

        // Clamp the camera's x and z coordinates within the boundaries of the box collider
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, boundaryBox.bounds.min.x, boundaryBox.bounds.max.x),
            targetPosition.y,
            Mathf.Clamp(targetPosition.z, boundaryBox.bounds.min.z, boundaryBox.bounds.max.z)
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _followVelocity,
            _smoothTime
        );
    }

    #endregion
}

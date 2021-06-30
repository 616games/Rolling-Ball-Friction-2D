using UnityEngine;

public class Ball : MonoBehaviour
{
    #region --Fields / Properties--
    
    /// <summary>
    /// Not to be confused with weight (gravity * mass).
    /// </summary>
    [SerializeField]
    private float _mass;
    
    /// <summary>
    /// An initial push to the right to get the balls moving.
    /// </summary>
    [SerializeField]
    private Vector3 _initialVelocity = new Vector3(.3f, 0, 0);
    
    /// <summary>
    /// Coefficient of friction for ice.
    /// </summary>
    [SerializeField]
    private float _iceFrictionCoefficient = .08f;

    /// <summary>
    /// Coefficient of friction for concrete.
    /// </summary>
    [SerializeField]
    private float _concreteFrictionCoefficient = .15f;

    /// <summary>
    /// Coefficient of friction for carpet.
    /// </summary>
    [SerializeField]
    private float _carpetFrictionCoefficient = .36f;
    
    /// <summary>
    /// The force exerted upwards on the ball from the ground.
    /// </summary>
    [SerializeField]
    private float _groundNormalForce = 1f;

    /// <summary>
    /// The force exerted downwards on the ball (Z axis in 2D).
    /// </summary>
    [SerializeField]
    private float _gravitationalConstant= .0001f;

    /// <summary>
    /// Speed and direction of the ball.
    /// </summary>
    private Vector3 _velocity;

    /// <summary>
    /// How fast the velocity is changing.
    /// </summary>
    private Vector3 _acceleration;

    /// <summary>
    /// Force that describes friction from an ice surface.
    /// </summary>
    private Vector3 _iceFriction;

    /// <summary>
    /// Force that describes friction from a concrete surface.
    /// </summary>
    private Vector3 _concreteFriction;

    /// <summary>
    /// Force that describes friction from a carpet surface.
    /// </summary>
    private Vector3 _carpetFriction;

    /// <summary>
    /// Force that describes gravity.
    /// </summary>
    private Vector3 _gravitationalForce;

    /// <summary>
    /// Cached Transform component.
    /// </summary>
    private Transform _transform;

    #endregion
    
    #region --Unity Specific Methods--
    
    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerStay(Collider _other)
    {
        if (_other.gameObject.CompareTag("Ice"))
        {
            ApplyForce(_iceFriction);
        }
        else if (_other.gameObject.CompareTag("Concrete"))
        {
            ApplyForce(_concreteFriction);
        }
        else if (_other.gameObject.CompareTag("Carpet"))
        {
            ApplyForce(_carpetFriction);
        }
    }
    
    #endregion
    
    #region --Custom Methods--

    /// <summary>
    /// Handles movement.
    /// </summary>
    private void Move()
    {
        ApplyForce(_gravitationalForce);
        _velocity += _acceleration;
        
        //Prevent the forces on the ball from changing its direction.
        if (_velocity.x < 0.0f)
        {
            _velocity = Vector3.zero;
        }

        Vector3 _position = _transform.position += _velocity;
        if (_position.z >= 0)
        {
            _position.z = 0;
        }
        
        _transform.position = _position;
    }

    /// <summary>
    /// Creates a net force added to acceleration after factoring in mass for the ball.
    /// </summary>
    private void ApplyForce(Vector3 _force)
    {
        if (_mass <= 0.0f)
        {
            _acceleration += _force;
            return;
        }

        _acceleration += _force / _mass;
    }

    /// <summary>
    /// Initializes variables and caches components.
    /// </summary>
    private void Init()
    {
        _iceFriction = new Vector3(CalculateFriction(_iceFrictionCoefficient), 0, 0);
        _concreteFriction = new Vector3(CalculateFriction(_concreteFrictionCoefficient), 0, 0);
        _carpetFriction = new Vector3(CalculateFriction(_carpetFrictionCoefficient), 0, 0);
        _gravitationalForce = new Vector3(0, 0, _gravitationalConstant * _mass);
        _transform = transform;
        _velocity = _initialVelocity;
    }

    /// <summary>
    /// Simulates the formula for friction in the real world.
    /// </summary>
    /// <returns></returns>
    private float CalculateFriction(float _coefficientOfFriction)
    {
        return -1f * _coefficientOfFriction * _groundNormalForce;
    }
    
    #endregion
    
}

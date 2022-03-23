using System.Collections;
using System.Collections.Generic;
using behaviour;
using tags;
using UnityEngine;

/// <summary>
/// CUBESPACE NINE
/// </summary>
public class SpaceshipController : MonoBehaviour
{
    [Range(0, 100f)] public float Velocity;
    [Range(0, 100f)] public float Rotation;

    [InspectorName("Torpedo launch speed")] [Range(0, 500f)]
    public float Torpspeed;

    public string Name;

    public GameObject Torpedo;
    [SerializeField] private SpaceshipState _state;

    private float _breaktimer;
    private bool isBreaking;
    /// <summary>
    /// this is used for the click movement.
    /// </summary>
    private Camera _camera;
    /// <summary>
    /// this is used for the click movement.
    /// </summary>
    private Vector3 _destination;
    private Rigidbody _rb;
    private IEnumerable<ShipSystems> _systems;
    private IEnumerable<WeaponSystem> _weapon;


    // Start is called before the first frame update
    private void Start()
    {
        _state = SpaceshipState.Stationary;
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
        Physics.gravity = new Vector3(0, 0, 0);
        _destination = _rb.position;
    }

    // Update is called once per frame
    private void Update()
    {
        // ClickMove();
        Move();
        Fire();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(CommonTagsUtil.Torpedo))
            RegisterHit();
    }

    /// <summary>
    /// Fires a projectile to the location of the mouse.
    /// The mouse position at the time of the click is the target of the projectile.
    /// </summary>
    private void Fire()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var mp = hit.point;
                var dir = new Vector3(mp.x - _rb.position.x, 0, mp.z - _rb.position.z);
                // calc quaternion(Rotation)
                Debug.Log("" + dir + " " + Quaternion.Euler(dir) + " " + mp);
                // Instantiate
                var torp = Instantiate(Torpedo, _rb.position, Quaternion.Euler(dir));
                torp.GetComponent<Rigidbody>().velocity = dir * Time.deltaTime * Torpspeed;
            }
        }
    }

    /// <summary>
    /// moves the game object using WASD controls.
    /// </summary>
    private void Move()
    {
        if(Input.GetKey(KeyCode.W))
            _rb.AddForce(_rb.transform.forward * Time.deltaTime*Velocity);
        if(Input.GetKey(KeyCode.S))
            _rb.AddForce(-_rb.transform.forward * Time.deltaTime*Velocity);
        if(Input.GetKey(KeyCode.A))
            _rb.transform.Rotate(new Vector3(0,1,0),-Rotation*Time.deltaTime);
        if(Input.GetKey(KeyCode.D))
            _rb.transform.Rotate(new Vector3(0,1,0),Rotation*Time.deltaTime);
    }

    /// <summary>
    /// Alternative click move script not beeing used atm. can be removed.
    /// </summary>
    private void ClickMove()
    {
       // if (Input.GetButtonDown("Fire1"))
        // {
        //     var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         _state = SpaceshipState.Moving;
        //         _destination = hit.point;
        //     }
        // }
    }

    private void RegisterHit()
    {
    }

    private void repair(bool hull, bool shield, bool energy, int droids)
    {
    }
}
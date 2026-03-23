using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionAsset InputActions;

    //private Animator m_animator;
    private InputAction m_moveAction;

    private Vector2 m_moveAmt;
    private Rigidbody m_rigidbody;

    [SerializeField] private float RunSpeed;

    private void OnEnable()
    {
        InputActions.FindActionMap("PlayerInput").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("PlayerInput").Disable();
    }

    private void Awake()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");

        //m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Running();
    }

    private void Running()
    {
        //m_animator.SetFloat("Speed", m_moveAmt.y);

        Vector3 moveDir = Vector3.forward * m_moveAmt.y + Vector3.right * m_moveAmt.x;
        moveDir.Normalize();
        //m_animator.SetFloat("Speed",moveDir.sqrMagnitude);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            m_rigidbody.MoveRotation(targetRotation);   
        }

        m_rigidbody.MovePosition(m_rigidbody.position + moveDir * RunSpeed * Time.deltaTime);
    }

    public void FreezePlayer()
    {
        enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void UnFreezePlayer()
    {
        enabled = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    
}

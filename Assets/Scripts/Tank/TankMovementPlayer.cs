using UnityEngine;

public class TankMovementPlayer : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    private string m_ForwardMovementAxisName;
	private string m_ReverseMovementAxisName;
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;
	private bool m_TankIsMoving;
    private float m_OriginalPitch;         


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
		m_ForwardMovementAxisName = "RightTrigger";
		m_ReverseMovementAxisName = "LeftTrigger";
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }


	// Store the player's input and make sure the audio for the engine is playing.
    private void Update()
    {
		m_MovementInputValue = Input.GetAxis (m_ForwardMovementAxisName);
		if (m_MovementInputValue < 0.1f)
			m_MovementInputValue = Input.GetAxis (m_ReverseMovementAxisName) * -1.0f;

		m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

		EngineAudio ();
    }


	// Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
    private void EngineAudio()
    {
		m_TankIsMoving = Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f;

		if (m_TankIsMoving) {
			VerifyEngineAudio (m_EngineIdling);
		} else {
			VerifyEngineAudio (m_EngineDriving);
		}
    }


	private void VerifyEngineAudio(AudioClip audio)
	{
		if (m_MovementAudio.clip != audio) {
			m_MovementAudio.clip = audio;
			m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
			m_MovementAudio.Play ();
		}
	}


    private void FixedUpdate()
    {
		Move ();
		Turn ();
    }


    private void Move()
    {
		Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
		m_Rigidbody.MovePosition (m_Rigidbody.position + movement);
    }


    private void Turn()
    {
		float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
		Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);
		m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }
}
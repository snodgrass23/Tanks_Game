using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;           
    public CameraControl m_CameraControl;   
    public Text m_MessageText;              
    public GameObject m_TankPrefab;
	public GameObject m_PlayerTankPrefab;
    public AITankManager[] m_Tanks;
	[HideInInspector] public PlayerTankManager m_PlayerTank;

    private int m_RoundNumber = 0;              
    private WaitForSeconds m_StartWait;     
	private WaitForSeconds m_EndWait;

    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllTanks()
    {
		for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        StartCoroutine(GameLoop());
    }


    private IEnumerator RoundStarting()
    {
		ResetAllTanks ();

		m_RoundNumber++;
		m_MessageText.text = "ROUND " + m_RoundNumber;

		yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
		m_MessageText.text = string.Empty;

		while (!AllTanksDestroyed ()) {
			yield return null;
		}
    }


    private IEnumerator RoundEnding()
    {
		string message = "You Win!";
		m_MessageText.text = message;

		yield return m_EndWait;
    }


    private bool AllTanksDestroyed()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft == 0;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }
}
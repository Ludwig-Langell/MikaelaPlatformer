using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChecker : MonoBehaviour
{
    [SerializeField] private GameObject panel, finishedText, unfinishedText;
    [SerializeField] private AudioClip finishedSoundEffect;
    [SerializeField] private AudioClip unfinishedSoundEffect;
    [SerializeField] private int levelIndex;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float zoomedCameraZ = -15f;
    [SerializeField] private float cameraTweenDuration = 0.35f;
    private AudioSource audioSource;
    private Animator anim;
    private CameraFollow cameraFollow;
    private float originalCameraZ;
    private bool hasCachedCameraPosition;
    private readonly HashSet<Collider2D> playerCollidersInTrigger = new HashSet<Collider2D>();

    private void Awake()
    {
        CacheCamera();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerCollider(other) && playerCollidersInTrigger.Add(other))
        {
            if (playerCollidersInTrigger.Count > 1)
            {
                return;
            }

            CacheCamera();
            MoveCameraToZ(zoomedCameraZ);

            Rigidbody2D playerBody = other.attachedRigidbody;
            PlayerQuest playerQuest = other.GetComponent<PlayerQuest>();
            if (playerQuest == null && playerBody != null)
            {
                playerQuest = playerBody.GetComponent<PlayerQuest>();
            }

            if (playerQuest == null)
            {
                Debug.LogWarning("QuestChecker could not find PlayerQuest on the player.", this);
                return;
            }

            if (playerQuest.GetOrbs() >= playerQuest.GetorbsToCollect())
            {
                panel.SetActive(true);
                finishedText.SetActive(true);
                anim.SetTrigger("Finished");    //Finished representerar Flag från videoserien
                audioSource.PlayOneShot(finishedSoundEffect);
                
                Invoke(nameof(LoadNextLevel), 3.5f);
                //3.5 sekunder innan nästa level laddas in
            }
            else
            {
                panel.SetActive(true);
                unfinishedText.SetActive(true);
                audioSource.PlayOneShot(unfinishedSoundEffect);
            }
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelIndex);
        //NEW LEVEL
    }
    private void OnTriggerExit2D (Collider2D other)
    {
        if (!playerCollidersInTrigger.Remove(other) || playerCollidersInTrigger.Count > 0)
        {
            return;
        }

        if (targetCamera != null && hasCachedCameraPosition)
        {
            MoveCameraToZ(originalCameraZ);
        }

        panel.SetActive(false);
        finishedText.SetActive(false);
        unfinishedText.SetActive(false);
    }

    private bool IsPlayerCollider(Collider2D other)
    {
        Rigidbody2D otherBody = other.attachedRigidbody;
        return other.CompareTag("Player") ||
               (otherBody != null && otherBody.CompareTag("Player"));
    }

    private void CacheCamera()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera == null)
        {
            targetCamera = FindAnyObjectByType<Camera>();
        }

        if (targetCamera != null && !hasCachedCameraPosition)
        {
            originalCameraZ = targetCamera.transform.position.z;
            hasCachedCameraPosition = true;
        }

        if (targetCamera != null && cameraFollow == null)
        {
            cameraFollow = targetCamera.GetComponent<CameraFollow>();
        }
    }

    private void MoveCameraToZ(float targetZ)
    {
        if (cameraFollow != null && cameraFollow.isActiveAndEnabled)
        {
            cameraFollow.TweenToZ(targetZ, cameraTweenDuration);
            return;
        }

        if (targetCamera != null)
        {
            Vector3 position = targetCamera.transform.position;
            position.z = targetZ;
            targetCamera.transform.position = position;
        }
    }
}

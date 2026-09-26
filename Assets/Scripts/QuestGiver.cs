using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private GameObject textPopup;
    [SerializeField] private AudioClip questgiverSoundEffect;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float zoomedCameraZ = -15f;
    [SerializeField] private float cameraTweenDuration = 0.35f;
    private AudioSource audioSource;
    private float originalCameraZ;
    private bool hasCachedCameraPosition;
    private CameraFollow cameraFollow;
    private readonly HashSet<Collider2D> playerCollidersInTrigger = new HashSet<Collider2D>();

    private void Awake()
    {
        CacheCamera();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerCollider(other) && playerCollidersInTrigger.Add(other))
        {
            // Only zoom in on the first player collider entering.
            if (playerCollidersInTrigger.Count > 1)
            {
                return;
            }

            CacheCamera();

            if (targetCamera == null)
            {
                Debug.LogWarning("QuestGiver could not find a camera. Assign one in the Inspector.", this);
            }
            else
            {
                MoveCameraToZ(zoomedCameraZ);
            }

            SetTextPopupActive(true);

            if (audioSource != null && questgiverSoundEffect != null)
            {
                audioSource.PlayOneShot(questgiverSoundEffect);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerCollidersInTrigger.Remove(other) && playerCollidersInTrigger.Count == 0)
        {
            if (targetCamera != null && hasCachedCameraPosition)
            {
                MoveCameraToZ(originalCameraZ);
            }

            SetTextPopupActive(false);
        }
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

        SetCameraZ(targetZ);
    }

    private void SetTextPopupActive(bool active)
    {
        if (textPopup == null)
        {
            return;
        }

        if (textPopup == gameObject || transform.IsChildOf(textPopup.transform))
        {
            Debug.LogWarning("QuestGiver's textPopup reference must not point to itself or one of its parents.", this);
            return;
        }

        textPopup.SetActive(active);
    }

    private void SetCameraZ(float z)
    {
        Vector3 position = targetCamera.transform.position;
        position.z = z;
        targetCamera.transform.position = position;
    }
}

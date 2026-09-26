using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -20);
    [SerializeField] private float smoothing;
    private float zTweenStart;
    private float zTweenTarget;
    private float zTweenDuration;
    private float zTweenElapsed;
    private bool isTweeningZ;

    public void TweenToZ(float targetZ, float duration)
    {
        zTweenStart = transform.position.z;
        zTweenTarget = targetZ;
        zTweenDuration = duration;
        zTweenElapsed = 0f;
        isTweeningZ = true;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPosition = target.position + offset;
        targetPosition.z = GetCurrentZ();

        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        transform.position = newPosition;
    }

    private float GetCurrentZ()
    {
        if (!isTweeningZ)
        {
            return transform.position.z;
        }

        if (zTweenDuration <= 0f)
        {
            isTweeningZ = false;
            return zTweenTarget;
        }

        zTweenElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(zTweenElapsed / zTweenDuration);
        t = t * t * (3f - 2f * t);

        if (t >= 1f)
        {
            isTweeningZ = false;
        }

        return Mathf.Lerp(zTweenStart, zTweenTarget, t);
    }

}
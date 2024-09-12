using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    public enum Orientation { Left, Right, Top, Bottom };

    [SerializeField] Sprite Icon;
    [SerializeField] Orientation orientation;

    [Header("Animation")]
    [SerializeField] AnimationClip clip;
    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer iconRenderer = GetComponent<SpriteRenderer>();
        iconRenderer.sprite = Icon;

        SpriteRenderer arrowRender = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        Vector3 rotation = arrowRender.transform.localRotation.eulerAngles;

        if(orientation == Orientation.Right) { rotation.z = 0; }
        if (orientation == Orientation.Top) { rotation.z = 90; }
        if (orientation == Orientation.Bottom) { rotation.z = -90; }

        arrowRender.transform.localRotation = Quaternion.Euler(rotation);
    }

    public void Play()
    {
        if (animator != null && clip != null)
        {
            Debug.Log("Play parkour slide " + orientation);
            animator.Play(clip.name);
        }
    }

    public Orientation GetOrientation() { return orientation; }
}

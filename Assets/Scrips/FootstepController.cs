using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FootstepController : MonoBehaviour
{
    public float stepInterval = 0.5f; // tempo entre os passos
    private float stepTimer = 0f;
    private CharacterController controller;

    public EventReference footstepEvent; // arraste o evento "Footsteps" aqui

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        RaycastHit hit;
        int surfaceType = 0; // default

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            string tag = hit.collider.tag;
            switch (tag)
            {
                case "Wood":
                    surfaceType = 0;
                    break;
                case "Gravel":
                    surfaceType = 1;
                    break;
                case "Tatami":
                    surfaceType = 2;
                    break;
                case "Stone":
                    surfaceType = 3;
                    break;
            }
        }

        EventInstance footstep = RuntimeManager.CreateInstance(footstepEvent);
        footstep.setParameterByName("SurfaceType", surfaceType);
        footstep.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        footstep.start();
        footstep.release();
    }
}
using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera normalCamera;
    public CinemachineCamera distantCamera;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            normalCamera.Priority = 5;
            distantCamera.Priority = 10;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            normalCamera.Priority = 10;
            distantCamera.Priority = 5;
        }
    }
}

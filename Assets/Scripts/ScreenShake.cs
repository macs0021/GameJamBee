using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class ScreenShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    [SerializeField] private NoiseSettings shakeNoiseProfile;
    private NoiseSettings previousNoiseProfile;
    private float previousAmplitudeGain;

    private float shakeTimer;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        shakeTimer = 0.0f;
    }

    public void ShakeCamera(float intensity, float time)
    {
        previousNoiseProfile = perlinNoise.m_NoiseProfile;
        previousAmplitudeGain = perlinNoise.m_AmplitudeGain;

        perlinNoise.m_NoiseProfile = shakeNoiseProfile;
        perlinNoise.m_AmplitudeGain = intensity;

        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0.0f)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0.0f)
            {
                perlinNoise.m_NoiseProfile = previousNoiseProfile;
                perlinNoise.m_AmplitudeGain = previousAmplitudeGain;
            }
        }
    }
}

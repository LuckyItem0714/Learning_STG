using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private bool isShaking = false; //���݃V�F�C�N�����̖ڈ�

    void Awake()
    {
        instance = this;
    }

    //���̃��\�b�h���O������Ăяo���悤�ɕύX
    public void TriggerShake(float duration, float magnitude)
    {
        //�����V�F�C�N���Ȃ���΁A�V�����V�F�C�N���J�n����
        if (!isShaking)
        {
            StartCoroutine(Shake(duration, magnitude));
        }
    }

    private IEnumerator Shake (float duration, float magnitude)
    {
        isShaking = true; //�V�F�C�N�J�n�̊��𗧂Ă�
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null; //���̃t���[���܂ő҂�
        }

        transform.localPosition = originalPos; //�J���������Ƃ̈ʒu�ɖ߂�
        isShaking=false; //�V�F�C�N�I���̊������낷
    }
}

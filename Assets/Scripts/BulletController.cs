using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 moveDirection = Vector3.up; //�f�t�H���g�͏����

    // Update is called once per frame
    void Update()
    {
        //�ݒ肳�ꂽ�����ɁA�܂������i�ݑ�����
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    //�O������e�̐i�s������ݒ肷�邽�߂̃��\�b�h
    public void SetDirection(Vector3 newDirection)
    {
        moveDirection = newDirection;
    }
}

using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1; //���̒e���^����_���[�W��
    public Transform target; //�ǐՂ���ڕW

    private Vector3 moveDirection = Vector3.up; //�f�t�H���g�͏����

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            //�^�[�Q�b�g�̕����Ɍ������x�N�g�����v�Z
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            //���݂̐i�s�������A�^�[�Q�b�g�̕����ŏ㏑���X�V����
            moveDirection = directionToTarget;
        }

        //��ɁA�L�����ꂽmoveDirection�̕����ֈړ�����
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    //�O������e�̐i�s������ݒ肷�邽�߂̃��\�b�h
    public void SetDirection(Vector3 newDirection)
    {
        moveDirection = newDirection;
    }
}

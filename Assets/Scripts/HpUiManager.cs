using UnityEngine;
using UnityEngine.UI; // Image�Ȃǂ�UI�R���|�[�l���g���������߂ɕK�v
using System.Collections.Generic; // List���g�����߂ɕK�v

public class HpUiManager : MonoBehaviour
{
    public List<Image> heartIcons; //�n�[�g��Image�R���|�[�l���g���i�[���郊�X�g
    
    public void UpdateHp(int currentHp)
    {
        //�S�Ẵn�[�g�A�C�R�������[�v�ŏ�������
        for (int i = 0; i < heartIcons.Count; i++)
        {
            //�����A���݂̃��[�v�̃C���f�b�N�X(i)���A�v���C���[�̌���HP��菬�������
            if (i < currentHp)
            {
                //���̃n�[�g��\������
                heartIcons[i].enabled = true;
            }
            else
            {
                //���̃n�[�g���\���ɂ���
                heartIcons[i].enabled = false;
            }
        }
    }
}

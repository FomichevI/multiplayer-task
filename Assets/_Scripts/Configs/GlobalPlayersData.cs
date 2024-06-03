using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class GlobalPlayersData : NetworkBehaviour
{
    public void SetPlayerName(int playerID,string name, Action onError = null)
    {
        if (name.Length * sizeof(Char) > 32)
        {
            Debug.LogWarning("������� ������� ���");
            onError?.Invoke();
        }
        else
        {
            //if (IsStringCorrect(name))            
            //    _playerName.Value = name;            
            //else
            //    onError?.Invoke();
        }
    }

    // ����� ������� � ��������� �����, ������� ����� �������������� �������� ��� � string � ��������� ������ �������� �� string. ���� �������� ������ ����������
    // � ������� ���������� ������� ���
    private bool IsStringCorrect(string value)
    {
        bool isCorrect = true;
        if (value == null || value == "")
        {
            Debug.LogWarning("������ ���");
            return false;
        }
        else
        {
            foreach (char c in value)
            {
                if (Char.IsWhiteSpace(c) || Char.IsSymbol(c) || Char.IsSeparator(c) || Char.IsPunctuation(c))
                {
                    isCorrect = false;
                    break;
                }
            }
        }

        if (!isCorrect)
            Debug.LogWarning("������������ ���");
        return isCorrect;
    }
}

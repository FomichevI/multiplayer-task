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
            Debug.LogWarning("Слишком длинное имя");
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

    // Лучше вынести в отдельный класс, который будет конвертировать значения цен в string и проводить прочие операции со string. Либо возможна другая реализация
    // В текущей реализации оставим так
    private bool IsStringCorrect(string value)
    {
        bool isCorrect = true;
        if (value == null || value == "")
        {
            Debug.LogWarning("Пустое имя");
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
            Debug.LogWarning("Некорректное имя");
        return isCorrect;
    }
}

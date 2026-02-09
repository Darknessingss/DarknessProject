using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessageDatabase", menuName = "Dialogue/MessageDatabase")]
public class MessageDatabase : ScriptableObject
{
    [System.Serializable]
    public class MessageData
    {
        public int characterId;
        public string characterName;
        public string text;
        public Color backgroundColor = Color.white;
    }

    [SerializeField] private List<MessageData> messages = new List<MessageData>();

    public int MessageCount => messages.Count;

    public MessageData GetMessage(int index)
    {
        if (index >= 0 && index < messages.Count)
            return messages[index];
        return null;
    }

    public void AddDefaultMessages()
    {
        messages.Clear();

        // Персонаж 0
        AddMessage(0, "Алекс", "Привет! Как дела? Уже выполнил задание по программированию?", new Color(0.8f, 0.9f, 1f));
        AddMessage(1, "Мария", "Привет! Да, только что закончила. Было непросто, но интересно.", new Color(1f, 0.95f, 0.8f));
        AddMessage(0, "Алекс", "Отлично! Можешь помочь с Unity? У меня проблемы с анимацией персонажа.", new Color(0.8f, 0.9f, 1f));
        AddMessage(1, "Мария", "Конечно! В чем именно проблема? Используешь Animator Controller?", new Color(1f, 0.95f, 0.8f));
        AddMessage(0, "Алекс", "Да, состояния переключаются, но переходы работают некорректно.", new Color(0.8f, 0.9f, 1f));
        AddMessage(1, "Мария", "Проверь условия переходов. Иногда помогает сбросить параметры.", new Color(1f, 0.95f, 0.8f));
        AddMessage(0, "Алекс", "Спасибо за совет! Попробую. Как твой проект по геймдизайну?", new Color(0.8f, 0.9f, 1f));
        AddMessage(1, "Мария", "Почти готов! Осталось добавить звуковые эффекты и настроить баланс.", new Color(1f, 0.95f, 0.8f));
        AddMessage(0, "Алекс", "Звучит круто! Покажешь, когда закончишь?", new Color(0.8f, 0.9f, 1f));
        AddMessage(1, "Мария", "Обязательно! Завтра должна быть готова демо-версия.", new Color(1f, 0.95f, 0.8f));
    }

    private void AddMessage(int characterId, string name, string text, Color color)
    {
        messages.Add(new MessageData
        {
            characterId = characterId,
            characterName = name,
            text = text,
            backgroundColor = color
        });
    }
}
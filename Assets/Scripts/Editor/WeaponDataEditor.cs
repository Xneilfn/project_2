using System.Collections;

using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;


[CustomEditor(typeof(WeaponData))] // Привязываем этот редактор к компоненту WeaponData


public class WeaponDataEditor : Editor
{
    WeaponData weaponData;          // Целевой объект, который редактируем
    string[] weaponSubtypes;        // Список всех найденных подтипов Weapon
    int selectedWeaponSubtype;      // Индекс выбранного подтипа в выпадающем списке

    // Вызывается при активации редактора
    private void OnEnable()
    {
        weaponData = (WeaponData)target; // Получаем целевой объект

        // Находим все классы, наследующиеся от Weapon
        System.Type baseType = typeof(Weapon);
        List<System.Type> subTypes = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => baseType.IsAssignableFrom(p) && p != baseType)
            .ToList();

        // Преобразуем типы в строки с именами и добавляем опцию "None"
        List<string> subTypesString = subTypes.Select(t => t.Name).ToList();
        subTypesString.Insert(0, "None");
        weaponSubtypes = subTypesString.ToArray();

        // Восстанавливаем ранее выбранный подтип (если был)
        selectedWeaponSubtype = Math.Max(0, Array.IndexOf(weaponSubtypes, weaponData.behaviour));
    }

    // Главный метод отрисовки интерфейса инспектора
    public override void OnInspectorGUI()
    {
        // Создаём выпадающий список для выбора поведения оружия
        selectedWeaponSubtype = EditorGUILayout.Popup("Behaviour", Math.Max(0, selectedWeaponSubtype), weaponSubtypes);

        // Если выбран какой-то подтип (не "None")
        if (selectedWeaponSubtype > 0)
        {
            // Сохраняем выбранное имя в данные оружия
            weaponData.behaviour = weaponSubtypes[selectedWeaponSubtype].ToString();
            EditorUtility.SetDirty(weaponData); // Помечаем объект как изменённый для сохранения
            DrawDefaultInspector(); // Показываем остальные поля по умолчанию
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using TMPro;
using System.Text;

public class UIStatsDisplay : MonoBehaviour
{
    public PlayerStats player;
    public CharacterData character;
    public bool displayCurrrentHealth = false;
    public bool updateInEditor = false;
    TextMeshProUGUI statNames, statValues;

    private void OnEnable()
    {
        UpdateStatFields();
    }

    public CharacterData.Stats GetDisplayedStats()
    {
        if (player) return player.Stats;
        else if (character) return character.stats;
        return new CharacterData.Stats();
    }
    public static string PrettifyNames(StringBuilder input)
    {
        if (input.Length <= 0) return string.Empty;
        StringBuilder result = new StringBuilder();
        char last = '\0';
        for(int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (last == '\0' || char.IsWhiteSpace(last))
            {
                c = char.ToUpper(c);
            } else if (char.IsUpper(c))
            {
                result.Append(' ');
            }
            result.Append(c);
            last = c;
        }
        return result.ToString();
    }
    public void UpdateStatFields()
    {
        if (!player && !character) return;

        if (!statNames) statNames = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (!statValues) statValues = transform.GetChild(1).GetComponent<TextMeshProUGUI>();


        StringBuilder names = new StringBuilder();
        StringBuilder values = new StringBuilder();
        FieldInfo[] fields = typeof(CharacterData.Stats).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields) 
        {
            names.AppendLine(field.Name);

            object val = field.GetValue(GetDisplayedStats());
            float fval = val is int ? (int)val : (float)val;

            PropertyAttribute attribute = (PropertyAttribute)PropertyAttribute.GetCustomAttribute(field, typeof(PropertyAttribute));
            if (attribute != null && field.FieldType == typeof(float))
            {
                float percentage = Mathf.Round(fval * 100 - 100);

                if (Mathf.Approximately(percentage, 0))
                {
                    values.Append('-').Append('\n');
                }

                else
                {
                    if (percentage > 0)
                        values.Append('+');
                    //else values.Append("-");
                    values.Append(percentage).Append('%').Append('\n');
                }
            }
            else
            {
                values.Append(fval).Append('\n');
            }

            statNames.text = PrettifyNames(names);
            statValues.text = values.ToString();

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData {

    [SerializeField] private string m_id;
    [SerializeField] private string m_name;
    [SerializeField] private string m_birth;
    [SerializeField] private string m_sex;

    public SaveData(string id, string name, string birth, string sex)
    {
        m_id = id;
        m_name = name;
        m_birth = birth;
        m_sex = sex;
    }

    public string GetGender()
    {
        return m_sex;
    }
}

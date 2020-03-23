using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class AchievementSave
{
    [XmlElement]
    public string Key { get; set; }

    [XmlElement]
    public float Value { get; set; }

    [XmlElement]
    public bool Unlocked { get; set; }
}

[System.Serializable]
public class Achievement
{
    [SerializeField] private string _key;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _valueToUnlock;

    public string Key { get => _key; set => _key = value; }
    public string Name { get => _name; set => _name = value; }
    public string Description { get => _description; set => _description = value; }
    public float ValueToUnlock { get => _valueToUnlock; set => _valueToUnlock = value; }
    public Sprite Sprite { get => _sprite; set => _sprite = value; }
    public float Value { get; set; }
    public bool Unlocked { get; set; }
}

public class Achievements : MonoBehaviour
{
    public static Achievements achievements => FindObjectOfType<Achievements>();

    [SerializeField] private Achievement[] _achievements;

    public Achievement[] AchievementList { get => _achievements; }

    private Dictionary<string, Achievement> _dict;
    // Start is called before the first frame update
    void Awake()
    {
        // TODO: load values/unlockes
        var _restored = Load();

        _dict = _achievements.ToDictionary(a => a.Key);
        foreach (var a in _dict.Values)
        {
            if (_restored.TryGetValue(a.Key, out var save))
            {
                if (save.Unlocked)
                {
                    a.Unlocked = save.Unlocked;
                    a.Value = a.ValueToUnlock;
                }
                else
                {
                    a.Value = save.Value;
                    TryUnlock(a);
                }
            }
        }

    }

    public void Add(string key, float value)
    {
        if (_dict.TryGetValue(key, out var achievement))
        {
            achievement.Value += value;
            TryUnlock(achievement);
        }
        else
        {
            Debug.LogWarning($"No achievement '{key}'");
        }
    }

    public void Set(string key, float value)
    {
        if (_dict.TryGetValue(key, out var achievement))
        {
            achievement.Value = value;
            TryUnlock(achievement);

        }
        else
        {
            Debug.LogWarning($"No achievement '{key}'");
        }
    }

    public void TryUnlock(Achievement achievement)
    {
        if (achievement.Value >= achievement.ValueToUnlock)
        {
            achievement.Unlocked = true;
        }
    }


    private void Save()
    {
        var _toSaveDict = new List<AchievementSave>(
            _dict.Select(a => new AchievementSave()
            {
                Key = a.Key,
                Value = a.Value.Value,
                Unlocked = a.Value.Unlocked
            })
        );
        var xmlSerializer = new XmlSerializer(_toSaveDict.GetType());

        using (var textWriter = new StringWriter())
        {
            xmlSerializer.Serialize(textWriter, _toSaveDict);

            PlayerPrefs.SetString("achievements", textWriter.ToString());
        }

        PlayerPrefs.Save();

    }

    private Dictionary<string, AchievementSave> Load()
    {
        if (!PlayerPrefs.HasKey("achievements"))
        {
            return new Dictionary<string, AchievementSave>();
        }

        try
        {
            var s = PlayerPrefs.GetString("achievements");
            var serializer = new XmlSerializer(typeof(List<AchievementSave>));
            using (var stringReader = new StringReader(s))
            {
                return ((List<AchievementSave>)serializer.Deserialize(stringReader))
                    .ToDictionary(a => a.Key, a => a);
            }
        }
        catch
        {
            return new Dictionary<string, AchievementSave>();
        }
    }

    private void OnDestroy()
    {
        Save();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class Score
{
    public string Name { get; set; }
    public int Points { get; set; }
}

public class Scores : MonoBehaviour
{
    public List<Score> ScoreList { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        if(PlayerPrefs.HasKey("scores"))
        {
            var s = PlayerPrefs.GetString("scores");
            var serializer = new XmlSerializer(typeof(List<Score>));
            using (var stringReader = new StringReader(s))
            {
                ScoreList = (List<Score>)serializer.Deserialize(stringReader);
            }
        }
        else
        {
            ScoreList = new List<Score>()
            {
                new Score() {Name = "Mad Man", Points = 100000},
                new Score() {Name = "Veteran", Points = 50000},
                new Score() {Name = "Pro", Points = 25000},
                new Score() {Name = "Star", Points = 12500},
                new Score() {Name = "Rising Star", Points = 6250}
            };

            Save();
        }
    }

    public bool IsHiScore(int score)
    {
        return ScoreList.Count < 6 || score > ScoreList.Min(s => s.Points);
    }

    public void Add(string name, int score)
    {
        ScoreList.Add(new Score() { Name = name, Points = score });
        ScoreList = ScoreList.OrderByDescending(s => s.Points).Take(10).ToList();
        Save();
    }

    private void Save()
    {
        var xmlSerializer = new XmlSerializer(ScoreList.GetType());

        using (var textWriter = new StringWriter())
        {
            xmlSerializer.Serialize(textWriter, ScoreList);

            PlayerPrefs.SetString("scores", textWriter.ToString());
        }

        PlayerPrefs.Save();
    }
}

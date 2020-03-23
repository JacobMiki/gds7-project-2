using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private GameObject _scoresContainer;
    [SerializeField] private GameObject _panelPrefab;

    [SerializeField] private Scores _scores;


    // Start is called before the first frame update
    void Start()
    {
        var children = new List<GameObject>();
        foreach (Transform child in _scoresContainer.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        var scores = _scores.ScoreList.Take(6);

        var pos = 0;
        foreach (var score in scores)
        {
            var ap = Instantiate(_panelPrefab, _scoresContainer.transform);
            var panel = ap.GetComponent<ScorePanel>();

            pos++;
            panel.Position.text = pos.ToString();
            panel.Name.text = score.Name;
            panel.Points.text = score.Points.ToString();

        }
    }
}

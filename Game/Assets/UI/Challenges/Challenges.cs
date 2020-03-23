using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenges : MonoBehaviour
{
    [SerializeField] private GameObject _grid;
    [SerializeField] private GameObject _panelPrefab;
    [SerializeField] private Sprite _lockedSprite;

    // Start is called before the first frame update
    void Start()
    {
        var children = new List<GameObject>();
        foreach (Transform child in _grid.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));


        var achievements = Achievements.achievements.AchievementList;

        Achievements.achievements.Set("TRY_AGAIN", 0);

        foreach(var achievement in achievements)
        {
            var ap = Instantiate(_panelPrefab, _grid.transform);
            var panel = ap.GetComponent<Panel>();
            panel.Description.text = achievement.Description;
            panel.Image.sprite = achievement.Unlocked ? achievement.Sprite : _lockedSprite;
        }
    }

}

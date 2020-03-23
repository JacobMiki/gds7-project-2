using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private Text _position;
    [SerializeField] private Text _name;
    [SerializeField] private Text _points;

    public Text Position { get => _position; }
    public Text Name { get => _name;  }
    public Text Points { get => _points;  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _description;

    public Image Image { get => _image; }
    public Text Description { get => _description; }
}

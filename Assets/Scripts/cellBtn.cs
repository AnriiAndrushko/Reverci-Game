using System;
using UnityEngine;
using UnityEngine.UI;

public class cellBtn : MonoBehaviour
{
    private deskGen mainBoard;
    private Button myBtn;
    private
    // Start is called before the first frame update
    void Start()
    {
        myBtn = GetComponent<Button>();
        myBtn.onClick.AddListener(GiveInfo);
        mainBoard = FindObjectOfType<deskGen>();
    }

    void GiveInfo()
    {
        mainBoard.DoTurn(Int32.Parse(gameObject.name.Split(' ')[1]));
    }
}

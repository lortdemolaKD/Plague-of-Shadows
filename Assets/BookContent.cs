using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookContent : MonoBehaviour
{
    [TextArea(10, 20)]
    [SerializeField] private string content;
    [Space]
    [SerializeField] private TMP_Text leftSide;
    [SerializeField] private TMP_Text rightSide;
    [Space]
    [SerializeField] private TMP_Text rightPad;
    [SerializeField] private TMP_Text leftPad;

    private void OnValidate()
    {
        UpdatePagin();

        if (leftSide.text == content)
            return;

        SetupContent();
    }
    private void Awake()
    {
        SetupContent();
        UpdatePagin();
    }

    private void SetupContent()
    {
        leftSide.text = content;
        rightSide.text = content;
    }
    private void UpdatePagin()
    {
        leftPad.text = leftSide.pageToDisplay.ToString();
        rightPad.text = rightSide.pageToDisplay.ToString();
    }
     
    public void prevPage()
    {
        if(leftSide.pageToDisplay < 1)
        {
            leftSide.pageToDisplay = 1;
            return;
        }
        if(leftSide.pageToDisplay -2 > 1) 
            leftSide.pageToDisplay -= 2;
        else
            leftSide.pageToDisplay = 1;
        rightSide.pageToDisplay = leftSide.pageToDisplay+1;

        UpdatePagin() ;
    }
    public void nextPage()
    {
        if (rightSide.pageToDisplay >= rightSide.textInfo.pageCount)
            return;
        if (leftSide.pageToDisplay >= leftSide.textInfo.pageCount-1)
        {
            leftSide.pageToDisplay = leftSide.textInfo.pageCount -1;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }
        else
        {
            leftSide.pageToDisplay += 2;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }


            UpdatePagin();
    }
}

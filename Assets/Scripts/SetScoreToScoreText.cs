using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetScoreToScoreText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI clearedText;
    // Start is called before the first frame update
    void Start()
    {
        if ( ScoreManager.Instance != null)
        {
            if ( scoreText != null )
            {
                scoreText.text = ScoreManager.Instance.Score.ToString();
            }
            if (clearedText != null)
            {
                clearedText.text = ScoreManager.Instance.CleardStageCount.ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

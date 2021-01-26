using UnityEngine;

public class TasksReport : MonoBehaviour
{
    public TasksReport()
    {
        this.bubbleTaskErrors = 0;
    }

    public float shootingTaskTimer { get; set; }
    public int shootingTaskErrors { get; set; }
    public bool isShootingTaskActive { get; set; }
   
    public float bubbleTaskTimer { get; set; }
    public int bubbleTaskErrors { get; set; }
    public bool isBubbleTaskActive { get; set; }

    public float translateSentenceTaskTimer { get; set; }
    public int translateSentenceTaskErrors { get; set; }
    public bool isTranslateSentenceTaskActive { get; set; }

    public float verbsTaskTimer { get; set; }
    public int verbsTaskErrors { get; set; }
    public bool isVerbsTaskActive { get; set; }



}

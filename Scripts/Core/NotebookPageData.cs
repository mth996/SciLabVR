using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExperimentEntry
{
    public string label;
    public string result;
    public bool passed;
}

[System.Serializable]
public class ExperimentSection
{
    public string experimentTitle;
    public int stars;
    public List<ExperimentEntry> entries = new List<ExperimentEntry>();
}

[System.Serializable]
public class NotebookPageData
{
    public int chapterNumber;
    public string chapterTitle;
    public bool isComplete;
    public List<ExperimentSection> sections = new List<ExperimentSection>();
}
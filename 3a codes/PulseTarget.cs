using UnityEngine;

public enum SubjectType
{
    Male,
    Female,
    Student,
    Teacher,
    LabAssistant,
    Resting,
    Walking,
    Running
}

public class PulseTarget : MonoBehaviour
{
    [Header("Settings")]
    public SubjectType subjectType;
    public int bpm;
    public string experimentType;

    [Header("UI Spawn Point")]
    public Transform uiSpawnPoint;

    [Header("Wrist Detector")]
    public WristDetector wristDetector;

    public string GetSubjectName()
    {
        switch (subjectType)
        {
            case SubjectType.Male:
                return Chapter1Texts.Male();

            case SubjectType.Female:
                return Chapter1Texts.Female();

            case SubjectType.Student:
                return Chapter1Texts.Student();

            case SubjectType.Teacher:
                return Chapter1Texts.Teacher();

            case SubjectType.LabAssistant:
                return Chapter1Texts.LabAssistant();

            case SubjectType.Resting:
                return Chapter1Texts.Resting();

            case SubjectType.Walking:
                return Chapter1Texts.Walking();

            case SubjectType.Running:
                return Chapter1Texts.Running();

            default:
                return "";
        }
    }

    public string GetResultLabel()
    {
        return GetSubjectName();
    }

    public void MarkMeasured()
    {
        if (wristDetector != null)
            wristDetector.MarkMeasured();
    }

    public void ResetForRetry()
    {
        if (wristDetector != null)
            wristDetector.ResetForRetry();
    }
}
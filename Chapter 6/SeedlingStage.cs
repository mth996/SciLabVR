using UnityEngine;

// Attach to each Seed parent GameObject
// (Seed1, Seed2, Seed3)
// Assign the 7 stage child GameObjects
// in order Day 0 to Day 6
public class SeedlingStage : MonoBehaviour
{
    [Header("Seedling Stages")]
    [Tooltip("Assign Day 0 to Day 6 " +
        "child GameObjects in order")]
    public GameObject[] stages =
        new GameObject[7];

    [Header("Ruler")]
    public VerticalRulerUI rulerUI;

    [Header("Actual Heights Per Day (mm)")]
    [Tooltip("Height of this seedling " +
        "on each day 0-6 in mm.\n" +
        "Adjust to match your mesh heights.")]
    public float[] heightsMm = new float[]
    {
        0f,   // Day 0
        5f,   // Day 1
        15f,  // Day 2
        30f,  // Day 3
        50f,  // Day 4
        65f,  // Day 5
        75f   // Day 6
    };

    private int currentDay = 0;

    private void Start()
    {
        // Hide all stages at start
        for (int i = 0; i < stages.Length; i++)
            SetActive(stages[i], false);

        // Show Day 0
        ShowDay(0);
    }

    // ─── Day Control ──────────────────────────────────────────────────────

    public void ShowDay(int day)
    {
        day = Mathf.Clamp(day, 0, 6);
        currentDay = day;

        // Hide all
        for (int i = 0; i < stages.Length; i++)
            SetActive(stages[i], false);

        // Show current day
        if (stages[day] != null)
            SetActive(stages[day], true);

        // Update ruler to show
        // this day's height
        if (rulerUI != null)
            rulerUI.SetSeedlingHeight(
                GetHeightMm(day));

        Debug.Log(gameObject.name
            + " Day " + day
            + " shown. Height: "
            + GetHeightMm(day) + "mm");
    }

    public float GetHeightMm(int day)
    {
        day = Mathf.Clamp(day, 0, 6);
        if (heightsMm != null
            && day < heightsMm.Length)
            return heightsMm[day];
        return 0f;
    }

    public int CurrentDay => currentDay;

    private void SetActive(
        GameObject obj, bool active)
    {
        if (obj != null)
            obj.SetActive(active);
    }
}
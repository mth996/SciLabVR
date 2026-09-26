using UnityEngine;

public static class Chapter2BTexts
{
    private static bool IsEnglish()
    {
        if (LanguageManager.Instance != null)
            return LanguageManager.Instance
                .IsEnglish();
        return true;
    }

    // ─── Chapter Info ─────────────────────────────────────────────────────

    public static string ChapterTitle()
    {
        return IsEnglish()
            ? "Chapter 2: Support & Growth"
            : "Bab 2: Sokongan & Pertumbuhan";
    }

    public static string ExperimentTitle()
    {
        return IsEnglish()
            ? "Growth Pattern of " +
              "Mung Bean Seedlings"
            : "Corak Pertumbuhan " +
              "Anak Benih Kacang Hijau";
    }

    public static string ExperimentHeader()
    {
        return IsEnglish()
            ? "MUNG BEAN SEEDLING " +
              "GROWTH EXPERIMENT"
            : "EKSPERIMEN PERTUMBUHAN " +
              "ANAK BENIH KACANG HIJAU";
    }

    public static string ExperimentComplete()
    {
        return IsEnglish()
            ? "Experiment Complete"
            : "Eksperimen Selesai";
    }

    // ─── Day Navigation ───────────────────────────────────────────────────

    public static string DayLabel(int day)
    {
        return IsEnglish()
            ? "Day " + day
            : "Hari " + day;
    }

    public static string MeasureInstruction()
    {
        return IsEnglish()
            ? "Read the ruler beside each\n" +
              "seedling and enter the height."
            : "Baca ukuran pada pembaris di\n" +
              "sebelah setiap anak benih dan\n" +
              "Catatkan ketinggian.";
    }

    public static string NextDayButton()
    {
        return IsEnglish()
            ? "NEXT DAY "
            : "HARI SETERUSNYA ";
    }

    public static string PrevDayButton()
    {
        return IsEnglish()
            ? " PREV DAY"
            : " HARI SEBELUMNYA";
    }

    public static string SubmitButton()
    {
        return IsEnglish()
            ? "SUBMIT MEASUREMENT"
            : "HANTAR REKOD";
    }

    // ─── Seedling Labels ──────────────────────────────────────────────────

    public static string Seedling1Label()
    {
        return IsEnglish()
            ? "Sprout 1"
            : "Anak Benih 1";
    }

    public static string Seedling2Label()
    {
        return IsEnglish()
            ? "Sprout 2"
            : "Anak Benih 2";
    }

    public static string Seedling3Label()
    {
        return IsEnglish()
            ? "Sprout 3"
            : "Anak Benih 3";
    }

    public static string MeanLabel()
    {
        return IsEnglish()
            ? "Average Height:"
            : "Purata Ketinggian:";
    }

    // ─── Input Panel ──────────────────────────────────────────────────────

    public static string InputTitle()
    {
        return IsEnglish()
            ? "Enter seedling heights (mm)"
            : "Catatkan ketinggian " +
              "anak benih (mm)";
    }

    public static string MeanCalculated(
        float mean)
    {
        return IsEnglish()
            ? "Mean height: " +
              mean.ToString("F1") + " mm"
            : ": " +
              mean.ToString("F1") + " mm";
    }

    // ─── Results ──────────────────────────────────────────────────────────

    public static string ResultCorrect(
        int day, float mean)
    {
        return IsEnglish()
            ? "Day " + day + " recorded!\n" +
              "Mean: " +
              mean.ToString("F1") + " mm"
            : "Hari " + day +
              " telah direkodkan\n" +
              "Purata: " +
              mean.ToString("F1") + " mm";
    }

    public static string ResultWrong(
        int day)
    {
        return IsEnglish()
            ? "Measurement is off for Day "
              + day + ".\nTry again."
            : "Pengukuran tidak tepat\n" +
              "untuk Hari " + day +
              ".\nCuba lagi.";
    }

    public static string AllDaysComplete()
    {
        return IsEnglish()
            ? "All 7 days recorded!\n" +
              "Check your results."
            : "Semua 7 hari direkodkan!\n" +
              "Semak keputusan anda.";
    }

    // ─── Conclusion ───────────────────────────────────────────────────────

    public static string ConclusionTitle()
    {
        return IsEnglish()
            ? "CONCLUSION"
            : "KESIMPULAN";
    }

    public static string Conclusion()
    {
        return IsEnglish()
            ?
            "The hypothesis is accepted.\n\n" +
            "The growth pattern of bean sprout\n" +
            "follows a sigmoid curve.\n\n" +
            "The Growth Height is slow at first,\n" +
            "then accelerates in the middle days,\n" +
            "after that slows again as the sprouts\n" +
            "reach its maximum height."
            :
            "Hipotesis diterima.\n\n" +
            "Pola pertumbuhan anak benih\n" +
            "kacang hijau mengikuti lengkungan\n" +
            "sigmoid.\n\n" +

            "Pertumbuhan anak benih perlahan\n" +
            "pada awalnya, seterusnya pesat\n" +
            "pada hari pertengahan, kemudian\n" +
            "perlahan semula apabila anak\n" +
            "benih mencapai ketinggian\n" +
            "maksimum.";
    }
    
    // ─── Notebook Button ──────────────────────────────────────────────────

    public static string NotebookButton()
    {
        return IsEnglish()
            ? "📓  NOTEBOOK"
            : "📓  BUKU NOTA";
    }

    public static string BackToMenuButton()
    {
        return IsEnglish()
            ? "BACK TO MENU"
            : "KEMBALI KE MENU";
    }

    public static string ProceedButton()
    {
        return IsEnglish()
            ? "PROCEED"
            : "TERUSKAN";
    }
    
        // ─── 2A Experiment Info ───────────────────────────────────────────────

    public static string ExperimentTitle2A()
    {
        return IsEnglish()
            ? "Strength of Hollow vs Solid Structures"
            : "Kekuatan Struktur Berongga Berbanding Pejal";
    }

    public static string ExperimentHeader2A()
    {
        return IsEnglish()
            ? "HOLLOW VS SOLID CYLINDER STRENGTH " +
              "EXPERIMENT"
            : "EKSPERIMEN KEKUATAN SILINDER BERONGGA " +
              "DAN PEJAL";
    }

    // ─── 2A Status ────────────────────────────────────────────────────────

    public static string PlaceHollowCylinders2A()
    {
        return IsEnglish()
            ? "Place all 4 hollow cylinders on\n" +
              "the highlighted spots."
            : "Letakkan keempat-empat silinder\n" +
              "berongga pada tempat yang\n" +
              "ditanda.";
    }

    public static string PlaceHollowBoard2A()
    {
        return IsEnglish()
            ? "Now place the wooden board\n" +
              "on top."
            : "Sekarang letakkan papan kayu\n" +
              "di atasnya.";
    }

    public static string StackHollowBooks2A()
    {
        return IsEnglish()
            ? "Place books on top, one at a\n" +
              "time, and watch how many it\n" +
              "can hold."
            : "Letakkan buku di atas, satu\n" +
              "demi satu, dan perhatikan\n" +
              "berapa banyak ia dapat\n" +
              "menampung.";
    }

    public static string HollowCollapsed2A(int count)
    {
        return IsEnglish()
            ? "The hollow structure collapsed\n" +
              "after " + count + " books!"
            : "Struktur berongga runtuh\n" +
              "selepas " + count + " buku!";
    }

    public static string PlaceSolidCylinders2A()
    {
        return IsEnglish()
            ? "Place all 4 solid cylinders on\n" +
              "the highlighted spots."
            : "Letakkan keempat-empat silinder\n" +
              "pejal pada tempat yang\n" +
              "ditanda.";
    }

    public static string PlaceSolidBoard2A()
    {
        return IsEnglish()
            ? "Now place the wooden board\n" +
              "on top."
            : "Sekarang letakkan papan kayu\n" +
              "di atasnya.";
    }

    public static string StackSolidBooks2A()
    {
        return IsEnglish()
            ? "Place books on top, one at a\n" +
              "time, and watch how many it\n" +
              "can hold."
            : "Letakkan buku di atas, satu\n" +
              "demi satu, dan perhatikan\n" +
              "berapa banyak ia dapat\n" +
              "menampung.";
    }

    public static string SolidCollapsed2A(int count)
    {
        return IsEnglish()
            ? "The solid structure collapsed\n" +
              "after " + count + " books!"
            : "Struktur pejal runtuh selepas\n" +
              count + " buku!";
    }

    // ─── 2A Labels ────────────────────────────────────────────────────────

    public static string HollowLabel2A()
    {
        return IsEnglish()
            ? "Hollow Cylinders"
            : "Silinder Berongga";
    }

    public static string SolidLabel2A()
    {
        return IsEnglish()
            ? "Solid Cylinders"
            : "Silinder Pejal";
    }

    public static string BookCountLabel2A(int count)
    {
        return IsEnglish()
            ? "Books placed: " + count
            : "Buku diletakkan: " + count;
    }

    // ─── 2A Buttons ───────────────────────────────────────────────────────

    public static string ProceedToSolidButton2A()
    {
        return IsEnglish()
            ? "PROCEED TO SOLID"
            : "TERUSKAN KE PEJAL";
    }

    // ─── 2A Results ───────────────────────────────────────────────────────

    public static string ComparisonHollowStronger2A()
    {
        return IsEnglish()
            ? "Hollow cylinders withstood more\n" +
              "books than solid cylinders"
            : "Silinder berongga menahan lebih\n" +
              "banyak buku berbanding silinder\n" +
              "pejal";
    }

    public static string ComparisonUnexpected2A()
    {
        return IsEnglish()
            ? "Unexpected result \u2014 check\n" +
              "the setup"
            : "Keputusan tidak dijangka \u2014\n" +
              "semak persediaan";
    }

    // ─── 2A Conclusion ────────────────────────────────────────────────────

    public static string ConclusionTitle2A()
    {
        return IsEnglish() ? "CONCLUSION" : "KESIMPULAN";
    }

    public static string Conclusion2A(
        int hollowCount, int solidCount)
    {
        return IsEnglish()
            ?
            "The hypothesis is accepted.\n\n" +
            "The hollow structure withstood " +
            hollowCount + " books, while the\n" +
            "solid structure collapsed after only " +
            solidCount + " books.\n\n" +
            "A hollow cylinder distributes weight\n" +
            "around its outer wall instead of\n" +
            "through a solid core, making it\n" +
            "structurally stronger for its weight\n" +
            "than an equivalent solid cylinder."
            :
            "Hipotesis diterima.\n\n" +
            "Struktur berongga menahan " +
            hollowCount + " buku,\n" +
            "manakala struktur pejal runtuh\n" +
            "selepas hanya " + solidCount +
            " buku.\n\n" +
            "Silinder berongga mengagihkan\n" +
            "beban di sekeliling dinding luarnya\n" +
            "berbanding melalui teras pejal,\n" +
            "menjadikannya lebih kukuh dari segi\n" +
            "struktur berbanding silinder pejal\n" +
            "yang setara.";
    }
}
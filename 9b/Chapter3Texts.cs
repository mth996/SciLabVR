using UnityEngine;

public static class Chapter3Texts
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
            ? "Chapter 3: Industrial Chemistry"
            : "Bab 3: Kimia Industri";
    }

    public static string ExperimentTitle3B()
    {
        return IsEnglish()
            ? "Corrosion Resistance"
            : "Ketahanan Kakisan";
    }

    public static string ExperimentTitle3C()
    {
        return IsEnglish()
            ? "Vulcanised Rubber " +
              "Heat Resistance"
            : "Ketahanan Haba " +
              "Getah Tervulkan";
    }

    public static string ExperimentComplete()
    {
        return IsEnglish()
            ? "Chapter 3 Complete"
            : "Bab 3 Selesai";
    }

    // ─── Shared Buttons ───────────────────────────────────────────────────

    public static string NotebookButton()
    {
        return IsEnglish()
            ? "📓  NOTEBOOK"
            : "📓  BUKU NOTA";
    }

    public static string QuitButton()
    {
        return IsEnglish()
            ? "QUIT"
            : "KELUAR";
    }

    public static string BackToMenuButton()
    {
        return IsEnglish()
            ? "BACK TO MENU"
            : "KEMBALI KE MENU";
    }

    public static string CloseButton()
    {
        return IsEnglish()
            ? "CLOSE"
            : "TUTUP";
    }

    // ─── 3B Status ────────────────────────────────────────────────────────

    public static string PlaceNails()
    {
        return IsEnglish()
            ? "Place the iron nail in test tube P\n" +
              "and copper nail in test tube Q."
            : "Letakkan paku besi ke dalam tabung uji P\n" +
              "dan paku tembaga ke dalam tabung uji Q.";
    }

    public static string IronNailPlaced()
    {
        return IsEnglish()
            ? "Iron nail placed in test tube P.\n" +
              "Now place copper nail in test tube Q."
            : "Paku besi diletakkan dalam tabung uji P.\n" +
              "Letakkan paku tembaga dalam tabung uji Q.";
    }

    public static string CopperNailPlaced()
    {
        return IsEnglish()
            ? "Copper nail placed in test tube Q.\n" +
              "Now place iron nail in test tube P."
            : "Paku tembaga diletakkan " +
              "dalam tabung uji Q.\n" +
              "Letakkan paku besi dalam tabung uji P.";
    }

    public static string BothNailsPlaced()
    {
        return IsEnglish()
            ? "Both nails placed!\n" +
              "Observe the corrosion over time..."
            : "Kedua-dua paku telah diletakkan!\n" +
              "Perhatikan kakisan dari " +
              "masa ke masa...";
    }

    public static string TimerRunning()
    {
        return IsEnglish()
            ? "Corrosion in progress...\n" +
              "Please wait."
            : "Kakisan sedang berlaku...\n" +
              "Tunggu sebentar.";
    }

    public static string FullCorrosionTime(
        int seconds)
    {
        return IsEnglish()
            ? "Full corrosion time: "
              + seconds + "s"
            : "Masa untuk kakisan penuh: "
              + seconds + "s";
    }

    public static string InspectNails()
    {
        return IsEnglish()
            ? "Time up!\n" +
              "Pick up the nails from the\n" +
              "test tubes."
            : "Masa tamat!\n" +
              "Sila ambil paku daripada\n" +
              "tabung uji.";
    }

    // ─── 3B Nail Labels ───────────────────────────────────────────────────

    public static string TubePLabel()
    {
        return IsEnglish()
            ? "TEST TUBE P  —  Iron Nail"
            : "Tabung Uji P  —  Paku Besi";
    }

    public static string TubeQLabel()
    {
        return IsEnglish()
            ? "TEST TUBE Q  —  Copper Nail"
            : "Tabung Uji Q  —  Paku Tembaga";
    }

    public static string IronNail()
    {
        return IsEnglish()
            ? "Iron Nail (Tube P)"
            : "Paku Besi (Tabung Uji P)";
    }

    public static string CopperNail()
    {
        return IsEnglish()
            ? "Copper Nail (Tube Q)"
            : "Paku Tembaga (Tabung Uji Q)";
    }

    // ─── 3B Input UI ──────────────────────────────────────────────────────

    public static string IronInputLabel()
    {
        return IsEnglish()
            ? "Iron Nail (Test Tube P)"
            :  "Paku Besi\n" + "(Tabung Uji P)";
    }

    public static string CopperInputLabel()
    {
        return IsEnglish()
            ? "Copper Nail (Test Tube Q)"
            :  "Paku Tembaga\n" + " (Tabung Uji Q)";
    }

    public static string ConfirmButton()
    {
        return IsEnglish()
            ? "CONFIRM RESULTS"
            : "SAHKAN KEPUTUSAN";
    }

    public static string SubmitButton3B()
    {
        return IsEnglish()
            ? "SUBMIT OBSERVATION"
            : "HANTAR PEMERHATIAN";
    }

    // ─── 3B Results ───────────────────────────────────────────────────────

    public static string RustPresent(int percent)
    {
        return IsEnglish()
            ? "Corroded: " + percent + "%"
            : "Terkakis: " + percent + "%";
    }

    public static string NoRust()
    {
        return IsEnglish()
            ? "No corrosion (0%)"
            : "Tiada kakisan (0%)";
    }

    public static string ResultCorrect(
        int ironGuess, int ironCorrect)
    {
        return IsEnglish()
            ? "Correct!\n" +
              "Iron: " + ironGuess +
              "% (answer: " + ironCorrect +
              "%)\nCopper: 0% - no corrosion."
            : "Betul!\n" +
              "Besi: " + ironGuess +
              "% (jawapan: " + ironCorrect +
              "%)\nTembaga: 0% - tiada kakisan.";
    }

    public static string ResultWrong(
        int ironGuess, int ironCorrect,
        int copperGuess)
    {
        return IsEnglish()
            ? "Inaccurate.\n" +
              "Iron: " + ironGuess +
              "% (correct answer: " +
              ironCorrect + "%)\n" +
              "Copper: " + copperGuess +
              "% (correct answer: 0%)"
            : "Tidak tepat.\n" +
              "Besi: " + ironGuess +
              "% (jawapan betul: " +
              ironCorrect + "%)\n" +
              "Tembaga: " + copperGuess +
              "% (jawapan betul: 0%)";
    }

    // ─── 3B Conclusion ────────────────────────────────────────────────────

    public static string ConclusionTitle()
    {
        return IsEnglish()
            ? "CONCLUSION"
            : "KESIMPULAN";
    }

    public static string Conclusion(
        int elapsed, int fullTime,
        int correctPercent)
    {
        return IsEnglish()
            ?
            "The hypothesis is accepted.\n\n" +
            "Elapsed time: " + elapsed + "s\n" +
            "Expected corrosion: " +
            correctPercent + "%\n\n" +
            "Iron nail is made from pure\n" +
            "metal. When exposed to air and\n" +
            "water, it will corrode.\n\n" +
            "Copper nail is made from alloy\n" +
            "which is a mix of metal and\n" +
            "non-metal material. It has high\n" +
            "resistance to corrosion."
            :
            "Hipotesis diterima.\n\n" +
            "Tempoh masa: " + elapsed + "s\n" +
            "Anggaran kakisan: " +
            correctPercent + "%\n\n" +
            "Paku besi diperbuat daripada\n" +
            "logam tulen. Apabila terdedah\n" +
            "kepada udara dan air, ia akan\n" +
            "terkakis.\n\n" +
            "Paku tembaga diperbuat\n" +
            "daripada aloi, iaitu gabungan\n" +
            "logam dan bahan bukan logam.\n" +
            "Ia mempunyai rintangan\n" +
            "kakisan yang tinggi.";
    }

    // ─── 3C Status ────────────────────────────────────────────────────────

    public static string PlaceStrips3C()
    {
        return IsEnglish()
            ? "Place natural rubber in\n" +
              "boiling tube A and vulcanised\n" +
              "rubber in boiling tube B."
            : "Masukkan getah asli ke\n" +
              "dalam tabung didih A dan\n" +
              "getah tervulkan ke dalam\n" +
              "tabung didih B.";
    }

    public static string NaturalStripPlaced3C()
    {
        return IsEnglish()
            ? "Natural rubber placed in\n" +
              "boiling tube A.\n" +
              "Now place vulcanised rubber " +
              "in boiling tube B."
            : "Getah asli diletakkan " +
              "dalam Tabung Didih A.\n" +
              "Letakkan getah tervulkan " +
              "dalam Tabung Didih B.";
    }

    public static string VulcanisedStripPlaced3C()
    {
        return IsEnglish()
            ? "Vulcanised rubber placed " +
              "in boiling tube B.\n" +
              "Now place natural rubber " +
              "in boiling tube A."
            : "Getah tervulkan diletakkan " +
              "dalam Tabung Didih B.\n" +
              "Letakkan getah asli " +
              "dalam Tabung Didih A.";
    }

    public static string WrongStrip3C(
        string tubeLabel)
    {
        return IsEnglish()
            ? "Wrong rubber type for tube " +
              tubeLabel + "!\nCheck the labels."
            : "Jenis getah salah untuk tabung uji " +
              tubeLabel + "!\nSila semak label.";
    }

    public static string HeatingInProgress()
    {
        return IsEnglish()
            ? "Heating in progress...\n" +
              "Observe the changes."
            : "Pemanasan sedang berlaku...\n" +
              "Perhatikan perubahan.";
    }

    public static string InspectStrips3C()
    {
        return IsEnglish()
            ? "Heating is complete!\n" +
              "Measure the length of the\n" +
              "rubber strips."
            : "Pemanasan selesai!\n" +
              "Baca ukuran pada pembaris\n" +
              "dan rekodkan panjang\n" +
              "setiap jalur getah\n" +
              "selepas dipanaskan.";
    }

    public static string BothStripsPlaced3C()
    {
        return IsEnglish()
            ? "Both rubber strips are placed.\n" +
              "Turn on both Bunsen burners\n" +
              "to start heating."
            : "Kedua-dua jalur getah telah\n" +
              "diletakkan.\n" +
              "Hidupkan kedua-dua penunu Bunsen\n" +
              "untuk memulakan pemanasan.";
    }

    // ─── 3C Ruler Labels ──────────────────────────────────────────────────

    public static string NaturalRulerLabel()
    {
        return IsEnglish()
            ? "Natural Rubber  (Test Tube A)"
            : "Getah Asli  (Tabung Uji A)";
    }

    public static string VulcanisedRulerLabel()
    {
        return IsEnglish()
            ? "Vulcanised Rubber  (Test Tube B)"
            : "Getah Tervulkan  (Tabung Uji B)";
    }

    // ─── 3C Input Panel ───────────────────────────────────────────────────

    public static string InputPanelTitle3C()
    {
        return IsEnglish()
            ? "Enter the length of each " +
              "rubber strip after heating"
            : "Masukkan panjang setiap " +
              "jalur getah selepas dipanaskan";
    }

    public static string NaturalRubber()
    {
        return IsEnglish()
            ? "Natural Rubber (Test Tube A)"
            : "Getah Asli \n" +
            "(Tabung Uji A)";
    }

    public static string VulcanisedRubber()
    {
        return IsEnglish()
            ? "Vulcanised Rubber (Test Tube B)"
            : "Getah Tervulkan \n" +
            "(Tabung Uji B)";
    }

    public static string SubmitButton3C()
    {
        return IsEnglish()
            ? "SUBMIT OBSERVATION"
            : "HANTAR PEMERHATIAN";
    }

    // ─── 3C Results ───────────────────────────────────────────────────────

    public static string ResultCorrect3C(
        float naturalGuess,
        float naturalCorrect,
        float vulcanisedGuess,
        float vulcanisedCorrect)
    {
        return IsEnglish()
            ? "Correct!\n" +
              "Natural: " +
              naturalGuess.ToString("F1") +
              " cm (answer: " +
              naturalCorrect.ToString("F1") +
              " cm)\n" +
              "Vulcanised: " +
              vulcanisedGuess.ToString("F1") +
              " cm (answer: " +
              vulcanisedCorrect.ToString("F1") +
              " cm)"
            : "Betul!\n" +
              "Getah Asli: " +
              naturalGuess.ToString("F1") +
              " sm (jawapan: " +
              naturalCorrect.ToString("F1") +
              " sm)\n" +
              "Getah Tervulkan: " +
              vulcanisedGuess.ToString("F1") +
              " sm (jawapan: " +
              vulcanisedCorrect.ToString("F1") +
              " sm)";
    }

    public static string ResultWrong3C(
        float naturalGuess,
        float naturalCorrect,
        float vulcanisedGuess,
        float vulcanisedCorrect)
    {
        return IsEnglish()
            ? "Not quite.\n" +
              "Natural: you said " +
              naturalGuess.ToString("F1") +
              " cm, answer was " +
              naturalCorrect.ToString("F1") +
              " cm.\n" +
              "Vulcanised: you said " +
              vulcanisedGuess.ToString("F1") +
              " cm, answer was " +
              vulcanisedCorrect.ToString("F1") +
              " cm."
            : "Tidak tepat.\n" +
              "Getah Asli: anda kata " +
              naturalGuess.ToString("F1") +
              " sm, jawapan ialah " +
              naturalCorrect.ToString("F1") +
              " sm.\n" +
              "Getah Tervulkan: anda kata " +
              vulcanisedGuess.ToString("F1") +
              " sm, jawapan ialah " +
              vulcanisedCorrect.ToString("F1") +
              " sm.";
    }

    // ─── 3C Conclusion ────────────────────────────────────────────────────

    public static string Conclusion3C(
        float naturalLength,
        float vulcanisedLength)
    {
        return IsEnglish()
            ?
            "Hypothesis is accepted.\n\n" +
            "Natural rubber shrank to "
            + naturalLength.ToString("F1") +
            " cm after heating.\n\n" +
            "Vulcanised rubber remained at "
            + vulcanisedLength.ToString("F1") +
            " cm after heating.\n\n" +
            "Natural rubber is less resistant\n" +
            "to heat and has shrunk more from\n" +
            "its original form, while\n" +
            "vulcanised rubber is more\n" +
            "resistant to heat and has not\n" +
            "shrunk as much."
            :
            "Hipotesis diterima.\n\n" +
            "Getah asli mengecut kepada "
            + naturalLength.ToString("F1") +
            " cm selepas dipanaskan.\n\n" +
            "Getah tervulkan kekal pada "
            + vulcanisedLength.ToString("F1") +
            " cm selepas dipanaskan.\n\n" +
            "Getah asli kurang tahan haba\n" +
            "dan telah mengecut banyak\n" +
            "daripada bentuk asalnya. Getah\n" +
            "tervulkan pula lebih tahan haba\n" +
            "dan tidak banyak mengecut.";

    }

    // ─── 3B Header ────────────────────────────────────────────────────────

    public static string ExperimentHeader3B()
    {
        return IsEnglish()
            ? "CORROSION RESISTANCE EXPERIMENT"
            : "EKSPERIMEN KETAHANAN KAKISAN";
    }
    
    // ─── 3D Experiment Info ───────────────────────────────────────────────

    public static string ExperimentTitle3D()
    {
        return IsEnglish()
            ? "Latex Coagulation"
            : "Penggumpalan Lateks";
    }

    // ─── 3D Chemical Labels ───────────────────────────────────────────────

    public static string EthanoicAcid3D()
    {
        return IsEnglish()
            ? "Ethanoic Acid (CH3COOH)"
            : "Asid Etanoik (CH3COOH)";
    }

    public static string AmmoniaSolution3D()
    {
        return IsEnglish()
            ? "Ammonia Solution (NH3)"
            : "Larutan Ammonia (NH3)";
    }

    // ─── 3D Status ────────────────────────────────────────────────────────

    public static string PlaceDropper3D()
    {
        return IsEnglish()
            ? "Pick up a dropper and add\n" +
              "drops to each beaker."
            : "Ambil penitis dan tambah\n" +
              "titisan ke setiap bikar.";
    }

    public static string DropsAdded3D(
        string chemicalName, int count)
    {
        return IsEnglish()
            ? chemicalName + ": " + count +
              " drops added"
            : chemicalName + ": " + count +
              " titisan ditambah";
    }

    public static string ReactionStarting3D()
    {
        return IsEnglish()
            ? "Coagulation reaction\n" +
              "starting..."
            : "Tindak balas penggumpalan\n" +
              "bermula...";
    }

    public static string ReactionComplete3D()
    {
        return IsEnglish()
            ? "Latex has coagulated\n" +
              "in Beaker A."
            : "Lateks telah menggumpal\n" +
              "dalam Bikar A.";
    }

    public static string NoReaction3D()
    {
        return IsEnglish()
            ? "No coagulation occurred\n" +
              "in Beaker B."
            : "Tiada penggumpalan berlaku\n" +
              "dalam Bikar B.";
    }

    public static string BothComplete3D()
    {
        return IsEnglish()
            ? "Both reactions complete!\n" +
              "Press Confirm to continue."
            : "Kedua-dua tindak balas\n" +
              "selesai! Tekan Sahkan untuk\n" +
              "teruskan.";
    }

    // ─── 3D Buttons ───────────────────────────────────────────────────────

    public static string ConfirmButton3D()
    {
        return IsEnglish()
            ? "CONFIRM RESULTS"
            : "SAHKAN KEPUTUSAN";
    }

    // ─── 3D Results ───────────────────────────────────────────────────────

    public static string AcidResult3D()
    {
        return IsEnglish()
            ? "Coagulated"
            : "Bergumpal";
    }

    public static string AmmoniaResult3D()
    {
        return IsEnglish()
            ? "No coagulation"
            : "Tiada penggumpalan";
    }

    // ─── 3D Conclusion ────────────────────────────────────────────────────

    public static string ConclusionTitle3D()
    {
        return IsEnglish()
            ? "CONCLUSION"
            : "KESIMPULAN";
    }

    public static string Conclusion3D()
    {
        return IsEnglish()
            ?
            "The hypothesis is accepted.\n\n" +
            "Ethanoic acid causes the latex\n" +
            "particles to lose their\n" +
            "electric charge, allowing them\n" +
            "to clump together and\n" +
            "coagulate.\n\n" +
            "Ammonia keeps the latex\n" +
            "particles negatively charged\n" +
            "so they repel each other,\n" +
            "preventing coagulation. This is\n" +
            "why ammonia is used to\n" +
            "preserve liquid latex."
            :
            "Hipotesis diterima.\n\n" +
            "Asid etanoik menyebabkan\n" +
            "zarah lateks kehilangan\n" +
            "cas elektriknya, membolehkan\n" +
            "ia bergumpal dan\n" +
            "menggumpal.\n\n" +
            "Ammonia mengekalkan cas\n" +
            "negatif pada zarah lateks\n" +
            "supaya ia saling menolak,\n" +
            "menghalang penggumpalan.\n" +
            "Inilah sebabnya ammonia\n" +
            "digunakan untuk mengawet\n" +
            "lateks cecair.";
    }
    
    // ─── 3A Experiment Info ───────────────────────────────────────────────

    public static string ExperimentTitle3A()
    {
        return IsEnglish()
            ? "Hardness of Alloy vs Pure Metal"
            : "Kekerasan Aloi Berbanding Logam Tulen";
    }

    public static string ExperimentHeader3A()
    {
        return IsEnglish()
            ? "COMPARING HARDNESS OF ALLOY " +
              "AND PURE METAL"
            : "PERBANDINGAN KEKERASAN ALOI " +
              "DAN LOGAM TULEN";
    }

    // ─── 3A Metal Labels ────────────────────────────────────────────────

    public static string Copper3A()
    {
        return IsEnglish()
            ? "Copper (Pure Metal)"
            : "Kuprum (Logam Tulen)";
    }

    public static string Bronze3A()
    {
        return IsEnglish()
            ? "Bronze (Alloy)"
            : "Gangsa (Aloi)";
    }

    // ─── 3A Status ────────────────────────────────────────────────────────

    public static string ReadyToRelease3A(
        string metalName)
    {
        return IsEnglish()
            ? "Press RELEASE to drop the\n" +
              "steel ball onto the " +
              metalName + " strip."
            : "Tekan LEPASKAN untuk " +
              "menjatuhkan\nbola keluli ke atas " +
              "jalur " + metalName + ".";
    }

    public static string BallFalling3A()
    {
        return IsEnglish()
            ? "Ball falling..."
            : "Bola sedang jatuh...";
    }

    public static string MeasureDent3A(
        string metalName)
    {
        return IsEnglish()
            ? "Measure the dent on the\n" +
              metalName + " strip."
            : "Ukur lekuk pada jalur\n" +
              metalName + ".";
    }

    public static string ProceedToBronzePrompt3A()
    {
        return IsEnglish()
            ? "Copper measurement recorded.\n" +
              "Press PROCEED to test bronze."
            : "Ukuran kuprum telah direkodkan.\n" +
              "Tekan TERUSKAN untuk menguji " +
              "gangsa.";
    }

    // ─── 3A Buttons ───────────────────────────────────────────────────────

    public static string ReleaseButton3A()
    {
        return IsEnglish() ? "RELEASE" : "LEPASKAN";
    }

    public static string ProceedToBronzeButton3A()
    {
        return IsEnglish()
            ? "PROCEED TO BRONZE"
            : "TERUSKAN KE GANGSA";
    }

    public static string SubmitButton3A()
    {
        return IsEnglish()
            ? "SUBMIT MEASUREMENT"
            : "HANTAR UKURAN";
    }

    // ─── 3A Input Panel ─────────────────────────────────────────────────

    public static string InputTitle3A(
        string metalName)
    {
        return IsEnglish()
            ? "Enter dent depth for " +
              metalName + " (mm)"
            : "Masukkan kedalaman lekuk " +
              "untuk " + metalName + " (mm)";
    }

    public static string DentLabel3A()
    {
        return IsEnglish()
            ? "Dent Depth (mm)"
            : "Kedalaman Lekuk (mm)";
    }

    // ─── 3A Results ───────────────────────────────────────────────────────

    public static string ResultRecorded3A(
        string metalName, float value)
    {
        return IsEnglish()
            ? metalName + " recorded: " +
              value.ToString("F1") + " mm"
            : metalName + " direkodkan: " +
              value.ToString("F1") + " mm";
    }

    public static string ComparisonCloser3A()
    {
        return IsEnglish()
            ? "Bronze dented less than copper"
            : "Gangsa kurang berlekuk " +
              "berbanding kuprum";
    }

    public static string ComparisonFar3A()
    {
        return IsEnglish()
            ? "Result unclear \u2014 check " +
              "measurements"
            : "Keputusan tidak jelas \u2014 " +
              "semak ukuran";
    }

    // ─── 3A Conclusion ──────────────────────────────────────────────────

    public static string ConclusionTitle3A()
    {
        return IsEnglish() ? "CONCLUSION" : "KESIMPULAN";
    }

    public static string Conclusion3A(
        float copperDent, float bronzeDent)
    {
        return IsEnglish()
            ?
            "The hypothesis is accepted.\n\n" +
            "Copper dented " +
            copperDent.ToString("F1") +
            " mm, while bronze dented only " +
            bronzeDent.ToString("F1") +
            " mm.\n\n" +
            "Bronze is an alloy \u2014 a mixture " +
            "of copper and tin. Mixing metals\n" +
            "disrupts the regular arrangement " +
            "of atoms, making the alloy\n" +
            "harder and more resistant to " +
            "denting than the pure metal."
            :
            "Hipotesis diterima.\n\n" +
            "Kuprum berlekuk " +
            copperDent.ToString("F1") +
            " mm, manakala gangsa hanya " +
            "berlekuk " +
            bronzeDent.ToString("F1") +
            " mm.\n\n" +
            "Gangsa adalah aloi \u2014 " +
            "gabungan kuprum dan timah.\n" +
            "Percampuran logam mengganggu " +
            "susunan atom yang teratur,\n" +
            "menjadikan aloi lebih keras dan " +
            "tahan lekuk berbanding logam " +
            "tulen.";
    }
    
    public static string ReadyToReleaseBoth3A()
    {
        return IsEnglish()
            ? "Press RELEASE to drop both\n" +
              "steel balls onto the copper\n" +
              "and bronze strips."
            : "Tekan LEPASKAN untuk " +
              "menjatuhkan\nkedua-dua bola " +
              "keluli ke atas jalur\nkuprum " +
              "dan gangsa.";
    }

    public static string MeasureDentBoth3A()
    {
        return IsEnglish()
            ? "Measure the dents on both\n" +
              "the copper and bronze strips."
            : "Ukur lekuk pada kedua-dua\n" +
              "jalur kuprum dan gangsa.";
    }

    public static string InputTitleBoth3A()
    {
        return IsEnglish()
            ? "Enter dent depths (mm)"
            : "Masukkan kedalaman lekuk (mm)";
    }
}
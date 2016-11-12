package com.uabproject.lettergo;

import android.util.Log;

import com.unity3d.player.UnityPlayerActivity;

public class recognitionFunctions extends UnityPlayerActivity {
    public static boolean recognizeLetter(String letter, String imageb64, int w, int h) {
        Log.e("RECOGNITION PROCESS", (letter + w + h));
        return true;
    }
}
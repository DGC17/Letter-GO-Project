/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 3.0.2
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

package wrappers;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.util.Log;

import com.unity3d.player.UnityPlayerActivity;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.Random;
import wrappers.FileUtils;

import android.content.res.*;

public class Classifier extends UnityPlayerActivity {

    //Sliding Window parameters

    //Rescale input image to this size
    private static int rescale_width = 1;
    private static int rescale_height = 36;

    //Window parameters
    private static int w_scales = 1;
    private static int w_width = 32;
    private static int w_height = 32;
    private static int w_step = 1;

    //CNN parameters.
    private static int max_recognitions = 10; //Maximum number of returned recognitons
    private static double prob_TH = 0.9; //Only recognitions with a class prob over prob_TH are considered.

    private long swigCPtr;
    protected boolean swigCMemOwn;

    protected Classifier(long cPtr, boolean cMemoryOwn) {
        swigCMemOwn = cMemoryOwn;
        swigCPtr = cPtr;
    }

    protected static long getCPtr(Classifier obj) {
        return (obj == null) ? 0 : obj.swigCPtr;
    }

    protected void finalize() {
        delete();
    }

    public synchronized void delete() {
        if (swigCPtr != 0) {
            if (swigCMemOwn) {
                swigCMemOwn = false;
                classifierJNI.delete_Classifier(swigCPtr);
            }
            swigCPtr = 0;
        }
    }

    public void loadModel(String model_file, String trained_file, String mean_file, String label_file, int NUM_THREADS) {
        classifierJNI.Classifier_loadModel(swigCPtr, this, model_file, trained_file, mean_file, label_file, NUM_THREADS);
    }

    public void unloadModel() {
        classifierJNI.Classifier_unloadModel(swigCPtr, this);
    }

    public PairVector classify(byte[] bytes, int width, int height, int num_results) {
        return new PairVector(classifierJNI.Classifier_classify(swigCPtr, this, bytes, width, height, num_results), true);
    }

    public PairVector predictImageSlidingWindow(byte[] bytes, int width, int height, int max_recognitions, int w_width, int w_height, int w_step, int rescale_width, int rescale_height, int w_scales, double prob_TH) {
        return new PairVector(classifierJNI.Classifier_predictImageSlidingWindow(swigCPtr, this, bytes, width, height, max_recognitions, w_width, w_height, w_step, rescale_width, rescale_height, w_scales, prob_TH), true);
    }

    public void loadDefaultModel() {
        Log.i("CLASSIFIER", "LOADING MODEL...");

        String proto_file_name = "/sdcard/letter_recognition/deploy.prototxt";
        String caffe_model_file_name = "/sdcard/letter_recognition/model.caffemodel";
        String synset_words_file_name = "/sdcard/letter_recognition/synset_words.txt";
        String mean_file = "/sdcard/letter_recognition/mean.binaryproto";

        //We check if they exists in the device using only one of the files...
        File f = new File(proto_file_name);

        if (!f.exists()) {
            Log.i("CLASSIFIER", "LOADING NECESSARY FILES INTO DEVICE...");
            File directory = new File("/sdcard/letter_recognition");
            Log.i("CLASSIFIER", "CREATING NEW DIRECTORY...");
            directory.mkdir();
            Log.i("CLASSIFIER", "NEW DIRECTORY CREATED!");
            Context context = com.uabproject.lettergo.UnityPlayerActivity.getContext();
            Log.i("CLASSIFIER", "CONTEXT RETRIEVED!");
            AssetManager assetManager = context.getAssets();
            Log.i("CLASSIFIER", "ASSETS RETRIEVED!");
            String[] files = null;

            try {
                files = assetManager.list("letter_recognition");
            } catch (IOException e) {
                Log.i("CLASSIFIER", "CANNOT LIST THE FILES IN ASSETS...");
            }

            if (files != null) for (String filename : files) {

                InputStream in = null;
                OutputStream out = null;
                try {
                    in = assetManager.open("letter_recognition/" + filename);
                    File output = new File(directory, filename);
                    out = new FileOutputStream(output, true);
                    copyFile(in, out);
                } catch(IOException e) {
                    Log.i("CLASSIFIER", "CANNOT SAVE THE FILE INTO THE DEVICE..");
                    Log.i("CLASSIFIER", e.toString());
                } finally {
                    if (in != null) {
                        try {
                            in.close();
                        } catch (IOException e) {
                            // NOOP
                        }
                    }
                    if (out != null) {
                        try {
                            out.close();
                        } catch (IOException e) {
                            // NOOP
                        }
                    }
                }
            }

        } else {
            Log.i("CLASSIFIER", "FILES ALREADY EXISTS IN THE DEVICE...");
        }

        loadModel(proto_file_name, caffe_model_file_name, mean_file, synset_words_file_name, 2);
    }

    private void copyFile(InputStream in, OutputStream out) throws IOException {
        byte[] buffer = new byte[1024];
        int read;
        while((read = in.read(buffer)) != -1){
            out.write(buffer, 0, read);
        }
    }

    public String recognizeLetter(String letter, byte[] data, int width, int height) throws FileNotFoundException {

        Log.i("CLASSIFIER", "RECOGNIZING LETTER... Width: " + width + " Height: " + height);

        /*
        Log.i("CLASSIFIER", "SAVING IMAGE...");
        Bitmap bm = BitmapFactory.decodeByteArray(data, 0, data.length);
        OutputStream stream = new FileOutputStream("/sdcard/test.jpg");
        bm.compress(Bitmap.CompressFormat.JPEG, 100, stream);
        */
        //Convert the byte array to OpenCV compatible format
        Bitmap bitmap = BitmapFactory.decodeByteArray(data.clone(), 0, data.length, new BitmapFactory.Options());
        byte[] yuvData = FileUtils.GetNV21(bitmap.getWidth(), bitmap.getHeight(), bitmap);



        double aspect_ratio = (double) width / height;
        rescale_width = (int) (aspect_ratio * rescale_height);

        Log.i("CLASSIFIER", "Starting sliding window..");
        PairVector results = predictImageSlidingWindow(yuvData, bitmap.getWidth(), bitmap.getHeight(), max_recognitions, w_width, w_height, w_step, rescale_width, rescale_height, w_scales, prob_TH);
        Log.i("CLASSIFIER", "Finishing sliding window..");

        String output = "";
        int count = 0;

        while (count < results.size()) {

            Pair pairValues = results.get(count);
            String firstValue = pairValues.getFirst();
            float secondValue = pairValues.getSecond();

            output += firstValue + ":" + secondValue + "&";

            Log.i("CLASSIFIER", firstValue + ":" + secondValue);
            count++;
        }

        if (count > 0) output = output.substring(0, output.length() - 1);

        Log.i("CLASSIFIER", output);

        return output;
    }


    public Classifier() {
        this(classifierJNI.new_Classifier(), true);
    }

}
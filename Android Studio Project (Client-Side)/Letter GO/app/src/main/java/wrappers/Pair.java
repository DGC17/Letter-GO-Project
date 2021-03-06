/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 3.0.2
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

package wrappers;

public class Pair {
  private long swigCPtr;
  protected boolean swigCMemOwn;

  protected Pair(long cPtr, boolean cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  protected static long getCPtr(Pair obj) {
    return (obj == null) ? 0 : obj.swigCPtr;
  }

  protected void finalize() {
    delete();
  }

  public synchronized void delete() {
    if (swigCPtr != 0) {
      if (swigCMemOwn) {
        swigCMemOwn = false;
        classifierJNI.delete_Pair(swigCPtr);
      }
      swigCPtr = 0;
    }
  }

  public Pair() {
    this(classifierJNI.new_Pair__SWIG_0(), true);
  }

  public Pair(String first, float second) {
    this(classifierJNI.new_Pair__SWIG_1(first, second), true);
  }

  public Pair(Pair p) {
    this(classifierJNI.new_Pair__SWIG_2(Pair.getCPtr(p), p), true);
  }

  public void setFirst(String value) {
    classifierJNI.Pair_first_set(swigCPtr, this, value);
  }

  public String getFirst() {
    return classifierJNI.Pair_first_get(swigCPtr, this);
  }

  public void setSecond(float value) {
    classifierJNI.Pair_second_set(swigCPtr, this, value);
  }

  public float getSecond() {
    return classifierJNI.Pair_second_get(swigCPtr, this);
  }

}

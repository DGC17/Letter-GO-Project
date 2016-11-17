/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 3.0.2
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

package wrappers;

public class PairPrediction {
  private long swigCPtr;
  protected boolean swigCMemOwn;

  protected PairPrediction(long cPtr, boolean cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  protected static long getCPtr(PairPrediction obj) {
    return (obj == null) ? 0 : obj.swigCPtr;
  }

  protected void finalize() {
    delete();
  }

  public synchronized void delete() {
    if (swigCPtr != 0) {
      if (swigCMemOwn) {
        swigCMemOwn = false;
        classifierJNI.delete_PairPrediction(swigCPtr);
      }
      swigCPtr = 0;
    }
  }

  public PairPrediction() {
    this(classifierJNI.new_PairPrediction__SWIG_0(), true);
  }

  public PairPrediction(PairVector first, PairVector second) {
    this(classifierJNI.new_PairPrediction__SWIG_1(PairVector.getCPtr(first), first, PairVector.getCPtr(second), second), true);
  }

  public PairPrediction(PairPrediction p) {
    this(classifierJNI.new_PairPrediction__SWIG_2(PairPrediction.getCPtr(p), p), true);
  }

  public void setFirst(PairVector value) {
    classifierJNI.PairPrediction_first_set(swigCPtr, this, PairVector.getCPtr(value), value);
  }

  public PairVector getFirst() {
    long cPtr = classifierJNI.PairPrediction_first_get(swigCPtr, this);
    return (cPtr == 0) ? null : new PairVector(cPtr, false);
  }

  public void setSecond(PairVector value) {
    classifierJNI.PairPrediction_second_set(swigCPtr, this, PairVector.getCPtr(value), value);
  }

  public PairVector getSecond() {
    long cPtr = classifierJNI.PairPrediction_second_get(swigCPtr, this);
    return (cPtr == 0) ? null : new PairVector(cPtr, false);
  }

}

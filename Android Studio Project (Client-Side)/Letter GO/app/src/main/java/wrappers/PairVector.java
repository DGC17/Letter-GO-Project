/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 3.0.2
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

package wrappers;

public class PairVector {
  private long swigCPtr;
  protected boolean swigCMemOwn;

  protected PairVector(long cPtr, boolean cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  protected static long getCPtr(PairVector obj) {
    return (obj == null) ? 0 : obj.swigCPtr;
  }

  protected void finalize() {
    delete();
  }

  public synchronized void delete() {
    if (swigCPtr != 0) {
      if (swigCMemOwn) {
        swigCMemOwn = false;
        classifierJNI.delete_PairVector(swigCPtr);
      }
      swigCPtr = 0;
    }
  }

  public PairVector() {
    this(classifierJNI.new_PairVector__SWIG_0(), true);
  }

  public PairVector(long n) {
    this(classifierJNI.new_PairVector__SWIG_1(n), true);
  }

  public long size() {
    return classifierJNI.PairVector_size(swigCPtr, this);
  }

  public long capacity() {
    return classifierJNI.PairVector_capacity(swigCPtr, this);
  }

  public void reserve(long n) {
    classifierJNI.PairVector_reserve(swigCPtr, this, n);
  }

  public boolean isEmpty() {
    return classifierJNI.PairVector_isEmpty(swigCPtr, this);
  }

  public void clear() {
    classifierJNI.PairVector_clear(swigCPtr, this);
  }

  public void add(Pair x) {
    classifierJNI.PairVector_add(swigCPtr, this, Pair.getCPtr(x), x);
  }

  public Pair get(int i) {
    return new Pair(classifierJNI.PairVector_get(swigCPtr, this, i), false);
  }

  public void set(int i, Pair val) {
    classifierJNI.PairVector_set(swigCPtr, this, i, Pair.getCPtr(val), val);
  }

}

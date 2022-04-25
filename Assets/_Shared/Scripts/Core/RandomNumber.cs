using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Core {
  [Serializable]
  [InlineProperty]
  public abstract class RandomNumber<T>
    where T : unmanaged,
    IComparable,
    IComparable<T>,
    IConvertible,
    IEquatable<T>,
    IFormattable {
    // TODO: Excluding & including values for non-continuous range 

    [SerializeField] [HorizontalGroup] [LabelWidth(30)] [SuffixLabel("(min)")] [HideLabel]
    protected T _min;

    [SerializeField] [HorizontalGroup] [LabelWidth(30)] [SuffixLabel("(max)")] [HideLabel]
    protected T _max;

    protected T? _value;

    public static implicit operator T(RandomNumber<T> @this) => @this.Value;

    public RandomNumber(T min, T max) {
      _min = min;
      _max = max;
    }

    // ? Use bool instead to check for performance
    public T Value => _value ?? Randomize(); // lazy init

    /// <summary>
    ///   Re-randomize the current value and return it.
    /// </summary>
    public T Random => Randomize();

    protected abstract T Randomize();

    // REFACTOR: Implement one common method for both int & float
    // private T Randomize() {
    //   dynamic min = _min;
    //   dynamic max = _max;
    //   _value = UnityEngine.Random.Range(min, max);
    //   return _value.Value;
    // }
  }

  [Serializable]
  [InlineProperty]
  public class RandomFloat : RandomNumber<float> {
    public RandomFloat(float min, float max) : base(min, max) { }

    protected override float Randomize() {
      _value = UnityEngine.Random.Range(_min, _max);
      return _value.Value;
    }
  }

  [Serializable]
  [InlineProperty]
  public class RandomInt : RandomNumber<int> {
    public RandomInt(int min, int max) : base(min, max) { }

    protected override int Randomize() {
      _value = UnityEngine.Random.Range(_min, _max);
      return _value.Value;
    }
  }
}
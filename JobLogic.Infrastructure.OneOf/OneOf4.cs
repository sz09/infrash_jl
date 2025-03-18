using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.OneOf
{
    public class OneOf<T0, T1, T2, T3>
    {
        [JsonProperty]
        readonly T0 _data0;
        [JsonProperty]
        readonly T1 _data1;
        [JsonProperty]
        readonly T2 _data2;
        [JsonProperty]
        readonly T3 _data3;
        [JsonProperty]
        readonly int? _index;


        [JsonConstructor]
        private OneOf()
        {

        }

        OneOf(T0 data)
        {
            _index = 0;
            _data0 = data;
        }

        OneOf(T1 data)
        {
            _index = 1;
            _data1 = data;
        }
        OneOf(T2 data)
        {
            _index = 2;
            _data2 = data;
        }
        OneOf(T3 data)
        {
            _index = 3;
            _data3 = data;
        }

        public static implicit operator OneOf<T0, T1, T2, T3>(T0 t) => new OneOf<T0, T1, T2, T3>(t);
        public static implicit operator OneOf<T0, T1, T2, T3>(T1 t) => new OneOf<T0, T1, T2, T3>(t);
        public static implicit operator OneOf<T0, T1, T2, T3>(T2 t) => new OneOf<T0, T1, T2, T3>(t);
        public static implicit operator OneOf<T0, T1, T2, T3>(T3 t) => new OneOf<T0, T1, T2, T3>(t);

        [Obsolete("Use TryPick with fallbackValue instead")]
        public bool TryPick(out T0 value)
        {
            if (_index == 0)
            {
                value = _data0;
                return true;
            }
            value = default;
            return false;
        }

        public bool TryPick(out T0 value, out OneOf<T1,T2,T3> fallbackValue)
        {
            if (_index == 0)
            {
                value = _data0;
                fallbackValue = default;
                return true;
            }
            else
            {
                if(_index == 1)
                {
                    fallbackValue = _data1;
                }
                else if(_index == 2)
                {
                    fallbackValue = _data2;
                }
                else if (_index == 3)
                {
                    fallbackValue = _data3;
                }
                else
                {
                    throw new InvalidOperationException();
                }
                value = default;
                return false;
            }
        }
        [Obsolete("Use TryPick with fallbackValue instead")]
        public bool TryPick(out T1 value)
        {
            if (_index == 1)
            {
                value = _data1;
                return true;
            }
            value = default;
            return false;
        }

        public bool TryPick(out T1 value, out OneOf<T0, T2, T3> fallbackValue)
        {
            if (_index == 1)
            {
                value = _data1;
                fallbackValue = default;
                return true;
            }
            else
            {
                if (_index == 0)
                {
                    fallbackValue = _data0;
                }
                else if (_index == 2)
                {
                    fallbackValue = _data2;
                }
                else if (_index == 3)
                {
                    fallbackValue = _data3;
                }
                else
                {
                    throw new InvalidOperationException();
                }
                value = default;
                return false;
            }
        }
        [Obsolete("Use TryPick with fallbackValue instead")]
        public bool TryPick(out T2 value)
        {
            if (_index == 2)
            {
                value = _data2;
                return true;
            }
            value = default;
            return false;
        }

        public bool TryPick(out T2 value, out OneOf<T0, T1, T3> fallbackValue)
        {
            if (_index == 2)
            {
                value = _data2;
                fallbackValue = default;
                return true;
            }
            else
            {
                if (_index == 0)
                {
                    fallbackValue = _data0;
                }
                else if (_index == 1)
                {
                    fallbackValue = _data1;
                }
                else if (_index == 3)
                {
                    fallbackValue = _data3;
                }
                else
                {
                    throw new InvalidOperationException();
                }
                value = default;
                return false;
            }
        }

        [Obsolete("Use TryPick with fallbackValue instead")]
        public bool TryPick(out T3 value)
        {
            if (_index == 3)
            {
                value = _data3;
                return true;
            }
            value = default;
            return false;
        }

        public bool TryPick(out T3 value, out OneOf<T0, T1, T2> fallbackValue)
        {
            if (_index == 3)
            {
                value = _data3;
                fallbackValue = default;
                return true;
            }
            else
            {
                if (_index == 0)
                {
                    fallbackValue = _data0;
                }
                else if (_index == 1)
                {
                    fallbackValue = _data1;
                }
                else if (_index == 2)
                {
                    fallbackValue = _data2;
                }
                else
                {
                    throw new InvalidOperationException();
                }
                value = default;
                return false;
            }
        }

        public TResult Match<TResult>(Func<T0, TResult> f0, Func<T1, TResult> f1, Func<T2, TResult> f2, Func<T3, TResult> f3)
        {
            if (_index == 0 && f0 != null)
            {
                return f0(_data0);
            }

            if (_index == 1 && f1 != null)
            {
                return f1(_data1);
            }

            if (_index == 2 && f2 != null)
            {
                return f2(_data2);
            }

            if (_index == 3 && f3 != null)
            {
                return f3(_data3);
            }

            throw new InvalidOperationException();
        }

        bool Equals(OneOf<T0, T1, T2, T3> other)
        {
            if (_index != other._index)
            {
                return false;
            }
            switch (_index)
            {
                case 0: return Equals(_data0, other._data0);
                case 1: return Equals(_data1, other._data1);
                case 2: return Equals(_data2, other._data2);
                case 3: return Equals(_data3, other._data3);
                default: return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;


            return obj is OneOf<T0, T1, T2, T3> && Equals((OneOf<T0, T1, T2, T3>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode;
                switch (_index)
                {
                    case 0:
                        hashCode = _data0?.GetHashCode() ?? 0;
                        break;
                    case 1:
                        hashCode = _data1?.GetHashCode() ?? 0;
                        break;
                    case 2:
                        hashCode = _data2?.GetHashCode() ?? 0;
                        break;
                    case 3:
                        hashCode = _data3?.GetHashCode() ?? 0;
                        break;
                    default:
                        hashCode = 0;
                        break;
                }

                return (hashCode * 397) ^ _index.Value;
            }
        }
    }
}

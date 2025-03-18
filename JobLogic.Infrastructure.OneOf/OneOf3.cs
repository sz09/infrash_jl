using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.OneOf
{
    
    public class OneOf<T0, T1, T2>
    {
        [JsonProperty]
        readonly T0 _data0;
        [JsonProperty]
        readonly T1 _data1;
        [JsonProperty]
        readonly T2 _data2;
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

        public static implicit operator OneOf<T0, T1, T2>(T0 t) => new OneOf<T0, T1, T2>(t);
        public static implicit operator OneOf<T0, T1, T2>(T1 t) => new OneOf<T0, T1, T2>(t);
        public static implicit operator OneOf<T0, T1, T2>(T2 t) => new OneOf<T0, T1, T2>(t);

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

        public bool TryPick(out T0 value, out OneOf<T1, T2> fallbackValue)
        {
            if (_index == 0)
            {
                fallbackValue = default;
                value = _data0;
                return true;
            }
            else
            {
                if (_index == 1)
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
        public bool TryPick(out T1 value, out OneOf<T0, T2> fallbackValue)
        {
            if (_index == 1)
            {
                fallbackValue = default;
                value = _data1;
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
                else
                {
                    throw new InvalidOperationException();
                }
                value = default;
                return false;
            }
        }


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

        public bool TryPick(out T2 value, out OneOf<T0, T1> fallbackValue)
        {
            if (_index == 2)
            {
                fallbackValue = default;
                value = _data2;
                return true;
            }
            else
            {
                if (_index == 0)
                {
                    fallbackValue = _data0;
                }
                else if(_index == 1)
                {
                    fallbackValue = _data1;
                }
                else
                {
                    throw new InvalidOperationException();
                }
                value = default;
                return false;
            }
        }

        public TResult Match<TResult>(Func<T0, TResult> f0, Func<T1, TResult> f1, Func<T2, TResult> f2)
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

            throw new InvalidOperationException();
        }

        bool Equals(OneOf<T0, T1, T2> other)
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
                default: return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;


            return obj is OneOf<T0, T1, T2> && Equals((OneOf<T0, T1, T2>)obj);
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
                    default:
                        hashCode = 0;
                        break;
                }

                return (hashCode * 397) ^ _index.Value;
            }
        }
    }
}

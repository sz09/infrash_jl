using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.OneOf
{
    public class OneOf<T0, T1>
    {
        [JsonProperty]
        readonly T0 _data0;
        [JsonProperty]
        readonly T1 _data1;
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

        public static implicit operator OneOf<T0, T1>(T0 t) => new OneOf<T0, T1>(t);
        public static implicit operator OneOf<T0, T1>(T1 t) => new OneOf<T0, T1>(t);
        
        [Obsolete("Use TryPick with fallbackValue instead")]
        public bool TryPick(out T0 value)
        {
            if(_index == 0)
            {
                value = _data0;
                return true;
            }
            value = default;
            return false;
        }

        public bool TryPick(out T0 value, out T1 fallbackValue)
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
                    value = default;
                    return false;
                }
                else
                {
                    throw new InvalidOperationException();
                }
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

        public bool TryPick(out T1 value, out T0 fallbackValue)
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
                    value = default;
                    fallbackValue = _data0;
                    return false;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public TResult Match<TResult>(Func<T0, TResult> f0, Func<T1, TResult> f1)
        {
            if (_index == 0 && f0 != null)
            {
                return f0(_data0);
            }
            if (_index == 1 && f1 != null)
            {
                return f1(_data1);
            }
            throw new InvalidOperationException();
        }

        bool Equals(OneOf<T0, T1> other)
        {
            if (_index != other._index)
            {
                return false;
            }
            switch (_index)
            {
                case 0: return Equals(_data0, other._data0);
                case 1: return Equals(_data1, other._data1);
                default: return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;


            return obj is OneOf<T0, T1> && Equals((OneOf<T0, T1>)obj);
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
                    default:
                        hashCode = 0;
                        break;
                }

                return (hashCode * 397) ^ _index.Value;
            }
        }
    }

}

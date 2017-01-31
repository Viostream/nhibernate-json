namespace NHibernate.Json
{
    using System;
    using System.Data;
    using System.Runtime.Serialization;
    using SqlTypes;
    using UserTypes;

    public class JsonColumnType<T> : IUserType
    {
        public Type ReturnedType
        {
            get { return typeof (T); }
        }

        public object Assemble(object cached, object owner)
        {
            //Used for casching, as our object is immutable we can just return it as is
            return cached;
        }

        public object DeepCopy(object value)
        {
            //We deep copy the Translation by creating a new instance with the same contents
            if (value == null)
                return null;
            if (value.GetType() != typeof (T))
                return null;
            return FromJson(ToJson((T) value));
        }

        public object Disassemble(object value)
        {
            //Used for casching, as our object is immutable we can just return it as is
            return value;
        }

        public new bool Equals(object x, object y)
        {
            //Use json-query-string to see if their equal 
            // on value so we use this implementation
            if (x == null || y == null)
                return false;
            if (x.GetType() != typeof (T) || y.GetType() != typeof (T))
                return false;
            return ToJson((T) x).Equals(ToJson((T) y));
        }

        public int GetHashCode(object x)
        {
            if (x != null && x.GetType() == typeof (T))
                return ToJson((T) x).GetHashCode();
            return x.GetHashCode();
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            //We get the string from the database using the NullSafeGet used to get strings 
            var jsonString = (string) NHibernateUtil.String.NullSafeGet(rs, names[0]);
            //And save it in the T object. This would be the place to make sure that your string 
            //is valid for use with the T class
            return FromJson(jsonString);
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            //Set the value using the NullSafeSet implementation for string from NHibernateUtil
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);
                return;
            }
            value = ToJson((T) value);
            NHibernateUtil.String.NullSafeSet(cmd, value, index);
        }

        public object Replace(object original, object target, object owner)
        {
            //As our object is immutable we can just return the original
            return original;
        }

        public SqlType[] SqlTypes
        {
            get
            {
                //We store our translation in a single column in the database that can contain a string
                var types = new SqlType[1];
                types[0] = new SqlType(DbType.String);
                return types;
            }
        }

        public static T FromJson(string jsonString)
        {
            if (!string.IsNullOrWhiteSpace(jsonString))
                return CreateObject(typeof (T));
            var decompressed = JsonCompressor.Decompress(jsonString);
            return JsonConvertor.Deserialize<T>(decompressed);
        }

        private static T CreateObject(Type jsonType)
        {
            object result;
            try
            {
                result = Activator.CreateInstance(jsonType, true);
            }
            catch (Exception)
            {
                result = FormatterServices.GetUninitializedObject(jsonType);
            }

            return (T) result;
        }

        public static string ToJson(T obj)
        {
            var serialised = JsonConvertor.Serialize(obj);
            return JsonCompressor.Compress(serialised);
        }
    }
}
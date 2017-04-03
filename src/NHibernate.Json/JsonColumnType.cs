namespace NHibernate.Json
{
    using System;
    using System.Data;
    using System.Runtime.Serialization;
    using SqlTypes;
    using UserTypes;

    public class JsonColumnType<T> : IUserType where T : class
    {
        public Type ReturnedType
        {
            get { return typeof (T); }
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            var source = value as T;
            if (source == null)
                return null;
            return Deserialise(Serialise(source));
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public new bool Equals(object x, object y)
        {
            var left = x as T;
            var right = y as T;

            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            return Serialise(left).Equals(Serialise(right));
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var returnValue = NHibernateUtil.String.NullSafeGet(rs, names[0]);
            var json = returnValue == null ? "{}" : returnValue.ToString();
            return Deserialise(json);
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            var column = value as T;
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, "{}", index);
                return;
            }
            value = Serialise(column);
            NHibernateUtil.String.NullSafeSet(cmd, value, index);
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public SqlType[] SqlTypes
        {
            get
            {
                return new SqlType[] { SqlTypeFactory.GetString(8000) };
            }
        }

        public T Deserialise(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                return CreateObject(typeof (T));

            var decompressed = JsonCompressor.Decompress(jsonString);
            return JsonWorker.Deserialize<T>(decompressed);
        }

        public string Serialise(T obj)
        {
            if (obj == null)
                return "{}";
            var serialised = JsonWorker.Serialize(obj);
            return JsonCompressor.Compress(serialised);
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

       
    }
}
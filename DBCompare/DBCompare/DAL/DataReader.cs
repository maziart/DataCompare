using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DBCompare.DAL
{
    static class DataReader
    {
        private static Dictionary<Type, object> ObjectReaders;
        static DataReader()
        {
            ObjectReaders = new Dictionary<Type, object>();
        }
        private static ObjectReader<T> GetReader<T>() where T : class, new()
        {
            var type = typeof(T);
            if (!ObjectReaders.ContainsKey(type))
            {
                ObjectReaders.Add(type, new ObjectReader<T>());
            }
            return (ObjectReader<T>)ObjectReaders[type];
        }
        public static List<T> ToList<T>(this IDataReader reader) where T : class, new()
        {
            var objectReader = GetReader<T>();
            var list = new List<T>();
            while (reader.Read())
            { 
                list.Add(objectReader.CreateFrom(reader));
            }
            return list;
        }
        public static List<T> ToPrimitiveList<T>(this IDataReader reader) 
        {
            var list = new List<T>();
            while (reader.Read())
            {
                var value = reader[0];
                if (value == DBNull.Value)
                    value = DefaultValues.GetDefault<T>();
                list.Add((T)value);
            }
            return list;
        }
        public static T FirstOrDefault<T>(this IDataReader reader) where T : class, new()
        {
            if (!reader.Read())
                return null;
            var objectReader = GetReader<T>();
            return objectReader.CreateFrom(reader);
        }
        public static T First<T>(this IDataReader reader) where T : class, new()
        {
            if (!reader.Read())
                throw new InvalidOperationException("Sequence contains no elements");
            var objectReader = GetReader<T>();
            return objectReader.CreateFrom(reader);
        }
        public static T SingleOrDefault<T>(this IDataReader reader) where T : class, new()
        {
            if (!reader.Read())
                return null;
            var objectReader = GetReader<T>();
            var result = objectReader.CreateFrom(reader);
            if (reader.Read())
                throw new InvalidOperationException("Sequence contains more than one element");
            return result;
        }
        public static T Single<T>(this IDataReader reader) where T : class, new()
        {
            if (!reader.Read())
                throw new InvalidOperationException("Sequence contains no elements");
            var objectReader = GetReader<T>();
            var result = objectReader.CreateFrom(reader);
            if (reader.Read())
                throw new InvalidOperationException("Sequence contains more than one element");
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;

namespace DBCompare.DAL
{
    class ObjectReader<T> where T: class, new()
    {
        private Dictionary<string, PropertyInfo> Properties;
        public ObjectReader()
        {
            Properties = new Dictionary<string, PropertyInfo>();
            var type = typeof(T);
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.GetCustomAttributes(typeof(DataReaderIgnoreAttribute), false).Length > 0)
                    continue;
                Properties.Add(property.Name, property);
            }
        }
        public T CreateFrom(IDataReader reader)
        {
            var result = new T();
            foreach (var property in Properties)
            {
                var value = reader[property.Key];
                if (value == DBNull.Value)
                    value = DefaultValues.GetDefault<T>();
                property.Value.SetValue(result, value, null);
            }
            return result;
        }
    }
}

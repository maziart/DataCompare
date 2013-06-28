using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCompare.Config
{
    public class ApplicationArguments 
    {
        public static void Initialize(string[] args)
        {
            if (_Instance != null)
                throw new InvalidOperationException("ApplicationArguments has aleary been initialized");

            _Instance = new ApplicationArguments();
            _Instance.Parse(args);
        }
        private ApplicationArguments()
        {
            Arguments = new List<Argument>();
        }
        private void Parse(string[] args)
        {
            foreach (var arg in args)
            {
                var argument = new Argument(arg);
                if (Arguments.Any(n => n.Key == argument.Key))
                    throw new InvalidOperationException("Duplicate argument: " + argument.Key);
                Arguments.Add(argument);
            }
        }
        private static ApplicationArguments _Instance;
        public static ApplicationArguments Instance
        {
            get
            {
                if (_Instance == null)
                    throw new InvalidOperationException("ApplicationArguments has not been initialized");
                return _Instance;
            }
        }
        private List<Argument> Arguments;
        public string this[ArgumentType key]
        {
            get
            {
                var arg = Arguments.FirstOrDefault(n => n.Key == key);
                return arg == null ? null : arg.Value;
            }
        }
    }
    public class Argument 
    {
        public ArgumentType Key { get; private set; }
        public string Value { get; private set; }
        public Argument(string arg)
        {
            var index = arg.IndexOf('=');
            Key = index < 0 ? ArgumentType.Open : (ArgumentType)Enum.Parse(typeof(ArgumentType), arg.Substring(0, index), true);
            Value = index < 0 ? arg : arg.Substring(index + 1);
        } 
    }
    public enum ArgumentType
    {
        Open,
        Port
    }
}

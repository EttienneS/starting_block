using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Assets.Helpers
{
    public sealed class Instrumenter : IDisposable
    {
        private Instrumenter(string name)
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
            Name = name;
        }

        public string Name { get; set; }
        public Stopwatch Stopwatch { get; set; }

        public static Instrumenter Start(string info = "")
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrames()[1];
            var method = frame.GetMethod();

            var name = $"[{method.DeclaringType.Name}.{method.Name}]";
            if (!string.IsNullOrEmpty(info))
            {
                name += $" {info}";
            }
            return new Instrumenter(name);
        }

        public void Dispose()
        {
            Stamp("Completed");
        }

        public void Stamp(string message = "Stamp")
        {
            Debug.Log($"{Name} {message}:{Stopwatch.ElapsedMilliseconds}");
        }
    }
}
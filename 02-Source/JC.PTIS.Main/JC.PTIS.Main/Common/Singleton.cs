using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Common
{
    public abstract class Singleton<T> where T : class
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(() =>
            (T)Activator.CreateInstance(typeof(T), nonPublic: true));

        public static T Instance => _instance.Value;

        protected Singleton() { }
    }
}

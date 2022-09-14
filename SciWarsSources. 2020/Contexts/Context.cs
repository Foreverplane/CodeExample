using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public static class Context {
        private static readonly List<ContextDataService> _contextServices = new List<ContextDataService>();

        public static void AddContext<TContext>(TContext c) where TContext: ContextDataService
        {
            _contextServices.Add(c);
            Debug.Log($"Context <b>{c.GetType().Name}</b> added!");
        }

        public static void RemoveContext<TContext>(TContext c) where TContext : ContextDataService {
            _contextServices.Remove(c);
            Debug.Log($"Context <b>{c.GetType().Name}</b> removed!");
        }

        public static TContext GetContext<TContext>() where TContext : ContextDataService
        {
            return _contextServices.OfType<TContext>().First();
        }
    }
}
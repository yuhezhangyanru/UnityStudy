﻿using System;
namespace Assets.Assets.Scripts
{
    public class Singleton<T> where T : new()
    {
        private static T instance = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
        public static T Instance
        {
            get
            {
                return Singleton<T>.instance;
            }
        }

        protected Singleton()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YACUF.Utilities
{
    /// <summary>
    /// class with extension-methods for enumerables
    /// </summary>
    public static class EnumerableExtension
    {
        #region check-functions

        /// <summary>
        /// checks if IEnumerable and at least one element isn't equal null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool AnyOrNotNull<T>(this IEnumerable<T> source)
        {
            if (source != null && source.Any())
                return true;
            else
                return false;
        }

        /// <summary>
        /// checks if IEnumerable has at least one element and if so, how many
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IEnumerable to check</param>
        /// <param name="elementCount">number of elements</param>
        /// <returns>true if at least one element exists</returns>
        public static bool HasElements<T>(this IEnumerable<T> source, out int elementCount)
        {
            bool result = false;
            elementCount = 0;

            if (source != null)
            {
                elementCount = source.Count();
                result = (elementCount > 0);
            }

            return result;
        }

        /// <summary>
        /// checks if IEnumerable has at least one element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IEnumerable to check</param>
        /// <returns>true if at least one element exists</returns>
        public static bool HasElements<T>(this IEnumerable<T> source)
        {
            bool result = false;

            if (source != null)
            {
                result = (source.Count() > 0);
            }

            return result;
        }

        /// <summary>
        /// checks if IEnumerable has x-amount of elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IEnumerable to check</param>
        /// <param name="expectedElementCount">expected number of elements</param>
        /// <returns>true if number of elements is equal to expected amount</returns>
        public static bool HasXElements<T>(this IEnumerable<T> source, int expectedElementCount)
        {
            bool result = false;

            if (source != null)
            {
                result = (source.Count() == expectedElementCount);
            }

            return result;
        }

        #endregion



        /// <summary>
        /// gets the description of an enum (if '[Description("")]' is written above an enum-items)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns>description</returns>
        public static string GetEnumDescription<T>(this T enumValue) where T : struct, IConvertible
        {
            string result = "";

            if (typeof(T).IsEnum)
            {
                string? description = enumValue.ToString();

                if (description != null)
                {
                    FieldInfo? fieldInfo = enumValue.GetType().GetField(description);

                    if (fieldInfo != null)
                    {
                        var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (attrs != null && attrs.Length > 0)
                        {
                            description = ((DescriptionAttribute)attrs[0]).Description;
                        }
                    }

                    return description;
                }
            }

            return result;
        }

        #region add / combination-functions

        /// <summary>
        /// adds an item to a given list, if said item isn't in the list yet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="newItem"></param>
        public static void AddUniqueItem<T>(this List<T> sourceList, T newItem)
        {
            if (sourceList == null)
            {
                sourceList = new List<T>();
            }

            bool itemFound = false;

            foreach (T item in sourceList)
            {
                if (EqualityComparer<T>.Default.Equals(item, newItem))
                {
                    itemFound = true;
                    break;
                }
            }

            if (!itemFound)
            {
                sourceList.Add(newItem);
            }
        }

        /// <summary>
        /// tries to add an item to a given list, if said item isn't in the list yet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList">list the new item is supposed to be added to</param>
        /// <param name="newItem">new item</param>
        /// <returns>true if item could get added</returns>
        public static bool TryAddUniqueItem<T>(this List<T> sourceList, T newItem)
        {
            if (sourceList == null)
            {
                sourceList = new List<T>();
            }

            bool itemFound = false;

            foreach (T item in sourceList)
            {
                if (EqualityComparer<T>.Default.Equals(item, newItem))
                {
                    itemFound = true;
                    break;
                }
            }

            if (!itemFound)
            {
                sourceList.Add(newItem);
            }

            return !itemFound;
        }

        /// <summary>
        /// tries to add a new value and key to a dicationary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns> true if successfull, false if key already exists</returns>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue>? dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                return true;
            }

            return false;
        }


        /// <summary>
        /// creates new list outn of two seperate lists
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static List<T> CombineLists<T>(List<T> list1, List<T> list2)
        {
            List<T> result = new List<T>();
            if (list1.HasElements(out int sourceCount))
            {
                for (int i = 0; i < sourceCount; i++)
                {
                    result.Add(list1[i]);
                }
            }

            if (list2.HasElements(out int additionalCount))
            {
                for (int i = 0; i < additionalCount; i++)
                {
                    result.Add(list2[i]);
                }
            }

            result.Sort();

            return result;

        }

        #endregion
    }
}
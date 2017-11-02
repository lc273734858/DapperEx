using FWW.Framework.Common.ErrorHandling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    public interface ICheck<T>
    {
        void IsOk(T value, string valuename);
    }
    public class Check
    {
        public static void Require<T>(T value, string objectName, params ICheck<T>[] checks)
        {
            foreach (var item in checks)
            {
                item.IsOk(value, objectName);
            }
        }
        public static void GreaterThanOrEqual<T>(T value, string objectName, T comparevalue)
        {
            if (value is T && ((IComparable)value).CompareTo(comparevalue) >= 0)
                return;
            throw new Exception(string.Format("{0}必须大于等于{1}", objectName, value));
        }
        public static void GreaterThan<T>(T value, string objectName, T comparevalue)
        {
            if (value is T && ((IComparable)value).CompareTo(comparevalue) > 0)
                return;
            throw new Exception(string.Format("{0}必须大于等于{1}", objectName, value));
        }
        public static void NotNullOrEmpty(string value, string objectName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Framework.Common.ErrorHandling.FriendlyException(string.Format("{0}的值为空或者长度为零。", objectName));
            }
        }
        public static void NotNullOrEmpty(object obj, string objectName)
        {
            bool isok = true;
            if (obj == null)
                isok = false;
            else if (obj is Array)
                isok = (obj as Array).Length > 0;
            else if (obj is ICollection)
                isok = (obj as ICollection).Count > 0;
            if (isok == false)
            {
                throw new Framework.Common.ErrorHandling.FriendlyException(string.Format("{0}的值为空或者长度为零。", objectName));
            }
        }
        public static void Require(bool assertion, string message)
        {
            if (!assertion) throw new FriendlyException(message);
        }
    }
}

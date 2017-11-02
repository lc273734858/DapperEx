using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityCacheManager
    {
        #region Fields
        public const string Creator = "Creator";
        public const string CreationDate = "CreationDate";
        public const string Modifier = "Modifier";
        public const string ModifiedDate = "ModifiedDate";
        private static Dictionary<Type, EntityCacheInfo> _Cache = new Dictionary<Type, EntityCacheInfo>();
        #endregion

        #region TryAddInit
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        static public bool TryGetValue(Type type, out EntityCacheInfo info)
        {
            if (_Cache.TryGetValue(type, out info))
            {
                return true;
            }
            else
            {
                try
                {
                    info = GeneratorSqlMethodInfo(type);
                    Add(type, info);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        static private void Add(Type type, EntityCacheInfo info)
        {
            lock (_Cache)
            {
                if (!_Cache.ContainsKey(type))
                {
                    _Cache.Add(type, info);
                }
            }
        }
        static private EntityCacheInfo GeneratorSqlMethodInfo(Type type)
        {
            EntityCacheInfo info = new EntityCacheInfo(type);
            var tablename = type.GetCustomAttribute<TableAttribute>(false) as TableAttribute;
            if (tablename != null)
            {
                info.TableName = (tablename as TableAttribute).Name;
            }
            else
            {
                info.TableName = type.Name;
            }
            List<PropertyInfo> insertPropertys = new List<PropertyInfo>();
            List<PropertyInfo> updatePropertys = new List<PropertyInfo>();
            List<PropertyInfo> updateAllPropertys = new List<PropertyInfo>();
            List<PropertyInfo> primaryPropertys = new List<PropertyInfo>();
            List<PropertyInfo> simpleWherePropertys = new List<PropertyInfo>();
            List<PropertyInfo> updateInfoPropertys = new List<PropertyInfo>();//更新特征字段,Modiffer&Date
            List<PropertyInfo> insertInfoPropertys = new List<PropertyInfo>();//新增特征字段，Creator&Date

            List<string> fields = new List<string>();
            foreach (var property in type.GetProperties())
            {
                var ignoreAttr = property.GetCustomAttribute<IgnoreAttribute>(false) as IgnoreAttribute;
                if (ignoreAttr == null)
                {
                    if (property.Name.Equals(Creator, StringComparison.OrdinalIgnoreCase) || property.Name.Equals(CreationDate, StringComparison.OrdinalIgnoreCase))
                    {
                        insertInfoPropertys.Add(property);
                    }
                    if (property.Name.Equals(Modifier, StringComparison.OrdinalIgnoreCase) || property.Name.Equals(ModifiedDate, StringComparison.OrdinalIgnoreCase))
                    {
                        updateInfoPropertys.Add(property);
                    }
                    var primaryAttr = property.GetCustomAttribute<PrimaryKeyAttribute>(false) as PrimaryKeyAttribute;
                    if (primaryAttr != null)
                    {
                        info.PrimaryKeys.Add(property.Name);
                        primaryPropertys.Add(property);
                        if ((primaryAttr as PrimaryKeyAttribute).AutoIncrement)
                        {
                            info.IdentityField = property.Name;
                        }
                        else
                        {
                            insertPropertys.Add(property);
                        }
                    }
                    else
                    {
                        updatePropertys.Add(property);
                        insertPropertys.Add(property);
                    }
                    fields.Add(property.Name);
                    updateAllPropertys.Add(property);
                    simpleWherePropertys.Add(property);
                }
                else
                {
                    if ((ignoreAttr as IgnoreAttribute).All == false)
                    {
                        simpleWherePropertys.Add(property);
                    }
                    continue;
                }
            }
            Check.NotNullOrEmpty(info.PrimaryKeys, "主键");
            info.ParimaryKeyWhere = CreateSqlFunc(type, primaryPropertys, "{0}=@{0}", 0);
            info.GetUpdateByPrimaryKeysFunc = CreateSqlFunc(type, updatePropertys, ",{0}=@{0}", 1);
            info.GetUpdateByWhereFunc = CreateUpdateFunc(type, updateAllPropertys, ",{0}={1}", 1);

            info.InsertDeserializer = CreateInsertSqlFunc(type, insertPropertys, ",{0}", ",@{0}", 1);
            info.SimpleWhereDeserializer = CreateSqlFunc(type, simpleWherePropertys, " and {0}=@{0}", 4);

            return info;
        }
        #endregion

        #region DymicMethod
        #region MI
        private static MethodInfo readLineMI = typeof(Console).GetMethod(
                    "ReadLine",
                    new Type[0]);
        static Type[] wlParams = new Type[] { typeof(object) };
        static MethodInfo writeLineMI = typeof(Console).GetMethod("WriteLine", wlParams);
        private static MethodInfo appendFormatMI = typeof(StringBuilder).GetMethod("AppendFormat", new Type[] { typeof(string), typeof(object) });
        private static MethodInfo appendFormatMI2 = typeof(StringBuilder).GetMethod("AppendFormat", new Type[] { typeof(string), typeof(object), typeof(object) });
        private static MethodInfo appendMI = typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) });
        private static MethodInfo addSqlDataParamterMI = typeof(SqlDataParameter).GetMethod("AddParamter", new Type[] { typeof(string), typeof(object) });
        private static MethodInfo isNullOrEmptyMI = typeof(System.String).GetMethod("IsNullOrEmpty", new Type[] { typeof(string) });
        private static FieldInfo dateTimeMinValueMI = typeof(DateTime).GetField("MinValue");
        private static MethodInfo sbLengthMI = typeof(StringBuilder).GetProperty("Length").GetGetMethod();
        private static MethodInfo sbRemoveMI = typeof(StringBuilder).GetMethod("Remove", new Type[] { typeof(Int32), typeof(Int32) });
        private static MethodInfo sbToStringMI = typeof(StringBuilder).GetMethod("ToString", new Type[] { });
        private static MethodInfo dateTimeEqualsMI = typeof(DateTime).GetMethod("Equals", new Type[] { typeof(DateTime), typeof(DateTime) });

        #endregion

        #region MIMethod
        /// <summary>
        /// Creates the SQL function.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="formatstring">The formatstring.</param>
        /// <param name="movelength">The movelength.</param>
        /// <param name="checkignore">是否检查忽略属性</param>
        /// <returns>Func{System.ObjectSystem.String}.</returns>
        static public Func<object, string> CreateSqlFunc(Type type, string formatstring, int movelength)
        {
            var dm = new DynamicMethod(string.Format("GetSql{0}", Guid.NewGuid()), typeof(string), new[] { typeof(object) }, type, true);
            var il = dm.GetILGenerator();
            CreateSqlDynamicFunc(il, type.GetProperties(), formatstring, movelength);
            return (Func<object, string>)dm.CreateDelegate(typeof(Func<object, string>));
        }
        static public Func<object, string> CreateSqlFunc(Type type, IEnumerable<PropertyInfo> propertys, string formatstring, int movelength)
        {
            var dm = new DynamicMethod(string.Format("GetSql{0}", Guid.NewGuid()), typeof(string), new[] { typeof(object) }, type, true);
            var il = dm.GetILGenerator();
            CreateSqlDynamicFunc(il, propertys, formatstring, movelength);
            return (Func<object, string>)dm.CreateDelegate(typeof(Func<object, string>));
        }
        /// <summary>
        /// 创建一个将实体值装入字典的方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertys"></param>
        /// <param name="formatstring"></param>
        /// <param name="movelength"></param>
        /// <returns></returns>
        static public Func<object, SqlDataParameter, string> CreateUpdateFunc(Type type, IEnumerable<PropertyInfo> propertys, string formatstring, int movelength)
        {
            var dm = new DynamicMethod(string.Format("GetSql{0}", Guid.NewGuid()), typeof(string), new[] { typeof(object), typeof(SqlDataParameter) }, type, true);
            var il = dm.GetILGenerator();
            CreateUpdateSqlDynamicFunc(il, propertys, formatstring, movelength);
            return (Func<object, SqlDataParameter, string>)dm.CreateDelegate(typeof(Func<object, SqlDataParameter, string>));
        }
        /// <summary>
        /// 生成InsertSql
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertys"></param>
        /// <param name="formatstring"></param>
        /// <param name="formatstringValue"></param>
        /// <param name="movelength"></param>
        /// <returns></returns>
        static public Func<object, string> CreateInsertSqlFunc(Type type, IEnumerable<PropertyInfo> propertys, string formatstring, string formatstringValue, int movelength)
        {
            var dm = new DynamicMethod(string.Format("GetSql{0}", Guid.NewGuid()), typeof(string), new[] { typeof(object) }, type, true);
            var il = dm.GetILGenerator();
            CreateInsertSqlDynamicFunc(il, propertys, formatstring, formatstringValue, movelength, true);
            return (Func<object, string>)dm.CreateDelegate(typeof(Func<object, string>));
        }

        #region CreateMethod
        static private string GetRealFieldName(PropertyInfo property)
        {
            return property.Name;
            //string reallfieldname = property.Name;
            //var colatrs = property.GetCustomAttributes(typeof(ColumnAttribute), true);
            //if (colatrs.Length > 0)
            //{
            //    var nameatr = (colatrs[0] as ColumnAttribute);
            //    if (string.IsNullOrEmpty(nameatr.Name) == false)
            //    {
            //        reallfieldname = nameatr.Name;
            //    }
            //}
            //return reallfieldname;
        }
        /// <summary>
        /// 有值的组成一条需要的Sql
        /// </summary>
        /// <param name="il">The il.</param>
        /// <param name="propertys">属性集合</param>
        /// <param name="formatstring">The formatstring.</param>
        /// <param name="movelength">The movelength.</param>
        /// <param name="checkignore">是否检查忽略属性</param>
        static public void CreateSqlDynamicFunc(ILGenerator il, IEnumerable<PropertyInfo> propertys, string formatstring, int movelength, bool checkignore = false)
        {
            var sb = il.DeclareLocal(typeof(StringBuilder));
            il.Emit(OpCodes.Newobj, typeof(StringBuilder).GetConstructor(new Type[] { }));
            il.Emit(OpCodes.Stloc_0);

            foreach (var property in propertys)
            {
                if (checkignore) if (property.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0) { continue; }
                var reallfieldname = GetRealFieldName(property);
                Label isnull = il.DefineLabel();
                var typeCode = Type.GetTypeCode(property.PropertyType);

                if (property.PropertyType.IsValueType)
                {
                    #region ValueType
                    var basetype = Nullable.GetUnderlyingType(property.PropertyType);
                    if (basetype != null)
                    {
                        var nulltype = typeof(Nullable<>).MakeGenericType(basetype);
                        il.Emit(OpCodes.Ldarg_0);
                        il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                        il.Emit(OpCodes.Box, nulltype);
                        il.Emit(OpCodes.Ldnull);
                        il.Emit(OpCodes.Ceq);
                        //il.EmitCall(OpCodes.Callvirt, nulltype.GetProperty("HasValue").GetGetMethod(), null);
                        il.Emit(OpCodes.Brtrue, isnull);
                    }
                    else
                    {
                        switch (typeCode)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                            case TypeCode.UInt16:
                            case TypeCode.Int16:
                            case TypeCode.UInt32:
                            case TypeCode.Int32:
                            case TypeCode.UInt64:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:

                                break;
                            case TypeCode.DateTime:
                                il.Emit(OpCodes.Ldarg_0);
                                il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                                il.Emit(OpCodes.Ldsfld, dateTimeMinValueMI);
                                il.EmitCall(OpCodes.Call, dateTimeEqualsMI, null);
                                il.Emit(OpCodes.Brtrue, isnull);
                                break;
                            default:
                                break;
                        }
                    }
                    il.Emit(OpCodes.Nop);
                    AppendFormatStringBuilder(il, sb, formatstring, reallfieldname);
                    il.Emit(OpCodes.Nop);
                    il.MarkLabel(isnull);
                    #endregion
                }
                else if (property.PropertyType == typeof(string))
                {
                    #region StringType
                    il.Emit(OpCodes.Ldarg_0);
                    il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Ceq);
                    il.Emit(OpCodes.Brtrue, isnull);
                    il.Emit(OpCodes.Nop);
                    AppendFormatStringBuilder(il, sb, formatstring, reallfieldname);
                    il.Emit(OpCodes.Nop);
                    il.MarkLabel(isnull);
                    #endregion
                }
                else
                {
                    continue;
                }
            }
            if (movelength > 0)
            {
                Label lengthMinnerThanZero = il.DefineLabel();//lable
                il.Emit(OpCodes.Ldloc, sb);
                il.EmitCall(OpCodes.Callvirt, sbLengthMI, null);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Cgt);
                il.Emit(OpCodes.Brfalse, lengthMinnerThanZero);
                il.Emit(OpCodes.Nop);
                RemoveStringBuilder(il, sb, movelength);
                il.Emit(OpCodes.Nop);
                il.MarkLabel(lengthMinnerThanZero);
            }
            il.Emit(OpCodes.Ldloc, sb);
            il.EmitCall(OpCodes.Callvirt, sbToStringMI, null);
            il.Emit(OpCodes.Ret);
        }
        /// <summary>
        /// Creates the SQL dynamic function.
        /// </summary>
        /// <param name="il">The il.</param>
        /// <param name="propertys">属性集合</param>
        /// <param name="formatstring">The formatstring.</param>
        /// <param name="formatstringValue">The formatstring value.</param>
        /// <param name="movelength">The movelength.</param>
        /// <param name="checkignore">是否检查忽略属性</param>
        static public void CreateInsertSqlDynamicFunc(ILGenerator il, IEnumerable<PropertyInfo> propertys, string formatstring, string formatstringValue, int movelength, bool checkignore = false)
        {
            var sb = il.DeclareLocal(typeof(StringBuilder));
            var sbvalue = il.DeclareLocal(typeof(StringBuilder));
            il.Emit(OpCodes.Newobj, typeof(StringBuilder).GetConstructor(new Type[] { }));
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Newobj, typeof(StringBuilder).GetConstructor(new Type[] { }));
            il.Emit(OpCodes.Stloc_1);

            foreach (var property in propertys)
            {
                if (checkignore) if (property.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0) { continue; }
                var reallfieldname = GetRealFieldName(property);
                Label isnull = il.DefineLabel();
                var typeCode = Type.GetTypeCode(property.PropertyType);

                if (property.PropertyType.IsValueType)
                {
                    #region ValueType
                    var basetype = Nullable.GetUnderlyingType(property.PropertyType);
                    if (basetype != null)
                    {
                        var nulltype = typeof(Nullable<>).MakeGenericType(basetype);
                        il.Emit(OpCodes.Ldarg_0);
                        il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                        il.Emit(OpCodes.Box, nulltype);
                        il.Emit(OpCodes.Ldnull);
                        il.Emit(OpCodes.Ceq);
                        //il.EmitCall(OpCodes.Callvirt, nulltype.GetProperty("HasValue").GetGetMethod(), null);
                        il.Emit(OpCodes.Brtrue, isnull);
                    }
                    else
                    {
                        switch (typeCode)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                            case TypeCode.UInt16:
                            case TypeCode.Int16:
                            case TypeCode.UInt32:
                            case TypeCode.Int32:
                            case TypeCode.UInt64:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:

                                break;
                            case TypeCode.DateTime:
                                il.Emit(OpCodes.Ldarg_0);
                                il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                                il.Emit(OpCodes.Ldsfld, dateTimeMinValueMI);
                                il.EmitCall(OpCodes.Call, dateTimeEqualsMI, null);
                                il.Emit(OpCodes.Brtrue, isnull);
                                break;
                            default:
                                break;
                        }
                    }
                    il.Emit(OpCodes.Nop);
                    AppendFormatStringBuilder(il, sb, formatstring, reallfieldname);
                    AppendFormatStringBuilder(il, sbvalue, formatstringValue, property.Name);
                    il.Emit(OpCodes.Nop);
                    il.MarkLabel(isnull);
                    #endregion
                }
                else if (property.PropertyType == typeof(string))
                {
                    #region StringType
                    il.Emit(OpCodes.Ldarg_0);
                    il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Ceq);
                    il.Emit(OpCodes.Brtrue, isnull);
                    il.Emit(OpCodes.Nop);
                    AppendFormatStringBuilder(il, sb, formatstring, property.Name);
                    AppendFormatStringBuilder(il, sbvalue, formatstringValue, reallfieldname);
                    il.Emit(OpCodes.Nop);
                    il.MarkLabel(isnull);
                    #endregion
                }
                else
                {
                    continue;
                }
            }
            if (movelength > 0)
            {
                Label lengthMinnerThanZero = il.DefineLabel();//lable
                il.Emit(OpCodes.Ldloc_0);
                il.EmitCall(OpCodes.Callvirt, sbLengthMI, null);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Cgt);
                il.Emit(OpCodes.Brfalse, lengthMinnerThanZero);
                il.Emit(OpCodes.Nop);
                RemoveStringBuilder(il, sb, movelength);
                RemoveStringBuilder(il, sbvalue, movelength);
                il.Emit(OpCodes.Nop);
                il.MarkLabel(lengthMinnerThanZero);
            }
            il.Emit(OpCodes.Ldstr, "({0}) values ({1})");
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldloc_1);
            il.EmitCall(OpCodes.Call, typeof(System.String).GetMethod("Format", new Type[] { typeof(string), typeof(object), typeof(object) }), null);
            il.Emit(OpCodes.Ret);
        }
        /// <summary>
        /// 有值的组成Sql同时将值保存在SqlDataParamter
        /// </summary>
        /// <param name="il">The il.</param>
        /// <param name="propertys">属性集合</param>
        /// <param name="formatstring">The formatstring.</param>
        /// <param name="movelength">The movelength.</param>
        /// <param name="checkignore">是否检查忽略属性</param>
        static public void CreateUpdateSqlDynamicFunc(ILGenerator il, IEnumerable<PropertyInfo> propertys, string formatstring, int movelength, bool checkignore = false)
        {
            var sb = il.DeclareLocal(typeof(StringBuilder));
            il.Emit(OpCodes.Newobj, typeof(StringBuilder).GetConstructor(new Type[] { }));
            il.Emit(OpCodes.Stloc_0);

            foreach (var property in propertys)
            {
                if (checkignore) if (property.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0) { continue; }
                var reallfieldname = GetRealFieldName(property);
                Label isnull = il.DefineLabel();
                var typeCode = Type.GetTypeCode(property.PropertyType);
                

                if (property.PropertyType.IsValueType)
                {
                    #region ValueType
                    var basetype = Nullable.GetUnderlyingType(property.PropertyType);
                    if (basetype != null)
                    {
                        var nulltype = typeof(Nullable<>).MakeGenericType(basetype);
                        il.Emit(OpCodes.Ldarg_0);
                        il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                        il.Emit(OpCodes.Box, nulltype);
                        il.Emit(OpCodes.Ldnull);
                        il.Emit(OpCodes.Ceq);
                        //il.EmitCall(OpCodes.Callvirt, nulltype.GetProperty("HasValue").GetGetMethod(), null);
                        il.Emit(OpCodes.Brtrue, isnull);
                    }
                    else
                    {
                        switch (typeCode)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                            case TypeCode.UInt16:
                            case TypeCode.Int16:
                            case TypeCode.UInt32:
                            case TypeCode.Int32:
                            case TypeCode.UInt64:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                break;
                            case TypeCode.DateTime:
                                il.Emit(OpCodes.Ldarg_0);
                                il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                                il.Emit(OpCodes.Ldsfld, dateTimeMinValueMI);
                                il.EmitCall(OpCodes.Call, dateTimeEqualsMI, null);
                                il.Emit(OpCodes.Brtrue, isnull);
                                break;
                            default:
                                break;
                        }
                    }
                    il.Emit(OpCodes.Nop);
                    var valuekey=SqlDataParamterAdd(il, OpCodes.Ldarg_0, OpCodes.Ldarg_1, property);
                    AppendFormatStringBuilder2(il, sb, formatstring, reallfieldname,valuekey);
                    il.Emit(OpCodes.Nop);
                    il.MarkLabel(isnull);
                    #endregion
                }
                else if (property.PropertyType == typeof(string))
                {
                    #region StringType
                    il.Emit(OpCodes.Ldarg_0);
                    il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Ceq);
                    il.Emit(OpCodes.Brtrue, isnull);
                    il.Emit(OpCodes.Nop);
                    
                    var valuekey=SqlDataParamterAdd(il, OpCodes.Ldarg_0, OpCodes.Ldarg_1, property);
                    AppendFormatStringBuilder2(il, sb, formatstring, reallfieldname,valuekey);
                    il.Emit(OpCodes.Nop);
                    il.MarkLabel(isnull);
                    #endregion
                }
                else
                {
                    continue;
                }
            }
            if (movelength > 0)
            {
                RemoveStringBuilder(il, sb, movelength);
            }
            il.Emit(OpCodes.Ldloc_0);
            il.EmitCall(OpCodes.Callvirt, sbToStringMI, null);
            il.Emit(OpCodes.Ret);
        }
        #endregion

        #region CreateMethodHelp
        /// <summary>
        /// 移除StringBuilder从0开始字符串
        /// </summary>
        /// <param name="il"></param>
        /// <param name="sb"></param>
        /// <param name="movelength"></param>
        private static void RemoveStringBuilder(ILGenerator il, LocalBuilder sb, int movelength)
        {
            il.Emit(OpCodes.Ldloc, sb);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ldc_I4, movelength);
            il.EmitCall(OpCodes.Call, sbRemoveMI, null);
            il.Emit(OpCodes.Pop);
        }
        /// <summary>
        /// StringBuilder添加Format字段
        /// </summary>
        /// <param name="il"></param>
        /// <param name="sb"></param>
        /// <param name="formatstring"></param>
        /// <param name="name"></param>
        private static void AppendFormatStringBuilder(ILGenerator il, LocalBuilder sb, string formatstring, string name)
        {
            il.Emit(OpCodes.Ldloc, sb);
            il.Emit(OpCodes.Ldstr, formatstring);
            il.Emit(OpCodes.Ldstr, name);
            il.EmitCall(OpCodes.Call, appendFormatMI, null);
            il.Emit(OpCodes.Pop);
        }
        private static void AppendFormatStringBuilder2(ILGenerator il, LocalBuilder sb, string formatstring, string name,LocalBuilder key)
        {
            il.Emit(OpCodes.Ldloc, sb);
            il.Emit(OpCodes.Ldstr, formatstring);
            il.Emit(OpCodes.Ldstr, name);
            il.Emit(OpCodes.Ldloc,key);
            //il.EmitWriteLine(key);
            il.EmitCall(OpCodes.Call, appendFormatMI2, null);
            il.Emit(OpCodes.Pop);
        }
        /// <summary>
        /// SqlDataParamter添加值
        /// </summary>
        /// <param name="il"></param>
        /// <param name="model"></param>
        /// <param name="paramter"></param>
        /// <param name="property"></param>
        private static LocalBuilder SqlDataParamterAdd(ILGenerator il, OpCode model, OpCode paramter, PropertyInfo property)
        {
            var key = il.DeclareLocal(typeof(string));
            il.Emit(paramter);
            il.Emit(OpCodes.Ldstr, property.Name);
            il.Emit(model);
            il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
            il.Emit(OpCodes.Box, property.PropertyType);
            il.EmitCall(OpCodes.Call, addSqlDataParamterMI, null);
            il.Emit(OpCodes.Stloc,key);
            return key;
        }
        ///// <summary>
        ///// 获取字段对应的数据库名称
        ///// </summary>
        ///// <param name="property"></param>
        ///// <returns></returns>
        //private static string GetRealFieldName(PropertyInfo property)
        //{
        //    var attr = property.GetCustomAttribute(typeof(ColumnAttribute));
        //    if (attr != null)
        //    {
        //        var col = attr as ColumnAttribute;
        //        if (string.IsNullOrEmpty(col.Name))
        //        {
        //            return property.Name;
        //        }
        //        else
        //        {
        //            return col.Name;
        //        }
        //    }
        //    return property.Name;
        //}
        #endregion
        #endregion
        #endregion
    }
}

using ProtocolLib.CommonLib.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AM.DBService.Services.Plc.Driver
{
    /// <summary>
    /// PLC 协议程序集注册表。
    /// 负责从目录扫描协议库 DLL，发现 IProtocol 实现，并按协议名注册到字典。
    /// </summary>
    public static class ProtocolAssemblyRegistry
    {
        private static readonly object SyncRoot = new object();
        private static readonly Dictionary<string, Type> ProtocolTypeMap =
            new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        private static readonly HashSet<string> LoadedAssemblyPaths =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private static bool _initialized;

        /// <summary>
        /// 尝试按协议类型名解析协议实现类型。
        /// </summary>
        public static bool TryResolve(string protocolType, out Type protocolImplType)
        {
            protocolImplType = null;

            EnsureInitialized();

            string key = NormalizeKey(protocolType);
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            lock (SyncRoot)
            {
                return ProtocolTypeMap.TryGetValue(key, out protocolImplType);
            }
        }

        /// <summary>
        /// 获取当前已注册协议键。
        /// </summary>
        public static IList<string> GetRegisteredKeys()
        {
            EnsureInitialized();

            lock (SyncRoot)
            {
                return ProtocolTypeMap.Keys.OrderBy(p => p).ToList();
            }
        }

        /// <summary>
        /// 强制重新扫描协议目录。
        /// </summary>
        public static void Reload()
        {
            lock (SyncRoot)
            {
                ProtocolTypeMap.Clear();
                LoadedAssemblyPaths.Clear();
                _initialized = false;
            }

            EnsureInitialized();
        }

        private static void EnsureInitialized()
        {
            if (_initialized)
            {
                return;
            }

            lock (SyncRoot)
            {
                if (_initialized)
                {
                    return;
                }

                foreach (var directory in GetProbeDirectories())
                {
                    ScanDirectory(directory);
                }

                _initialized = true;
            }
        }

        private static IEnumerable<string> GetProbeDirectories()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrWhiteSpace(baseDirectory))
            {
                yield return baseDirectory;

                string protocolsDirectory = Path.Combine(baseDirectory, "Protocols");
                if (Directory.Exists(protocolsDirectory))
                {
                    yield return protocolsDirectory;
                }
            }
        }

        private static void ScanDirectory(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                return;
            }

            var dllFiles = Directory.GetFiles(directory, "ProtocolLib.*.dll", SearchOption.TopDirectoryOnly);
            foreach (var dllFile in dllFiles)
            {
                LoadProtocolAssembly(dllFile);
            }
        }

        private static void LoadProtocolAssembly(string assemblyPath)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath) || !File.Exists(assemblyPath))
            {
                return;
            }

            if (IsCommonAssembly(assemblyPath))
            {
                return;
            }

            if (LoadedAssemblyPaths.Contains(assemblyPath))
            {
                return;
            }

            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            catch
            {
                return;
            }

            LoadedAssemblyPaths.Add(assemblyPath);
            RegisterAssemblyProtocols(assembly);
        }

        private static void RegisterAssemblyProtocols(Assembly assembly)
        {
            if (assembly == null)
            {
                return;
            }

            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types == null
                    ? new Type[0]
                    : ex.Types.Where(p => p != null).ToArray();
            }

            foreach (var type in types)
            {
                if (type == null || !type.IsClass || type.IsAbstract)
                {
                    continue;
                }

                if (!typeof(IProtocol).IsAssignableFrom(type))
                {
                    continue;
                }

                RegisterProtocolType(type, assembly);
            }
        }

        private static void RegisterProtocolType(Type type, Assembly assembly)
        {
            var keys = new List<string>();

            string protocolName = GetStaticProtocolName(type);
            if (!string.IsNullOrWhiteSpace(protocolName))
            {
                keys.Add(protocolName);
            }

            if (assembly != null)
            {
                string assemblyName = assembly.GetName().Name;
                if (!string.IsNullOrWhiteSpace(assemblyName))
                {
                    keys.Add(assemblyName);
                    if (assemblyName.StartsWith("ProtocolLib.", StringComparison.OrdinalIgnoreCase))
                    {
                        keys.Add(assemblyName.Substring("ProtocolLib.".Length));
                    }
                }
            }

            keys.Add(type.Name);

            foreach (var rawKey in keys)
            {
                string key = NormalizeKey(rawKey);
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                if (!ProtocolTypeMap.ContainsKey(key))
                {
                    ProtocolTypeMap[key] = type;
                }
            }
        }

        private static string GetStaticProtocolName(Type type)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

            var field = type.GetField("ProtocolName", flags);
            if (field != null && field.FieldType == typeof(string))
            {
                return field.GetValue(null) as string;
            }

            var property = type.GetProperty("ProtocolName", flags);
            if (property != null && property.PropertyType == typeof(string) && property.GetIndexParameters().Length == 0)
            {
                return property.GetValue(null, null) as string;
            }

            return null;
        }

        private static bool IsCommonAssembly(string assemblyPath)
        {
            string fileName = Path.GetFileName(assemblyPath);
            return string.Equals(fileName, "ProtocolLib.CommonLib.dll", StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeKey(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var chars = value
                .Trim()
                .Where(ch => char.IsLetterOrDigit(ch))
                .Select(char.ToLowerInvariant)
                .ToArray();

            return new string(chars);
        }
    }
}
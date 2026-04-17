using AM.Core.Context;
using AM.DBService.Services.System;
using AM.Model.Common;
using AM.Model.License;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.Tests
{
    internal static class Program
    {
        private const string ValidLicenseJson = "{\"licenseId\":\"LIC-LOCAL-001\",\"authorization\":{\"moduleKeys\":[\"Motion\"],\"pageKeys\":[\"Motion.DI\",\"Motion.Monitor\"]},\"validity\":{},\"signature\":{}}";

        private static int Main(string[] args)
        {
            List<ScenarioCase> scenarios = CreateScenarios();
            string[] normalizedArgs = NormalizeArgs(args);

            if (normalizedArgs.Any(IsHelpArgument))
            {
                PrintUsage(scenarios);
                return 0;
            }

            if (normalizedArgs.Any(IsListArgument))
            {
                PrintScenarioList(scenarios);
                return 0;
            }

            List<ScenarioCase> selectedScenarios = FilterScenarios(scenarios, normalizedArgs);
            if (selectedScenarios.Count <= 0)
            {
                Console.WriteLine("未匹配到任何测试场景。可使用 --list 查看可用组和场景。");
                return 2;
            }

            int passedCount = 0;

            for (int index = 0; index < selectedScenarios.Count; index++)
            {
                ScenarioCase scenario = selectedScenarios[index];

                ConfigContext.Instance.Initialize(new Config());
                LicenseRuntimeContext.Instance.Reset();

                Result<DeviceLicenseState> result = scenario.Loader.Load();
                DeviceLicenseState state = LicenseRuntimeContext.Instance.Current;
                bool passed = result != null && scenario.Assert(state);

                Console.WriteLine("[{0}] {1}", passed ? "PASS" : "FAIL", scenario.DisplayName);
                Console.WriteLine("  Message: {0}", state == null ? string.Empty : state.Message);
                Console.WriteLine("  ErrorCode: {0}", state == null || state.ValidationResult == null ? string.Empty : state.ValidationResult.ErrorCode);

                if (passed)
                {
                    passedCount++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Summary: {0}/{1} scenarios passed.", passedCount, selectedScenarios.Count);
            return passedCount == selectedScenarios.Count ? 0 : 1;
        }

        private static List<ScenarioCase> CreateScenarios()
        {
            return new List<ScenarioCase>
            {
                new ScenarioCase(
                    "license",
                    "license-file",
                    "missing",
                    "license.lic 缺失",
                    new LicenseRuntimeLoader(
                        new StubLicenseFileService(Result<string>.Fail(-1, "missing")),
                        new StubLicenseCryptoService(Result<string>.OkItem(ValidLicenseJson)),
                        new StubLicenseValidator(Result<LicenseValidationResult>.OkItem(new LicenseValidationResult { Success = true })),
                        null),
                    state => !state.IsValid && !state.HasLicense && string.Equals(state.ValidationResult.ErrorCode, "LICENSE_FILE_MISSING", StringComparison.OrdinalIgnoreCase)),

                new ScenarioCase(
                    "license",
                    "license-file",
                    "corrupted",
                    "license.lic 损坏",
                    new LicenseRuntimeLoader(
                        new StubLicenseFileService(Result<string>.OkItem("###broken###")),
                        new StubLicenseCryptoService(Result<string>.Fail(-2, "decode failed")),
                        new StubLicenseValidator(Result<LicenseValidationResult>.OkItem(new LicenseValidationResult { Success = true })),
                        null),
                    state => !state.IsValid && string.Equals(state.ValidationResult.ErrorCode, "LICENSE_DECODE_FAILED", StringComparison.OrdinalIgnoreCase)),

                new ScenarioCase(
                    "license",
                    "license-time",
                    "expired",
                    "授权已过期",
                    CreateLoader(new LicenseValidationResult
                    {
                        Success = false,
                        HasLicenseFile = true,
                        IsSignatureValid = true,
                        IsHardwareMatched = true,
                        IsExpired = true,
                        IsInGracePeriod = false,
                        ErrorCode = "LICENSE_EXPIRED",
                        Message = "授权已过期且不在宽限期内",
                        ExpiresAt = DateTime.Now.AddDays(-1)
                    }),
                    state => !state.IsValid && state.IsExpired && !state.IsInGracePeriod && string.Equals(state.ValidationResult.ErrorCode, "LICENSE_EXPIRED", StringComparison.OrdinalIgnoreCase)),

                new ScenarioCase(
                    "license",
                    "license-time",
                    "grace",
                    "授权处于宽限期",
                    CreateLoader(new LicenseValidationResult
                    {
                        Success = true,
                        HasLicenseFile = true,
                        IsSignatureValid = true,
                        IsHardwareMatched = true,
                        IsExpired = true,
                        IsInGracePeriod = true,
                        Message = "授权已过期，当前处于宽限期",
                        ExpiresAt = DateTime.Now.AddDays(-1)
                    }),
                    state => state.IsValid && state.IsExpired && state.IsInGracePeriod && state.PageKeys.Count == 2),

                new ScenarioCase(
                    "license",
                    "license-hardware",
                    "hardware-mismatch",
                    "硬件强绑定不匹配",
                    CreateLoader(new LicenseValidationResult
                    {
                        Success = false,
                        HasLicenseFile = true,
                        IsSignatureValid = true,
                        IsHardwareMatched = false,
                        ErrorCode = "LICENSE_HARDWARE_MISMATCH",
                        Message = "硬件强绑定字段不匹配: CpuId"
                    }),
                    state => !state.IsValid && string.Equals(state.ValidationResult.ErrorCode, "LICENSE_HARDWARE_MISMATCH", StringComparison.OrdinalIgnoreCase))
            };
        }

        private static string[] NormalizeArgs(string[] args)
        {
            return args == null
                ? new string[0]
                : args
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .ToArray();
        }

        private static List<ScenarioCase> FilterScenarios(List<ScenarioCase> scenarios, string[] args)
        {
            if (args == null || args.Length <= 0)
            {
                return scenarios;
            }

            HashSet<string> filters = new HashSet<string>(args, StringComparer.OrdinalIgnoreCase);
            if (filters.Contains("all"))
            {
                return scenarios;
            }

            return scenarios
                .Where(x => filters.Contains(x.FamilyKey) || filters.Contains(x.GroupKey) || filters.Contains(x.ScenarioKey))
                .ToList();
        }

        private static bool IsHelpArgument(string arg)
        {
            return string.Equals(arg, "-h", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(arg, "--help", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(arg, "/?", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(arg, "help", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsListArgument(string arg)
        {
            return string.Equals(arg, "-l", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(arg, "--list", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(arg, "list", StringComparison.OrdinalIgnoreCase);
        }

        private static void PrintUsage(List<ScenarioCase> scenarios)
        {
            Console.WriteLine("AM.Tests 本地验证入口");
            Console.WriteLine();
            Console.WriteLine("用法:");
            Console.WriteLine("  dotnet run --project .\\AM.Tests\\AM.Tests.csproj");
            Console.WriteLine("  dotnet run --project .\\AM.Tests\\AM.Tests.csproj -- --list");
            Console.WriteLine("  dotnet run --project .\\AM.Tests\\AM.Tests.csproj -- license");
            Console.WriteLine("  dotnet run --project .\\AM.Tests\\AM.Tests.csproj -- license-time");
            Console.WriteLine("  dotnet run --project .\\AM.Tests\\AM.Tests.csproj -- expired");
            Console.WriteLine();
            PrintScenarioList(scenarios);
        }

        private static void PrintScenarioList(List<ScenarioCase> scenarios)
        {
            Console.WriteLine("可用场景组:");
            foreach (string family in scenarios.Select(x => x.FamilyKey).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine("  {0}", family);
            }

            foreach (string group in scenarios.Select(x => x.GroupKey).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine("  {0}", group);
            }

            Console.WriteLine();
            Console.WriteLine("可用单场景:");
            foreach (ScenarioCase scenario in scenarios)
            {
                Console.WriteLine("  {0,-18} {1}", scenario.ScenarioKey, scenario.DisplayName);
            }
        }

        private static LicenseRuntimeLoader CreateLoader(LicenseValidationResult validationResult)
        {
            return new LicenseRuntimeLoader(
                new StubLicenseFileService(Result<string>.OkItem(ValidLicenseJson)),
                new StubLicenseCryptoService(Result<string>.OkItem(ValidLicenseJson)),
                new StubLicenseValidator(Result<LicenseValidationResult>.OkItem(validationResult)),
                null);
        }

        private sealed class ScenarioCase
        {
            public ScenarioCase(string familyKey, string groupKey, string scenarioKey, string displayName, LicenseRuntimeLoader loader, Func<DeviceLicenseState, bool> assert)
            {
                FamilyKey = familyKey;
                GroupKey = groupKey;
                ScenarioKey = scenarioKey;
                DisplayName = displayName;
                Loader = loader;
                Assert = assert;
            }

            public string FamilyKey { get; private set; }

            public string GroupKey { get; private set; }

            public string ScenarioKey { get; private set; }

            public string DisplayName { get; private set; }

            public LicenseRuntimeLoader Loader { get; private set; }

            public Func<DeviceLicenseState, bool> Assert { get; private set; }
        }

        private sealed class StubLicenseFileService : LicenseFileService
        {
            private readonly Result<string> _readResult;

            public StubLicenseFileService(Result<string> readResult)
                : base(null)
            {
                _readResult = readResult;
            }

            public override Result<string> ReadLicenseText()
            {
                return _readResult;
            }
        }

        private sealed class StubLicenseCryptoService : LicenseCryptoService
        {
            private readonly Result<string> _decodeResult;

            public StubLicenseCryptoService(Result<string> decodeResult)
                : base(null)
            {
                _decodeResult = decodeResult;
            }

            public override Result<string> DecodeLicenseText(string licenseText)
            {
                return _decodeResult;
            }
        }

        private sealed class StubLicenseValidator : LicenseValidator
        {
            private readonly Result<LicenseValidationResult> _validationResult;

            public StubLicenseValidator(Result<LicenseValidationResult> validationResult)
                : base(true)
            {
                _validationResult = validationResult;
            }

            public override Result<LicenseValidationResult> Validate(DeviceLicense license, string licenseJson)
            {
                return _validationResult;
            }
        }
    }
}
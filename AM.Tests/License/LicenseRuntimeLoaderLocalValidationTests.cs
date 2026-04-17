using AM.Core.Context;
using AM.DBService.Services.System;
using AM.Model.Common;
using AM.Model.License;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AM.Tests.License
{
    [TestFixture]
    public class LicenseRuntimeLoaderLocalValidationTests
    {
        private const string ValidLicenseJson = "{\"licenseId\":\"LIC-LOCAL-001\",\"authorization\":{\"moduleKeys\":[\"Motion\"],\"pageKeys\":[\"Motion.DI\",\"Motion.Monitor\"]},\"validity\":{},\"signature\":{}}";

        [SetUp]
        public void SetUp()
        {
            ConfigContext.Instance.Initialize(new Config());
            LicenseRuntimeContext.Instance.Reset();
        }

        [Test]
        public void Load_WhenLicenseFileMissing_ShouldFallbackToInvalidState()
        {
            var loader = new LicenseRuntimeLoader(
                new StubLicenseFileService(Result<string>.Fail(-1, "missing")),
                new StubLicenseCryptoService(),
                new StubLicenseValidator(),
                null);

            Result<DeviceLicenseState> result = loader.Load();

            Assert.That(result.Success, Is.True);
            Assert.That(LicenseRuntimeContext.Instance.Current.IsValid, Is.False);
            Assert.That(LicenseRuntimeContext.Instance.Current.HasLicense, Is.False);
            Assert.That(LicenseRuntimeContext.Instance.Current.ValidationResult.ErrorCode, Is.EqualTo("LICENSE_FILE_MISSING"));
        }

        [Test]
        public void Load_WhenLicenseFileCorrupted_ShouldFallbackToDecodeFailedState()
        {
            var loader = new LicenseRuntimeLoader(
                new StubLicenseFileService(Result<string>.OkItem("###broken###")),
                new StubLicenseCryptoService(Result<string>.Fail(-2, "decode failed")),
                new StubLicenseValidator(),
                null);

            Result<DeviceLicenseState> result = loader.Load();

            Assert.That(result.Success, Is.True);
            Assert.That(LicenseRuntimeContext.Instance.Current.IsValid, Is.False);
            Assert.That(LicenseRuntimeContext.Instance.Current.ValidationResult.ErrorCode, Is.EqualTo("LICENSE_DECODE_FAILED"));
        }

        [Test]
        public void Load_WhenLicenseExpired_ShouldReturnExpiredInvalidState()
        {
            var loader = CreateLoaderWithValidation(new LicenseValidationResult
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
            });

            Result<DeviceLicenseState> result = loader.Load();

            Assert.That(result.Success, Is.True);
            Assert.That(result.Item.IsValid, Is.False);
            Assert.That(result.Item.IsExpired, Is.True);
            Assert.That(result.Item.IsInGracePeriod, Is.False);
            Assert.That(result.Item.ValidationResult.ErrorCode, Is.EqualTo("LICENSE_EXPIRED"));
            Assert.That(result.Item.PageKeys, Is.Empty);
        }

        [Test]
        public void Load_WhenLicenseInGracePeriod_ShouldKeepAuthorizedPages()
        {
            var loader = CreateLoaderWithValidation(new LicenseValidationResult
            {
                Success = true,
                HasLicenseFile = true,
                IsSignatureValid = true,
                IsHardwareMatched = true,
                IsExpired = true,
                IsInGracePeriod = true,
                Message = "授权已过期，当前处于宽限期",
                ExpiresAt = DateTime.Now.AddDays(-1)
            });

            Result<DeviceLicenseState> result = loader.Load();

            Assert.That(result.Success, Is.True);
            Assert.That(result.Item.IsValid, Is.True);
            Assert.That(result.Item.IsExpired, Is.True);
            Assert.That(result.Item.IsInGracePeriod, Is.True);
            Assert.That(result.Item.PageKeys, Is.EquivalentTo(new[] { "Motion.DI", "Motion.Monitor" }));
        }

        [Test]
        public void Load_WhenHardwareMismatch_ShouldReturnHardwareMismatchState()
        {
            var loader = CreateLoaderWithValidation(new LicenseValidationResult
            {
                Success = false,
                HasLicenseFile = true,
                IsSignatureValid = true,
                IsHardwareMatched = false,
                IsExpired = false,
                IsInGracePeriod = false,
                ErrorCode = "LICENSE_HARDWARE_MISMATCH",
                Message = "硬件强绑定字段不匹配: CpuId"
            });

            Result<DeviceLicenseState> result = loader.Load();

            Assert.That(result.Success, Is.True);
            Assert.That(result.Item.IsValid, Is.False);
            Assert.That(result.Item.ValidationResult.ErrorCode, Is.EqualTo("LICENSE_HARDWARE_MISMATCH"));
            Assert.That(result.Item.PageKeys, Is.Empty);
        }

        private static LicenseRuntimeLoader CreateLoaderWithValidation(LicenseValidationResult validationResult)
        {
            return new LicenseRuntimeLoader(
                new StubLicenseFileService(Result<string>.OkItem(ValidLicenseJson)),
                new StubLicenseCryptoService(Result<string>.OkItem(ValidLicenseJson)),
                new StubLicenseValidator(Result<LicenseValidationResult>.OkItem(validationResult)),
                null);
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

            public StubLicenseCryptoService()
                : this(Result<string>.OkItem(ValidLicenseJson))
            {
            }

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

            public StubLicenseValidator()
                : this(Result<LicenseValidationResult>.Fail(-1, "unused"))
            {
            }

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
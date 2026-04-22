using AM.Core.Context;
using AM.DBService.Services.System;
using AM.Model.Common;
using AM.Model.License;
using NUnit.Framework;

namespace AM.Tests.License
{
    [TestFixture]
    public class LicenseValidatorDeveloperEditionTests
    {
        [SetUp]
        public void SetUp()
        {
            ConfigContext.Instance.Initialize(new Config
            {
                Setting = new Setting
                {
                    DesktopAppEdition = LicenseConstants.DeveloperAppEdition,
                    LicenseCustomerCode = "AM-10001",
                    LicenseSiteCode = "SZ01",
                    LicenseMachineModel = "AM-STD-01"
                }
            });
        }

        [Test]
        public void Validate_WhenDeveloperLicenseMatchesRuntimeScope_ShouldPassWithoutHardwareBinding()
        {
            var validator = new LicenseValidator(
                new StubHardwareInfoCollector(new DeviceHardwareInfo
                {
                    ClientId = "DIFFERENT-CLIENT",
                    MachineCode = "DIFFERENT-MACHINE",
                    CpuId = "DIFFERENT-CPU",
                    MachineModel = "WRONG-MODEL"
                }),
                new StubLicenseCryptoService(),
                null);

            var license = new DeviceLicense
            {
                Software = new DeviceLicenseSoftware
                {
                    AppEdition = LicenseConstants.DeveloperAppEdition,
                    MinAppVersion = "0.0.1",
                    MaxAppVersion = "0.0.1"
                },
                GrantScope = new DeviceLicenseGrantScope
                {
                    CustomerCode = "AM-10001",
                    SiteCode = "SZ01",
                    MachineModel = "AM-STD-01"
                },
                DeviceBinding = new DeviceLicenseBinding
                {
                    ClientId = "client-001",
                    MachineCode = "machine-001",
                    CpuId = "cpu-001"
                },
                Validity = new DeviceLicenseValidity()
            };

            Result<LicenseValidationResult> result = validator.Validate(license, "{}");

            Assert.That(result.Success, Is.True);
            Assert.That(result.Item, Is.Not.Null);
            Assert.That(result.Item.Success, Is.True);
            Assert.That(result.Item.IsHardwareMatched, Is.True);
            Assert.That(result.Item.ErrorCode, Is.Empty);
        }

        [Test]
        public void Validate_WhenDeveloperLicenseVersionFallsWithinConfiguredRange_ShouldPass()
        {
            var validator = new LicenseValidator(
                new StubHardwareInfoCollector(new DeviceHardwareInfo
                {
                    MachineModel = "WRONG-MODEL"
                }),
                new StubLicenseCryptoService(),
                null);

            var license = new DeviceLicense
            {
                Software = new DeviceLicenseSoftware
                {
                    AppEdition = LicenseConstants.DeveloperAppEdition,
                    MinAppVersion = "0.0.1",
                    MaxAppVersion = "0.0.1"
                },
                GrantScope = new DeviceLicenseGrantScope
                {
                    CustomerCode = "AM-10001",
                    SiteCode = "SZ01",
                    MachineModel = "AM-STD-01"
                },
                Validity = new DeviceLicenseValidity()
            };

            Result<LicenseValidationResult> result = validator.Validate(license, "{}");

            Assert.That(result.Success, Is.True);
            Assert.That(result.Item, Is.Not.Null);
            Assert.That(result.Item.Success, Is.True);
            Assert.That(result.Item.ErrorCode, Is.Empty);
        }

        [Test]
        public void Validate_WhenDeveloperLicenseScopeMismatches_ShouldFailWithDeveloperScopeCode()
        {
            var validator = new LicenseValidator(
                new StubHardwareInfoCollector(new DeviceHardwareInfo
                {
                    MachineModel = "AM-STD-01"
                }),
                new StubLicenseCryptoService(),
                null);

            var license = new DeviceLicense
            {
                Software = new DeviceLicenseSoftware
                {
                    AppEdition = LicenseConstants.DeveloperAppEdition,
                    MinAppVersion = "9.9.9",
                    MaxAppVersion = "10.0.0"
                },
                GrantScope = new DeviceLicenseGrantScope
                {
                    CustomerCode = "AM-10001",
                    SiteCode = "SZ01",
                    MachineModel = "AM-STD-01"
                },
                Validity = new DeviceLicenseValidity()
            };

            Result<LicenseValidationResult> result = validator.Validate(license, "{}");

            Assert.That(result.Success, Is.True);
            Assert.That(result.Item, Is.Not.Null);
            Assert.That(result.Item.Success, Is.False);
            Assert.That(result.Item.ErrorCode, Is.EqualTo("LICENSE_APP_VERSION_OUT_OF_RANGE"));
            Assert.That(result.Item.Message, Is.EqualTo("授权版本不在许可范围内: AppVersion"));
        }

        [Test]
        public void Validate_WhenDeveloperLicenseVersionFallsOutsideConfiguredRange_ShouldFailWithDeveloperScopeCode()
        {
            var validator = new LicenseValidator(
                new StubHardwareInfoCollector(new DeviceHardwareInfo
                {
                    MachineModel = "AM-STD-01"
                }),
                new StubLicenseCryptoService(),
                null);

            var license = new DeviceLicense
            {
                Software = new DeviceLicenseSoftware
                {
                    AppEdition = LicenseConstants.DeveloperAppEdition,
                    MinAppVersion = "0.0.3",
                    MaxAppVersion = "0.0.5"
                },
                GrantScope = new DeviceLicenseGrantScope
                {
                    CustomerCode = "CUS-10001",
                    SiteCode = "SZ01",
                    MachineModel = "AM-STD-01"
                },
                Validity = new DeviceLicenseValidity()
            };

            Result<LicenseValidationResult> result = validator.Validate(license, "{}");

            Assert.That(result.Success, Is.True);
            Assert.That(result.Item, Is.Not.Null);
            Assert.That(result.Item.Success, Is.False);
            Assert.That(result.Item.ErrorCode, Is.EqualTo("LICENSE_APP_VERSION_OUT_OF_RANGE"));
            Assert.That(result.Item.Message, Is.EqualTo("授权版本不在许可范围内: AppVersion"));
        }

        [Test]
        public void Validate_WhenNonDeveloperLicenseVersionFallsOutsideConfiguredRange_ShouldFailBeforeHardwareValidation()
        {
            ConfigContext.Instance.Initialize(new Config
            {
                Setting = new Setting
                {
                    DesktopAppEdition = "Professional",
                    LicenseCustomerCode = "AM-10001",
                    LicenseSiteCode = "SZ01",
                    LicenseMachineModel = "AM-STD-01"
                }
            });

            var validator = new LicenseValidator(
                new StubHardwareInfoCollector(new DeviceHardwareInfo
                {
                    ClientId = "client-002",
                    MachineCode = "machine-002",
                    CpuId = "cpu-002"
                }),
                new StubLicenseCryptoService(),
                null);

            var license = new DeviceLicense
            {
                Software = new DeviceLicenseSoftware
                {
                    AppEdition = "Professional",
                    MinAppVersion = "9.9.9",
                    MaxAppVersion = "10.0.0"
                },
                DeviceBinding = new DeviceLicenseBinding
                {
                    ClientId = "client-001",
                    MachineCode = "machine-001",
                    CpuId = "cpu-001"
                },
                Validity = new DeviceLicenseValidity()
            };

            Result<LicenseValidationResult> result = validator.Validate(license, "{}");

            Assert.That(result.Success, Is.True);
            Assert.That(result.Item, Is.Not.Null);
            Assert.That(result.Item.Success, Is.False);
            Assert.That(result.Item.ErrorCode, Is.EqualTo("LICENSE_APP_VERSION_OUT_OF_RANGE"));
            Assert.That(result.Item.Message, Is.EqualTo("授权版本不在许可范围内: AppVersion"));
        }

        [Test]
        public void Validate_WhenNonDeveloperLicenseHardwareMismatches_ShouldKeepOriginalHardwareValidationAfterVersionPasses()
        {
            ConfigContext.Instance.Initialize(new Config
            {
                Setting = new Setting
                {
                    DesktopAppEdition = "Professional",
                    LicenseCustomerCode = "AM-10001",
                    LicenseSiteCode = "SZ01",
                    LicenseMachineModel = "AM-STD-01"
                }
            });

            var validator = new LicenseValidator(
                new StubHardwareInfoCollector(new DeviceHardwareInfo
                {
                    ClientId = "client-002",
                    MachineCode = "machine-002",
                    CpuId = "cpu-002"
                }),
                new StubLicenseCryptoService(),
                null);

            var license = new DeviceLicense
            {
                Software = new DeviceLicenseSoftware
                {
                    AppEdition = "Professional",
                    MinAppVersion = "0.0.1",
                    MaxAppVersion = "0.0.5"
                },
                DeviceBinding = new DeviceLicenseBinding
                {
                    ClientId = "client-001",
                    MachineCode = "machine-001",
                    CpuId = "cpu-001"
                },
                Validity = new DeviceLicenseValidity()
            };

            Result<LicenseValidationResult> result = validator.Validate(license, "{}");

            Assert.That(result.Success, Is.True);
            Assert.That(result.Item, Is.Not.Null);
            Assert.That(result.Item.Success, Is.False);
            Assert.That(result.Item.ErrorCode, Is.EqualTo("LICENSE_HARDWARE_MISMATCH"));
        }

        private sealed class StubHardwareInfoCollector : HardwareInfoCollector
        {
            private readonly DeviceHardwareInfo _hardware;

            public StubHardwareInfoCollector(DeviceHardwareInfo hardware)
                : base(null, null)
            {
                _hardware = hardware;
            }

            public override Result<DeviceHardwareInfo> CollectCurrent()
            {
                return Result<DeviceHardwareInfo>.OkItem(_hardware ?? new DeviceHardwareInfo());
            }
        }

        private sealed class StubLicenseCryptoService : LicenseCryptoService
        {
            public StubLicenseCryptoService()
                : base(null)
            {
            }

            public override Result VerifyLicenseSignature(string licenseJson, DeviceLicense license)
            {
                return Result.Ok("ok");
            }
        }
    }
}
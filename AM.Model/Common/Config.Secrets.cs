using Newtonsoft.Json;

namespace AM.Model.Common
{
    /// <summary>
    /// 一般设置中的敏感默认值配置。
    /// 当前将 DeviceAppSecret、授权验签公钥和授权申请签名私钥收口到单独 partial 文件中，避免堆叠在 Config.cs。
    /// </summary>
    public partial class Setting
    {
        /// <summary>
        /// 内置设备接入共享应用密钥。
        /// 当前直接在代码中赋值，且不写入 config.json。
        /// </summary>
        public const string BuiltInDeviceAppSecret = "amcontrolwinf-device-root-secret-whatisyourcolofullife";

        /// <summary>
        /// 内置授权许可验签公钥 PEM。
        /// 当前直接在代码中赋值，且不从外部文件读取。
        /// </summary>
        public const string BuiltInLicenseValidationPublicKeyPem = @"-----BEGIN RSA PUBLIC KEY-----
    MIIBCgKCAQEAwBu3U6nZF9K85VtFJjJzrLUo7ac5jpILSYcvYs5EeeDCPCILYzNF
    +LLV4m7Ty73ulvqGmEKSfWybWpdCq2gxCRBqao9MODyB2XoEjhdm1wdtpaEwK8Py
    6CHdZFT7954m7op1z4+Sn1IKuFlOXfVklescPYOPcf/yI2Kyk2pcXo/apD94TSdk
    t6CdWBGvWXg+4LWJleumnV4Y4kQcg97dC422rGHiQAbNDDlhezuzDSMvW7WofoVS
    FNppcQnUsNL65iUjZMhnyffhiyKyH83O93PR1VSPUCJbkoQwcMjut5SZ2QoOUrfj
    XePstYH4xFmcF6YV2mymbbXRwjJmAWeOkQIDAQAB
    -----END RSA PUBLIC KEY-----";

        /// <summary>
        /// 内置授权申请签名私钥 PEM。
        /// 当前直接在代码中赋值，且不从外部文件读取。
        /// </summary>
        public const string BuiltInLicenseRequestSigningPrivateKeyPem = @"-----BEGIN RSA PRIVATE KEY-----
    MIIEpAIBAAKCAQEAmMnMJbrCAPg8Hq7g0ZZY6z/lRsmudlCDyPAYVgYnNgKUSJH3
    Dr/zycp1jlBK4KJoAt2c1bDkCVf95ubcRt1pQTFGgBBWithAWqK1X/M4Crl6It2M
    ZP3FNssd00j55wMc2U4Rw2CzA3adeC6+HCkDYbJY/RZyyanfYuGSkOLpH/p2Erhy
    r0x0l4yO9YYuepltVlOrEc4qQWkrF8IZKL6Ru6h+QvihtCCGg/kskyxu/uI2IEhn
    cD9az+GgUb0nRwo8Wc+MRi2stCfc1/7dXUS/nM68LDjKa0ns77+yd2BfTkIUtfw2
    dy39jze23+pF5Cj/EmNnsQxdpnjw7R/2RdUyAQIDAQABAoIBAQCILg0cb9otHRQQ
    0RQnGpeEjr3fmzE52Ues2HsIaZGXbTMXf6ox5lr3N9IRl3U1xOtp6na4bGLEBT6U
    CJ67NutypOXOjUFlnZu2bSG2NMV1oZ2/57IT7bBSxzV9NaUwBzE2aoQgnVbRNm7i
    SN8/oDqYYGs6oUCMiDFIJAALzYO18Jmpz/NIy5lQNzEVWPMboMsBkadEtjkJTgR3
    60sbsclo9Ykvc30GvqcWi95AMITKLxywUONoBr4Rsm5vmhqUgcYcjFjAtBfE1aU5
    6TwsIivYRT+arqMYBNJpqlzxa5E015r+N/V3wbPv6wUf5AHXOGOEzPXCuFUWiGD0
    7Npn/8nFAoGBAMNAxqfgDwoQSTdlrlG3iefodh+y9hTrZoIU9FYv7wckRSAGeh9q
    wq5wKsdRljdjTU4LVl37k6aizMeOZiQ2bk8t2X1xcbHzzd2ovuTitKmlcHJHdZzq
    s8wfqptD0NKFiPzrrGpogoEwhyYsYnmFy8Zl6VNE7NNrjo9f1KfluZFrAoGBAMhS
    2egd86BEuXOQcqc+YNZaHnSi68egQ6no339w0cxGN/DAJyK26kRo3jex0Aun9WGI
    2ES/hS9SCZ9SAht6Q07PWKowZrE6u8NmNtmfrW1kfopn07SY1e2Pi5+vUp7XyWs1
    jcXKnKgPwVC3RukyPl+d12DRDcK6BYrMq+xWbilDAoGBAKB3iwyK+zE1I0cw/OvR
    4LfEd9gjd1TIBi4gVJLEwDhpWZoxWIqbbjZ4nt/CsKcCqJTcgnWo/bb0k0HqSQ23
    4j6Wmukao+sxSN7EAWcQ3pOSEPEKw6FbzrqSx97lBCkQb/4VHlUxYRgVruzzi1b+
    W/PI69JwkgZLnhU9GAOIrFLTAoGAcHqEu95FcxHjh03t8pYFzZWgUCFCfj4wf/Cv
    vWDdi/NJabaawtUtyOeRDniatlDCaNdrh356C83mdTyYzlDiBhhKUpUGtDCkSNzV
    o3AS8r9ghdoyds7yH4dgAHNy0pmbEMVrK0nOmFbwVp/yAvIhL5Ly4fu/3DyS7BJr
    3jHRfP0CgYAfsCjJ5I8O5nqReK9CpPC/SJ+qz0M1rqIhBRkCKXzfzeyH7PUOBPn1
    nXv8hVHpwPYO6qtEvCXpkd5QDBJsrzlnqxk8JuSFlfklHMScorBC6hozB9BFsOWW
    fgGzi+0iKSgGhQXb4Z1fQgbS1pZX5OfCtEqbeX0x6wrkIkYt4GvEhA==
    -----END RSA PRIVATE KEY-----";

        /// <summary>
        /// 设备接入共享应用密钥。
        /// 当前用于按 `HKDF-SHA256(appSecret, appCode, deviceId)` 派生设备请求密钥，再对 register、heartbeat、report 做 AES-GCM 加密。
        /// 该值当前直接使用代码默认值，不写入 config.json。
        /// </summary>
        [JsonIgnore]
        public string DeviceAppSecret { get; set; } = BuiltInDeviceAppSecret;

        /// <summary>
        /// 授权许可验签公钥 PEM。
        /// 当前直接从代码默认值读取，不写入 config.json，也不从外部文件读取。
        /// </summary>
        [JsonIgnore]
        public string LicenseValidationPublicKeyPem { get; set; } = BuiltInLicenseValidationPublicKeyPem;

        /// <summary>
        /// 授权申请签名私钥 PEM。
        /// 当前直接从代码默认值读取，不写入 config.json，也不从外部文件读取。
        /// </summary>
        [JsonIgnore]
        public string LicenseRequestSigningPrivateKeyPem { get; set; } = BuiltInLicenseRequestSigningPrivateKeyPem;
    }
}
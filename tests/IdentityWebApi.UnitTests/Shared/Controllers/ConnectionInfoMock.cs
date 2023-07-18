using Microsoft.AspNetCore.Http;

using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.UnitTests.Shared.Controllers;

/// <summary>
/// Test class for mocking <see cref="ConnectionInfo"/>.
/// </summary>
public class ConnectionInfoMock : ConnectionInfo
{
    public override Task<X509Certificate2> GetClientCertificateAsync(CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public override string Id { get; set; }

    public override IPAddress RemoteIpAddress { get; set; }

    public override int RemotePort { get; set; }

    public override IPAddress LocalIpAddress { get; set; }

    public override int LocalPort { get; set; }

    public override X509Certificate2 ClientCertificate { get; set; }
}

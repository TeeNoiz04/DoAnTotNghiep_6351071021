using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace QuoteFlow.RequesterContexts;

internal class RequesterContext : IRequesterContext, ITransientDependency
{
    private static readonly AsyncLocal<string?> _username = new();
    private static readonly AsyncLocal<string?> _fullName = new();
    private static readonly AsyncLocal<Guid?> _id = new();

    public Guid? Id
    {
        get => _id.Value;
        set => _id.Value = value;
    }

    public string? Username
    {
        get => _username.Value;
        set => _username.Value = value;
    }

    public string? FullName
    {
        get => _fullName.Value;
        set => _fullName.Value = value;
    }

    //public string? Username { get; set; } = "";
    //public string? FullName { get; set; }
}

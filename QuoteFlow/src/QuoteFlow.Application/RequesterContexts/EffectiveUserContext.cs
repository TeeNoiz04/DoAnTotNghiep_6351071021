using QuoteFlow.Shared.Utils;
using System;
using System.Collections.Generic;
using Volo.Abp.Users;

namespace QuoteFlow.RequesterContexts;

public class EffectiveUserContext : IEffectiveUserContext
{
    private readonly IRequesterContext _requesterContext;
    private readonly ICurrentUser _currentUser;

    public EffectiveUserContext(IRequesterContext requesterContext, ICurrentUser currentUser)
    {
        _requesterContext = requesterContext;
        _currentUser = currentUser;
    }

    public Guid? Id => _requesterContext.Id ?? _currentUser.Id;

    public string? Username => !string.IsNullOrWhiteSpace(_requesterContext.Username)
        ? _requesterContext.Username
        : _currentUser.UserName;

    public string? FullName => !string.IsNullOrWhiteSpace(_requesterContext.FullName)
        ? _requesterContext.FullName
        : UserHelper.GetFullName(_currentUser.Name, _currentUser.SurName);

    public IEnumerable<string> Roles
    {
        get
        {
            return _currentUser.Roles ?? [];
        }
    }
}

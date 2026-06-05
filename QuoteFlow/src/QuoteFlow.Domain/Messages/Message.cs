using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Volo.Abp;

namespace QuoteFlow.Messages;

public class Message : ExtendedFullAuditedAggregateRoot<Guid>
{
    /* ----------  Persistent columns  ---------- */
    private string _userName = default!;
    [NotNull]
    public string UserName
    {
        get => _userName;
        set
        {
            Check.NotNullOrWhiteSpace(value, nameof(UserName));
            Check.Length(value, nameof(UserName), MessageConsts.UserNameMaxLength, 0);
            _userName = value;
        }
    }

    private string _fullName = default!;
    [NotNull]
    public string FullName
    {
        get => _fullName;
        set
        {
            Check.NotNullOrWhiteSpace(value, nameof(FullName));
            Check.Length(value, nameof(FullName), MessageConsts.FullNameMaxLength, 0);
            _fullName = value;
        }
    }

    /// <summary>
    /// Comma‑separated e‑mail list, mapped to DB.
    /// Do **not** assign directly; use <see cref="Recipients"/> instead.
    /// </summary>
    [NotNull]
    public string SendTo { get; private set; } = default!;

    private string _comment = default!;
    [NotNull]
    public string Comment
    {
        get => _comment;
        set
        {
            Check.NotNullOrWhiteSpace(value, nameof(Comment));
            _comment = value;
        }
    }

    /* ----------  Non‑mapped helpers  ---------- */

    /// <summary>
    /// Convenience list view for business code / UI.
    /// </summary>
    public IReadOnlyList<string> Recipients => _recipients.AsReadOnly();

    /* ----------  Internal state  ---------- */
    private readonly List<string> _recipients = [];

    /* ----------  EF / Abp ctor  ---------- */
    protected Message() { }

    /* ----------  Aggregate‑root ctor  ---------- */
    public Message(
        Guid id,
        string userName,
        string fullName,
        IEnumerable<string> recipients,
        string comment)
    {
        Id = id;
        UserName = userName;
        FullName = fullName;
        Comment = comment;
        ReplaceAllRecipients(recipients);
    }

    /* ----------  Behaviour / domain logic  ---------- */

    public Message AddRecipient(string email)
    {
        ValidateEmail(email);
        if (_recipients.Exists(x => x.Equals(email, StringComparison.OrdinalIgnoreCase)))
            return this; // idempotent
        _recipients.Add(email);
        SyncSendToColumn();
        return this;
    }

    public Message RemoveRecipient(string email)
    {
        _recipients.RemoveAll(e => e.Equals(email, StringComparison.OrdinalIgnoreCase));
        SyncSendToColumn();
        return this;
    }

    public Message ReplaceAllRecipients(IEnumerable<string> emails)
    {
        Check.NotNull(emails, nameof(emails));
        var cleaned = emails
            .Select(e => e?.Trim())
            .Where(e => !e.IsNullOrWhiteSpace())
            .Distinct(StringComparer.OrdinalIgnoreCase)!
            .ToList<string>();

        if (cleaned.Count == 0)
            throw new BusinessException(QuoteFlowDomainErrorCodes.Discussion.RecipientRequired);

        cleaned.ForEach(ValidateEmail);

        _recipients.Clear();
        _recipients.AddRange(cleaned);
        SyncSendToColumn();

        return this;
    }

    /* ----------  Private utils  ---------- */

    private static void ValidateEmail(string email)
    {
        try
        {
            _ = new MailAddress(email);
        }
        catch
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.InvalidEmailAddress)
                .WithData("email", email);
        }
    }

    private void SyncSendToColumn()
    {
        SendTo = string.Join(",", _recipients);
    }
}
using QuoteFlow.Messages;
using System;
using System.Collections.Generic;

namespace QuoteFlow.DPOs;
public class DPOMessage : Message
{
    public Guid DPOId { get; set; }

    protected DPOMessage()
    {
    }

    public DPOMessage(Guid id, Guid dpoId, string userName, string fullName, IEnumerable<string> sendToEmails, string comment)
        : base(id, userName, fullName, sendToEmails, comment)
    {
        DPOId = dpoId;
    }
}
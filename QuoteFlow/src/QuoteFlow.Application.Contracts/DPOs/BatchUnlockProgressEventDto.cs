using System;

namespace QuoteFlow.DPOs;

/// <summary>
/// Unified event DTO for batch unlock progress tracking via Server-Sent Events (SSE)
/// </summary>
public class BatchUnlockProgressEventDto
{
    /// <summary>
    /// Event type: "started", "progress", or "complete"
    /// </summary>
    public string EventType { get; set; } = null!;

    // Started event fields

    /// <summary>
    /// DPO ID (populated in "started" event)
    /// </summary>
    public Guid? DpoId { get; set; }

    // Progress event fields

    /// <summary>
    /// DPO Detail ID being processed (populated in "progress" events)
    /// </summary>
    public Guid? DpoDetailId { get; set; }

    /// <summary>
    /// Status of current detail: "processing", "success", or "error"
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Error message if status is "error"
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Number of lock stock records successfully unlocked for this detail
    /// </summary>
    public int? UnlockedCount { get; set; }

    /// <summary>
    /// Number of lock stock records skipped (ReleasedLock = 1)
    /// </summary>
    public int? SkippedCount { get; set; }

    // Shared progress tracking

    /// <summary>
    /// Current detail number being processed (1-based)
    /// </summary>
    public int Current { get; set; }

    /// <summary>
    /// Total number of details to process
    /// </summary>
    public int Total { get; set; }

    // Complete event fields

    /// <summary>
    /// Total details processed (populated in "complete" event)
    /// </summary>
    public int? TotalProcessed { get; set; }

    /// <summary>
    /// Number of details successfully unlocked (populated in "complete" event)
    /// </summary>
    public int? Succeeded { get; set; }

    /// <summary>
    /// Number of details that failed (populated in "complete" event)
    /// </summary>
    public int? Failed { get; set; }

    /// <summary>
    /// Total lock stock records unlocked across all details (populated in "complete" event)
    /// </summary>
    public int? TotalUnlocked { get; set; }

    /// <summary>
    /// Total lock stock records skipped across all details (populated in "complete" event)
    /// </summary>
    public int? TotalSkipped { get; set; }
}

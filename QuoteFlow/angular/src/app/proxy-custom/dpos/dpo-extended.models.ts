/**
 * Custom models for DPO extended functionality
 * These are NOT auto-generated and should be maintained manually
 */

export interface BatchUnlockProgressEventDto {
  // Event identification
  eventType: 'started' | 'progress' | 'complete';

  // Started event fields
  dpoId?: string;

  // Progress event fields
  dpoDetailId?: string;
  status?: 'processing' | 'success' | 'error';
  errorMessage?: string;
  unlockedCount?: number;
  skippedCount?: number;

  // Shared progress tracking
  current: number;
  total: number;

  // Complete event fields
  totalProcessed?: number;
  succeeded?: number;
  failed?: number;
  totalUnlocked?: number;
  totalSkipped?: number;
}

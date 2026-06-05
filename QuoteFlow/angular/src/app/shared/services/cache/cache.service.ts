import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { shareReplay, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CacheService {
  private cache: Map<string, any> = new Map<string, any>();
  private observableCache: Map<string, Observable<any>> = new Map<string, Observable<any>>();

  /**
   * Get data from cache or fetch it using the provided fetcher function
   * @param key Cache key
   * @param fetcher Function to fetch data if not in cache
   * @param maxAge Cache expiration time in milliseconds (optional)
   */
  get<T>(key: string, fetcher: () => Observable<T>, maxAge?: number): Observable<T> {
    // If we have a pending request, return it
    if (this.observableCache.has(key)) {
      return this.observableCache.get(key);
    }

    // If we have cached data and it's not expired
    if (this.cache.has(key)) {
      const item = this.cache.get(key);
      if (!maxAge || Date.now() - item.timestamp < maxAge) {
        return of(item.value);
      }
      // Data is expired, remove it
      this.cache.delete(key);
    }

    // Get fresh data
    const observable = fetcher().pipe(
      tap(response => {
        // Store in cache
        this.cache.set(key, {
          value: response,
          timestamp: Date.now(),
        });
        // Remove from observable cache once completed
        setTimeout(() => this.observableCache.delete(key));
      }),
      // Share the same response with multiple subscribers
      shareReplay(1),
    );

    // Store observable in cache
    this.observableCache.set(key, observable);
    return observable;
  }

  /**
   * Clear cache for a specific key
   * @param key Cache key
   */
  clearCache(key?: string): void {
    if (key) {
      this.cache.delete(key);
      this.observableCache.delete(key);
    } else {
      this.cache.clear();
      this.observableCache.clear();
    }
  }
}

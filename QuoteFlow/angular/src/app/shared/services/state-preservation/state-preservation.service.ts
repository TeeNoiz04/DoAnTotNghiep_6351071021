import { Injectable } from '@angular/core';
import { ListStatePreservation } from '../../models/state-preservation.model';

@Injectable({
  providedIn: 'root',
})
export class StatePreservationService {
  private stateStore: { [key: string]: any } = {};

  saveState<T extends ListStatePreservation>(key: string, state: T) {
    this.stateStore[key] = { ...state };
  }

  getState<T extends ListStatePreservation>(key: string): T | undefined {
    return this.stateStore[key];
  }

  clearState(key: string) {
    delete this.stateStore[key];
  }

  clearAllStates() {
    this.stateStore = {};
  }
}
